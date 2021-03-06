﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/inventions")]
    public class InventionsController : CrudController<InventionDto>
    {
        private readonly IInventionService _inventionService;

        public InventionsController(IInventionService inventionService, ILogService loggerService, IInventAppContext inventAppContext) : base(inventionService, loggerService, inventAppContext)
        {
            _inventionService = inventionService;
        }

        [HttpGet]
        [Route("cheapest")]
        public async Task<IHttpActionResult> GetCheapest(decimal maxPrice) => await ExecuteAsync(async () => await _inventionService.GetCheapestAsync(maxPrice));

        [HttpPatch]
        public async Task<IHttpActionResult> UpdateStateAsync([FromUri] Guid id, [FromBody] InventionStateDto dto) => await ExecuteAsync(async () => await _inventionService.UpdateStateAsync(id, dto));
    }
}
