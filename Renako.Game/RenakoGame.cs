﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Database;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game
{
    public partial class RenakoGame : RenakoGameBase
    {
        private DependencyContainer dependencies;

        private RenakoScreenStack mainScreenStack;
        private SettingsScreenStack settingsScreenStack;
        private RenakoBackgroundScreenStack backgroundScreenStack;
        private LogoScreenStack logoScreenStack;
        private InternalBeatmapImporter internalBeatmapImporter;
        private new BeatmapCollectionReader beatmapCollectionReader;

        [BackgroundDependencyLoader]
        private void load()
        {
            dependencies.CacheAs(backgroundScreenStack = new RenakoBackgroundScreenStack());
            dependencies.CacheAs(mainScreenStack = new RenakoScreenStack());
            dependencies.CacheAs(logoScreenStack = new LogoScreenStack());
            dependencies.CacheAs(beatmapCollectionReader = new BeatmapCollectionReader(Host.Storage, BeatmapsCollection));
            dependencies.CacheAs(logoScreenStack.LogoScreenObject);
            Add(backgroundScreenStack);
            Add(mainScreenStack);
            Add(logoScreenStack);
            Add(RenakoAudioManager);
            loadComponentSingleFile(settingsScreenStack = new SettingsScreenStack(), Add, true);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected override void LoadComplete()
        {
            base.LoadComplete();

            internalBeatmapImporter = new InternalBeatmapImporter(AudioManager, TextureStore, Host);

            if (!LocalConfig.Get<bool>(RenakoSetting.FirstImport) && !DebugUtils.IsNUnitRunning)
            {
                Logger.Log("First time import detected, importing internal beatmaps...");
                BeatmapsCollection.GenerateTestCollection();
                internalBeatmapImporter.Import();
                LocalConfig.SetValue(RenakoSetting.FirstImport, true);
            }

            beatmapCollectionReader.Read();
            BeatmapsCollection.SortBeatmapSetsByID();

            if (LocalConfig.Get<bool>(RenakoSetting.UseRenakoThemeSong))
            {
                WorkingBeatmap.BeatmapSet = BeatmapsCollection.GetBeatmapSetByID(BeatmapsCollection.THEME_SONG_BEATMAP_SET_ID);
                WorkingBeatmap.Beatmap = BeatmapsCollection.GetBeatmapByID(BeatmapsCollection.THEME_SONG_BEATMAP_ID);
            }
            else
            {
                WorkingBeatmap.BeatmapSet = BeatmapsCollection.GetRandomBeatmapSet();
                WorkingBeatmap.Beatmap = BeatmapsCollection.GetFirstBeatmapFromBeatmapSet(WorkingBeatmap.BeatmapSet);
            }

            RenakoAudioManager.Track?.Stop();

            backgroundScreenStack.AdjustMaskAlpha(1);
            Scheduler.AddDelayed(() =>
            {
                backgroundScreenStack.SeekBackgroundVideo(RenakoAudioManager.Track?.CurrentTime ?? 0);
                backgroundScreenStack.ResetMaskAlpha(250);
            }, 3500);

            mainScreenStack.Push(new WarningScreen());
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            if (host.Window != null)
            {
                host.Window.DragDrop += path =>
                {
                    // TODO: Add import function here
                    Logger.Log($"Got file drag and drop: {path}");
                };
            }
        }

        public Type GetCurrentScreenType()
        {
            return mainScreenStack.CurrentScreen.GetType();
        }

        /// <summary>
        /// Re-reads the beatmap collection from disk and refreshes the song selection screen if present.
        /// </summary>
        private void refreshBeatmapCollection()
        {
            beatmapCollectionReader.Read();

            if (mainScreenStack.CurrentScreen is SongSelectionScreen)
            {
                mainScreenStack.CurrentScreen.Exit();
                mainScreenStack.Push(new SongSelectionScreen());
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    BeatmapManager.NextBeatmapSet(true);
                    return true;

                case Key.F5:
                    refreshBeatmapCollection();
                    return true;

                case Key.F6:
                    BeatmapManager.PreviousBeatmapSet();
                    return true;

                case Key.F7:
                    // TODO: Pause
                    return true;

                case Key.F8:
                    BeatmapManager.NextBeatmapSet();
                    return true;
            }

            return base.OnKeyDown(e);
        }

        private Task asyncLoadStream;

        /// <summary>
        /// Queues loading the provided component in sequential fashion.
        /// This operation is limited to a single thread to avoid saturating all cores.
        /// </summary>
        /// <param name="component">The component to load.</param>
        /// <param name="loadCompleteAction">An action to invoke on load completion (generally to add the component to the hierarchy).</param>
        /// <param name="cache">Whether to cache the component as type <typeparamref name="T"/> into the game dependencies before any scheduling.</param>
        private T loadComponentSingleFile<T>(T component, Action<Drawable> loadCompleteAction, bool cache = false)
            where T : class
        {
            if (cache)
                dependencies.CacheAs(component);

            var drawableComponent = component as Drawable ?? throw new ArgumentException($"Component must be a {nameof(Drawable)}", nameof(component));

            // schedule is here to ensure that all component loads are done after LoadComplete is run (and thus all dependencies are cached).
            // with some better organisation of LoadComplete to do construction and dependency caching in one step, followed by calls to loadComponentSingleFile,
            // we could avoid the need for scheduling altogether.
            Schedule(() =>
            {
                var previousLoadStream = asyncLoadStream;

                // chain with existing load stream
                asyncLoadStream = Task.Run(async () =>
                {
                    if (previousLoadStream != null)
                        await previousLoadStream.ConfigureAwait(false);

                    try
                    {
                        Logger.Log($"💉 Loading {component}...");

                        // Since this is running in a separate thread, it is possible for RenakoGame to be disposed after LoadComponentAsync has been called
                        // throwing an exception. To avoid this, the call is scheduled on the update thread, which does not run if IsDisposed = true
                        Task task = null;
                        var del = new ScheduledDelegate(() => task = LoadComponentAsync(drawableComponent, loadCompleteAction));
                        Scheduler.Add(del);

                        // The delegate won't complete if RenakoGame has been disposed in the meantime
                        while (!IsDisposed && !del.Completed)
                            await Task.Delay(10).ConfigureAwait(false);

                        // Either we're disposed or the load process has started successfully
                        if (IsDisposed)
                            return;

                        Debug.Assert(task != null);

                        await task.ConfigureAwait(false);

                        Logger.Log($"💉 Loaded {component}!");
                    }
                    catch (OperationCanceledException)
                    {
                    }
                });
            });

            return component;
        }
    }
}
