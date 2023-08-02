using Microsoft.AspNetCore.Mvc;
using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.BLL.Services;

namespace Service_Bus_TMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpPost]
    public IActionResult AddTask(TaskModel taskModel)
    {
        _taskService.AddTask(taskModel);
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateTaskStatus(TaskUpdate taskUpdate)
    {
        var updatedTask = _taskService.UpdateTask(taskUpdate);
        return Ok(updatedTask);
    }
    
    [HttpGet]
    public ActionResult<List<TaskModel>> GetAllTasks()
    {
        var tasks = _taskService.GetAllTasks();
        return Ok(new { tasks.Count, tasks });
    }
}