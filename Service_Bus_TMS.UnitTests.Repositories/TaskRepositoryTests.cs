using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Service_Bus_TMS.DAL.DbContexts;
using Service_Bus_TMS.DAL.Entities;
using Service_Bus_TMS.DAL.Repositories;

namespace Service_Bus_TMS.UnitTests.Repositories;

public class TaskRepositoryTests
{
    private DbContextOptions<AppDbContext> _dbContextOptions;
        
    [SetUp]
    public void Setup()
    {
        /*_dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;*/
    }

    [Test]
    public void Add_AddsTaskToDatabase()
    {
        // Arrange
        using (var context = new AppDbContext(_dbContextOptions))
        {
            var repository = new TaskRepository(context);
            var task = new Task { TaskID = 1, TaskName = "Sample Task" };

            // Act
            repository.Add(task);
        }

        // Assert
        using (var context = new AppDbContext(_dbContextOptions))
        {
            var addedTask = context.Tasks.FirstOrDefault();
            Assert.That(addedTask, Is.Not.Null);
            Assert.That(addedTask?.TaskName, Is.EqualTo("Sample Task"));
        }
    }

    [Test]
    public void GetById_ReturnsCorrectTask()
    {
        // Arrange
        using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Tasks.Add(new Task { TaskID = 1, TaskName = "Sample Task" });
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_dbContextOptions))
        {
            var repository = new TaskRepository(context);

            // Act
            var task = repository.GetById(1);

            // Assert
            Assert.That(task, Is.Not.Null);
            Assert.That(task?.TaskName, Is.EqualTo("Sample Task"));
        }
    }

    [Test]
    public void GetAll_ReturnsAllTasks()
    {
        // Arrange
        using (var context = new AppDbContext(_dbContextOptions))
        {
            context.Tasks.Add(new Task { TaskID = 1, TaskName = "Task 1" });
            context.Tasks.Add(new Task { TaskID = 2, TaskName = "Task 2" });
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_dbContextOptions))
        {
            var repository = new TaskRepository(context);

            // Act
            var tasks = repository.GetAll();

            // Assert
            Assert.That(tasks.Count, Is.EqualTo(2));
            Assert.That(tasks.Any(t => t?.TaskName == "Task 1"), Is.True);
            Assert.That(tasks.Any(t => t?.TaskName == "Task 2"), Is.True);
        }
    }
}