

using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.BLL.Utilities;

public static class Mapper
{
    public static Task ToEntitie(this TaskModel taskModel)
    {
        return new Task
        {
            TaskID = taskModel.TaskID,
            TaskName = taskModel.TaskName,
            Description = taskModel.Description,
            Status = taskModel.Status,
            AssignedTo = taskModel.AssignedTo
        };
    }
    
    public static TaskModel ToModel(this Task taskModel)
    {
        return new TaskModel
        {
            TaskID = taskModel.TaskID,
            TaskName = taskModel.TaskName,
            Description = taskModel.Description,
            Status = taskModel.Status,
            AssignedTo = taskModel.AssignedTo
        };
    }
}