using ShockTherapy.Utils;

namespace ShockTherapy.PiShock;

public enum Mode
{
    [EnumConvert("s")] Shock,
    [EnumConvert("v")] Vibrate,
    [EnumConvert("b")] Beep
}