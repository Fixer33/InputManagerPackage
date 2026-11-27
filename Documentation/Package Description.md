# Input Manager (com.fixer33.input-manager) - Package Description

## Purpose
Facilitate working with mobile and PC inputs together (confirmed by the author) by giving game code one place to read input from, by action name. Sources for PC devices, mobile controls and simulated input all feed the same actions, so game code never depends on which device or input system produced them.

Confirmed by the author:
- Target platforms, PC: Mac, UWP, Windows, WebDesktop.
- Target platforms, Mobile: iOS, Android, WebMobile.
- It should support both the old (legacy Input Manager) and the new (Input System) input systems.

## Who uses it
The author's Unity games, and `com.fixer33.test-automation`, whose bot tests press actions through `GameInput.Virtual`.

## Architecture
- `GameInput` (static): the facade game code reads. Combines all registered sources: a button is pressed when any source reports it pressed; a vector is the strongest value any source reports. Tracks buttons for pressed/released edges, sampled once per frame by a hidden updater (`[GameInput]`, execution order -10000) created before the first scene loads.
- `IInputSource`: `TryReadButton(action, out bool)`, `TryReadVector(action, out Vector2)`. Returning false means "this source doesn't know the action".
- `VirtualInputSource`: input set from code; always registered as `GameInput.Virtual`.
- `LegacyInputSource`: maps actions to legacy buttons, keys and axes. Compiled only with `ENABLE_LEGACY_INPUT_MANAGER`.
- `InputSystemSource` (namespace `Fixer33.InputManager.InputSystemSupport`): reads actions from an `InputActionAsset` by name.

## Public API
| Type | Members |
|---|---|
| `GameInput` | `IsPressed`, `WasPressedThisFrame`, `WasReleasedThisFrame`, `GetVector`, `Enabled`, `Virtual`, `Sources`, `AddSource`, `RemoveSource`, `Track`, `Tick`, `Reset` |
| `VirtualInputSource` | `SetButton`, `SetVector`, `Clear`, `ClearAll` |
| `LegacyInputSource` | `MapButton`, `MapKey`, `MapVector` (fluent) |
| `InputSystemSource` | constructor `(InputActionAsset, bool enableAsset = true)`, `Dispose` |

## Assemblies
| Asmdef | Name | Root namespace | Notes |
|---|---|---|---|
| `Runtime/com.fixer33.input-manager.asmdef` | `com.fixer33.input-manager` | `Fixer33.InputManager` | No dependencies. |
| `InputSystem/com.fixer33.input-manager.InputSystem.asmdef` | `com.fixer33.input-manager.InputSystem` | `Fixer33.InputManager.InputSystemSupport` | Compiled only when `com.unity.inputsystem` ≥ 1.4.0 is installed (version define `FIXER33_INPUT_SYSTEM`). |
| `Editor/com.fixer33.input-manager.Editor.asmdef` | `com.fixer33.input-manager.Editor` | `Fixer33.InputManager.Editor` | Empty, reserved for editor tools. |
| `Tests/Editor/...Editor.Tests.asmdef` | `com.fixer33.input-manager.Editor.Tests` | `Fixer33.InputManager.Tests` | EditMode tests. |

## Dependencies
None required. `com.unity.inputsystem` is optional.

## Current state
Core action layer, virtual, legacy and Input System sources implemented with EditMode tests. Not yet implemented: touch / on-screen controls, device-switch detection, rebinding (see Open Questions).
