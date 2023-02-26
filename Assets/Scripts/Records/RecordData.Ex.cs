using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using Cfg.railway;
using UnityEngine;

namespace MobileKit
{
    [Skip]
    public partial class RecordData
    {
        public int GameHour = 8;
        public int GameMinute = 59;

        public int CurrentRoads = 1;
        public bool newRoadUnlock;


        public bool isFirstLogin = true;
        
        public bool FirstBoxTrain = true;

        public int increaseProfitWatchedVideoNum = 0;
        public bool profitDiamondGet = false;

        public DateTime increaseProfitStopTime;
        public DateTime profitcdTime;
        public DateTime QuickPassAdWatchMoment;
        
        public int MaxTaskIndex;
        public Dictionary<int, int> CurTaskIndex = new Dictionary<int, int>();

        public bool offLineManager;
        public int offLineTime = 1;
        public Dictionary<string, int> OffLineMan = new Dictionary<string, int>();
        
        //1f-1.5f
        public float profitAdd = 1f;
        public Dictionary<string, float> SpBuildings = new Dictionary<string, float>();
        

        
        private int cash = 5000;

        public int Cash
        {
            get => cash;
            set
            {
                int delta = value - cash;
                if (RecordManager.Instance.Initted && delta > 0)
                {
                    // TaskManager.CashRevenue += delta;
                }
                cash = Mathf.Min(value, 1000000000);
                Messenger<int>.Broadcast(GameEvent.ON_CASH_UPDATE, cash);
            }
        }
        
        private int diamond = 30;

        public int Diamond
        {
            get => diamond;
            set
            {
                diamond = Mathf.Min(value, 1000000000);
                // Messenger<int>.Broadcast(GameEvent.ON_DIAMOND_UPDATE, diamond);
            }
        }

        private int adTicket = 0;

        public int AdTicket
        {
            get => adTicket;
            set
            {
                adTicket = Mathf.Min(value, 1000000000);
            }
        }

        public int CommonKeyCount = 0;
        public int RareKeyCount = 0;
        public int EpicKeyCount = 0;




        public string lastSynDate = "";
        public DateTime firstLoginDate = DateTime.MinValue;
        public DateTime lastRefreshOpenBoxTime = DateTime.MinValue;
        public DateTime lastOpenBoxTime = DateTime.MinValue;
        public DateTime lastTrainPackTime = DateTime.MinValue;
        public DateTime lastSpPackTime = DateTime.MinValue;
        public int freeBoxCount = 6;
        public int GetFreeBoxCount(){
            return freeBoxCount;
        }
        public void SetFreeBoxCount( int count ){
            freeBoxCount = count;
        }
        public void AddFreeBoxCount( int count ){
            freeBoxCount += count;
            if (freeBoxCount<0)
            {
                freeBoxCount = 0;
            }
        }

        public void SetNewDate( string dateStr ){
            Debug.Log($" SetNewDate dateStr : {dateStr} ");
            lastSynDate = dateStr;
            lastTrainPackTime = DateTime.MinValue;
        }


        

            
        public bool IsFirstDay() {
            if ( firstLoginDate == DateTime.MinValue )
            {
                return false;
            }
            TimeSpan ts = DateTime.Now.Subtract(firstLoginDate);
            return ts.TotalSeconds <= 86400;
        }

        public bool IsIn3Day() {
            if ( firstLoginDate == DateTime.MinValue )
            {
                return false;
            }
            TimeSpan ts = DateTime.Now.Subtract(firstLoginDate);
            return ts.TotalSeconds <= 259200;
        }
        
        public DateTime LastAcceBubbleTime;


        public List<int> unlockedSpecialFacilityIds;

        public bool IsSpecialFacilityUnlocked(int specialFacilityId)
        {
            unlockedSpecialFacilityIds ??= new List<int>();
            if (unlockedSpecialFacilityIds.Contains(specialFacilityId))
            {
                return true;
            }
            return false;
        }
        

        public bool UnlockSpecialFacilityId(int specialFacilityId)
        {
            unlockedSpecialFacilityIds ??= new List<int>();
            if (!unlockedSpecialFacilityIds.Contains(specialFacilityId))
            {
                unlockedSpecialFacilityIds.Add(specialFacilityId);
                return true;
            }
            return false;
        }
            
        
        
        
        // trainId -> info 
        public Dictionary<int, TrainRecordData> TrainRecordData = new Dictionary<int, TrainRecordData>();
        
        //setDestinationPage
        //未指派线路的火车
        public List<int> availableHorizontalTrainRecordData = new List<int>();
        //指派线路的火车 包含未指派时刻的
        public List<List<int>> onRoadsTrainsRecordData = new List<List<int>>();
        
        // //timetablePage
        // //未指派时刻的火车
        // public List<int> availableTimeTableTrainRecordData = new List<int>();
        // //指派时刻的火车
        // public List<int> timeTableTrainRecordData = new List<int>();
        
        

        public bool IsTrainExist(int trainID)
        {
            if (TrainRecordData.TryGetValue(trainID, out var trainData)) return true;
            else
            {
                return false;
            }
        }

        
        /// <summary>
        /// 实载插值
        /// </summary>
        public List<int> customerOffset = new List<int>();
        public List<float> seatPercent = new List<float>();
        
        /// <summary>
        /// 当前时刻乘客数量
        /// </summary>
        public List<int> _customFlowInClockList = new List<int>();
        public List<float> _seatPercentInClockList = new List<float>();

        /// <summary>
        /// Facility Id -> Count
        /// </summary>
        public Dictionary<int, int> StarSpawnCount = new Dictionary<int, int>();

        private int starCollectCount;
        public int StarCollectCount
        {
            get
            {
                return starCollectCount;
            }
            set
            {
                starCollectCount = value;
            }
        }

        
    }
    
    
    
    


    
    [Serializable]
    public class DistrictRecordData
    {
        
        /// <summary>
        /// UTC Time
        /// </summary>
        public DateTime BuildStartTime;
        
        /// <summary>
        /// UTC Time
        /// </summary>
        public DateTime BuildCompleteTime;
        
        public Dictionary<int, int> FacilitiesLevel = new Dictionary<int, int>();

        public int GetFacilityLevel(int facilityId)
        {
            if (!FacilitiesLevel.TryGetValue(facilityId, out int level))
            {
                // level = TableManager.TbFacilities.GetById(facilityId).InitLevel;
                FacilitiesLevel.Add(facilityId, 0);
            }
            return level;
        }

        public void SetFacilityLevel(int facilityId, int level)
        {
            FacilitiesLevel[facilityId] = level;
        }
    }
    
    [Serializable]
    public class TrainRecordData
    {
        public int trainId;
        public int stars = 0;
        public int count = 0;
        public int eliteRoadId;
        
        
        public bool isBoarding;
        public bool isOnRoad;
        
        
        
        public bool isOnTimeTable;
        public int curRoadId = -1;
        //从0开始 显示从1开始需要+1
        public int roadClock = -1;
        //月台
        public int platformIndex = -1;

        
        
        
        public int qualityId = -1;
    }
    

}