using UnityEngine.InputSystem;
using InputSystemApi = UnityEngine.InputSystem.InputSystem;

namespace Fixer33.InputManager.InputSystemSupport.Glyphs
{
    /// <summary>Kind of device a binding glyph belongs to. A view maps it to key caps, gamepad icons and so on.</summary>
    public enum InputDeviceKind
    {
        KeyboardMouse,
        Gamepad,
        Touch,
    }

    /// <summary>What to show for an action: the binding's display string and the device kind it belongs to.</summary>
    public readonly struct BindingGlyph
    {
        /// <summary>Display string of the binding, for example "E", "Space" or "Button South".</summary>
        public readonly string DisplayString;

        /// <summary>Device kind of the binding.</summary>
        public readonly InputDeviceKind Device;

        public BindingGlyph(string displayString, InputDeviceKind device)
        {
            DisplayString = displayString;
            Device = device;
        }

        public override string ToString() => $"{DisplayString} ({Device})";
    }

    /// <summary>Resolves the text to show for an input action from its current (possibly rebound) binding.</summary>
    public static class BindingGlyphs
    {
        /// <summary>
        /// Finds the first binding of <paramref name="action"/> for <paramref name="device"/> and returns its display string.
        /// A composite (WASD) counts as one binding. Returns false when the action has no binding for that device.
        /// </summary>
        /// <param name="keyboardMouseOverride">Shown instead of the binding on keyboard and mouse when not empty
        /// (for bindings that read badly, such as mouse delta or the number row).</param>
        public static bool TryResolve(InputAction action, InputDeviceKind device, string keyboardMouseOverride, out BindingGlyph glyph)
        {
            glyph = default;
            if (action == null)
                return false;

            var bindings = action.bindings;
            for (int i = 0; i < bindings.Count; i++)
            {
                InputBinding binding = bindings[i];
                if (binding.isPartOfComposite)
                    continue;

                // A composite has no device of its own; its first part tells which device it is for.
                string path = binding.isComposite && i + 1 < bindings.Count ? bindings[i + 1].effectivePath : binding.effectivePath;
                if (BelongsTo(path, device) == false)
                    continue;

                string display = device == InputDeviceKind.KeyboardMouse && string.IsNullOrEmpty(keyboardMouseOverride) == false
                    ? keyboardMouseOverride
                    : action.GetBindingDisplayString(i, InputBinding.DisplayStringOptions.DontIncludeInteractions);
                if (string.IsNullOrEmpty(display))
                    continue;

                glyph = new BindingGlyph(display, device);
                return true;
            }

            return false;
        }

        /// <summary>Same as the overload with an override, without one.</summary>
        public static bool TryResolve(InputAction action, InputDeviceKind device, out BindingGlyph glyph) =>
            TryResolve(action, device, null, out glyph);

        /// <summary>Resolves for <paramref name="preferred"/> first, then for the other device kinds.</summary>
        public static bool TryResolveAny(InputAction action, InputDeviceKind preferred, out BindingGlyph glyph)
        {
            if (TryResolve(action, preferred, out glyph))
                return true;

            for (var kind = InputDeviceKind.KeyboardMouse; kind <= InputDeviceKind.Touch; kind++)
            {
                if (kind != preferred && TryResolve(action, kind, out glyph))
                    return true;
            }

            return false;
        }

        /// <summary>The device kind of an input control.</summary>
        public static InputDeviceKind KindOf(InputControl control) => KindOf(control?.device);

        /// <summary>The device kind of an input device. Unknown devices count as keyboard and mouse.</summary>
        public static InputDeviceKind KindOf(InputDevice device) => device switch
        {
            Gamepad => InputDeviceKind.Gamepad,
            Touchscreen => InputDeviceKind.Touch,
            _ => InputDeviceKind.KeyboardMouse,
        };

        private static bool BelongsTo(string path, InputDeviceKind device)
        {
            string layout = InputControlPath.TryGetDeviceLayout(path);
            if (string.IsNullOrEmpty(layout))
                return false;

            bool isTouch = InputSystemApi.IsFirstLayoutBasedOnSecond(layout, "Touchscreen");
            return device switch
            {
                InputDeviceKind.Gamepad => InputSystemApi.IsFirstLayoutBasedOnSecond(layout, "Gamepad"),
                InputDeviceKind.Touch => isTouch,
                _ => InputSystemApi.IsFirstLayoutBasedOnSecond(layout, "Keyboard") ||
                     (InputSystemApi.IsFirstLayoutBasedOnSecond(layout, "Pointer") && isTouch == false),
            };
        }
    }
}
