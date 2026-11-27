#if ENABLE_LEGACY_INPUT_MANAGER
using System.Collections.Generic;
using UnityEngine;

namespace Fixer33.InputManager
{
    /// <summary>
    /// Reads actions from the legacy Input Manager (Project Settings > Input Manager).
    /// Only compiled when Active Input Handling includes the legacy system.
    /// </summary>
    public sealed class LegacyInputSource : IInputSource
    {
        private readonly Dictionary<string, string> _buttonNames = new();
        private readonly Dictionary<string, List<KeyCode>> _keys = new();
        private readonly Dictionary<string, (string x, string y)> _axes = new();

        /// <summary>Maps an action to a legacy button name, e.g. MapButton("Jump", "Jump").</summary>
        public LegacyInputSource MapButton(string action, string legacyButton)
        {
            _buttonNames[action] = legacyButton;
            return this;
        }

        /// <summary>Maps an action to a key; several keys per action are allowed.</summary>
        public LegacyInputSource MapKey(string action, KeyCode key)
        {
            if (!_keys.TryGetValue(action, out var list)) _keys[action] = list = new List<KeyCode>();
            list.Add(key);
            return this;
        }

        /// <summary>Maps a vector action to two legacy axes, e.g. MapVector("Move", "Horizontal", "Vertical").</summary>
        public LegacyInputSource MapVector(string action, string xAxis, string yAxis)
        {
            _axes[action] = (xAxis, yAxis);
            return this;
        }

        public bool TryReadButton(string action, out bool pressed)
        {
            pressed = false;
            bool known = false;
            if (_buttonNames.TryGetValue(action, out var name))
            {
                known = true;
                pressed |= Input.GetButton(name);
            }
            if (_keys.TryGetValue(action, out var keys))
            {
                known = true;
                foreach (var key in keys) pressed |= Input.GetKey(key);
            }
            return known;
        }

        public bool TryReadVector(string action, out Vector2 value)
        {
            if (_axes.TryGetValue(action, out var axes))
            {
                value = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw(axes.x), Input.GetAxisRaw(axes.y)), 1f);
                return true;
            }
            value = Vector2.zero;
            return false;
        }
    }
}
#endif
