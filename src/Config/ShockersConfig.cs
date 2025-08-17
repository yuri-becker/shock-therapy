using System;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace ShockTherapy.Config;

public class ShockersConfig
{
    private readonly ManualLogSource _logger = new(typeof(ShockersConfig).FullName);

    private readonly ConfigEntry<uint> _device;

    private readonly ConfigEntry<string> _shareCode;

    private readonly ConfigEntry<string> _shockers;

    public ShockersConfig(ConfigFile file)
    {
        Logger.Sources.Add(_logger);

        _device = file.Bind<uint>(
            ConfigSections.Shockers,
            "Device",
            0,
            "ID of PiShock Hub. See at https://pishock.com/#/control by clicking on the ⋮."
        );
        _shareCode = file.Bind(
            ConfigSections.Shockers,
            "ShareCode",
            "",
            "Optionally, define a ShareCode if this is someone else's device. Leave empty if this is your own device."
        );
        _shockers = file.Bind(
            ConfigSections.Shockers,
            "Shockers",
            "",
            "Comma-seperated IDs of Shockers to use (e.g. \"11225, 11226\"). You can see your shocker's ID by clicking on the Cog symbol at https://pishock.com/#/control."
        );
    }

    public uint Device => _device.Value;
    public string ShareCode => _shareCode.Value.Trim();

    public uint[] Shockers
    {
        get
        {
            try
            {
                return _shockers.Value.Split(",").Select(uint.Parse).ToArray();
            }
            catch (FormatException)
            {
                _logger.LogError("Shockers could not be parsed. Please check your configuration.");
                return [];
            }
        }
    }

    public bool IsShareCode => !string.IsNullOrEmpty(ShareCode.Trim());
}