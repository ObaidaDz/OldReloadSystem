# Server Notes

Old Reload System does not need any player commands.

The plugin listens for:

- player spawn
- weapon purchase
- active weapon ammo changes

On spawn it gives every supported weapon its full reserve pool in memory. On purchase it refreshes the bought weapon. During gameplay it watches reloads and keeps reserve ammo correct.

If you replace the DLL while the server is online:

```text
css_plugins unload OldReloadSystem
css_plugins load OldReloadSystem
```

If players are already mid-round, a respawn or rebuy will refresh the clean ammo state.
