using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum CartridgeStatus
{
    [Display(Name = "На складе")]
    InStock,
    [Display(Name = "Установлен")]
    Installed,
    [Display(Name = "Пустой")]
    Empty,
    [Display(Name = "Отправлен на заправку")]
    Refilling,
    [Display(Name = "Списан")]
    WriteOff
}