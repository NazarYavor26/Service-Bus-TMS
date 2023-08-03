using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Service_Bus_TMS.BLL.Models;
using Service_Bus_TMS.BLL.Services;
using Service_Bus_TMS.DAL.Entities;
using Service_Bus_TMS.DAL.Enums;
using Service_Bus_TMS.DAL.Repositories;

namespace Service_Bus_TMS.UnitTests.Services;

[TestFixture]
public class TaskServiceTests
{
    private Mock<IServiceBusHandler> _serviceBusHandlerMock;
    private Mock<ITaskRepository> _taskRepositoryMock;
    private TaskService _taskService;

    [SetUp]
    public void SetUp()
    {
        _serviceBusHandlerMock = new Mock<IServiceBusHandler>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_serviceBusHandlerMock.Object, _taskRepositoryMock.Object);
    }

    [Test]
    public void AddTask_ValidTask_SuccessfullyAddedAndSent()
    {
        // Arrange
        var taskModel = new TaskAdd();

        // Act
        _taskService.AddTask(taskModel);

        // Assert
        _taskRepositoryMock.Verify(repo => repo.Add(It.IsAny<Task>()), Times.Once);
        _serviceBusHandlerMock.Verify(handler => handler.SendMessage(It.IsAny<Task>()), Times.Once);
    }

    [Test]
    public void UpdateTask_ValidTaskToUpdate_StatusUpdatedAndSent()
    {
        // Arrange
        var taskId = 1;
        var taskUpdate = new TaskUpdate { TaskID = taskId, NewStatus = TaskStatus.Completed };
        var existingTaskEntity = new Task { TaskID = taskId, ReceiveStatus = ReceiveStatus.NotReceived };
        _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns(existingTaskEntity);

        // Act
        var result = _taskService.UpdateTask(taskUpdate);

        // Assert
        Assert.That(existingTaskEntity.Status, Is.EqualTo(TaskStatus.Completed));
        _serviceBusHandlerMock.Verify(handler => handler.SendMessage(It.IsAny<Task>()), Times.Once);
    }

    [Test]
    public void UpdateTask_InvalidTaskToUpdate_NoStatusUpdate()
    {
        // Arrange
        var taskId = 1;
        var taskUpdate = new TaskUpdate { TaskID = taskId, NewStatus = TaskStatus.Completed };
        _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns((Task)null);

        // Act
        var result = _taskService.UpdateTask(taskUpdate);

        // Assert
        Assert.IsNull(result);
        _taskRepositoryMock.Verify(repo => repo.GetById(taskId), Times.Once);
        _serviceBusHandlerMock.Verify(handler => handler.SendMessage(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void GetAllTasks_MultipleTasksReceived_Success()
    {
        // Arrange
        var taskEntities = new List<Task>
        {
            new(),
            new()
        };
        _serviceBusHandlerMock.Setup(handler => handler.ReceiveMessage()).Returns(taskEntities);

        // Act
        var result = _taskService.GetAllTasks();

        // Assert
        Assert.That(result.Count, Is.EqualTo(taskEntities.Count));
        _serviceBusHandlerMock.Verify(handler => handler.ReceiveMessage(), Times.Once);
    }
}