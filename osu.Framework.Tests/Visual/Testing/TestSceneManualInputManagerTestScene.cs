// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Framework.Testing;
using osuTK;
using osuTK.Input;

namespace osu.Framework.Tests.Visual.Testing
{
    public class TestSceneManualInputManagerTestScene : ManualInputManagerTestScene
    {
        protected override Vector2 InitialMousePosition => new Vector2(10f);

        [Test]
        public void TestResetInput()
        {
            AddStep("move mouse", () => InputManager.MoveMouseTo(Vector2.Zero));
            AddStep("press mouse", () => InputManager.PressButton(MouseButton.Left));
            AddStep("press key", () => InputManager.PressKey(Key.Z));
            AddStep("press joystick", () => InputManager.PressJoystickButton(JoystickButton.Button1));

            AddStep("reset input", ResetInput);

            AddAssert("mouse position reset", () => InputManager.CurrentState.Mouse.Position == InitialMousePosition);
            AddAssert("all input states released", () =>
                !InputManager.CurrentState.Mouse.Buttons.HasAnyButtonPressed &&
                !InputManager.CurrentState.Keyboard.Keys.HasAnyButtonPressed &&
                !InputManager.CurrentState.Joystick.Buttons.HasAnyButtonPressed);
        }

        [Test]
        public void TestCursorReceivesEventsWhenMaskedAway()
        {
            Drawable cursor = null;

            AddStep("retrieve cursor", () => cursor = InputManager.ChildrenOfType<CursorContainer>().Single().ActiveCursor);

            AddStep("move mouse to screen zero", () => InputManager.MoveMouseTo(Vector2.Zero));
            AddAssert("ensure cursor masked away", () => cursor.IsMaskedAway);
            AddAssert("cursor allows event propagation", () => cursor.PropagatePositionalInputSubTree);
        }

        [Test]
        public void TestMousePositionSetToInitial() => AddAssert("mouse position set to initial", () => InputManager.CurrentState.Mouse.Position == InitialMousePosition);
    }
}
