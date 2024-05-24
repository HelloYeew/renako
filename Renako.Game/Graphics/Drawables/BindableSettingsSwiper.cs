using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

public partial class BindableSettingsSwiper : CompositeDrawable
{
    private BindableSettingsContainer leftContainer3;
    private BindableSettingsContainer leftContainer2;
    private BindableSettingsContainer leftContainer1;
    private BindableSettingsContainer centerContainer;
    private BindableSettingsContainer rightContainer1;
    private BindableSettingsContainer rightContainer2;
    private BindableSettingsContainer rightContainer3;

    private List<BindableSettingsSwiperItem> items = new List<BindableSettingsSwiperItem>();

    private readonly BindableInt currentIndex = new BindableInt();
    public readonly Bindable<BindableSettingsSwiperItem> CurrentItem = new Bindable<BindableSettingsSwiperItem>();

    public BindableSettingsSwiper()
    {
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(1, 150);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChild = new FillFlowContainer()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(50, 0),
            Children = new Drawable[]
            {
                leftContainer3 = new BindableSettingsContainer(),
                leftContainer2 = new BindableSettingsContainer(),
                leftContainer1 = new BindableSettingsContainer(),
                centerContainer = new BindableSettingsContainer()
                {
                    Size = new Vector2(220, 220)
                },
                rightContainer1 = new BindableSettingsContainer(),
                rightContainer2 = new BindableSettingsContainer(),
                rightContainer3 = new BindableSettingsContainer()
            }
        };

        centerContainer.SetAsPrimary();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (items.Count > 0)
            UpdateContainerItem();
    }

    public void Next()
    {
        if (currentIndex.Value + 1 < items.Count)
        {
            currentIndex.Value++;
        }
        else
        {
            currentIndex.Value = 0;
        }

        UpdateContainerItem();
    }

    public void Previous()
    {
        if (currentIndex.Value - 1 >= 0)
        {
            currentIndex.Value--;
        }
        else
        {
            currentIndex.Value = items.Count - 1;
        }

        UpdateContainerItem();
    }

    public void First()
    {
        currentIndex.Value = 0;
        UpdateContainerItem();
    }

    public List<BindableSettingsSwiperItem> Items
    {
        get => items;
        set
        {
            items = value;
            UpdateContainerItem();
        }
    }

    public void AddItem(BindableSettingsSwiperItem item)
    {
        items.Add(item);
        UpdateContainerItem();
    }

    public void RemoveItem(BindableSettingsSwiperItem item)
    {
        items.Remove(item);
        UpdateContainerItem();
    }

    /// <summary>
    /// Set the index of the swiper.
    /// </summary>
    /// <param name="index">The index to set.</param>
    public void SetIndex(int index)
    {
        currentIndex.Value = index;
        UpdateContainerItem();
    }

    /// <summary>
    /// Get the current index of the swiper.
    /// </summary>
    public int CurrentIndex
    {
        get => currentIndex.Value;
        set
        {
            currentIndex.Value = value;
            UpdateContainerItem();
        }
    }

    public void UpdateContainerItem()
    {
        if (items.Count == 0)
        {
            // Clear all containers
            leftContainer3.ClearItem();
            leftContainer2.ClearItem();
            leftContainer1.ClearItem();
            centerContainer.ClearItem();
            rightContainer1.ClearItem();
            rightContainer2.ClearItem();
            rightContainer3.ClearItem();
            return;
        }

        // Update the left container 3.
        // If current index - 3 is less than 0, set as blank container
        if (currentIndex.Value - 3 < 0)
        {
            leftContainer3.ClearItem();
        }
        else
        {
            leftContainer3.SetItem(items[currentIndex.Value - 3]);
        }

        // Update the left container 2.
        // If current index - 2 is less than 0, set as blank container
        if (currentIndex.Value - 2 < 0)
        {
            leftContainer2.ClearItem();
        }
        else
        {
            leftContainer2.SetItem(items[currentIndex.Value - 2]);
        }

        // Update the left container 1.
        // If current index - 1 is less than 0, set as blank container
        if (currentIndex.Value - 1 < 0)
        {
            leftContainer1.ClearItem();
        }
        else
        {
            leftContainer1.SetItem(items[currentIndex.Value - 1]);
        }

        // Update the center container.
        centerContainer.SetItem(items[currentIndex.Value]);

        // Update the right container 1.
        // If current index + 1 is more than the item count, set as blank container
        if (currentIndex.Value + 1 >= items.Count)
        {
            rightContainer1.ClearItem();
        }
        else
        {
            rightContainer1.SetItem(items[currentIndex.Value + 1]);
        }

        // Update the right container 2.
        // If current index + 2 is more than the item count, set as blank container
        if (currentIndex.Value + 2 >= items.Count)
        {
            rightContainer2.ClearItem();
        }
        else
        {
            rightContainer2.SetItem(items[currentIndex.Value + 2]);
        }

        // Update the right container 2.
        // If current index + 2 is more than the item count, set as blank container
        if (currentIndex.Value + 3 >= items.Count)
        {
            rightContainer3.ClearItem();
        }
        else
        {
            rightContainer3.SetItem(items[currentIndex.Value + 3]);
        }

        CurrentItem.Value = items[currentIndex.Value];
    }

    /// <summary>
    /// Add <see cref="Action"/> that will be invoked when the index is changed.
    /// </summary>
    /// <param name="action">The action to be invoked.</param>
    public void BindIndexChangeAction(Action<ValueChangedEvent<int>> action)
    {
        currentIndex.BindValueChanged(action);
    }

    /// <summary>
    /// Increment the current item value.
    /// </summary>
    public void IncrementCurrentItem()
    {
        CurrentItem.Value?.Increment();
    }

    /// <summary>
    /// Decrement the current item value.
    /// </summary>
    public void DecrementCurrentItem()
    {
        CurrentItem.Value?.Decrement();
    }
}

