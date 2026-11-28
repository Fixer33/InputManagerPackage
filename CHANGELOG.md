## [1.2.0] - 2026-11-28

### Added
- Binding glyphs (Input System assembly, namespace `Fixer33.InputManager.InputSystemSupport.Glyphs`): `BindingGlyphs.TryResolve` gives the display string of an action's current binding for keyboard and mouse, gamepad or touch (composites count as one binding, optional keyboard text override); `TryResolveAny` falls back to other devices; `KindOf` maps a control or device to `InputDeviceKind`.
- `ActiveInputDevice`: the device kind the player pressed last, with a `Changed` event, for switching hints between key caps and gamepad icons.
- EditMode tests for both (`Tests/InputSystem`).

## [1.1.0] - 2026-11-28

### Added
- `GameInput`: static facade to read actions by name (`IsPressed`, `WasPressedThisFrame`, `WasReleasedThisFrame`, `GetVector`) from all registered sources, with `Enabled` and per-frame edge detection.
- `IInputSource` interface for input sources.
- `VirtualInputSource` (`GameInput.Virtual`): input set from code, for bot tests and on-screen controls.
- `LegacyInputSource`: legacy Input Manager buttons, keys and axes (compiled only when legacy input is enabled).
- `InputSystemSource` in the optional `com.fixer33.input-manager.InputSystem` assembly (compiled only when `com.unity.inputsystem` 1.4+ is installed).
- EditMode tests, README, Usage Guide.

## [1.0.1] - 2026-11-20

### Added
- `Documentation/Package Description.md` describing the planned purpose, target platforms, assemblies and current state of the package.
- `Documentation/Open Questions.md` listing unresolved scope, dependency and packaging questions.
