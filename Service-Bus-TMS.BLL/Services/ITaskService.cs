using System.Collections.Generic;
using Service_Bus_TMS.BLL.Models;

namespace Service_Bus_TMS.BLL.Services;

public interface ITaskService
{
    void AddTask(TaskModel task);

    TaskModel UpdateTask(TaskUpdate task);

    List<TaskModel> GetAllTasks();
}