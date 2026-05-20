# Old Reload System

CounterStrikeSharp plugin for CS2 servers.

This is the small reload behavior plugin used on Ras El Hanout DZ. It keeps the package simple: copy the plugin folder, load it, play.

Source is included under `src/OldReloadSystem`.

## Install

Copy the `addons` folder into your server `csgo` directory:

```text
game/csgo/addons/counterstrikesharp/plugins/OldReloadSystem/
```

Then reload CounterStrikeSharp plugins or restart the server.

Console load command:

```text
css_plugins load OldReloadSystem
```

## Files

```text
addons/
  counterstrikesharp/
    plugins/
      OldReloadSystem/
        OldReloadSystem.dll
        OldReloadSystem.deps.json
src/
  OldReloadSystem/
    OldReloadPlugin.cs
    AmmoState.cs
    OldReloadSystem.csproj
```

## Build

```bash
dotnet build src/OldReloadSystem/OldReloadSystem.csproj -c Release
```

## Notes

- Built for CounterStrikeSharp.
- No database setup.
- No server-specific config included.

Made for Ras El Hanout DZ by Maximus.
