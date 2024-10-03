using CourseHub.API.Realtime.Services.Stream;
using CourseHub.API.Realtime.Services;
using Microsoft.AspNetCore.Mvc;
using CourseHub.API.Controllers.Shared;

namespace CourseHub.API.Realtime.Controllers;

//...
public class RealtimeController : BaseController
{
    [HttpGet("participants")]
    public Dictionary<string, Participant> GetParticipants()
    {
        return ConnectionsHandler.GetParticipants();
    }

    [HttpGet("rooms")]
    public List<Room> GetRooms()
    {
        return ConnectionsHandler.GetRooms();
    }
}
