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
