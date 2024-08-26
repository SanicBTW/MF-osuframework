using ModdingFramework;
using ModdingFramework.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace OFModTest.Game
{
    public partial class MainScreen : Screen
    {
        public class TestEvent : TargetedEvent
        {
            public readonly Screen Screen;

            public TestEvent(Screen screen) : base(null)
            {
                Screen = screen;
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // this is how you get instances of possible overwritten classes, if the class didnt get overwritten
            // then it returns the default class instance 

            SpinningBox box = ClassRegistry.CreateInstance<SpinningBox>();
            box.Anchor = Anchor.Centre;

            ModList modList = ClassRegistry.CreateInstance<ModList>();
            modList.Margin = new MarginPadding(15);
            modList.Size = new osuTK.Vector2(640, 640);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Violet,
                    RelativeSizeAxes = Axes.Both,
                },
                new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                box,
                modList
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // since we dont have the mod / cant access the mod, we fire an event to the intent
            // if the mod is known / has access to the mod, you should totally use TriggerTargetedEvent passing the mod
            // you would think that this is like a global event but not really, it only dispatches to the target intent
            // if a mod doesnt have the intent added, it wont dispatch the event
            EventManager.TriggerEventByIntent(new TestEvent(this), GameIntents.SCREEN_LOAD_COMPLETE);

            // listen for class overwrites
            ClassRegistry.ClassOverwritten += classRegistry_ClassOverwritten;
        }

        private void classRegistry_ClassOverwritten(ModdingFramework.Data.ClassRegistryItem obj)
        {
            ClassRegistry.ClassOverwritten -= classRegistry_ClassOverwritten;

            // refresh the current screen to accept the new classes
            Scheduler.AddOnce(() =>
            {
                ((ScreenStack)Parent).Exit();
                ((ScreenStack)Parent).Push(new MainScreen());
            });
        }
    }
}
