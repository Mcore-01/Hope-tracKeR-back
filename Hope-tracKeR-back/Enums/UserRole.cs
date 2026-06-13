using System.ComponentModel.DataAnnotations;

namespace Hope_tracKeR_back.Enums;

public enum UserRole
{
    [Display(Name = "Администратор")]
    Admin,
    [Display(Name = "Сотрудник")]
    Employee
}