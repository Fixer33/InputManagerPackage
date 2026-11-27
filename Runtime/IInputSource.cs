using UnityEngine;

namespace Fixer33.InputManager
{
    /// <summary>
    /// Something that can report the state of named input actions ("Move", "Jump", "Interact"):
    /// the Input System, the legacy Input Manager, on-screen controls, or simulated input from tests.
    /// </summary>
    public interface IInputSource
    {
        /// <summary>Reads a button action. Returns false when this source doesn't know the action.</summary>
        bool TryReadButton(string action, out bool pressed);

        /// <summary>Reads a vector action (x = right, y = forward/up). Returns false when this source doesn't know the action.</summary>
        bool TryReadVector(string action, out Vector2 value);
    }
}
