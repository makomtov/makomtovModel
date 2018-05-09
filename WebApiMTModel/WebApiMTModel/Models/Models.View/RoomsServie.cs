using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiMTModel.Models;

namespace WebApiMTModel.Models.Models.View
{
    public class RoomsServie
    {
        public List<RoomsDetailsView> GetURooms()
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();

                List<RoomsDetailsView> list = context.RoomsTbl.
                     Join(context.StatusTbl,
                     u => u.RoomStatus, v => v.StatusId,
                     (u, v) => new 
                     {
                        u.RoomID,
                         u.RoomCapacity,
                         u.RoomComments,
                          u.RoomDescription,
                         u.RoomStatus,
                         RoomStatusName=v.StatusName


                     }).Where(room => room.RoomStatus == 21).
                     Join(context.RoomSetting,
                     s=>s.RoomID,r=>r.Id,
                     (s,r)=>new RoomsDetailsView
                     {
                         RoomCapacity=s.RoomCapacity
                        

                     }
                     ).Distinct()
                         .ToList();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    List<DogInRoomDetailsView> dogsInRoom = list[i].dogsInRoom;
                //    list[i].dogsInRoom = new List<DogInRoomDetailsView>();
                //    foreach (RoomSetting )
                //    {
                //        DogInRoomDetailsView dogInRoom = new DogInRoomDetailsView();
                //        dogInRoom.Comments = dog.Comments;
                //        dogInRoom.DogBirthDate=dog.

                //    }

                //}




                return list;
                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}