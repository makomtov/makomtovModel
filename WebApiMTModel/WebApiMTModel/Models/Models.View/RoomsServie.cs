using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiMTModel.Models;

namespace WebApiMTModel.Models.Models.View
{
    public class RoomsServie
    {
        public List<RoomsDetailsView> GetRoomsSetting(DateTime fromDate,DateTime toDate)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();

                List<RoomsDetailsView> list = context.RoomsTbl.
                     Join(context.StatusTbl,
                     u => u.RoomStatus, v => v.StatusId,
                     (u, v) => new RoomsDetailsView
                     {
                       RoomID=  u.RoomID,
                       RoomCapacity=  u.RoomCapacity,
                        RoomComments= u.RoomComments,
                        RoomDescription= u.RoomDescription,
                        RoomStatus= u.RoomStatus,
                         RoomStatusName = v.StatusName


                     }).Where(room => room.RoomStatus == 21)
                     .Distinct()
                         .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].dogsInRoom = GetDogsInRoom(list[i].RoomID, fromDate, toDate);
                    
                }




                return list;
                

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private List<DogInRoomDetailsView> GetDogsInRoom(int roomNumber,DateTime fromDate,DateTime toDate)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<DogInRoomDetailsView> list = context.RoomSetting.
                    Join(context.OrdersTbl,
                     u => u.OrderNumber, v => v.OrderNumber,
                     (u, v) => new
                     {
                         u.Comments,
                         u.DogNumber,
                         v.FromDate,
                         u.RoomNumber,
                         v.ToDate,
                         v.OrderUserId,
                         u.RoomFromDate,
                         u.RoomToDate,
                     }

                    ).Where(u=>u.RoomFromDate<=fromDate && u.RoomToDate>=toDate && u.RoomNumber==roomNumber).
                    Join(context.UsersTbl,
                    o=>o.OrderUserId,u=>u.UserID,
                    (o,u)=>new 
                    {
                        u.UserFirstName,
                        u.UserLastName,
                        u.UserPhone1,
                        u.UserPhone2,
                        u.veterinarTbl.VeterinarName,
                        u.veterinarTbl.VeterinarPhone1,
                        o.Comments,
                        o.DogNumber,
                        o.FromDate,
                        o.ToDate,
                        o.OrderUserId,
                        o.RoomFromDate,
                        o.RoomToDate,
                       
                    }).
                    
                    Join(context.UserDogs,
                    o=>o.DogNumber,d=>d.DogNumber,
                    (o,d)=>new DogInRoomDetailsView
                    {
                      Comments=  o.Comments,
                       DogNumber= o.DogNumber,
                       FromDateInRoom= (DateTime) o.RoomFromDate,
                       ToDateInRoom= (DateTime)o.RoomToDate,
                       FromDateInPension=(DateTime)o.FromDate,
                       ToDateInPension=(DateTime)o.ToDate,
                       DogUserID= o.OrderUserId,
                       UserFirstName= o.UserFirstName,
                       UserLastName= o.UserLastName,
                       UserPhone1= o.UserPhone1,
                       UserPhone2= o.UserPhone2,
                       DogBirthDate=(DateTime) d.DogBirthDate,
                       DogComments= d.DogComments,
                       DogDig=(bool) d.DogDig,
                       DogFriendlyWith=(int) d.DogFriendlyWith,
                       DogGender= d.DogGender,
                       DogImage= d.DogImage,
                       DogJump=(bool) d.DogJump,
                       DogName= d.DogName,
                       DogNeuter=(bool) d.DogNeuter,
                       DogRabiesVaccine=(DateTime) d.DogRabiesVaccine,
                       DogShvav= d.DogShvav,
                       DogType= d.DogType,
                       VeterinarName=o.VeterinarName,
                       VeterinarPhone1=o.VeterinarPhone1,
                      
                    
                        
                    }).
                    Distinct().ToList();
                return list;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void AddDogToRoom(DogInRoomDetailsView dog, int roomNumber)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                RoomSetting roomSetting = new RoomSetting();
                roomSetting.Comments = dog.Comments;
                roomSetting.DogNumber = dog.DogNumber;
                roomSetting.Id = roomNumber;
                roomSetting.OrderNumber = dog.DogorderNumber;
                roomSetting.RoomFromDate = dog.FromDateInRoom;
                roomSetting.RoomToDate = dog.ToDateInRoom;
               
                context.RoomSetting.Add(roomSetting);
                context.SaveChanges();


            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void RemoveDogFromRoom(int dogNumber, int RoomNumber)
        {
            try
            {

                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    var dogt = context.Set<RoomSetting>().Find(dogNumber);
                    
                    if (dogt != null)
                    {
                        dogt.RoomToDate = DateTime.Now;
                        context.Entry(dogt).CurrentValues.SetValues(dogt);
                        context.SaveChanges();
                    }
                    else
                    {

                        throw new Exception();
                    }
               
                context.Dispose();
            }
                    }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}