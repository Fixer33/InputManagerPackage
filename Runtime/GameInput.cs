using System.Collections.Generic;
using UnityEngine;

namespace Fixer33.InputManager
{
    /// <summary>
    /// Single place game code reads input from, by action name. Combines every registered
    /// <see cref="IInputSource"/>: a button is pressed when any source reports it pressed, and a
    /// vector is the strongest value any source reports. <see cref="Virtual"/> is always a source,
    /// so tests and on-screen controls can inject input exactly where player input arrives.
    /// </summary>
    public static class GameInput
    {
        private sealed class ButtonState
        {
            public bool Current;
            public bool Previous;
        }

        private static readonly List<IInputSource> _sources = new();
        private static readonly Dictionary<string, ButtonState> _buttons = new();
        private static VirtualInputSource _virtual;

        /// <summary>When false, every read returns released / zero. Use for cutscenes or menus.</summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>Input injected by code (tests, on-screen controls). Always registered.</summary>
        public static VirtualInputSource Virtual
        {
            get
            {
                if (_virtual == null)
                {
                    _virtual = new VirtualInputSource();
                    _sources.Insert(0, _virtual);
                }
                return _virtual;
            }
        }

        public static IReadOnlyList<IInputSource> Sources
        {
            get
            {
                _ = Virtual;
                return _sources;
            }
        }

        public static void AddSource(IInputSource source)
        {
            _ = Virtual;
            if (source != null && !_sources.Contains(source))
                _sources.Add(source);
        }

        public static void RemoveSource(IInputSource source)
        {
            if (source != null && source != _virtual)
                _sources.Remove(source);
        }

        /// <summary>Starts tracking an action so its pressed/released edges are detected from the next frame.</summary>
        public static void Track(string action) => GetState(action);

        public static bool IsPressed(string action)
        {
            GetState(action);
            return Enabled && ReadButton(action);
        }

        public static bool WasPressedThisFrame(string action)
        {
            var state = GetState(action);
            return Enabled && state.Current && !state.Previous;
        }

        public static bool WasReleasedThisFrame(string action)
        {
            var state = GetState(action);
            return Enabled && !state.Current && state.Previous;
        }

        public static Vector2 GetVector(string action)
        {
            if (!Enabled) return Vector2.zero;
            _ = Virtual;
            var best = Vector2.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i].TryReadVector(action, out var value) && value.sqrMagnitude > best.sqrMagnitude)
                    best = value;
            }
            return best;
        }

        /// <summary>
        /// Samples tracked buttons for edge detection. Called once per frame before other scripts
        /// by the package's updater; call it yourself only in EditMode tests.
        /// </summary>
        public static void Tick()
        {
            foreach (var pair in _buttons)
            {
                pair.Value.Previous = pair.Value.Current;
                pair.Value.Current = ReadButton(pair.Key);
            }
        }

        /// <summary>Removes all sources except <see cref="Virtual"/>, clears injected input and tracked actions.</summary>
        public static void Reset()
        {
            _sources.Clear();
            _buttons.Clear();
            _virtual = null;
            Enabled = true;
        }

        private static ButtonState GetState(string action)
        {
            if (!_buttons.TryGetValue(action, out var state))
            {
                bool now = ReadButton(action);
                state = new ButtonState { Current = now, Previous = now };
                _buttons[action] = state;
            }
            return state;
        }

        private static bool ReadButton(string action)
        {
            _ = Virtual;
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i].TryReadButton(action, out bool pressed) && pressed)
                    return true;
            }
            return false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnPlay() => Reset();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateUpdater() => GameInputUpdater.Create();
    }
}
