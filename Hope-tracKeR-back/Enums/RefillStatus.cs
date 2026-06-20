using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum RefillStatus
{
    [Display(Name = "В процессе")]
    InProgress,
    [Display(Name = "Завершен")]
    Completed
}