using System.ComponentModel;

namespace VFoody.Domain.Enums;

public enum QuestionTypes
{
    [Description("Radio")]
    Radio = 1,
    [Description("CheckBox")]
    CheckBox = 2,
}