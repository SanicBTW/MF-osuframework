using System;
using System.Linq;
using ModdingFramework;
using ModdingFramework.Attributes;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace OFModTest.Game
{
    // If your class uses constructor parameters, you need to pass all the types that are used inside of it
    [OverridableClass(typeof(IMod))]

    public partial class ModListEntry : Container
    {
        protected IMod Mod;
        protected SpriteText Header;
        protected BasicCheckbox Toggle;

        public ModListEntry(IMod mod)
        {
            Mod = mod;
            AutoSizeAxes = Axes.Both;
            Margin = new MarginPadding(15);
        }

        [BackgroundDependencyLoader]
        private void load(OFModTestGameBase game)
        {
            Bindable<bool> bindable = new(game.ModManager.EnabledMods.Select((mod) => mod.Manifest.Name).Contains(Mod.Manifest.Name));
            Container bg;
            Children = new Drawable[]
            {
                bg = GetBG(),
                new FillFlowContainer()
                {
                    Padding = new MarginPadding(15),
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        Header = new()
                        {
                            Text = $"{Mod.Manifest.Name} by {Mod.Manifest.Author}",
                            Font = FontUsage.Default.With(size: 24)
                        },
                        Toggle = new()
                        {
                            Current = bindable,
                        }
                    }
                }
            };

            bindable.BindValueChanged((ev) =>
            {
                Toggle.LabelText = $"{(ev.NewValue ? "Disable" : "Enable")}";
                if (ev.NewValue)
                {
                    game.ModManager.EnableMod(Mod.Manifest.Name);
                    bg.Child.FadeColour(Colour4.Green, 500D);
                }
                else
                {
                    game.ModManager.DisableMod(Mod.Manifest.Name);
                    bg.Child.FadeColour(Colour4.Red, 500D);
                }
            }, true);
        }

        protected virtual Container GetBG() => new()
        {
            RelativeSizeAxes = Axes.Both,
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Blue,
                Alpha = 0.25f,
            }
        };
    }
}
