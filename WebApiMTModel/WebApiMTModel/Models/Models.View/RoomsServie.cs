using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApiMTModel.Models;

namespace WebApiMTModel.Models.Models.View
{
    public class RoomsServie
    {
        public void AddRoom(RoomsDetailsView roomsDetailsView)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    RoomsTbl roomsTbl = new RoomsTbl();
                    if (roomsDetailsView.RoomCapacity > 0)
                        roomsTbl.RoomCapacity = roomsDetailsView.RoomCapacity;
                    else
                        roomsTbl.RoomCapacity = 10;
                    if (roomsDetailsView.RoomComments != "")
                        roomsTbl.RoomComments = roomsDetailsView.RoomComments;
                    if(roomsDetailsView.RoomDescription!="")
                    roomsTbl.RoomDescription = roomsDetailsView.RoomDescription;
                    roomsTbl.RoomStatus = 21;
                    
                 
                    context.RoomsTbl.Add(roomsTbl);
                    context.SaveChanges();

                }

            }
            catch (SqlException ex)
            { throw ex; }

        }
        public List<RoomsDetailsView> GetRoomsSetting(DateTime date, int  shift)
        {
            try
            {
                Entities context = new Entities();

                List<RoomsDetailsView> list = context.RoomsTbl.
                     Join(context.StatusTbl,
                     u => u.RoomStatus, v => v.StatusId,
                     (u, v) => new RoomsDetailsView
                     {
                         RoomID = u.RoomID,
                         RoomCapacity = u.RoomCapacity,
                         RoomComments = u.RoomComments,
                         RoomDescription = u.RoomDescription,
                         RoomStatus = u.RoomStatus,
                         RoomStatusName = v.StatusName,


                     }).Where(room => room.RoomStatus == 21)
                     .Distinct()
                         .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].dogsInRoom = GetDogsInRoom(list[i].RoomID, date, shift);

                }




                return list;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //public List<RoomsDetailsView> GetRoomsSetting(DateTime fromDate,DateTime toDate,int fromShift,int toShift)
        //{
        //    try
        //    {
        //        Entities context = new Entities();

        //        List<RoomsDetailsView> list = context.RoomsTbl.
        //             Join(context.StatusTbl,
        //             u => u.RoomStatus, v => v.StatusId,
        //             (u, v) => new RoomsDetailsView
        //             {
        //               RoomID=  u.RoomID,
        //               RoomCapacity=  u.RoomCapacity,
        //                RoomComments= u.RoomComments,
        //                RoomDescription= u.RoomDescription,
        //                RoomStatus= u.RoomStatus,
        //                RoomStatusName = v.StatusName,
                        

        //             }).Where(room => room.RoomStatus == 21)
        //             .Distinct()
        //                 .ToList();
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            list[i].dogsInRoom = GetDogsInRoom(list[i].RoomID, fromDate, toDate);
                    
        //        }


        //        DateTime dt = fromDate;
        //        int shift = fromShift;
        //        while (dt.CompareTo(toDate) < 0)
        //        {
        //            list[i].dogsInRoom = GetDogsInRoom(list[i].RoomID, dt, shift);
        //            shift++;
        //            if (shift == 3) { shift = 1; dt.AddDays(1); }
        //        }
        //        if (fromDate.CompareTo(toDate) == 0)
        //        {
        //            if (fromShift == toShift)

        //            }

        //        return list;
                

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        //מחזיר רשימת כלבים הנמצאים בחדר ביום מסויים במשמרת מסויימת
        private List<DogInRoomDetailsView> GetDogsInRoom(int roomNumber, DateTime date, int shift)
        {
            try
            {
                Entities context = new Entities();

                List<DogInRoomDetailsView> list = context.RoomSetting.
                    Join(context.OrdersTbl,
                     u => u.OrderNumber, v => v.OrderNumber,
                     (u, v) => new
                     {
                         u.Id,
                         u.Comments,
                         u.DogNumber,
                         v.FromDate,
                         u.RoomNumber,
                         v.ToDate,
                         v.OrderUserId,
                         u.RoomFromDate,
                         u.RoomToDate,
                         v.OrderNumber,
                         u.RoomShiftFrom,
                         u.RoomShiftTo,
                     }).Where(u => ((u.RoomFromDate < date.Date && u.RoomToDate > date.Date && u.RoomNumber == roomNumber) || ((u.RoomFromDate == date.Date && u.RoomShiftFrom <= shift && u.RoomNumber == roomNumber) || (u.RoomToDate == date.Date && u.RoomShiftTo >= shift && u.RoomNumber == roomNumber))))
                    .Join(context.UsersTbl,
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
                        o.OrderNumber,
                        o.OrderUserId,
                        o.RoomFromDate,
                        o.RoomToDate,
                        o.RoomNumber,
                        o.RoomShiftFrom,
                        o.RoomShiftTo,
                        o.Id,
                    }).
                    Join(context.UserDogs,
                    o => o.DogNumber, d => d.DogNumber,
                    (o, d) => new DogInRoomDetailsView
                    {
                        id = o.Id,
                        Comments = o.Comments,
                        DogNumber = o.DogNumber,
                        FromDateInRoom = o.RoomFromDate,
                        ToDateInRoom = o.RoomToDate,
                        FromDateInPension = (DateTime)o.FromDate,
                        ToDateInPension = (DateTime)o.ToDate,
                        DogUserID = o.OrderUserId,
                        UserFirstName = o.UserFirstName,
                        UserLastName = o.UserLastName,
                        UserPhone1 = o.UserPhone1,
                        UserPhone2 = o.UserPhone2,
                        DogBirthDate = d.DogBirthDate,
                        DogComments = d.DogComments,
                        DogDig = (bool)d.DogDig,
                        DogFriendlyWith = (int)d.DogFriendlyWith,
                        DogGender = d.DogGender,
                        DogImage = d.DogImage,
                        DogJump = (bool)d.DogJump,
                        DogName = d.DogName,
                        DogNeuter = d.DogNeuter,
                        DogRabiesVaccine = d.DogRabiesVaccine,
                        DogShvav = d.DogShvav,
                        DogType = d.DogType,
                        VeterinarName = o.VeterinarName,
                        VeterinarPhone1 = o.VeterinarPhone1,
                        RoomNumberDB = o.RoomNumber,
                        ManagerComments = d.ManagerComments,
                        OrderNumber = o.OrderNumber,
                        RoomShiftFrom = (int)o.RoomShiftFrom,
                        RoomShiftTo = (int)o.RoomShiftTo,

                    }).Distinct().ToList();

            //    if (shift != 0)
            //    { 
            //    list= list.FindAll(item => item.RoomShiftFrom==shift);
            //}

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
                Entities context = new Entities();
                List<DogInRoomDetailsView> list = context.RoomSetting.
                    Join(context.OrdersTbl,
                     u => u.OrderNumber, v => v.OrderNumber,
                     (u, v) => new
                     {
                         u.Id,
                         u.Comments,
                         u.DogNumber,
                         v.FromDate,
                         u.RoomNumber,
                         v.ToDate,
                         v.OrderUserId,
                         u.RoomFromDate,
                         u.RoomToDate,
                         //u.RoomShiftFrom,
                         //u.RoomShiftTo,
                         v.OrderNumber,
                     }

                    ).Where(u => u.RoomFromDate <= fromDate.Date && u.RoomToDate >= toDate.Date && u.RoomNumber == roomNumber).
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
                        o.OrderNumber,
                        o.OrderUserId,
                        o.RoomFromDate,
                        o.RoomToDate,
                        o.RoomNumber,
                        o.Id,
                    }).
                    Join(context.UserDogs,
                    o => o.DogNumber, d => d.DogNumber,
                    (o, d) => new DogInRoomDetailsView
                    {
                        id=o.Id,
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
                        ManagerComments=d.ManagerComments,
                        OrderNumber=o.OrderNumber,
                        
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
                Entities context = new Entities();
                RoomSetting roomSetting = new RoomSetting();
                roomSetting.Comments = dog.Comments;
                roomSetting.DogNumber = dog.DogNumber;
                roomSetting.RoomNumber = roomNumber;
                roomSetting.OrderNumber = dog.OrderNumber;
                roomSetting.RoomFromDate = dog.FromDateInRoom;
                roomSetting.RoomToDate = dog.ToDateInPension;
                roomSetting.RoomShiftFrom = dog.RoomShiftFrom;
                roomSetting.RoomShiftTo = dog.RoomShiftTo;
                context.RoomSetting.Add(roomSetting);
                context.SaveChanges();


            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateRoomsDetailsAndSetting(List<RoomsDetailsView> listRoomsDetails)

        {
            try
            {
                using (Entities context = new Entities())
                {
                    foreach (RoomsDetailsView  room in listRoomsDetails)
                    {
                        var roomt = context.Set<RoomsTbl>().Find(room.RoomID);

                        if (roomt != null)
                        {

                            context.Entry(roomt).CurrentValues.SetValues(room);
                           
                            foreach (DogInRoomDetailsView dog in room.dogsInRoom)
                            {
                                if (dog.id != 0)
                                {
                                    var dogt = context.Set<RoomSetting>().Find(dog.id);
                                    if (dog.RoomNumberDB != room.RoomID)
                                    {
                                        RemoveDogFromRoom(dog, dog.RoomNumberDB);
                                        AddDogToRoom(dog, room.RoomID);
                                    }
                                   
                                }
                                else
                                {
                                    AddDogToRoom(dog, room.RoomID);
                                }
                                

                            }
                            context.SaveChanges();

                        }
                        
                    }
                    context.Dispose();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void RemoveDogFromRoom(DogInRoomDetailsView dog, int RoomNumber)
        {
            try
            {

                using (Entities context = new Entities())
                {
                    //context.Blogs
                    //.Where(b => b.Name == "ADO.NET Blog")
                    //.FirstOrDefault();
                    var dogt = context.RoomSetting.Where(d => d.RoomNumber == RoomNumber && d.DogNumber==dog.DogNumber && d.OrderNumber==dog.OrderNumber).First();
                    dog.RoomShiftTo =(int) dogt.RoomShiftTo;
                    dog.ToDateInRoom = (DateTime)dogt.RoomToDate;

                    if (dogt != null)
                    {
                        if (dog.RoomShiftFrom == 1)   //אם הוצאתי כלב במשמרת בוקר, הוא היה בחדר הקודם עד אתמול במשמרת ערב
                        {
                            dogt.RoomToDate = dog.FromDateInRoom.AddDays(-1);
                            dogt.RoomShiftTo = 2;
                        }
                        else //RoomShiftFrom==2   //אם הוצאתי כלב במשמרת ערב, הוא היה בחדר הקודם עד היום במשמרת בוקר
                        {
                            dogt.RoomToDate = dog.FromDateInRoom;
                            dogt.RoomShiftTo = 1;
                        }
                           
                      //  context.Entry(dogt).CurrentValues.SetValues(dog);
                        //   context.Entry(dogt).State = EntityState.Deleted;
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

        public void RemoveDogFromFutureRoomSetting(DogDetailsView dog)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    context.RoomSetting.RemoveRange(context.RoomSetting.Where(x => x.DogNumber == dog.DogNumber && x.RoomFromDate>DateTime.Now));
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //כל הכלבים בכל ההזמנות בתאריך והמשמרת המבוקשים
        public List<DogInRoomDetailsView> GetDogsNoSetting(DateTime date, int shift)
       
            {
                List<DogInRoomDetailsView> dogsInRoom = new List<DogInRoomDetailsView>();
                using (Entities context = new Entities())
                {

                    int rooms = context.RoomsTbl.Count();
                    //כל הכלבים המשובצים לחדרים בתאריכים המבןקשים

                    for (int i = 1; i <= rooms; i++)
                    {
                        dogsInRoom.AddRange(GetDogsInRoom(i, date,shift));
                    }

                    OrderService orderService = new OrderService();
                    //כל ההזמנות
                    List<OrderDetailsViewManager> listOrders = orderService.GetAllOrdersAndDogsManager();
                    // כל ההזמנות בתאריך והמשמרת המבוקשים
                    List<OrderDetailsViewManager> listOrderInDates = listOrders.Where(
                                                                           p => (p.OrderStatus == 12 || p.OrderStatus == 15)
                                                                          && (p.FromDate < date && p.ToDate > date)
                                                                           || (p.FromDate == date && p.ShiftNumberFrom<=shift)
                                                                          || (p.ToDate == date && p.ShiftNumberTo >= shift)

                                                                         ).ToList();
                    List<DogsInOrderView> dogsInOrders = new List<DogsInOrderView>();
                    //כל הכלבים בכל ההזמנות בתאריך והמשמרת המבוקשים 
                    for (int i = 0; i < listOrderInDates.Count; i++)
                    {
                        dogsInOrders.AddRange(orderService.GetDogsForOrder(listOrderInDates[i].OrderNumber));
                    }

                    List<DogInRoomDetailsView> outOfRoomsList = new List<DogInRoomDetailsView>();
                    foreach (DogsInOrderView dogInOrder in dogsInOrders)
                    {
                        if (isExist(dogInOrder.DogNumber, dogsInRoom) == -1) //כלב בהזמנה ולא משובץ בחדר
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
                            dogOutOfRoom.OrderNumber = dogInOrder.OrderNumber;

                            OrderDetailsViewManager orderDetailsViewManager = listOrderInDates.Find(o => o.OrderNumber == dogOutOfRoom.OrderNumber);
                            dogOutOfRoom.DogUserID = orderDetailsViewManager.Userid;
                            dogOutOfRoom.FromDateInPension = orderDetailsViewManager.FromDate;
                            dogOutOfRoom.ToDateInPension = orderDetailsViewManager.ToDate;
                            dogOutOfRoom.ToDateInRoom = orderDetailsViewManager.ToDate;
                            dogOutOfRoom.RoomShiftTo = orderDetailsViewManager.ShiftNumberTo;
                            dogOutOfRoom.UserFirstName = orderDetailsViewManager.userFirstName;
                            dogOutOfRoom.UserLastName = orderDetailsViewManager.userLastName;
                            var user = context.UsersTbl.Find(dogOutOfRoom.DogUserID);
                            dogOutOfRoom.UserPhone1 = user.UserPhone1;
                            dogOutOfRoom.UserPhone2 = user.UserPhone2;
                            var vet = context.veterinarTbl.Find(user.UserVeterinarId);
                            dogOutOfRoom.VeterinarName = vet.VeterinarName;
                            dogOutOfRoom.VeterinarPhone1 = vet.VeterinarPhone1;
                            dogOutOfRoom.RoomNumberDB = 0;
                            outOfRoomsList.Add(dogOutOfRoom);

                        }
                    }
                    return outOfRoomsList;
                }

            }
        //public List<DogInRoomDetailsView> GetDogsNoSetting(DateTime fromDate, DateTime toDate)
        //{
        //    List<DogInRoomDetailsView> dogsInRoom = new List<DogInRoomDetailsView>();
        //    using (Entities context = new Entities())
        //    {

        //        int rooms = context.RoomsTbl.Count();
        //        //כל הכלבים המשובצים לחדרים בתאריכים המבןקשים

        //        for (int i = 0; i < rooms; i++)
        //        {
        //            dogsInRoom.AddRange(GetDogsInRoom(i, fromDate, toDate));
        //        }

        //        OrderService orderService = new OrderService();
        //        //כל ההזמנות
        //        List<OrderDetailsViewManager> listOrders= orderService.GetAllOrdersAndDogsManager();
        //        // כל ההזמנות בתאריכים המבוקשים
        //        List<OrderDetailsViewManager> listOrderInDates = listOrders.Where(
        //                                                               p=>(p.OrderStatus == 12 || p.OrderStatus == 11 || p.OrderStatus == 15)
        //                                                              && (p.FromDate >= fromDate &&  p.ToDate<= fromDate)
        //                                                               || (p.ToDate >= fromDate && p.ToDate <= toDate)
        //                                                              || (p.FromDate <= fromDate && p.ToDate > toDate)

        //                                                             ).ToList();


        //        List<DogsInOrderView> dogsInOrders = new List<DogsInOrderView>();
        //        //כל הכלבים בכל ההזמנות בתאריכים המבוקשים
        //        for (int i = 0; i < listOrderInDates.Count; i++)
        //        {
        //            dogsInOrders.AddRange(orderService.GetDogsForOrder(listOrderInDates[i].OrderNumber));
        //        }
        //        List<DogInRoomDetailsView> outOfRoomsList = new List<DogInRoomDetailsView>();
        //    foreach (DogsInOrderView dogInOrder in dogsInOrders)
        //    {
        //        if(isExist(dogInOrder.DogNumber,dogsInRoom)==-1) //כלב בהזמנה ולא משובץ בחדר
        //        {
        //            DogInRoomDetailsView dogOutOfRoom = new DogInRoomDetailsView();
        //            dogOutOfRoom.DogComments = dogInOrder.DogComments;
        //            dogOutOfRoom.DogBirthDate = dogInOrder.DogBirthDate;
        //            dogOutOfRoom.DogDig = dogInOrder.DogDig;
        //            // dogOutOfRoom.DogFood=dogInOrder.
        //            dogOutOfRoom.DogFriendlyWith = dogInOrder.DogFriendlyWith;
        //            dogOutOfRoom.DogGender = dogInOrder.DogGender;
        //            dogOutOfRoom.DogImage = dogInOrder.DogImage;
        //            dogOutOfRoom.DogJump = dogInOrder.DogJump;
        //            dogOutOfRoom.DogName = dogInOrder.DogName;
        //            dogOutOfRoom.DogNeuter = dogInOrder.DogNeuter;
        //            dogOutOfRoom.DogNumber = dogInOrder.DogNumber;
        //            dogOutOfRoom.DogorderNumber = dogInOrder.OrderNumber;
        //            dogOutOfRoom.DogRabiesVaccine = dogInOrder.DogRabiesVaccine;
        //            dogOutOfRoom.DogShvav = dogInOrder.DogShvav;
        //            dogOutOfRoom.DogStatus = dogInOrder.DogStatus;
        //            dogOutOfRoom.DogTraining = dogInOrder.DogTraining;
        //            dogOutOfRoom.DogType = dogInOrder.DogType;
        //            dogOutOfRoom.DogUserID = dogInOrder.DogUserID;
        //            dogOutOfRoom.OrderNumber = dogInOrder.OrderNumber;

        //            OrderDetailsViewManager orderDetailsViewManager = listOrderInDates.Find(o => o.OrderNumber == dogOutOfRoom.OrderNumber);
        //            dogOutOfRoom.DogUserID = orderDetailsViewManager.Userid;
        //            dogOutOfRoom.FromDateInPension = orderDetailsViewManager.FromDate;
        //            dogOutOfRoom.ToDateInPension = orderDetailsViewManager.ToDate;
        //            dogOutOfRoom.ToDateInRoom= orderDetailsViewManager.ToDate;
        //            dogOutOfRoom.RoomShiftTo = orderDetailsViewManager.ShiftNumberTo;
        //            dogOutOfRoom.UserFirstName = orderDetailsViewManager.userFirstName;
        //            dogOutOfRoom.UserLastName = orderDetailsViewManager.userLastName;
        //            var user = context.UsersTbl.Find(dogOutOfRoom.DogUserID);
        //           dogOutOfRoom.UserPhone1 = user.UserPhone1;
        //                dogOutOfRoom.UserPhone2 = user.UserPhone2;
        //                var vet= context.veterinarTbl.Find(user.UserVeterinarId);
        //                dogOutOfRoom.VeterinarName = vet.VeterinarName;
        //                dogOutOfRoom.VeterinarPhone1 = vet.VeterinarPhone1;
        //                dogOutOfRoom.RoomNumberDB = 0;
        //                outOfRoomsList.Add(dogOutOfRoom);

        //        }
        //    }
        //        return outOfRoomsList;
        //    }

        //    }

       
            public void UpdateRoomsSetting(List<RoomsDetailsView> list)
        {

        }
    }
}