using System.Linq;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using Renako.Game.Graphics;

namespace Renako.Game.Tests.Visual.Colours;

public partial class TestSceneDifficultyLevelColour : RenakoTestScene
{
    [Test]
    public void TestColours()
    {
        AddStep("load colour displays", () =>
        {
            Child = new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5f),
                Scale = new Vector2(0.65f),
                ChildrenEnumerable = Enumerable.Range(0, 20).Select(i => new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(10f),
                    ChildrenEnumerable = Enumerable.Range(0, 10).Select(j =>
                    {
                        var colour = RenakoColour.ForDifficultyLevel(1f * i + 0.1f * j);

                        return new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0f, 10f),
                            Children = new Drawable[]
                            {
                                new CircularContainer
                                {
                                    Masking = true,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(75f, 25f),
                                    Children = new Drawable[]
                                    {
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Colour = colour,
                                        },
                                        new RenakoSpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Colour = Color4.White,
                                            Text = colour.ToHex(),
                                        },
                                    }
                                },
                                new RenakoSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = $"{1f * i + 0.1f * j:0.00}"
                                }
                            }
                        };
                    })
                })
            };
        });
    }
}