public partial class BindableSettingsContainer : Container
{
    private BindableSettingsSwiperItem item;

    private readonly Box box;

    private readonly SpriteText nameText;
    private readonly SpriteText valueText;
    private readonly Button increaseButton;
    private readonly Button decreaseButton;

    private Sample upClickSample;
    private Sample downClickSample;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        // TODO: CHange this to a better sound
        upClickSample = audioManager.Samples.Get("UI/click-small-left");
        downClickSample = audioManager.Samples.Get("UI/click-small-right");
    }

    public BindableSettingsContainer()
    {
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        Size = new Vector2(100, 100);
        Masking = true;
        CornerRadius = 10;
        FillMode = FillMode.Fill;
        Children = new Drawable[]
        {
            box = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("F2DFE9")
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    nameText = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 24, RenakoFont.FontWeight.Bold),
                        Text = "Name".ToUpper(),
                        Colour = Color4Extensions.FromHex("776271")
                    },
                    increaseButton = new BasicButton()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(20, 20),
                        Alpha = 0,
                        Action = () =>
                        {
                            item?.Increment();
                            upClickSample?.Play();
                            checkButtonStatus();
                        },
                        Child = new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Icon = FontAwesome.Solid.ChevronUp,
                            Size = new Vector2(20),
                            Colour = Colour4.Black
                        }
                    },
                    valueText = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 20),
                        Text = "0",
                        Colour = Colour4.Black
                    },
                    decreaseButton = new BasicButton()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(20, 20),
                        Alpha = 0,
                        Action = () =>
                        {
                            item?.Decrement();
                            downClickSample?.Play();
                            // TODO: Maybe better if this is done with action on bindable
                            checkButtonStatus();
                        },
                        Child = new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Icon = FontAwesome.Solid.ChevronDown,
                            Size = new Vector2(20),
                            Colour = Colour4.Black
                        }
                    }
                }
            }
        };
    }

    public void SetItem(BindableSettingsSwiperItem item)
    {
        this.item = item;
        box.Alpha = 1f;
        nameText.Text = item.Name.ToUpper();
        item.BindableInt.UnbindAll();
        item.BindableInt.BindValueChanged(_ => valueText.Text = item.BindableInt.Value + item.Unit, true);
    }

    /// <summary>
    /// Check the button status based on the amount.
    /// </summary>
    private void checkButtonStatus()
    {
        if (item == null) return;

        if (item.BindableInt.Value > item.MaxValue)
        {
            increaseButton.Enabled.Value = false;
            increaseButton.Alpha = 0.5f;
        }
        else
        {
            increaseButton.Enabled.Value = true;
            increaseButton.Alpha = 1;
        }

        if (item.BindableInt.Value < item.MinValue)
        {
            decreaseButton.Enabled.Value = false;
            decreaseButton.Alpha = 0.5f;
        }
        else
        {
            decreaseButton.Enabled.Value = true;
            decreaseButton.Alpha = 1;
        }
    }

    /// <summary>
    /// Change the container and text colour to indicate that this is the primary item.
    /// </summary>
    public void SetAsPrimary()
    {
        box.Colour = Color4Extensions.FromHex("E0BCD5");
        nameText.Colour = Color4Extensions.FromHex("763363");
        nameText.Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 36, RenakoFont.FontWeight.Bold);
        valueText.Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 32);
        increaseButton.Alpha = 1;
        decreaseButton.Alpha = 1;
    }

    public void ClearItem()
    {
        item = null;
        box.Alpha = 0f;
        nameText.Text = "";
        valueText.Text = "";
    }
}

public class BindableSettingsSwiperItem
{
    public string Name { get; }
    public Bindable<int> BindableInt { get; }
    public int IncrementStep { get; set; } = 1;

    public int MaxValue { get; set; } = int.MaxValue;
    public int MinValue { get; set; } = int.MinValue;
    public string Unit { get; set; } = "";

    public BindableSettingsSwiperItem(string name, Bindable<int> bindableInt)
    {
        Name = name;
        BindableInt = bindableInt;
    }

    public void Increment()
    {
        if (BindableInt.Value + IncrementStep <= MaxValue)
            BindableInt.Value += IncrementStep;
    }

    public void Decrement()
    {
        if (BindableInt.Value - IncrementStep >= MinValue)
            BindableInt.Value -= IncrementStep;
    }
}
