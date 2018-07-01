using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class RoomsDetailsView
    {
        public int RoomID { get; set; }
        public string RoomDescription { get; set; }
        public int RoomStatus { get; set; }
        public string RoomStatusName { get; set; }
        public Nullable<int> RoomCapacity { get; set; }
        public string RoomComments { get; set; }
        public  List<DogInRoomDetailsView> dogsInRoom { get; set; }
    }
}