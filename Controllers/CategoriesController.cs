﻿using FlexyBox.core.Commands.CreateCategory;
using FlexyBox.core.Commands.DeleteCategory;
using FlexyBox.core.Commands.UpdateCategory;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.dal.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexyBox.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> PutCategory([FromBody] UpdateCategoryCommand updateCategoryCommand)
        {
            var result = await _mediator.Send(updateCategoryCommand);
            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult<Category>> PostCategory([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            var result = await _mediator.Send(createCategoryCommand);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteCategory(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            return Ok(result);
        }
    }
}
