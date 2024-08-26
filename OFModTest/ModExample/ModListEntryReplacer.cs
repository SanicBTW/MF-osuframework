using ModdingFramework;
using ModdingFramework.Data;
using OFModTest.Game;

namespace ModExample
{
    // This mod shows an example of simple class overwriting and the class has some constructor parameters
    // To see how the constructor parameters are declared inside the OverridableClass Attribute, look at OFModTest.Game.ModListEntry
    public class ModListEntryReplacer : IMod
    {
        public ModManifest Manifest => new()
        {
            Name = "Replace ModListEntry",
            Author = "sanco",
            Description = "Replaces the Mod List Entry design",
            Version = new Version(1, 0, 0, 0)
        };

        public void OnEnable()
        {
            ClassRegistry.OverwriteClass<ModListEntry, NewModListEntry>();
        }

        public void OnDisable()
        {
            ClassRegistry.OverwriteClass<ModListEntry, ModListEntry>();
        }
    }
}
