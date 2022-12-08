using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MusiCom.Areas.Admin.AdminConstants;

namespace MusiCom.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller that will be inherited by all controllers in the Admin Area
    /// </summary>
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
    }
}
