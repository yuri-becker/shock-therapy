using BepInEx.Configuration;

namespace ShockTherapy.Config;

public class PunishmentConfig(string section, ConfigFile file)
{
    private readonly ConfigEntry<PunishmentType> _type = file.Bind(
        section,
        "Type",
        PunishmentType.None,
        "What the shocker should do."
    );

    private readonly ConfigEntry<float> _duration = file.Bind(
        section,
        "Duration",
        .5f,
        new ConfigDescription(
            "Duration in seconds (can be fractional).",
            new AcceptableValueRange<float>(0.1f, 15f)
        )
    );

    private readonly ConfigEntry<byte> _intensity = file.Bind<byte>(
        section,
        "Intensity",
        30,
        new ConfigDescription(
            "Intensity of the vibration/shock.",
            new AcceptableValueRange<byte>(1, 100)
        )
    );

    public PunishmentType Type => _type.Value;
    public float Duration => _duration.Value;
    public byte Intensity => _intensity.Value;

}