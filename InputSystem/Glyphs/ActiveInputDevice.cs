using System;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using InputSystemApi = UnityEngine.InputSystem.InputSystem;

namespace Fixer33.InputManager.InputSystemSupport.Glyphs
{
    /// <summary>
    /// Follows the device kind the player used last (any button press), so hints can show key caps or gamepad icons.
    /// Starts listening on first use and keeps listening for the rest of the session.
    /// </summary>
    public static class ActiveInputDevice
    {
        private static InputDeviceKind _current = InputDeviceKind.KeyboardMouse;
        private static IDisposable _subscription;
        private static Action<InputDeviceKind> _changed;

        /// <summary>Device kind of the last button press (keyboard and mouse until anything is pressed).</summary>
        public static InputDeviceKind Current
        {
            get
            {
                EnsureListening();
                return _current;
            }
        }

        /// <summary>Raised when the player switches to another device kind.</summary>
        public static event Action<InputDeviceKind> Changed
        {
            add
            {
                EnsureListening();
                _changed += value;
            }
            remove => _changed -= value;
        }

        /// <summary>Sets the current kind by hand (tests, or a game that detects devices another way).</summary>
        public static void Set(InputDeviceKind kind)
        {
            if (kind == _current)
                return;

            _current = kind;
            _changed?.Invoke(kind);
        }

        private static void EnsureListening()
        {
            if (_subscription != null)
                return;

            _subscription = InputSystemApi.onAnyButtonPress.Call(control => Set(BindingGlyphs.KindOf(control)));
        }

        // Domain reload may be disabled in the Editor: reset static state when entering Play Mode.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _subscription?.Dispose();
            _subscription = null;
            _changed = null;
            _current = InputDeviceKind.KeyboardMouse;
        }
    }
}
