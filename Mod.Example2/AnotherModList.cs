using OFModTest.Game;
using osu.Framework.Graphics;

namespace Mod.Example2
{
    public partial class AnotherModList : ModList
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Masking = true;
            CornerRadius = 15;
            // Children[0].FadeTo(1f, 1000D);
            Children[0].FadeColour(Colour4.DodgerBlue, 1000D);
        }
    }
}
