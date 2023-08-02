﻿using System.Collections.Generic;
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
    
    public void AddTask(TaskModel task)
    {
        _taskRepository.Add(task.ToEntitie());
        _serviceBusHandler.SendMessage(task.ToEntitie());
    }

    public TaskModel UpdateTask(TaskUpdate task)
    {
        var taskUpdate = _taskRepository.GetById(task.TaskID);
        
        if (taskUpdate != null && taskUpdate.ReceiveStatus != ReceiveStatus.Received)
        {
            taskUpdate.Status = task.NewStatus;
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
        _taskRepository.GetAll().Select(task => task.ReceiveStatus = ReceiveStatus.Received);
        _taskRepository.SaveChanges();
    }
}