using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace OldReloadSystem;

public sealed class OldReloadPlugin : BasePlugin
{
    private readonly Dictionary<ulong, Dictionary<string, AmmoState>> _playerAmmo = new();
    private bool _loaded;

    public override string ModuleName => "OLD REALOAD IS BACK BABY WIIIIW";
    public override string ModuleVersion => "4.9.1";
    public override string ModuleAuthor => "Maximus";

    public override void Load(bool hotReload)
    {
        _loaded = true;
        RegisterListener<Listeners.OnTick>(OnTick);
    }

    public override void Unload(bool hotReload)
    {
        _loaded = false;
    }

    private void OnTick()
    {
        if (!_loaded)
        {
            return;
        }

        CCSPlayerController[] players;
        try
        {
            players = Utilities.GetPlayers().ToArray();
        }
        catch
        {
            return;
        }

        foreach (var player in players)
        {
            try
            {
                if (player == null || !player.IsValid || player.PlayerPawn?.Value == null)
                {
                    continue;
                }

                var weapon = player.PlayerPawn.Value.WeaponServices?.ActiveWeapon.Value;
                if (weapon == null || !weapon.IsValid)
                {
                    continue;
                }

                var weaponName = weapon.DesignerName ?? string.Empty;
                if (!_playerAmmo.ContainsKey(player.SteamID))
                {
                    InitPlayer(player.SteamID);
                }

                var ammo = _playerAmmo[player.SteamID];
                if (!ammo.TryGetValue(weaponName, out var state) || state == null)
                {
                    continue;
                }

                var clip = weapon.Clip1;
                if (!state.IsInitialized)
                {
                    state.LastClip = clip;
                    state.IsInitialized = true;
                }

                if (clip > state.LastClip)
                {
                    var reloaded = clip - state.LastClip;
                    if (reloaded > state.Pool)
                    {
                        reloaded = state.Pool;
                        weapon.Clip1 = state.LastClip + reloaded;
                        Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
                        clip = weapon.Clip1;
                    }

                    state.Pool -= reloaded;
                }

                state.LastClip = clip;

                if (weapon.ReserveAmmo[0] != state.Pool)
                {
                    weapon.ReserveAmmo[0] = state.Pool;
                    Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_pReserveAmmo");
                }
            }
            catch
            {
            }
        }
    }

    [GameEventHandler(HookMode.Post)]
    public HookResult OnItemPurchase(EventItemPurchase @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !player.IsValid)
        {
            return HookResult.Continue;
        }

        var rawWeaponName = @event.Weapon ?? string.Empty;
        var weaponName = rawWeaponName.Contains("weapon_") ? rawWeaponName : $"weapon_{rawWeaponName}";

        if (!_playerAmmo.ContainsKey(player.SteamID))
        {
            InitPlayer(player.SteamID);
        }

        var fullAmmo = GetFullAmmo();
        if (!fullAmmo.TryGetValue(weaponName, out var pool))
        {
            return HookResult.Continue;
        }

        _playerAmmo[player.SteamID][weaponName].Pool = pool;
        _playerAmmo[player.SteamID][weaponName].IsInitialized = false;

        var activeWeapon = player.PlayerPawn?.Value?.WeaponServices?.ActiveWeapon.Value;
        var activeWeaponName = activeWeapon?.DesignerName ?? string.Empty;
        if (activeWeapon != null && activeWeaponName == weaponName)
        {
            activeWeapon.ReserveAmmo[0] = pool;
            Utilities.SetStateChanged(activeWeapon, "CBasePlayerWeapon", "m_pReserveAmmo");
        }

        return HookResult.Continue;
    }

    [GameEventHandler(HookMode.Post)]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if (@event.Userid != null)
        {
            InitPlayer(@event.Userid.SteamID);
        }

        return HookResult.Continue;
    }

    private void InitPlayer(ulong steamId)
    {
        var ammo = new Dictionary<string, AmmoState>();
        foreach (var item in GetFullAmmo())
        {
            ammo[item.Key] = new AmmoState
            {
                Pool = item.Value,
                LastClip = -1,
                IsInitialized = false
            };
        }

        _playerAmmo[steamId] = ammo;
    }

    private static Dictionary<string, int> GetFullAmmo()
    {
        return new Dictionary<string, int>
        {
            { "weapon_ak47", 90 },
            { "weapon_m4a1", 90 },
            { "weapon_m4a1_silencer", 80 },
            { "weapon_usp_silencer", 24 },
            { "weapon_hkp2000", 24 },
            { "weapon_glock", 120 },
            { "weapon_p250", 26 },
            { "weapon_deagle", 35 },
            { "weapon_revolver", 53 },
            { "weapon_fiveseven", 100 },
            { "weapon_tec9", 90 },
            { "weapon_cz75a", 12 },
            { "weapon_elite", 120 },
            { "weapon_awp", 10 },
            { "weapon_ssg08", 90 },
            { "weapon_scar20", 90 },
            { "weapon_g3sg1", 90 },
            { "weapon_galilar", 105 },
            { "weapon_famas", 90 },
            { "weapon_aug", 90 },
            { "weapon_sg556", 90 },
            { "weapon_mp9", 120 },
            { "weapon_mac10", 100 },
            { "weapon_mp7", 120 },
            { "weapon_mp5sd", 120 },
            { "weapon_ump45", 100 },
            { "weapon_p90", 100 },
            { "weapon_bizon", 120 },
            { "weapon_nova", 32 },
            { "weapon_xm1014", 32 },
            { "weapon_mag7", 32 },
            { "weapon_sawedoff", 32 },
            { "weapon_m249", 200 },
            { "weapon_negev", 300 }
        };
    }
}
