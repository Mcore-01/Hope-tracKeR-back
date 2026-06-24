using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum AddressType
{
    [Display(Name = "Кабинет")]
    Office,
    [Display(Name = "Склад")]
    Warehouse
}