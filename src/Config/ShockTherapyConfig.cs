using System;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace ShockTherapy.Config
{
    public class ShockTherapyConfig(ConfigFile file)
    {
        public PiShockConfig PiShock { get; } = new(file);
        public ShockersConfig Shockers { get; } = new(file);
        public PunishmentConfig SlightlyOff { get; } = new(ConfigSections.PunishmentSlightlyOff, file);
        public PunishmentConfig VeryOff { get; } = new(ConfigSections.PunishmentVeryOff, file);
        public PunishmentConfig Missed { get; } = new(ConfigSections.PunishmentMissed, file);
    }
}