# Input Manager - Open Questions

## Scope and design
1. Touch and on-screen controls for mobile: build them into this package (on-screen stick and buttons writing to `GameInput.Virtual`) or as a separate UI package on top of `com.fixer33.ui-core`?
2. Device-switch detection (show keyboard vs gamepad vs touch prompts): needed, and where?
3. Rebinding: in scope for this package, or left to the Input System's own rebinding UI?

## Dependencies
4. Should it depend on other `com.fixer33.*` packages (for example a Zenject installer through `core`)? Currently it depends on nothing.

## Package setup
5. Is the Editor assembly planned to hold something specific (inspector, settings window, debug view of current action values)?
6. Are samples planned?

## Answered
- Target platforms: PC = Mac, UWP, Windows, WebDesktop; Mobile = iOS, Android, WebMobile.
- Input systems: both the old (legacy) and the new (Input System) are to be supported.
- "Facilitate mobile and PC inputs" = a unified action layer that any device source feeds (decided while building 1.1.0; the author allowed shaping the package).
- Static facade vs service: static facade `GameInput` (works without Zenject; a Zenject wrapper can be added later).
- Input System dependency: optional, through `versionDefines` in a separate assembly.
- Package description improved in 1.1.0.
