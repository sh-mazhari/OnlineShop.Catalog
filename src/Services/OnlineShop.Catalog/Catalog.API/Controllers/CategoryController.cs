using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Application.Services.CategoryCQRS;
using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core.SeedWork;
using Catalog.Domain.Core;
using Microsoft.AspNetCore.Components.Forms;
using Catalog.Application.Services.CategoryCQRS.Queries;
using Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        //public CategoryController(IMediator mediator)
        //{
        //    _mediator = mediator;
        //}
        public CategoryController(IMediator mediator) =>
           (_mediator) = (mediator);

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetCategoryQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateCategoryRequest createCategoryRequest)
        {
            var result = await _mediator.Send(createCategoryRequest);
            return Ok(result);
        }
    }
}
