using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MusiCom.Areas.Admin.AdminConstants;

namespace MusiCom.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
    }
}
