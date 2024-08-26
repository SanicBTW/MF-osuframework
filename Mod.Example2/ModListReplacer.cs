using ModdingFramework;
using ModdingFramework.Data;
using ModdingFramework.Events;
using OFModTest.Game;
using osu.Framework.Logging;

namespace Mod.Example2
{
    // This mod replicates the example of SpinningBoxReplacer but with the
    // Mod List Entry including overwriting an overwritten class and
    // adding more than one overwrite in the same mode,
    // but overwriting an already overwritten class can cause weird issues
    // so its commented out in this example
    public class ModListReplacer : IMod
    {
        private bool enabled = false;

        public ModManifest Manifest => new()
        {
            Name = "A new Mod List",
            Author = "sanco",
            Description = "Redesign to the Mod List, much fresh",
            Version = new Version(1, 0, 0, 0)
        };

        public void OnEnable()
        {
            enabled = true;
            ClassRegistry.OverwriteClass<ModList, AnotherModList>();
            // ClassRegistry.OverwriteClass<ModListEntry, AnotherModListEntry>();
            EventManager.Register<HRUpdateApplicationEvent>(EventRegistryType.GLOBAL, OnHotReload);
        }

        public void OnDisable()
        {
            enabled = false;
            ClassRegistry.OverwriteClass<ModList, ModList>();
            // ClassRegistry.OverwriteClass<ModListEntry, ModListEntry>();
            EventManager.Unregister<HRUpdateApplicationEvent>(EventRegistryType.GLOBAL, OnHotReload);
        }

        // Read SpinningBoxReplacer Hot Reload
        public void OnHotReload(HRUpdateApplicationEvent ev)
        {
            if (enabled && ev.NewTypes.Contains(typeof(AnotherModList)))
            {
                ClassRegistry.OverwriteClass<ModList, AnotherModList>();
                // ClassRegistry.OverwriteClass<ModListEntry, AnotherModListEntry>();
            }

            Logger.Log($"Hot Reload!");
        }
    }
}
