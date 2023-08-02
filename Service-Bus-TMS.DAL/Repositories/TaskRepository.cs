using System.Collections.Generic;
using System.Linq;
using Service_Bus_TMS.DAL.DbContexts;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.DAL.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    
    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    public void Add(Task task)
    {
        _db.Tasks.Add(task);
        _db.SaveChanges();
    }
    
    public Task? GetById(int id)
    {
        return _db.Tasks.FirstOrDefault(x => x.TaskID == id);
    }
    
    public List<Task?> GetAll()
    {
        return _db.Tasks.ToList();
    }
}