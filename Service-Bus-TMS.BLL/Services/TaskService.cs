using System.Collections.Generic;
using System.Linq;
using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.BLL.Utilities;
using Service_Bus_TMS.DAL.Enums;
using Service_Bus_TMS.DAL.Repositories;

namespace Service_Bus_TMS.BLL.Services;

public class TaskService : ITaskService
{
    private readonly IServiceBusHandler _serviceBusHandler;
    private readonly ITaskRepository _taskRepository;

    public TaskService(IServiceBusHandler serviceBusHandler, ITaskRepository taskRepository)
    {
        _serviceBusHandler = serviceBusHandler;
        _taskRepository = taskRepository;
    }
    
    public void AddTask(TaskAdd task)
    {
        _taskRepository.Add(task.ToEntitie());
        _serviceBusHandler.SendMessage(_taskRepository.GetLast());
    }

    public TaskModel UpdateTask(TaskUpdate task)
    {
        var taskUpdate = _taskRepository.GetById(task.TaskID);
        
        if (taskUpdate != null && taskUpdate.ReceiveStatus != ReceiveStatus.Received)
        {
            taskUpdate.Status = task.NewStatus;
            _taskRepository.SaveChanges();
            _serviceBusHandler.SendMessage(taskUpdate);
        }

        return taskUpdate.ToModel();
    }

    public List<TaskModel> GetAllTasks()
    {
        var allTasksFromQueue = _serviceBusHandler.ReceiveMessage().Select(task => task.ToModel()).ToList();
        MarkAsReceived();

        return allTasksFromQueue;
    }

    private void MarkAsReceived()
    {
        foreach (var task in _taskRepository.GetAll().Where(task => task.ReceiveStatus != ReceiveStatus.Received))
        {
            task.ReceiveStatus = ReceiveStatus.Received;
        }
        
        _taskRepository.SaveChanges();
    }
}