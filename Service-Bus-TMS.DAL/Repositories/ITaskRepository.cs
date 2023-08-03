using System.Collections.Generic;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.DAL.Repositories;

public interface ITaskRepository
{
    void SaveChanges();
    
    void Add(Task task);
    
    Task GetById(int id);

    List<Task> GetAll();
    
    Task GetLast();

}