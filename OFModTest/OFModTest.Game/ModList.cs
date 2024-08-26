using ModdingFramework;
using ModdingFramework.Attributes;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace OFModTest.Game
{
    [OverridableClass]
    public partial class ModList : Container
    {
        protected BasicScrollContainer Scroller;
        protected FillFlowContainer FlowContainer;

        public ModList()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0.25f
                },
                Scroller = new()
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = FlowContainer = new()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OFModTestGameBase game)
        {
            FlowContainer.Add(new TextFlowContainer()
            {
                AutoSizeAxes = Axes.Both,
                Text = $"Current Mod Configuration Provider: {game.ModManager.ConfigurationProvider}",
                Margin = new MarginPadding(15)
            });

            foreach (IMod mod in game.ModManager.LoadedMods)
            {
                FlowContainer.Add(ClassRegistry.CreateInstance<ModListEntry>(mod));
            }
        }
    }
}
