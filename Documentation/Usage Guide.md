# Input Manager - Usage Guide

## Installation
Package Manager → **+** → **Install package from git URL...** → `https://github.com/Fixer33/InputManagerPackage.git` (or through `com.fixer33.package-installer`).

## Setup
Register the sources your game uses once, at startup:

```csharp
using Fixer33.InputManager;
using Fixer33.InputManager.InputSystemSupport; // only with the Input System
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBootstrap : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actions;

    private void Awake()
    {
        GameInput.AddSource(new InputSystemSource(_actions));
    }
}
```

Legacy Input Manager instead:

```csharp
GameInput.AddSource(new LegacyInputSource()
    .MapVector("Move", "Horizontal", "Vertical")
    .MapButton("Jump", "Jump")
    .MapKey("Interact", KeyCode.E));
```

Action names are the names in your Input Actions asset (`"Jump"`, or `"Player/Jump"` to pick a map).

## Reading input

```csharp
Vector2 move = GameInput.GetVector("Move");      // x = right, y = forward
if (GameInput.WasPressedThisFrame("Jump")) Jump();
if (GameInput.IsPressed("Sprint")) Sprint();
if (GameInput.WasReleasedThisFrame("Aim")) StopAiming();
```

Keep action names in one static class of constants in your game to avoid typos.

`GameInput.Enabled = false` blocks all game input (cutscenes, menus that read input their own way).

## Injecting input
On-screen controls and tests write to the virtual source; game code doesn't change:

```csharp
GameInput.Virtual.SetButton("Jump", true);
GameInput.Virtual.SetVector("Move", new Vector2(0, 1));
GameInput.Virtual.ClearAll();
```

`com.fixer33.test-automation` uses this to drive bot tests.

## Custom sources
Implement `IInputSource` and register it with `GameInput.AddSource`. Return false from `TryRead...` for actions the source doesn't handle.

## EditMode tests
Edge detection needs `GameInput.Tick()` once per frame; in Play Mode the package does it. In EditMode tests call `GameInput.Reset()` in `[SetUp]` and `GameInput.Tick()` where a frame would pass.

## Pitfalls
- A button's first press is detected as an edge only if the action was tracked before it was pressed. Reading the action every frame (normal game code) or injecting through `GameInput.Virtual` does that automatically; otherwise call `GameInput.Track("Action")` at startup.
- With Enter Play Mode Options (no domain reload) the package resets itself on play.
