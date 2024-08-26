using ModdingFramework;
using OFModTest.Game;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Mod.Example2
{
    public partial class AnotherModListEntry : ModListEntry
    {
        public AnotherModListEntry(IMod mod) : base(mod) { }

        protected override Container GetBG()
        {
            Box bg = new()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black,
                Alpha = 0.25f,
            };

            return new()
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 25,
                Child = bg
            };
        }
    }
}
