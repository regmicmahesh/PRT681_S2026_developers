using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskService.DTOs;
using TaskService.Models;
using TaskService.Repositories;

namespace TaskService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    private int GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        // For testing without authentication, use default userId of 1
        return 1;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var userId = GetCurrentUserId();
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = GetCurrentUserId();
        var task = await _taskRepository.GetTaskByIdAsync(id, userId);
        if (task == null)
        {
            return NotFound(new { Message = "Task not found." });
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var userId = GetCurrentUserId();
        var task = new TaskItem
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            IsCompleted = false
        };

        var taskId = await _taskRepository.CreateTaskAsync(task);
        task.Id = taskId;

        return CreatedAtAction(nameof(GetTask), new { id = taskId }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
    {
        var userId = GetCurrentUserId();
        var existingTask = await _taskRepository.GetTaskByIdAsync(id, userId);
        if (existingTask == null)
        {
            return NotFound(new { Message = "Task not found." });
        }

        existingTask.Title = request.Title;
        existingTask.Description = request.Description;
        existingTask.DueDate = request.DueDate;
        existingTask.IsCompleted = request.IsCompleted;

        var updated = await _taskRepository.UpdateTaskAsync(existingTask);
        if (!updated)
        {
            return BadRequest(new { Message = "Failed to update task." });
        }

        return Ok(existingTask);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetCurrentUserId();
        var deleted = await _taskRepository.DeleteTaskAsync(id, userId);
        if (!deleted)
        {
            return NotFound(new { Message = "Task not found." });
        }

        return NoContent();
    }
}
