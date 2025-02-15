using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models.RoleModel
{
    public class RoleChangeModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public SelectList? Users { get; set; }
        public SelectList? Roles { get; set; }
    }
}
