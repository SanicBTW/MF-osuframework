using System.Diagnostics;
using ModdingFramework;
using ModdingFramework.Data;
using ModdingFramework.Events;
using OFModTest.Game;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using static OFModTest.Game.MainScreen;

namespace ModExample
{
    // This mod shows an example of class overwriting and also handling a hot reload event (in a dirty way) to apply the new changes
    // Including how to listen to events, and targeted events targetting intents
    public class SpinningBoxReplacer : IMod
    {
        private bool enabled = false;

        public ModManifest Manifest => new()
        {
            Name = "Replace SpinningBox",
            Author = "sanco",
            Description = "Replaces the SpinningBox animations",
            Version = new Version(1, 0, 0, 0)
        };

        public void OnEnable()
        {
            enabled = true;
            ClassRegistry.OverwriteClass<SpinningBox, NewSpinningBox>();

            // register a global event for THIS mod to handle the hot reload update application event
            EventManager.Register<HRUpdateApplicationEvent>(EventRegistryType.GLOBAL, OnHotReload);

            // register for the targeted intent event, this event requires the GameIntents.SCREEN_LOAD_COMPLETE intent, when hitting the event on MainScreen, this shouldnt hit at all
            EventManager.Register<TestEvent>(EventRegistryType.TARGETED, OnScreenLoaded, this);
        }

        public void OnDisable()
        {
            enabled = false;
            ClassRegistry.OverwriteClass<SpinningBox, SpinningBox>();

            // unregister the global event
            EventManager.Unregister<HRUpdateApplicationEvent>(EventRegistryType.GLOBAL, OnHotReload);

            // unregister the targeted intent event for cleanup
            EventManager.Unregister<TestEvent>(EventRegistryType.TARGETED, OnScreenLoaded, this);
        }

        // this are some old comments regarding the old API code which passed OnHotReload(HotReloadEvents.<>)
        // The event type of CACHE_CLEAR is used to clear old types from the assembly i believe
        // The event type of UPDATE_APPLICATION is where the changes take place so we apply again the overwritten class to fire an event in the game

        // handle the reload event handler
        public void OnHotReload(HRUpdateApplicationEvent ev)
        {
            // ONLY reload if the new types from the event contain changes from a type that is used inside the mod
            if (enabled && ev.NewTypes.Contains(typeof(NewSpinningBox)))
            {
                Logger.Log("Overwritten class with new class");
                ClassRegistry.OverwriteClass<SpinningBox, NewSpinningBox>();
            }

            Logger.Log($"Hot Reload!");
        }

        public void OnScreenLoaded(TestEvent ev)
        {
            throw new Exception("This shouldn't hit!");
        }
    }
}
