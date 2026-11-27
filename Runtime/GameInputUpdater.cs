using UnityEngine;

namespace Fixer33.InputManager
{
    /// <summary>Calls <see cref="GameInput.Tick"/> once per frame, before other scripts.</summary>
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-10000)]
    internal sealed class GameInputUpdater : MonoBehaviour
    {
        private static GameInputUpdater _instance;

        internal static void Create()
        {
            if (_instance) return;
            var go = new GameObject("[GameInput]") { hideFlags = HideFlags.HideInHierarchy };
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<GameInputUpdater>();
        }

        private void Update() => GameInput.Tick();
    }
}
