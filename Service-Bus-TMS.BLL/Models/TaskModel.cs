using Newtonsoft.Json;
using Service_Bus_TMS.DAL.Enums;

namespace Service_Bus_TMS.BLL.Models;

public class TaskModel
{
    public int TaskID { get; set; }
    
    public string TaskName { get; set; }
    
    public string Description { get; set; }
    
    public TaskStatus Status { get; set; }
    
    public string AssignedTo { get; set; }
}