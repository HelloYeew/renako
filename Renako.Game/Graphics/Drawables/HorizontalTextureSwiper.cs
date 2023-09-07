using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

public partial class HorizontalTextureSwiper<T> : CompositeDrawable
{
    private Button chevronLeftContainer;
    private Button chevronRightContainer;

    private TextureSwiperContainer leftContainer2;
    private TextureSwiperContainer leftContainer1;
    private TextureSwiperContainer centerContainer;
    private TextureSwiperContainer rightContainer1;
    private TextureSwiperContainer rightContainer2;

    public List<TextureSwiperItem<T>> Items { get; set; } = new List<TextureSwiperItem<T>>();

    private readonly BindableInt currentIndex = new BindableInt();
    public readonly Bindable<T> CurrentItem = new Bindable<T>();

    public HorizontalTextureSwiper()
    {
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(1, 150);
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
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
                    leftContainer2 = new TextureSwiperContainer(),
                    leftContainer1 = new TextureSwiperContainer(),
                    centerContainer = new TextureSwiperContainer()
                    {
                        Rotation = 10,
                        Size = new Vector2(220, 220)
                    },
                    rightContainer1 = new TextureSwiperContainer(),
                    rightContainer2 = new TextureSwiperContainer()
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
        if (currentIndex.Value + 1 < Items.Count)
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
            currentIndex.Value = Items.Count - 1;
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
    /// Add <see cref="ISwiperItem{T}"/> to the swiper.
    /// </summary>
    /// <param name="item">The item that will be added.</param>
    public void AddItem(TextureSwiperItem<T> item)
    {
        Items.Add(item);
        UpdateContainerItem();
    }

    /// <summary>
    /// Remove <see cref="ISwiperItem{T}"/> from the swiper.
    /// </summary>
    /// <param name="item">The item that will be removed.</param>
    public void RemoveItem(TextureSwiperItem<T> item)
    {
        Items.Remove(item);
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
    /// Get the current index of the swiper item.
    /// </summary>
    public int CurrentIndex => currentIndex.Value;

    /// <summary>
    /// Update the container item in the swiper.
    /// </summary>
    public void UpdateContainerItem()
    {
        // Update the left container 2.
        // If current index - 2 is less than 0, set as blank container
        if (currentIndex.Value - 2 < 0)
        {
            leftContainer2.ClearTexture();
        }
        else
        {
            leftContainer2.SetTexture(Items[currentIndex.Value - 2].Texture);
        }

        // Update the left container 1.
        // If current index - 1 is less than 0, set as blank container
        if (currentIndex.Value - 1 < 0)
        {
            leftContainer1.ClearTexture();
        }
        else
        {
            leftContainer1.SetTexture(Items[currentIndex.Value - 1].Texture);
        }

        // Update the center container.
        centerContainer.SetTexture(Items[currentIndex.Value].Texture);

        // Update the right container 1.
        // If current index + 1 is more than the items count, set as blank container
        if (currentIndex.Value + 1 >= Items.Count)
        {
            rightContainer1.ClearTexture();
        }
        else
        {
            rightContainer1.SetTexture(Items[currentIndex.Value + 1].Texture);
        }

        // Update the right container 2.
        // If current index + 2 is more than the items count, set as blank container
        if (currentIndex.Value + 2 >= Items.Count)
        {
            rightContainer2.ClearTexture();
        }
        else
        {
            rightContainer2.SetTexture(Items[currentIndex.Value + 2].Texture);
        }

        CurrentItem.Value = Items[currentIndex.Value].Item;
    }

    /// <summary>
    /// Add <see cref="Action"/> that will be invoked when the index is changed.
    /// </summary>
    /// <param name="action"></param>
    public void BindIndexChangeAction(Action<ValueChangedEvent<int>> action)
    {
        currentIndex.BindValueChanged(action);
    }
}

/// <summary>
/// The item that will be used in <see cref="HorizontalTextureSwiper{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the item.</typeparam>
public class TextureSwiperItem<T>
{
    public T Item { get; set; }

    public Texture Texture { get; set; }
}

/// <summary>
/// The container that will be use in <see cref="HorizontalTextureSwiper{T}"/>.
/// </summary>
public partial class TextureSwiperContainer : Container
{
    private readonly Sprite textureBox;

    public TextureSwiperContainer()
    {
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        Size = new Vector2(70, 70);
        Masking = true;
        CornerRadius = 10;
        FillMode = FillMode.Fill;
        Child = textureBox = new Sprite()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        };
    }

    /// <summary>
    /// Set the texture of the container.
    /// </summary>
    /// <param name="texture">The texture that will be set.</param>
    public void SetTexture(Texture texture)
    {
        textureBox.Texture = texture;
    }

    /// <summary>
    /// Clear the texture of the container.
    /// </summary>
    public void ClearTexture()
    {
        textureBox.Texture = null;
    }
}
