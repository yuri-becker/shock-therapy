using System;
using ShockTherapy.Config;

namespace ShockTherapy.PiShock;

internal static class ConfigExtensions
{
    internal static Mode ToMode(this PunishmentType value)
    {
        switch (value)
        {
            case PunishmentType.None:
                return Mode.Beep;
            case PunishmentType.Vibration:
                return Mode.Vibrate;
            case PunishmentType.Shock:
                return Mode.Shock;
            default:
                throw new ArgumentException();
        }
    }

    internal static string Channel(this ShockersConfig config) =>
        config.IsShareCode ? $"c{config.Device}-sops-{config.ShareCode}" : $"c{config.Device}-ops";
}