using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMTModel.Models.Models.View;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApiMTModel.Controllers
{
    [System.Web.Http.RoutePrefix("api/Rooms")]
    public class RoomsController : ApiController
    {
        public RoomsController()
        {
            //  this.userViewModelValidator = userViewModelValidator;
        }

        //api/Rooms/GetRoomsSetting
         [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.Route("GetRoomsSetting")]
      //  [AllowAnonymous]
        [HttpPost]
     
            public List<RoomsDetailsView> GetRoomsSetting([FromBody]JObject dates)
       // public List<RoomsDetailsView> GetRoomsSetting()
        {//בחדר מספר 0 נמצאים כל הכלבים בהזמנות שלא שובצו
            // DateTime fromDate=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(2); DateTime toDate= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4);
            RoomsServie roomService = new RoomsServie();
            //DateTime fromDate = DateTime.Now;
            //DateTime toDate = DateTime.Now.AddDays(1);
            string json = JsonConvert.SerializeObject(dates);
            //DateTime fromDate = JObject.Parse(json)["@fromDate"];
            //var toDate = JObject.Parse(json)["@toDate"];
            var data = (JObject)JsonConvert.DeserializeObject(json);
            DateTime fromDate = data["fromDate"].Value<DateTime>();
            //var jArray = JArray.Parse(json);
            //DateTime fromDate = jArray[0]["fromDate"].Value<DateTime>();
            DateTime toDate = data["toDate"].Value<DateTime>();
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
        public void RemoveDogFromRoom(DogInRoomDetailsView dog, int RoomNumber)
        {
            RoomsServie roomsServie = new RoomsServie();
            roomsServie.RemoveDogFromRoom(dog, RoomNumber);
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
        /// <summary>
        /// מעדכנת איכלוס חדרים לפי הרשימה
        /// </summary>
        /// <param name="listRoomsDetails"></param>

        [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.Route("UpdateRoomsDetailsAndSetting")]
        [HttpPost]
        public HttpResponseMessage UpdateRoomsDetailsAndSetting(List<RoomsDetailsView> listRoomsDetails)
        {
           
                try
                {

                RoomsServie roomsServie = new RoomsServie();
                roomsServie.UpdateRoomsDetailsAndSetting(listRoomsDetails);
                return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }
           
           
        }
    }
}
