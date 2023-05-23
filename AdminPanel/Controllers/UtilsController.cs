using AdminPanel.Models;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Mvc;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace AdminPanel.Controllers
{
    [RoleRequirementAuthorization(RoleType.Admin)]
    public class UtilsController : Controller
    {
        public IActionResult Pagination(int currpage, int pages, int pagesize)
        {
            return PartialView("~/Views/Shared/Partial/PaginationAjax.cshtml", new PaginationModel
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = currpage,
                    Pages = pages,
                    PageSize = pagesize
                }
            });
        }
    }
}
