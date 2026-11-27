# Input Manager

Read player input by **action name** ("Move", "Jump", "Interact") instead of reading devices. Any number of sources feed the same actions: the Input System, the legacy Input Manager, on-screen controls, or simulated input from bot tests (`com.fixer33.test-automation`).

```csharp
using Fixer33.InputManager;

if (GameInput.WasPressedThisFrame("Jump")) Jump();
Vector2 move = GameInput.GetVector("Move");
```

## Authors

- [@Fixer33](https://github.com/Fixer33)

## Installation

Install with UPM: Package Manager → **+** → **Install package from git URL...** →
`https://github.com/Fixer33/InputManagerPackage.git`

The Input System source compiles automatically when `com.unity.inputsystem` 1.4 or newer is installed. The legacy source compiles when Active Input Handling includes the old Input Manager.

## Documentation

- [Package Description](Documentation/Package%20Description.md)
- [Usage Guide](Documentation/Usage%20Guide.md)
- [Open Questions](Documentation/Open%20Questions.md)
