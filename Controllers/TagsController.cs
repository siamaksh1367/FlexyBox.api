using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetTags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexyBox.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTagResponse>>> GetTags()
        {
            var tags = await _mediator.Send(new GetTagsQuery());
            return Ok(tags);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetTagResponse>>> CreateTags([FromBody] CreateTagCommand createTagCommand)
        {
            var tags = await _mediator.Send(createTagCommand);
            return Ok(tags);
        }
    }
}
