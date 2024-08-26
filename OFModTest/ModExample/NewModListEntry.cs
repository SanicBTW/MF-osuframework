using ModdingFramework;
using OFModTest.Game;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace ModExample
{
    // Look at ModListEntry to know how the constructor parameters were declared to make the Class Registry overwrite work
    // If the class you want to overwrite doesn't provide the constructor parameters inside the Attribute declaration
    // The game will crash mentioning something about Missing Method Exception or basically reflection failing
    public partial class NewModListEntry : ModListEntry
    {
        public NewModListEntry(IMod mod) : base(mod) { }

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
                CornerRadius = 15,
                Child = bg
            };
        }
    }
}
