using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Application.Services.CategoryCQRS;
using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core.SeedWork;
using Catalog.Domain.Core;
using Microsoft.AspNetCore.Components.Forms;

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


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateCategoryRequest createCategoryRequest)
        {
            var result = await _mediator.Send(createCategoryRequest);
            return Ok(result);
        }
    }
}
