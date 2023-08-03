using System.Collections.Generic;
using System.Linq;
using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.BLL.Utilities;
using Service_Bus_TMS.DAL.Enums;
using Service_Bus_TMS.DAL.Repositories;
using Service_Bus_TMS.BLL.Exceptions;

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
        if (task.TaskID < 0)
        {
            throw new BadRequestException("Task id must be greater than 0!");
        }
        
        var taskUpdate = _taskRepository.GetById(task.TaskID);
        
        if (taskUpdate == null)
        {
            throw new NotFoundException("Task not found!");
        }
        
        if (taskUpdate.ReceiveStatus == ReceiveStatus.Received)
        {
            throw new NotFoundException("Task already received!");
        }
        
        taskUpdate.Status = task.NewStatus;
        _serviceBusHandler.SendMessage(taskUpdate);

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