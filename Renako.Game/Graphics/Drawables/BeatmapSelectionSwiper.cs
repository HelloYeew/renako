using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using Renako.Game.Beatmaps;

namespace Renako.Game.Graphics.Drawables;

public partial class BeatmapSelectionSwiper : CompositeDrawable
{
    private BeatmapSelectionSwiperContainer leftContainer3;
    private BeatmapSelectionSwiperContainer leftContainer2;
    private BeatmapSelectionSwiperContainer leftContainer1;
    private BeatmapSelectionSwiperContainer centerContainer;
    private BeatmapSelectionSwiperContainer rightContainer1;
    private BeatmapSelectionSwiperContainer rightContainer2;
    private BeatmapSelectionSwiperContainer rightContainer3;

    public List<Beatmap> BeatmapsList { get; set; } = new List<Beatmap>();

    private Texture texture;

    private readonly BindableInt currentIndex = new BindableInt();
    public readonly Bindable<Beatmap> CurrentItem = new Bindable<Beatmap>();

    public BeatmapSelectionSwiper()
    {
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(1, 150);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new FillFlowContainer()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(50, 0),
                Children = new Drawable[]
                {
                    centerContainer = new BeatmapSelectionSwiperContainer()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(70, 70),
                        Masking = true,
                        CornerRadius = 10,
                        FillMode = FillMode.Fill,
                        Children = new Drawable[]
                        {
                            leftContainer3 = new BeatmapSelectionSwiperContainer(),
                            leftContainer2 = new BeatmapSelectionSwiperContainer(),
                            leftContainer1 = new BeatmapSelectionSwiperContainer(),
                            centerContainer = new BeatmapSelectionSwiperContainer()
                            {
                                Rotation = 10,
                                Size = new Vector2(220, 220)
                            },
                            rightContainer1 = new BeatmapSelectionSwiperContainer(),
                            rightContainer2 = new BeatmapSelectionSwiperContainer(),
                            rightContainer3 = new BeatmapSelectionSwiperContainer()
                        }
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        UpdateContainerItem();
    }

    /// <summary>
    /// Move to the next item.
    /// </summary>
    public void Next()
    {
        if (currentIndex.Value + 1 < BeatmapsList.Count)
        {
            currentIndex.Value++;
        }
        else
        {
            currentIndex.Value = 0;
        }

        UpdateContainerItem();
    }

    /// <summary>
    /// Move to the previous item.
    /// </summary>
    public void Previous()
    {
        if (currentIndex.Value - 1 >= 0)
        {
            currentIndex.Value--;
        }
        else
        {
            currentIndex.Value = BeatmapsList.Count - 1;
        }

        UpdateContainerItem();
    }

    /// <summary>
    /// Swipe to the first item.
    /// </summary>
    public void First()
    {
        currentIndex.Value = 0;
        UpdateContainerItem();
    }

    /// <summary>
    /// Add <see cref="Beatmap"/> to the swiper.
    /// </summary>
    /// <param name="beatmap">The beatmap that will be added.</param>
    public void AddBeatmap(Beatmap beatmap)
    {
        BeatmapsList.Add(beatmap);
        UpdateContainerItem();
    }

    /// <summary>
    /// Remove <see cref="Beatmap"/> from the swiper.
    /// </summary>
    /// <param name="beatmap">The beatmap that will be removed.</param>
    public void RemoveBeatmap(Beatmap beatmap)
    {
        BeatmapsList.Remove(beatmap);
        UpdateContainerItem();
    }

    /// <summary>
    /// Set the index of the swiper item.
    /// </summary>
    /// <param name="index">The index that will be set.</param>
    public void SetIndex(int index)
    {
        currentIndex.Value = index;
        UpdateContainerItem();
    }

    /// <summary>
    /// Set the item of the swiper to the specified item.
    /// If the item is not found, nothing will be changed.
    /// </summary>
    /// <param name="beatmap">The item that will be set.</param>
    public void SetItem(Beatmap beatmap)
    {
        for (int i = 0; i < BeatmapsList.Count; i++)
        {
            if (BeatmapsList[i].Equals(beatmap))
            {
                currentIndex.Value = i;
                UpdateContainerItem();
                break;
            }
        }
    }

    /// <summary>
    /// Get the current index of the swiper item.
    /// </summary>
    public int CurrentIndex
    {
        get => currentIndex.Value;
        set => currentIndex.Value = value;
    }

    /// <summary>
    /// Update the container item in the swiper.
    /// </summary>
    public void UpdateContainerItem()
    {
        // Update the left container 3.
        // If current index - 3 is less than 0, set as blank container
        if (currentIndex.Value - 3 < 0)
        {
            leftContainer3.ClearContainer();
        }
        else
        {
            leftContainer3.SetTexture(texture);
            leftContainer3.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value - 3].DifficultyRating));
        }

        // Update the left container 2.
        // If current index - 2 is less than 0, set as blank container
        if (currentIndex.Value - 2 < 0)
        {
            leftContainer2.ClearContainer();
        }
        else
        {
            leftContainer2.SetTexture(texture);
            leftContainer2.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value - 2].DifficultyRating));
        }

        // Update the left container 1.
        // If current index - 1 is less than 0, set as blank container
        if (currentIndex.Value - 1 < 0)
        {
            leftContainer1.ClearContainer();
        }
        else
        {
            leftContainer1.SetTexture(texture);
            leftContainer1.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value - 1].DifficultyRating));
        }

        // Update the center container.
        centerContainer.SetTexture(texture);
        centerContainer.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value].DifficultyRating));

        // Update the right container 1.
        // If current index + 1 is more than the BeatmapsList count, set as blank container
        if (currentIndex.Value + 1 >= BeatmapsList.Count)
        {
            rightContainer1.ClearContainer();
        }
        else
        {
            rightContainer1.SetTexture(texture);
            rightContainer1.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value + 1].DifficultyRating));
        }

        // Update the right container 2.
        // If current index + 2 is more than the BeatmapsList count, set as blank container
        if (currentIndex.Value + 2 >= BeatmapsList.Count)
        {
            rightContainer2.ClearContainer();
        }
        else
        {
            rightContainer2.SetTexture(texture);
            rightContainer2.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value + 2].DifficultyRating));
        }

        // Update the right container 2.
        // If current index + 2 is more than the BeatmapsList count, set as blank container
        if (currentIndex.Value + 3 >= BeatmapsList.Count)
        {
            rightContainer3.ClearContainer();
        }
        else
        {
            rightContainer3.SetTexture(texture);
            rightContainer3.SetBoxColor(RenakoColour.ForDifficultyLevel(BeatmapsList[currentIndex.Value + 3].DifficultyRating));
        }

        CurrentItem.Value = BeatmapsList[currentIndex.Value];
    }

    /// <summary>
    /// Add <see cref="Action"/> that will be invoked when the index is changed.
    /// </summary>
    /// <param name="action">The action to be invoked.</param>
    public void BindIndexChangeAction(Action<ValueChangedEvent<int>> action)
    {
        currentIndex.BindValueChanged(action);
    }
}

