using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMTModel.Models.Models.View;
using FluentValidation.Results;

namespace WebApiMTModel.Controllers
{
    [System.Web.Http.RoutePrefix("api/Rooms")]
    public class RoomsController : ApiController
    {
        //api/Rooms/GetRooms
        [System.Web.Http.Authorize(Roles = "admin")]

        [System.Web.Http.Route("")]
        public List<RoomsDetailsView> GetRooms()
        {
            RoomsServie roomService = new RoomsServie();
            return new List<RoomsDetailsView>();
          //  List<RoomsDetailsView> rooms = roomService.GetRooms();
          //  return rooms;
        }
    }
}
