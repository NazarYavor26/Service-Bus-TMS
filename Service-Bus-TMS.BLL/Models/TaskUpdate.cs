using Service_Bus_TMS.DAL.Enums;

namespace Service_Bus_TMS.BLL.Models;

public class TaskUpdate
{
    public int TaskID { get; set; }
    
    public TaskStatus NewStatus { get; set; }
    
    public string UpdatedBy { get; set; }
}