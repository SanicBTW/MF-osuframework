using OFModTest.Game;
using osu.Framework.Graphics;

namespace ModExample
{
    // See Mod1.cs for an example of this class overwriting SpinningBox
    public partial class NewSpinningBox : SpinningBox
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Name = "NewSpinny";
            InternalChild.ClearTransforms();
            InternalChild.Loop(b => b.ScaleTo(1f, 1000D, Easing.InOutElastic).Delay(1000D).ScaleTo(0.5f, 1000D, Easing.OutElastic).Delay(500D)
                .MoveToOffset(new osuTK.Vector2(220, 0), 1000D, Easing.OutBounce).Delay(1000D).MoveToOffset(new osuTK.Vector2(-420, 0), 1000D, Easing.OutBounce).Delay(1000D)
                .MoveTo(osuTK.Vector2.Zero, 1000D, Easing.InOutElastic));
        }
    }
}
