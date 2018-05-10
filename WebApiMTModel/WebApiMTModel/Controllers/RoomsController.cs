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
        public RoomsController()
        {
            //  this.userViewModelValidator = userViewModelValidator;
        }

        //api/Rooms/RoomsSetting
        //  [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.Route("GetRoomsSetting")]
        [AllowAnonymous]
        [HttpGet]
        public List<RoomsDetailsView> GetRoomsSetting(DateTime fromDate, DateTime toDate)
        {
           // DateTime fromDate=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(2); DateTime toDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4);
             RoomsServie roomService = new RoomsServie();
            return roomService.GetRoomsSetting(fromDate, toDate);
          //  List<RoomsDetailsView> rooms = roomService.GetRooms();
          //  return rooms;
        }
        /// <summary>
        /// מכניס כלב לחדר
        /// </summary>
        /// <param name="dog"></param>
        /// <param name="roomNumber"></param>
        [System.Web.Http.Route("AddDogToRoom")]
        [AllowAnonymous]
        [HttpPost]
        public void AddDogToRoom([FromBody]DogInRoomDetailsView dog, int roomNumber)
        {
            RoomsServie roomsServie = new RoomsServie();
            roomsServie.AddDogToRoom(dog,roomNumber);
        }
        /// <summary>
        /// מוציא כלב מחדר
        /// </summary>
        /// <param name="dogNumber"></param>
        /// <param name="RoomNumber"></param>
        /// 
        [System.Web.Http.Route("RemoveDogFromRoom")]
        [AllowAnonymous]
        [HttpGet]
        public void RemoveDogFromRoom(int dogNumber,int RoomNumber)
        {
            RoomsServie roomsServie = new RoomsServie();
            roomsServie.RemoveDogFromRoom(dogNumber, RoomNumber);
        }
        //מעדכנת איכלוס חדרים לפי הרשימה
        public void GetRoomsSetting([FromBody]List<RoomsDetailsView> list)
        {
        }

        }
}
