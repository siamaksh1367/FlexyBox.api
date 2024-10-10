using FlexyBox.core.Commands.CreateCategory;
using FlexyBox.core.Commands.DeleteCategory;
using FlexyBox.core.Commands.UpdateCategory;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.dal.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlexyBox.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoriesResponse>>> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(UpdateCategoryCommand updateCategoryCommand)
        {
            var result = await _mediator.Send(updateCategoryCommand);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreateCategoryCommand createCategoryCommand)
        {
            var result = await _mediator.Send(createCategoryCommand);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteCategory(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand() { Id = id });
            return Ok(result);
        }
    }
}
