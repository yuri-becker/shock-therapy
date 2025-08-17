using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ShockTherapy.Config;
using ShockTherapy.PiShock;

namespace ShockTherapy;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ShockTherapy : BaseUnityPlugin
{
    private new static readonly ManualLogSource Logger = new(typeof(ShockTherapy).FullName);

    // Static references unfortunately needed since Harmony patches are static.
    private static ShockTherapyConfig? _config;
    private static Client? _client;

    private void Awake()
    {
        BepInEx.Logging.Logger.Sources.Add(Logger);

        _config = new ShockTherapyConfig(Config);
        _client = new Client(_config);
        Harmony.CreateAndPatchAll(typeof(ShockPatch));
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnApplicationQuit()
    {
        _client?.Dispose();
        _client = null;
    }

    class ShockPatch
    {
        [HarmonyPatch(typeof(scnGame), nameof(scnGame.AddHitOffset))]
        [HarmonyPostfix]
        static void LogHit(ref int rowID, OffsetType offsetType)
        {
            Logger.LogDebug($"Detected hit {offsetType} at {rowID}");
            Shock(offsetType);
        }

        private static void Shock(OffsetType offsetType)
        {
            if (_config is null)
            {
                Logger.LogWarning("Config is not initialized!");
                return;
            }

            if (offsetType is OffsetType.SlightlyEarly or OffsetType.SlightlyLate)
                Punish(_config.SlightlyOff);
            else if (offsetType is OffsetType.VeryEarly or OffsetType.VeryLate)
                Punish(_config.VeryOff);
            else if (offsetType == OffsetType.Missed)
                Punish(_config.Missed);
        }

        static void Punish(PunishmentConfig punishment)
        {
            if (punishment.Type == PunishmentType.None)
            {
                Logger.LogDebug("PunishmentType is none. Not doing anything.");
                return;
            }

            if (_client is null)
            {
                Logger.LogWarning("Client is not initialized yet!");
                return;
            }

            Task.Run(async () => await _client.Shock(
                punishment.Type.ToMode(),
                punishment.Intensity,
                punishment.Duration
            ));
        }
    }
}