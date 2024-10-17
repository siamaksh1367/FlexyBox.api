using FlexyBox.core.Commands.CreateComment;
using FlexyBox.core.Queries.GetComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexyBox.api.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("post/{id}")]
        public async Task<ActionResult<IEnumerable<GetCommentsQuery>>> GetCommentsForPost(int id)
        {
            var comments = await _mediator.Send(new GetCommentsQuery() { PostId = id });
            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetCommentResponse>> PostComment([FromBody] CreateCommentCommand createCommentCommand)
        {
            var result = await _mediator.Send(createCommentCommand);
            return Ok(result);
        }
    }
}
