using Service_Bus_TMS.DAL.Enums;

namespace Service_Bus_TMS.DAL.Entities;

public class Task
{
    public int TaskID { get; set; }
    
    public string TaskName { get; set; }
    
    public string Description { get; set; }
    
    public TaskStatus Status { get; set; }
    
    public string AssignedTo { get; set; }
    
    public ReceiveStatus ReceiveStatus { get; set; }
    
    
}