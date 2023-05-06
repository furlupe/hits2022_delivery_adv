using AdminPanel.Models;
using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class UtilsController : Controller
    {
        public IActionResult Pagination(int currpage, int pages, int pagesize)
        {
            return PartialView("~/Views/Shared/Partial/PaginationAjax.cshtml", new PaginationModel { 
                PageInfo = new PageInfo { 
                    CurrentPage = currpage,
                    Pages = pages,
                    PageSize = pagesize
                } 
            });
        }
    }
}
