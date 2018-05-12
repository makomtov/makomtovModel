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
        {//בחדר מספר 0 נמצאים כל הכלבים בהזמנות שלא שובצו
            // DateTime fromDate=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(2); DateTime toDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4);
            RoomsServie roomService = new RoomsServie();
            List<RoomsDetailsView> list= roomService.GetRoomsSetting(fromDate, toDate);
            RoomsDetailsView roomsDetailsView = new RoomsDetailsView();
            roomsDetailsView.dogsInRoom = roomService.GetDogsNoSetting(fromDate, toDate);
            roomsDetailsView.RoomCapacity = 0;
            roomsDetailsView.RoomID = 0;
            list.Insert(0, roomsDetailsView);
            return list;
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
            roomsServie.AddDogToRoom(dog, roomNumber);
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
        public void RemoveDogFromRoom(int dogNumber, int RoomNumber)
        {
            RoomsServie roomsServie = new RoomsServie();
            roomsServie.RemoveDogFromRoom(dogNumber, RoomNumber);
        }
        
        /// <summary>
        /// שליפת כל הכלבים לפי תאריכי הגעה שעאין להם שיבוץ לחדר
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>


        public List<DogInRoomDetailsView> GetDogsNoSetting(DateTime fromDate, DateTime toDate)
        {
            RoomsServie roomsServie = new RoomsServie();
            return roomsServie.GetDogsNoSetting(fromDate, toDate);
        }

        //מעדכנת איכלוס חדרים לפי הרשימה
        public void UpdateRoomsSetting([FromBody]List<RoomsDetailsView> list)
        {

        }
    }
}
