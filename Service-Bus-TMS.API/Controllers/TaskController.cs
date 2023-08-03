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
    public IActionResult AddTask([FromForm]TaskAdd taskAdd)
    {
        _taskService.AddTask(taskAdd);
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateTaskStatus([FromForm]TaskUpdate taskUpdate)
    {
        var updatedTask = _taskService.UpdateTask(taskUpdate);
        return Ok(updatedTask);
    }
    
    [HttpGet]
    public ActionResult<List<TaskAdd>> GetAllTasks()
    {
        var tasks = _taskService.GetAllTasks();
        return Ok(new { tasks.Count, tasks });
    }
}