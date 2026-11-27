using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fixer33.InputManager.InputSystemSupport
{
    /// <summary>
    /// Reads actions from an Input Actions asset by action name ("Move", "Jump" or "Player/Jump").
    /// Only compiled when com.unity.inputsystem is installed.
    /// </summary>
    public sealed class InputSystemSource : IInputSource, IDisposable
    {
        private readonly InputActionAsset _asset;
        private readonly Dictionary<string, InputAction> _cache = new();

        public InputSystemSource(InputActionAsset asset, bool enableAsset = true)
        {
            _asset = asset ? asset : throw new ArgumentNullException(nameof(asset));
            if (enableAsset) _asset.Enable();
        }

        public bool TryReadButton(string action, out bool pressed)
        {
            var inputAction = Find(action);
            pressed = inputAction != null && inputAction.IsPressed();
            return inputAction != null;
        }

        public bool TryReadVector(string action, out Vector2 value)
        {
            var inputAction = Find(action);
            if (inputAction == null)
            {
                value = Vector2.zero;
                return false;
            }

            value = IsVector(inputAction) ? inputAction.ReadValue<Vector2>() : new Vector2(inputAction.ReadValue<float>(), 0f);
            return true;
        }

        public void Dispose() => _asset.Disable();

        private InputAction Find(string action)
        {
            if (!_cache.TryGetValue(action, out var inputAction))
            {
                inputAction = _asset.FindAction(action, false);
                _cache[action] = inputAction;
            }
            return inputAction;
        }

        private static bool IsVector(InputAction action)
        {
            var type = action.expectedControlType;
            return type == "Vector2" || type == "Stick" || type == "Dpad";
        }
    }
}
