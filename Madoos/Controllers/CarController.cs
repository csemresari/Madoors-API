using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Madoors.Models;

namespace Madoors.Controllers
{
    public class CarController : BaseAPIController
    {

        // http://localhost:1182/api/car?branchName=Cankaya
        public IEnumerable<CarPassModel> GetCars(string branchName)
        {
           List<CarPassModel> cars = new List<CarPassModel>();

           try
           {
               PersistencyManager db = m_Persistency_Manager;

               BRANCH testBranch = db.BRANCH.Where(br => br.branchName == branchName).FirstOrDefault();

               if (testBranch != null)
               {
                   if (db != null)
                   {
                       cars = getCars(db, testBranch.branchID);
                   }
               }
           }
           catch (Exception)
           {
               cars = null;
           }

           return cars;
        }

        public List<CarPassModel> getCars(PersistencyManager db, Guid branchGuid)
        {
           List<CarPassModel> carList = new List<CarPassModel>();

           try
           {
               var tempList = (from cp in db.CARPASS
                               where cp.branchID == branchGuid
                               select new
                               {
                                   carPassID = cp.carPassID,
                                   branchID = cp.branchID,
                                   plateNo = cp.plateNo,
                                   state = cp.state,
                                   image = cp.image
                               }).ToList();

               foreach (var item in tempList)
               {
                   CarPassModel carModel = new CarPassModel();

                   carModel.carPassID = item.carPassID;
                   carModel.branchID = item.branchID.ToString();
                   carModel.plateNo = item.plateNo;
                   carModel.state = item.state;
                   carModel.image = item.image;

                   carList.Add(carModel);
               }
           }
           catch (Exception exc)
           {
               Console.WriteLine(exc.StackTrace);
           }

           return carList;
        }

        [HttpPost]  //http://localhost:1182/api/car?branchName=Cankaya...
        public bool Post(String entranceID, [FromBody]CarPassModel model)
        {
            try
            {
                PersistencyManager db = m_Persistency_Manager;

                Guid entranceGuid = new Guid(entranceID.ToString());
                ENTRANCE testEntrance = db.ENTRANCE.Where(br => br.entranceName == model.entranceName && br.entranceID == entranceGuid).FirstOrDefault();

                if (testEntrance != null && testEntrance.entranceName == model.entranceName)
                {

                    var list = db.CARPASS.Where(br => br.entranceID == entranceGuid);

                    foreach (var item in list.ToList())
                    {
                        CARPASS delete = db.CARPASS.Where(br => br.entranceID == item.entranceID).FirstOrDefault();
                        db.CARPASS.DeleteObject(delete);
                        db.SaveChanges();
                    }

                    CARPASS carPass = new CARPASS();
                    carPass.carPassID = Guid.NewGuid();
                    carPass.entranceID = testEntrance.entranceID;

                    carPass.plateNo = model.plateNo;
                    carPass.state = model.state;
                    carPass.image = model.image;
                    carPass.time = model.time;
                    carPass.dbCreationTime = DateTime.Now;

                    db.CARPASS.AddObject(carPass);
                    db.SaveChanges();

                    PushNotification(testEntrance.BRANCH.branchName, model.entranceName, "Yeni Araç Geçişi!", carPass.carPassID);
                }

                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }

        public CarPassModel GetTheLastCarPassed(string entranceID, int value)
        {
            CarPassModel car = new CarPassModel();

            try
            {
                PersistencyManager db = m_Persistency_Manager;

                Guid entranceGuid = new Guid(entranceID.ToString());
                ENTRANCE testEntrance = db.ENTRANCE.Where(br => br.entranceID == entranceGuid).FirstOrDefault();

                if (testEntrance != null)
                {
                    if (db != null)
                    {
                        Guid branchGuid = new Guid(testEntrance.branchID.ToString());

                        car = getCar(db, branchGuid, value);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                car = null;
            }

            return car;
        }

        public CarPassModel getCar(PersistencyManager db, Guid branchGuid, int value)
        {
            CarPassModel carModel = new CarPassModel();
             
            try
            {
                if (value == 1)
                {
                    var tempList = (from cp in db.CARPASS
                                    where cp.ENTRANCE.branchID == branchGuid
                                    orderby cp.dbCreationTime descending
                                    select new
                                    {
                                        carPassID = cp.carPassID,
                                        entranceID = cp.entranceID,
                                        plateNo = cp.plateNo,
                                        state = cp.state,
                                        image = cp.image,
                                        time = cp.time,
                                        entranceName = cp.ENTRANCE.entranceName
                                    }).ToList();

                    carModel.carPassID = tempList[0].carPassID;
                    carModel.entranceID = tempList[0].entranceID.ToString();
                    carModel.plateNo = tempList[0].plateNo;
                    carModel.state = tempList[0].state;
                    carModel.image = tempList[0].image;
                    carModel.time = tempList[0].time;
                    carModel.entranceName = tempList[0].entranceName;
                }
                else
                {
                    var tempList = (from cp in db.CARPASS
                                    where cp.ENTRANCE.branchID == branchGuid
                                    orderby cp.dbCreationTime ascending
                                    select new
                                    {
                                        carPassID = cp.carPassID,
                                        entranceID = cp.entranceID,
                                        plateNo = cp.plateNo,
                                        state = cp.state,
                                        image = cp.image,
                                        time = cp.time,
                                        entranceName = cp.ENTRANCE.entranceName
                                    }).ToList();


                    carModel.carPassID = tempList[0].carPassID;
                    carModel.entranceID = tempList[0].entranceID.ToString();
                    carModel.plateNo = tempList[0].plateNo;
                    carModel.state = tempList[0].state;
                    carModel.image = tempList[0].image;
                    carModel.time = tempList[0].time;
                    carModel.entranceName = tempList[0].entranceName;
                }
                
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.StackTrace);
            }

            return carModel;
        }
    }
}
        

 
