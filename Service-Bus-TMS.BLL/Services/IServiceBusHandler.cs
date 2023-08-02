using System.Collections.Generic;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.BLL.Services;

public interface IServiceBusHandler
{
    void SendMessage(Task task);
    
    List<Task> ReceiveMessage();
}