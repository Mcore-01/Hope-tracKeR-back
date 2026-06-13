using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum ItemCategory
{
    [Display(Name = "Техника")]
    Technique,
    [Display(Name = "Расходник")]
    Consumables
}