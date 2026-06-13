using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum ItemStatus
{
    [Display(Name = "В наличии")]
    InStock,
    [Display(Name = "В ремонте")]
    Repair,
    [Display(Name = "Выдан")]
    Issued,
    [Display(Name = "Сломан")]
    Broken
}