using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMaintenceRequestsDao _maintenceRequestsDao;

        public MaintenanceRequestController(IMaintenceRequestsDao maintenceRequestsDao) {
            _maintenceRequestsDao = maintenceRequestsDao;
        }

        [HttpGet("owner")]
        [Authorize(Roles ="Owner")]
        public ActionResult<IList<MaintenanceRequestsViewsForOwner>> GetMaintenanceRequestsForOwner() {
            var id = Convert.ToInt32(User.FindFirst("sub")?.Value);
            return Ok(_maintenceRequestsDao.GetMaintenanceRequestsForOwner(id));
        }

        [HttpGet("tenant")]
        [Authorize(Roles = "Tenant")]
        public ActionResult<IList<MaintenanceRequestsViewsForTenant>> GetMaintenanceRequestsByTenant() {
            var id = Convert.ToInt32(User.FindFirst("sub")?.Value);
            return Ok(_maintenceRequestsDao.GetMaintenanceRequestsForTenant(id));
        }

        [HttpGet("employee")]
        [Authorize(Roles = "Employee")]
        public ActionResult<IList<MaintenanceRequestsViewsForTenant>> GetMaintenanceRequestsByEmployee()
        {
            var id = Convert.ToInt32(User.FindFirst("sub")?.Value);
            return Ok(_maintenceRequestsDao.GetMaintenanceRequestsForEmployee(id));
        }

        [HttpPost]
        [Authorize(Roles = "Tenant")]
        public ActionResult CreateRequest(MaintenanceRequest request)
        {
            int tenantId = Convert.ToInt32(User.FindFirst("sub")?.Value);

            _maintenceRequestsDao.CreateRequest(request, tenantId);
            return NoContent();
        }

        [HttpPut("employee/update")]
        [Authorize(Roles = "Employee")]
        public ActionResult NewUpdateRequeststatus(StatusIdRequestId statusIdRequestId)
        {
            _maintenceRequestsDao.UpdateRequeststatus(statusIdRequestId);
            return NoContent();
        }
    }
}
