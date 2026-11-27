using System.Collections.Generic;
using UnityEngine;

namespace Fixer33.InputManager
{
    /// <summary>
    /// Input set from code: bot tests, on-screen buttons and sticks, replays.
    /// Reached through <see cref="GameInput.Virtual"/>.
    /// </summary>
    public sealed class VirtualInputSource : IInputSource
    {
        private readonly Dictionary<string, bool> _buttons = new();
        private readonly Dictionary<string, Vector2> _vectors = new();

        public void SetButton(string action, bool pressed)
        {
            GameInput.Track(action); // track before changing, so the first injected press is seen as an edge
            _buttons[action] = pressed;
        }

        public void SetVector(string action, Vector2 value) => _vectors[action] = Vector2.ClampMagnitude(value, 1f);

        public void Clear(string action)
        {
            _buttons.Remove(action);
            _vectors.Remove(action);
        }

        public void ClearAll()
        {
            _buttons.Clear();
            _vectors.Clear();
        }

        public bool TryReadButton(string action, out bool pressed) => _buttons.TryGetValue(action, out pressed);

        public bool TryReadVector(string action, out Vector2 value) => _vectors.TryGetValue(action, out value);
    }
}
