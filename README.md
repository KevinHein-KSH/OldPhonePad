# OldPhonePad

Small .NET library that translates “multi-tap” keypad input (old mobile phones) into text.

## Behavior

- Keys map like a classic phone keypad:
  - `2` → `ABC`, `3` → `DEF`, `4` → `GHI`, `5` → `JKL`, `6` → `MNO`, `7` → `PQRS`, `8` → `TUV`, `9` → `WXYZ`, `0` → space
- Repeating a key cycles through its letters (e.g. `2222` on key `2` becomes `A`).
- A space (` `) in the input acts as a “pause” and commits the currently-typed character. This lets you type consecutive letters from the same key (e.g. `2 2#` → `AA`).
- `*` is backspace:
  - If a character is being composed, it is first committed, then the last output character is removed.
  - Extra backspaces on an empty output are ignored.
- A trailing `#` is treated as an optional terminator and ignored.
- Output is returned in uppercase.
- Unsupported characters throw `ArgumentException`.

## Examples

- `8 88777444666664#` → `TURING`
- `4433555 555666#` → `HELLO`
- `227*#` → `B`
- `999337777#` → `YES`

## Usage

This repo is a class library + tests.

## Build & Test

Requires .NET 8 SDK.

```bash
dotnet build
dotnet test
```

## Repo Structure

- `OldPhonePad/` — class library (`OldPhonePadTranslator`)
- `OldPhonePad.Tests/` — xUnit test project

## AI Usage Disclosure

I used an AI assistant as a reasoning aid to discuss ambiguous requirements, validate design decisions, and review edge cases.
All code, structure, and final implementation decisions were written and understood by me.

Tools used: ChatGPT, Codex, and Visual Studio Code.