/// <summary>
/// The container for the swiper item.
/// </summary>
public partial class BeatmapSelectionSwiperContainer : Container
{
    private Texture texture;
    private Colour4 boxColor;
    private readonly Box box;
    private readonly Container mainContainer;

    public BeatmapSelectionSwiperContainer()
    {
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        Size = new Vector2(70, 70);
        Masking = true;
        CornerRadius = 10;
        FillMode = FillMode.Fill;
        Children = new Drawable[]
        {
            box = new Box()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Colour = boxColor
            },
            mainContainer = new Container()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Size = new Vector2(0.9f),
                CornerRadius = 10,
                Children = new Drawable[]
                {
                    new Sprite()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fill,
                        Texture = texture
                    }
                }
            }
        };
    }

    /// <summary>
    /// Set texture in container's sprite
    /// </summary>
    /// <param name="texture">The texture that will be set</param>
    public void SetTexture(Texture texture)
    {
        this.texture = texture;
        mainContainer.Alpha = 1;
    }

    /// <summary>
    /// Set the color of the box
    /// </summary>
    /// <param name="color">The color that will be set</param>
    public void SetBoxColor(Colour4 color)
    {
        boxColor = color;
        box.Alpha = 1;
    }

    /// <summary>
    /// Clear the container or make it disappear.
    /// </summary>
    public void ClearContainer()
    {
        box.Alpha = 0;
        texture = null;
        mainContainer.Alpha = 0;
    }
}
