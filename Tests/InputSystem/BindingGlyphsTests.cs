using Fixer33.InputManager.InputSystemSupport.Glyphs;
using NUnit.Framework;
using UnityEngine.InputSystem;

namespace Fixer33.InputManager.Tests.InputSystemSupport
{
    public class BindingGlyphsTests
    {
        private InputAction _action;

        [SetUp]
        public void SetUp()
        {
            _action = new InputAction("Interact");
            _action.AddBinding("<Keyboard>/e");
            _action.AddBinding("<Gamepad>/buttonSouth");
        }

        [TearDown]
        public void TearDown() => _action.Dispose();

        [Test]
        public void TryResolve_PicksBindingOfRequestedDevice()
        {
            Assert.That(BindingGlyphs.TryResolve(_action, InputDeviceKind.KeyboardMouse, out var keyboard), Is.True);
            Assert.That(keyboard.Device, Is.EqualTo(InputDeviceKind.KeyboardMouse));
            // The text follows the current keyboard layout (an "E" key may show as "У"), so compare with Input System's own string.
            Assert.That(keyboard.DisplayString,
                Is.EqualTo(_action.GetBindingDisplayString(0, InputBinding.DisplayStringOptions.DontIncludeInteractions)));

            Assert.That(BindingGlyphs.TryResolve(_action, InputDeviceKind.Gamepad, out var gamepad), Is.True);
            Assert.That(gamepad.Device, Is.EqualTo(InputDeviceKind.Gamepad));
        }

        [Test]
        public void TryResolve_NoBindingForDevice_ReturnsFalse()
        {
            Assert.That(BindingGlyphs.TryResolve(_action, InputDeviceKind.Touch, out _), Is.False);
        }

        [Test]
        public void TryResolve_KeyboardOverrideReplacesText()
        {
            Assert.That(BindingGlyphs.TryResolve(_action, InputDeviceKind.KeyboardMouse, "Mouse", out var glyph), Is.True);
            Assert.That(glyph.DisplayString, Is.EqualTo("Mouse"));
        }

        [Test]
        public void TryResolve_CompositeCountsAsOneBinding()
        {
            using var move = new InputAction("Move");
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");

            Assert.That(BindingGlyphs.TryResolve(move, InputDeviceKind.KeyboardMouse, out var glyph), Is.True);
            Assert.That(glyph.DisplayString,
                Is.EqualTo(move.GetBindingDisplayString(0, InputBinding.DisplayStringOptions.DontIncludeInteractions)),
                "The composite's own display string, not one of its parts");
            Assert.That(glyph.DisplayString, Does.Contain("/"));
        }

        [Test]
        public void ActiveInputDevice_SetRaisesChanged()
        {
            InputDeviceKind? received = null;
            void OnChanged(InputDeviceKind kind) => received = kind;

            ActiveInputDevice.Set(InputDeviceKind.KeyboardMouse);
            ActiveInputDevice.Changed += OnChanged;
            try
            {
                ActiveInputDevice.Set(InputDeviceKind.Gamepad);
                Assert.That(received, Is.EqualTo(InputDeviceKind.Gamepad));
                Assert.That(ActiveInputDevice.Current, Is.EqualTo(InputDeviceKind.Gamepad));
            }
            finally
            {
                ActiveInputDevice.Changed -= OnChanged;
                ActiveInputDevice.Set(InputDeviceKind.KeyboardMouse);
            }
        }
    }
}
