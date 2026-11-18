# Input Manager (com.fixer33.input-manager) - Package Description

## Purpose (planned)
The package is planned to facilitate working with mobile and PC inputs together (confirmed by the author). No behaviour beyond the points below has been specified or implemented.

Confirmed by the author:
- Target platforms, PC: Mac, UWP, Windows, WebDesktop.
- Target platforms, Mobile: iOS, Android, WebMobile.
- It should support both the old (legacy Input Manager) and the new (Input System) input systems.

## Who uses it
Intended consumers are the author's own Unity projects (personal and work) (assumed, in line with the other `com.fixer33.*` packages). Nothing consumes it yet, since there is no API.

## Current state
Empty scaffold, version 1.0.0.
- `package.json`: name `com.fixer33.input-manager`, display name "Input Manager", description "Input Manager package", Unity `6000.0`, author Fixer33.
- No C# source files, no samples, no tests, no README, no CHANGELOG, no license file.
- There is therefore no public API to document.

## Assemblies
| Asmdef | Name | Root namespace | Notes |
|---|---|---|---|
| `Runtime/com.fixer33.input-manager.asmdef` | `com.fixer33.input-manager` | `Fixer33.InputManager` | All platforms, auto-referenced, no references, no define constraints, no version defines. |
| `Editor/com.fixer33.input-manager.Editor.asmdef` | `com.fixer33.input-manager.Editor` | `Fixer33.InputManager.Editor` | Editor platform only, references the runtime assembly. |

## Dependencies
- `package.json` declares none (no `dependencies` block).
- Neither asmdef references any other assembly, except that the Editor asmdef references the runtime one.
- Notably, the Unity Input System package (`com.unity.inputsystem`) is not declared, although both input systems are to be supported. How the new system is handled (optional via `versionDefines` or required) is open.

## What is missing
- All code: input abstraction, mobile (touch) and PC (keyboard, mouse, gamepad) handling (assumed scope).
- Implementation for both the legacy and the new input system.
- Dependency or version defines for the Input System.
- Documentation beyond this folder (README, CHANGELOG), samples and tests.
- A meaningful package description (the current one only repeats the name).
