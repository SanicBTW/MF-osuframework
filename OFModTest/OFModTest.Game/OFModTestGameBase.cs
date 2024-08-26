using OFModTest.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using ModdingFramework;
using System.Reflection;

namespace OFModTest.Game
{
    public partial class OFModTestGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        // Initializing this before registering the overridable classes might end on the mods not taking any effect.
        public ModLoader ModManager;

        protected override Container<Drawable> Content { get; }

        protected OFModTestGameBase()
        {
            ClassRegistry.RegisterOverridableClasses(Assembly.GetExecutingAssembly());

            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1366, 768)
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(OFModTestResources.ResourceAssembly));
            ModManager = new(folder: "", filePrefix: "Mod.Example", config: new OFConfigProvider(Host.Storage));
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            DependencyContainer dep = new DependencyContainer(base.CreateChildDependencies(parent));
            dep.CacheAs(this);
            return dep;
        }
    }
}
