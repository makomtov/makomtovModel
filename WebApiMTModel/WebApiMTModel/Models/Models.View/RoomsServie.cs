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

                    ).Where(u => u.RoomFromDate <= fromDate && u.RoomToDate >= toDate && u.RoomNumber == roomNumber).
                    Join(context.UsersTbl,
                    o => o.OrderUserId, u => u.UserID,
                    (o, u) => new
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
                        o.RoomNumber,

                    }).

                    Join(context.UserDogs,
                    o => o.DogNumber, d => d.DogNumber,
                    (o, d) => new DogInRoomDetailsView
                    {
                        Comments = o.Comments,
                        DogNumber = o.DogNumber,
                        FromDateInRoom = (DateTime)o.RoomFromDate,
                        ToDateInRoom = (DateTime)o.RoomToDate,
                        FromDateInPension = (DateTime)o.FromDate,
                        ToDateInPension = (DateTime)o.ToDate,
                        DogUserID = o.OrderUserId,
                        UserFirstName = o.UserFirstName,
                        UserLastName = o.UserLastName,
                        UserPhone1 = o.UserPhone1,
                        UserPhone2 = o.UserPhone2,
                        DogBirthDate = (DateTime)d.DogBirthDate,
                        DogComments = d.DogComments,
                        DogDig = (bool)d.DogDig,
                        DogFriendlyWith = (int)d.DogFriendlyWith,
                        DogGender = d.DogGender,
                        DogImage = d.DogImage,
                        DogJump = (bool)d.DogJump,
                        DogName = d.DogName,
                        DogNeuter = (bool)d.DogNeuter,
                        DogRabiesVaccine = (DateTime)d.DogRabiesVaccine,
                        DogShvav = d.DogShvav,
                        DogType = d.DogType,
                        VeterinarName = o.VeterinarName,
                        VeterinarPhone1 = o.VeterinarPhone1,
                        RoomNumberDB =(int) o.RoomNumber,
                      
                    
                        
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
        private int isExist(int dogNumber,List<DogInRoomDetailsView> dogsInRoom)
        {
            for (int i = 0; i < dogsInRoom.Count; i++)
            {
                if (dogsInRoom[i].DogNumber == dogNumber)
                    return i;
            }
            return -1;
        }

        public List<DogInRoomDetailsView> GetDogsNoSetting(DateTime fromDate, DateTime toDate)
        {
            List<DogInRoomDetailsView> dogsInRoom = new List<DogInRoomDetailsView>();
            using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
            {
                
                int rooms = context.RoomsTbl.Count();
                //כל הכלבים המשובצים לחדרים התאריכים המבןקשים
                for (int i = 0; i < rooms; i++)
                {
                    dogsInRoom.AddRange(GetDogsInRoom(i, fromDate, toDate));
                }
           
                OrderService orderService = new OrderService();
                //כל ההזמנות
                List<OrderDetailsViewManager> listOrders= orderService.GetAllOrdersAndDogsManager();
                // כל ההזמנות בתאריכים המבוקשים
                List<OrderDetailsViewManager> listOrderInDates = listOrders.Where(
                                                                       p=>(p.OrderStatus == 12 || p.OrderStatus == 11 || p.OrderStatus == 15)
                                                                      && (p.FromDate >= fromDate && fromDate <= p.ToDate)
                                                                       || (p.ToDate >= fromDate && p.ToDate <= toDate)
                                                                      || (p.FromDate <= fromDate && p.ToDate > toDate)
                                                                     
                                                                     ).ToList();
                List<DogsInOrderView> dogsInOrders = new List<DogsInOrderView>();
                //כל הכלבים בכל ההזמנות בתאריכים המבוקשים
                for (int i = 0; i < listOrderInDates.Count; i++)
                {
                    dogsInOrders.AddRange(orderService.GetDogsForOrder(listOrderInDates[i].OrderNumber));
                }
                List<DogInRoomDetailsView> outOfRoomsList = new List<DogInRoomDetailsView>();
            foreach (DogsInOrderView dogInOrder in dogsInOrders)
            {
                if(isExist(dogInOrder.DogNumber,dogsInRoom)==-1) //כלב בהזמנה ולא משובץ בחדר
                {
                    DogInRoomDetailsView dogOutOfRoom = new DogInRoomDetailsView();
                    dogOutOfRoom.DogComments = dogInOrder.DogComments;
                    dogOutOfRoom.DogBirthDate = dogInOrder.DogBirthDate;
                    dogOutOfRoom.DogDig = dogInOrder.DogDig;
                    // dogOutOfRoom.DogFood=dogInOrder.
                    dogOutOfRoom.DogFriendlyWith = dogInOrder.DogFriendlyWith;
                    dogOutOfRoom.DogGender = dogInOrder.DogGender;
                    dogOutOfRoom.DogImage = dogInOrder.DogImage;
                    dogOutOfRoom.DogJump = dogInOrder.DogJump;
                    dogOutOfRoom.DogName = dogInOrder.DogName;
                    dogOutOfRoom.DogNeuter = dogInOrder.DogNeuter;
                    dogOutOfRoom.DogNumber = dogInOrder.DogNumber;
                    dogOutOfRoom.DogorderNumber = dogInOrder.OrderNumber;
                    dogOutOfRoom.DogRabiesVaccine = dogInOrder.DogRabiesVaccine;
                    dogOutOfRoom.DogShvav = dogInOrder.DogShvav;
                    dogOutOfRoom.DogStatus = dogInOrder.DogStatus;
                    dogOutOfRoom.DogTraining = dogInOrder.DogTraining;
                    dogOutOfRoom.DogType = dogInOrder.DogType;
                    dogOutOfRoom.DogUserID = dogInOrder.DogUserID;
                    OrderDetailsViewManager orderDetailsViewManager = listOrderInDates.Find(o => o.OrderNumber == dogOutOfRoom.OrderNumber);
                    dogOutOfRoom.FromDateInPension = orderDetailsViewManager.FromDate;
                    dogOutOfRoom.ToDateInPension = orderDetailsViewManager.ToDate;
                    dogOutOfRoom.ToDateInRoom= orderDetailsViewManager.ToDate;
                    dogOutOfRoom.UserFirstName = orderDetailsViewManager.userFirstName;
                    dogOutOfRoom.UserLastName = orderDetailsViewManager.userLastName;
                    var user = context.UsersTbl.Find(dogOutOfRoom.DogUserID);
                   dogOutOfRoom.UserPhone1 = user.UserPhone1;
                        dogOutOfRoom.UserPhone2 = user.UserPhone2;
                        var vet= context.veterinarTbl.Find(user.UserVeterinarId);
                        dogOutOfRoom.VeterinarName = vet.VeterinarName;
                        dogOutOfRoom.VeterinarPhone1 = vet.VeterinarPhone1;
                        dogOutOfRoom.RoomNumberDB = 0;
                        outOfRoomsList.Add(dogOutOfRoom);

                }
            }
                return outOfRoomsList;
            }
            
            }
        public void UpdateRoomsSetting(List<RoomsDetailsView> list)
        {

        }
    }
}