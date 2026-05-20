# Ammo Pools

The ammo values are hardcoded in `GetFullAmmo()`.

That is on purpose. This plugin is meant to be dropped into a server and left alone.

To change a value:

1. Open `src/OldReloadSystem/OldReloadPlugin.cs`.
2. Edit the weapon entry in `GetFullAmmo()`.
3. Build the project.
4. Replace the DLL on the server.
5. Reload the plugin.

Example:

```csharp
{ "weapon_ak47", 90 }
```

The plugin only manages weapons listed there. Unknown weapons are ignored.
