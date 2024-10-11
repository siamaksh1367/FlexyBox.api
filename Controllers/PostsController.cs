using FlexyBox.core.Commands.CreateComment;
using FlexyBox.core.Commands.CreatePost;
using FlexyBox.core.Commands.DeletePost;
using FlexyBox.core.Queries.GetPostsIncludingDetails;
using FlexyBox.dal.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlexyBox.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IMediator _mediator;
        public PostsController(IMediator mediator, ILogger<PostsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsIncludingDetail()
        {

            var result = await _mediator.Send(new GetPostsIncludingDetailsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, UpdatePostCommand updatePostCommand)
        {
            var result = await _mediator.Send(updatePostCommand);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreatePostResponse>> PostPost(CreatePostCommand createPostCommand)
        {
            var result = await _mediator.Send(createPostCommand);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeletePost(int id)
        {
            var result = await _mediator.Send(new DeletePostCommand() { Id = id });
            return Ok(result);
        }

    }
}
