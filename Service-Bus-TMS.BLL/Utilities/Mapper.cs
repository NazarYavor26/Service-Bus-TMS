

using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.BLL.Utilities;

public static class Mapper
{
    public static Task ToEntitie(this TaskAdd taskAdd)
    {
        return new Task
        {
            TaskName = taskAdd.TaskName,
            Description = taskAdd.Description,
            Status = taskAdd.Status,
            AssignedTo = taskAdd.AssignedTo
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