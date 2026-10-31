# Rip It! Card Collector — Sell XP

A BepInEx 6 (IL2CPP) mod for **Rip It! Card Collector** (v1.0.7, released 30/10/2026).

## What it does

Grants **experience points when a card is sold**, based on its rarity — exactly the same
rarity-based XP the game already gives when you place a *new* card into its slot.

Without this mod, selling cards only gives money. With it, every successful sale
(manual sell-slot drop, auto-sold duplicates, Shift+Click sell) also advances your XP bar.

## Requirements

- Game: `Rip It! Card Collector` v1.0.7 (extracted)
- [BepInEx 6 IL2CPP](https://github.com/BepInEx/BepInEx) already installed in the game folder
- .NET SDK 6.0+ to build (or just use the release DLL)

## Installation

1. Copy `RipIt.SellXP.dll` into `BepInEx/plugins/RipIt-SellXP/` inside the game folder.
2. Launch the game. Sell any card and your XP increases.
3. Logs go to `BepInEx/LogOutput.log` (look for lines prefixed `Rip It! Sell XP`).

## Building from source

```powershell
dotnet build -c Release
```

The project references the game's interop assemblies from `BepInEx/interop` and the
BepInEx core assemblies from `BepInEx/core`. If your game is in a different folder,
override it:

```powershell
dotnet build -c Release -p:GameDir="X:\Path\To\Game"
```

The plugin DLL is produced at `bin/Release/net6.0/RipIt.SellXP.dll`.

## How it works (technical notes)

- Harmony **Postfix** on `CurrencyManager.TrySellCard_Internal(CardInfo, out float)` —
  the single choke point every sell path goes through (manual sell slot, auto-sell
  duplicates, mods like Shift Bind).
- Only when the sell actually succeeds (`__result == true`) the mod calls
  `ExperienceManager.AddExperience(CardInfo)`, the game's own rarity-based XP method
  (rarity table + edition bonus + level-up handling).
- Because the patch lives on the lowest common sell method, XP is granted exactly once
  per sale, no matter which path sold the card.

## Compatibility

- Requires BepInEx 6 IL2CPP (net6 plugins).
- Built against game v1.0.7; other versions may need recompiling against their own interop assemblies.

## License

MIT
