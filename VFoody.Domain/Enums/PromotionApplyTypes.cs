using System.ComponentModel;

namespace VFoody.Domain.Enums;

public enum PromotionApplyTypes
{
    [Description("Percent")]
    Percent = 1,
    [Description("Absolute")]
    Absolute = 2,
}