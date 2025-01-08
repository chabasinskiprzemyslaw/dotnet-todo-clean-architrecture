using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Todo.CreateTodoItem;
using ToDo.Application.Todo.GetTodoItemsInTodoList;
using ToDo.Application.Todo.GetTodoList;
using ToDo.Application.Todo.GetTodoLists;

namespace ToDo.Api.Controllers.TodoLists;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/todolists")]
public class TodoListsController : ControllerBase
{
    private readonly ISender _sender;

    public TodoListsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodoLists(
        CancellationToken cancellationToken)
    {
        GetTodoListsQuery query = new GetTodoListsQuery();

        var resut = await _sender.Send(query, cancellationToken);

        return resut.IsSuccess ? Ok(resut.Value) : NotFound();
    }

    [HttpGet("{id}/info")]
    public async Task<IActionResult> GetTodoListInfo(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        GetTodoListInfoQuery query = new GetTodoListInfoQuery(id);

        var resut = await _sender.Send(query, cancellationToken);

        return resut.IsSuccess ? Ok(resut.Value) : NotFound();
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetTodoListItems(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        GetTodoListItemsQuery query = new GetTodoListItemsQuery(id);

        var resut = await _sender.Send(query, cancellationToken);

        return resut.IsSuccess ? Ok(resut.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoList(
        [FromBody] CreateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetTodoListInfo), new { id = result.Value }, null);
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> CreateTodoItem(
        [FromRoute] Guid id,
        [FromBody] CreateTodoItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTodoItemCommand(
            id,
            request.Title,
            request.Priority,
            request.Description,
            request.DueDate);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetTodoListItems), new { id = id }, null);
    }
}
