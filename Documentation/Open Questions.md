# Input Manager - Open Questions

## Scope and design
1. What exactly should "facilitate mobile and PC inputs" mean: a unified action layer, on-screen touch controls, device-switching detection, rebinding, or something else?
2. Which input device types are in scope within those platforms: touch, mouse, keyboard, gamepad, others?
3. Is it meant to be a static facade, a MonoBehaviour singleton, or a Zenject-installed service like other `com.fixer33.*` packages?

## Dependencies
4. Should it depend on other `com.fixer33.*` packages (for example `core` services, `ui-core` for on-screen controls)?
5. Since both input systems are supported, should the Input System (`com.unity.inputsystem`) be optional via `versionDefines` in the asmdef, or a hard dependency? Nothing is declared now.

## Package setup
6. Should the `description` in `package.json` be improved ("Input Manager package" only repeats the name)?
7. Is the Editor assembly planned to hold something specific (inspector, settings window), or is it just template output?
8. Are a README, CHANGELOG, LICENSE, samples and tests planned? When is it expected to get its first code?

## Answered
- Target platforms: PC = Mac, UWP, Windows, WebDesktop; Mobile = iOS, Android, WebMobile.
- Input systems: both the old (legacy) and the new (Input System) are to be supported.
