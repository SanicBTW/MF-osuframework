using ModdingFramework;
using ModdingFramework.Data;
using OFModTest.Game;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using System.Diagnostics;
using static OFModTest.Game.MainScreen;

namespace Mod.Example2
{
    // This mod shows an example of how the Intents work and the purpose of them, also manipulating the ScreenStack in a dirty way
    // NOTE: The screen stack manipulation should be done way better when plugging the framework, currently if the mod starts disabled, the reset screen function wont work at all
    // Look at MainScreen for the event dispatching and an example of a targeted event
    // Look SpinningBoxReplacer to see the behaviour of a targeted event being dispatched without meeting the intents
    public class ScreenFadeMod : IMod
    {
        private Screen lastScreen = null!;
        private Type screen = null!;
        private ScreenStack screenStack = null!;

        public ModManifest Manifest => new()
        {
            Name = "Fade Screen On Load",
            Author = "sanco",
            Description = "Fades the loaded screen to 0.25",
            Intents = [GameIntents.SCREEN_LOAD_COMPLETE],  // add the intent to let the event manager we are listening to that type of events
            Version = new Version(1, 0),
        };

        public void OnEnable()
        {
            // register for the targeted intent event, the intent is required to get the type of events
            EventManager.Register<TestEvent>(EventRegistryType.TARGETED, OnScreenLoaded, this);
            resetScreen();
        }

        public void OnDisable()
        {
            // unregister the targeted intent event
            EventManager.Unregister<TestEvent>(EventRegistryType.TARGETED, OnScreenLoaded, this);
            resetScreen();
        }

        public void OnScreenLoaded(TestEvent ev)
        {
            Debug.WriteLine("A screen finished loading!");

            lastScreen = ev.Screen;
            lastScreen.FadeTo(0.25f, 1000D);

            screen ??= lastScreen.GetType();
            screenStack ??= (ScreenStack)lastScreen.Parent;
        }

        private void resetScreen()
        {
            screenStack?.Exit();
            screenStack?.Push(Activator.CreateInstance(screen) as IScreen);
        }
    }
}
