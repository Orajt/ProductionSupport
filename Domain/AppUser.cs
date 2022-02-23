using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public int ProductionDepartmentId { get; set; }
        public ProductionDepartment ProductionDepartment { get; set; }
    }
}