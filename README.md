# Old Reload System

Small CounterStrikeSharp plugin for CS2 servers.

The newer CS2 reload/ammo behavior can feel strange on community servers that want the older reserve-ammo rhythm. This plugin keeps a server-side ammo pool per player and weapon, then keeps reserve ammo synced so reloads feel closer to the old style.

It is intentionally simple: no config, no database, no chat spam, no menu. Load it and play.

## Latest

`v4.9.1` is a small stability release:

- Added a clean loaded/unloaded guard for the tick listener.
- Avoids touching player/entity state while CS2 is shutting the entity system down.
- Skips bad player/weapon states instead of spamming the server console.
- Keeps the public package the same: copy `addons`, load the plugin, play.

## What It Does

- Tracks ammo per player by SteamID.
- Tracks each supported weapon separately.
- Restores full reserve ammo on spawn.
- Restores full reserve ammo when a player buys a supported weapon.
- Watches active weapon ammo every tick.
- When the clip goes up after a reload, it subtracts that amount from the saved reserve pool.
- If the reload tries to take more bullets than the saved pool has, it clamps the clip and fixes the weapon state.
- Keeps `m_pReserveAmmo` synced for the client.
- Does not use a database.
- Does not add player commands.

## Install

Download the release zip and copy the `addons` folder into your server `csgo` folder.

Final path:

```text
csgo/addons/counterstrikesharp/plugins/OldReloadSystem/
```

Load it:

```text
css_plugins load OldReloadSystem
```

Unload it:

```text
css_plugins unload OldReloadSystem
```

## Files

The repo has:

- `addons` - ready server package.
- `src` - source project.

The release zip only needs the `addons` folder.

## Supported Weapons

Ammo pools are built in for the normal CS2 weapon list:

```text
ak47, m4a1, m4a1_silencer, usp_silencer, hkp2000, glock,
p250, deagle, revolver, fiveseven, tec9, cz75a, elite,
awp, ssg08, scar20, g3sg1, galilar, famas, aug, sg556,
mp9, mac10, mp7, mp5sd, ump45, p90, bizon,
nova, xm1014, mag7, sawedoff, m249, negev
```

If a weapon is not in the list, the plugin leaves it alone.

## Build

Requirements:

- .NET 8 SDK
- CounterStrikeSharp API package

Build:

```bash
dotnet build src/OldReloadSystem/OldReloadSystem.csproj -c Release
```

Output:

```text
src/OldReloadSystem/bin/Release/net8.0/OldReloadSystem.dll
```

## Server Notes

This plugin works best as a quiet gameplay fix. It does not need a config because the ammo values are already in the source. If you want custom reserve values, edit `GetFullAmmo()` in `OldReloadPlugin.cs`, rebuild, and replace the DLL.

After replacing the DLL on a running server:

```text
css_plugins unload OldReloadSystem
css_plugins load OldReloadSystem
```

## Why It Exists

Some fixes are better when they stay small. I wanted old reload behavior back without adding another panel, command system, database table, or config file. This is just the reload patch, kept clean.

Maximus
