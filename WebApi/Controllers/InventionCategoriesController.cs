using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/inventionCategories")]
    public class InventionCategoriesController : CrudController<InventionCategoryDto>
    {
        private readonly IInventionCategoryService _inventionCategoryService;

        public InventionCategoriesController(
            IInventionCategoryService inventionCategoryService,
            ILogService loggerService,
            IInventAppContext inventAppContext
        ) : base(
            inventionCategoryService,
            loggerService,
            inventAppContext
        )
        {
            _inventionCategoryService = inventionCategoryService;
        }
    }
}