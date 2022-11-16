using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;


/// <summary>
/// 아이모드의 데이터 송수신을 책임짐. 
/// </summary>
public class ChildDataController : MonoBehaviour
{
    [FirestoreData]
    public class GameResult
    {
        [FirestoreProperty]
        public string 시작날짜 { get; set; } = "";

        [FirestoreProperty]
        public string 시작시간 { get; set; } = "";

        [FirestoreProperty]
        public int 레벨 { get; set; } = 1;

        [FirestoreProperty]
        public int 별개수 { get; set; } = 0;

        [FirestoreProperty]
        public int 플레이시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 훈련시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 완성률 { get; set; } = 0;

        [FirestoreProperty]
        public Dictionary<string, float> 호흡기록 { get; set; } = ChildDataController.BreatheResult;

        [FirestoreProperty]
        public Dictionary<string, float> 예상호흡기록 { get; set; } = ChildDataController.ExpectedBreatheResult;
    };


    [FirestoreData]
    public class PointInformation
    {
        [FirestoreProperty]
        public int 현재포인트 { get; set; } = 0;

        [FirestoreProperty]
        public int 목표점수 { get; set; } = 1000;

        [FirestoreProperty]
        public int 레벨 { get; set; } = 1;

        [FirestoreProperty]
        public string 보상제목 { get; set; } = "놀이공원";
    }

    [FirestoreData]
    public class ScheduleInformation
    {

        [FirestoreProperty]
        public string 요일 { get; set; } = "월요일";

        [FirestoreProperty]
        public string 제목 { get; set; } = "";

        [FirestoreProperty]
        public int 시 { get; set; } = 12;

        [FirestoreProperty]
        public int 분 { get; set; } = 0;

        [FirestoreProperty]
        public string 모드 { get; set; } = "표준모드";

        [FirestoreProperty]
        public int 들숨시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 들숨후참는시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 날숨시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 날숨후참는시간 { get; set; } = 0;

        [FirestoreProperty]
        public int 반복횟수 { get; set; } = 0;
    }


    public delegate void updateDelegate();

    static public Dictionary<string, int> RLresult = new Dictionary<string, int>();
    static public Dictionary<string, string> RLresult_str = new Dictionary<string, string>();
    static public Dictionary<string, int> CPresult= new Dictionary<string, int>();
    static public Dictionary<string, string> CPresult_str = new Dictionary<string, string>();

    static FirebaseFirestore db;

    static bool isReceived = false; 
    static bool canSend = false;

    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    static int point=0;

    /// <summary>
    /// 현재 보상을 얻기 위한 목표 점수.
    /// </summary>
    static int goalPoint=1000;

    /// <summary>
    /// 현재 진행 보상 단계. PointShop에서 검은색 점의 개수.
    /// </summary>
    static int level=1;
    /// <summary>
    /// 원 안에 있는 텍스트. 가령, "놀이공원".  
    /// </summary>
    static string rewardTitle="놀이공원";

    /// <summary>
    /// 레벨에 따른 보상 내용들.
    /// </summary>
    public static List<string> rewardTitleList = new List<string>();

    /// <summary>
    /// 현재 점수/목표 점수
    /// </summary>
    static float progressRatio=point/(float) goalPoint;


    /// <summary>
    /// 유저 ID.
    /// </summary>
    static string childID = "001";

    /// <summary>
    /// 부모 ID.
    /// </summary>
    public static string parentID = "001";

    /// <summary>
    /// 시작날짜 = "", 시작시간 = "", 레벨 = 0, 별개수 = starNum, 플레이시간 = 0, 호흡기록, 예상호흡기록
    /// </summary>
    public static GameResult fishGameResult = new GameResult();

    public static List<GameResult> fishGameResultList = new List<GameResult>();

    /// <summary>
    /// 호흡 결과는 여기에 기록해서 SendGameResult로 보낼 것. timestamp와 0~1 사이의 넣으면 됨. 
    /// </summary>
    public static Dictionary<string, float> BreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// correctHookPos는 여기에 기록해서 SendGameResult로 보낼 것. timestamp와 0~1 사이의 넣으면 됨. 
    /// </summary>
    public static Dictionary<string, float> ExpectedBreatheResult = new Dictionary<string, float> { { "0", 0 }, };


    public static List<ScheduleInformation> scheduleInformationList = new List<ScheduleInformation>();

    /// <summary>
    /// isReceived (bool), canSend(bool), point (int), level(int), rewardTitle (string), goalPoint (int), progressRatio (float), childID (string), parentID (string) 를 담은 Dictionary 반환.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, object> getValues()
    {

        Dictionary<string, object> A = new Dictionary<string, object>();
        A.Add("isReceived", isReceived);
        A.Add("canSend", canSend);
        A.Add("point", point);
        A.Add("level", level);
        A.Add("goalPoint", goalPoint);
        A.Add("rewardTitle", rewardTitle);
        A.Add("rewardTitleList", rewardTitleList);
        A.Add("progressRatio", progressRatio);
        A.Add("childID", childID);
        A.Add("parentID", parentID);

        return A;
    }

    /// <summary>
    /// true로 설정해야 보낼 수 있음. 
    /// </summary>
    /// <param name="canSend"></param>
    public static void setCanSend(bool canSend)
    {
       ChildDataController.canSend=canSend;
    }

    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    public static void setPoint(int pt)
    {
        point = pt;
    }

    /// <summary>
    /// 아이 모드의 point를 더함. 레벨업, 보상 내용까지 알아서 수정해줌. 보상 내용은 포인트샵을 가야 초기화됨. 
    /// </summary>
    /// <param name="pt"></param>
    public static void addPoint(int pt)
    {
        point = point+pt;
        if (point > goalPoint && level-1<rewardTitleList.Count)
        {
            level = level + 1;
            rewardTitle = rewardTitleList[level - 1];
        }
    }


    /// <summary>
    /// 현재 보상을 얻기 위한 목표 점수.
    /// </summary>
    public static void setGoalPoint(int pt)
    {
        goalPoint = pt;
    }


    /// <summary>
    /// 현재 진행 보상 단계. PointShop에서 검은색 점의 개수.
    /// </summary>
    public static void setLevel(int lv)
    {
        level = lv;
    }

    public static void setRewardTitle(string title)
    {
        rewardTitle = title;
    }

    public static void setRewardTitleList(List<string> titleList)
    {
        rewardTitleList.Clear();
        foreach(string title in titleList)
        {
            rewardTitleList.Add(title);
        }
    }

    /// <summary>
    /// 현재 점수/목표 점수
    /// </summary>
    public static void setProgressRatio(float ratio)
    {
        progressRatio = ratio;
    }


    /// <summary>
    /// 유저 ID.
    /// </summary>
    public static void setChildID(string id)
    {
        childID = id;
    }

    public static void setParentID(string id)
    {
        parentID = id;
    }


    /// <summary>
    /// 서버에 포인트, level, rewardTitle, pointString, 진행률, childID를 보냄. 
    /// </summary>
    public static void SendPoint()
    {
        if (!canSend)
        {
            Debug.Log("Not yet prepared.");
        }

        Debug.Log("Send point.");
        DocumentReference docRef = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("CurrentPoint");
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "현재포인트", point},
                { "레벨", level },
                { "보상제목", rewardTitle },
                { "목표점수", goalPoint },
        };

        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            
            if (task.IsCompleted)
            {
                Debug.Log("Added data to the document in the users collection.");
            }
            else { Debug.Log("Failed"); }
        });
    }

    public static void SendGameResult()
    {
        fishGameResult.호흡기록 = BreatheResult;
        fishGameResult.예상호흡기록 = ExpectedBreatheResult;

        Debug.Log( "Keys : "+fishGameResult.호흡기록.Keys.ToString());

        if (!canSend)
        {
            Debug.Log("Not yet prepared.");
        }

        Debug.Log("Send point for GameResult");

        string today = fishGameResult.시작날짜;       
        Debug.Log(today);
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("시작날짜", today);        

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {     
            
            QuerySnapshot todayQuerySnapshot = task.Result;
            string documentName = today + "_" + (todayQuerySnapshot.Count + 1);            

            DocumentReference docRef = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").Document(documentName);

            //Debug.Log(documentName+"\n시작날짜 : " + fishGameResult.시작날짜 + "\n" + "시작시간 : " + fishGameResult.시작시간 + "\n" + "레벨 : "+ fishGameResult.레벨 + "\n별개수 : " + fishGameResult.별개수 + "\n플레이시간 : " + fishGameResult.플레이시간+"\n호흡기록 : "+fishGameResult.호흡기록.Keys.Count);

            docRef.SetAsync(fishGameResult).ContinueWithOnMainThread(task => {

                if (task.IsCompleted)
                {
                    Debug.Log("Added data to the document "+documentName+" in the users collection.");
                }
                else { Debug.Log("Failed"); }
            });
        });
    }

    public static void ReceivePoint(updateDelegate updateCircle)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        Debug.Log("ReceivePoint 1");
        DocumentReference pointDoc = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("CurrentPoint");

        pointDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            { // document가 없으면 false
              //snapshot.ID
                Debug.Log("ChildDataController.receivePoint");
                PointInformation pointInfo = snapshot.ConvertTo<PointInformation>();
                level = pointInfo.레벨;
                goalPoint=pointInfo.목표점수;
                rewardTitle=pointInfo.보상제목;
                point=pointInfo.현재포인트;

                progressRatio = pointInfo.현재포인트 / (float)pointInfo.목표점수;

                Debug.Log("ReceivePoint 2 : "+pointInfo.레벨 + " " + pointInfo.목표점수 + ", " + pointInfo.보상제목 + ", " + pointInfo.현재포인트);

                isReceived = true;
                
                updateCircle();
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
          
        });

        

    }

    /// <summary>
    /// 호흡 기록을 가져와서 스케줄 배치.
    /// </summary>
    /// <param name="updateRecord"></param>
    public static void ReceiveBreath(updateDelegate updateRecord)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        Debug.Log("ReceiveBreath 1");

        string today = DateTime.Now.ToString("d");
        Debug.Log(today);
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("시작날짜", today);

        fishGameResultList.Clear();

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot todayQuerySnapshot = task.Result;

            foreach (DocumentSnapshot doc in todayQuerySnapshot.Documents)
            {
                Debug.Log("ReceiveBreath 2");
                fishGameResult = doc.ConvertTo<GameResult>();

                fishGameResultList.Add(fishGameResult);

                Debug.Log("완성률 : " + fishGameResult.완성률);

                Debug.Log("레벨 : " + fishGameResult.레벨);

                Debug.Log("예상 호흡기록 done.");

                fishGameResultList.Add(fishGameResult);
                
            }
            
            updateRecord();
            //Debug.Log(documentName+"\n시작날짜 : " + fishGameResult.시작날짜 + "\n" + "시작시간 : " + fishGameResult.시작시간 + "\n" + "레벨 : "+ fishGameResult.레벨 + "\n별개수 : " + fishGameResult.별개수 + "\n플레이시간 : " + fishGameResult.플레이시간+"\n호흡기록 : "+fishGameResult.호흡기록.Keys.Count);
        });



    }
    static public void receiveTimeCustom()
    {
        var result = new Dictionary<string, float>();

        /*
        int Exhale = new Dictionary<string, int>();
        var ExhaleStop = new Dictionary<string, int>();
        var Inhale = new Dictionary<string, int>();
        var InhalStop = new Dictionary<string, int>();
        var TotalTime = new Dictionary<string, int>(); 
        */
            
        Query TSQuery = db.Collection("ParentUsers").Document(parentID).Collection("TimeCustom");
        TSQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot TSQuerySnapshot = task.Result;
            foreach(DocumentSnapshot doc in TSQuerySnapshot.Documents)
            {
                var docDictionary = doc.ToDictionary();

                string log = "read TimeCustom data : ";

                foreach (KeyValuePair<string, object> pair in docDictionary) 
                {
                    result.Add(pair.Key, float.Parse(pair.Value.ToString()));
                    log += pair.Key + " : " + pair.Value + "\n";
                }
                Debug.Log(log);
            }
            FishGenerator.upTime = result["Inhale"];
            FishGenerator.upWaitTime = result["InhaleStop"];
            FishGenerator.downTime = result["Exhale"];
            FishGenerator.downWaitTime = result["ExhaleStop"];
        });          
    }

    static public void receiveRewardList(updateDelegate updateReward)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        Query RLquery = db.Collection("ParentUsers").Document(parentID).Collection("Point").WhereEqualTo("type", "list");
        RLquery = RLquery.WhereEqualTo("isChecked", false);
        RLquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot RLQuerySnapshot = task.Result;
            //Debug.Log(" : "+RLQuerySnapshot.Count);
            foreach (DocumentSnapshot doc in RLQuerySnapshot.Documents)
            {
                //Debug.Log("123");
                Dictionary<string, object> RewardLists = doc.ToDictionary();
                //Debug.Log("123");


                foreach (KeyValuePair<string, object> pair in RewardLists)
                {
                    //Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                    //Debug.Log(pair.Key == "레벨");
                    //Debug.Log("디버깅 : "+RewardLists[pair.Key]);                 
                }

                //Debug.Log("레벨 : "+RewardLists["레벨"].ToString());

                int level = System.Int32.Parse(RewardLists["레벨"].ToString());

                //Debug.Log("level is " + level);
                //Debug.Log(level);
                int point = System.Int32.Parse(RewardLists["포인트"].ToString());
                //Debug.Log("123");
                //Debug.Log("level : "+level + ", point : " + point);

                ChildDataController.RLresult.Add("포인트_"+ level.ToString(), point);

            }
            updateReward();
        });


    }

    static public void receiveRewardList_str(updateDelegate updateReward)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        Query RLquery = db.Collection("ParentUsers").Document(parentID).Collection("Point").WhereEqualTo("type", "list");
        RLquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            //Debug.Log(" receiveRewardList_str_1");
            QuerySnapshot RLQuerySnapshot = task.Result;
            foreach (DocumentSnapshot doc in RLQuerySnapshot.Documents)
            {

                //Debug.Log(" receiveRewardList_str_2");
                Dictionary<string, object> RewardLists = doc.ToDictionary();
                foreach (KeyValuePair<string, object> pair in RewardLists)
                {
                    //Debug.Log(" receiveRewardList_str_3");
                }
                int level = System.Int32.Parse(RewardLists["레벨"].ToString());
                string str = RewardLists["제목"].ToString();
                //Debug.Log("level : " + level + ", str : " + str);
                ChildDataController.RLresult_str.Add("제목_" + level.ToString(), str);

            }
            updateReward();
        });


    }

    static public void receiveCompPoint(updateDelegate updateReward)
    {
        CPresult.Clear();

        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        Query CPquery = db.Collection("ParentUsers").Document(parentID).Collection("Point").WhereEqualTo("type", "card");
        CPquery = CPquery.WhereEqualTo("isChecked", false);
        CPquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            //Debug.Log("receiving CompPoint");
            QuerySnapshot CPQuerySnapshot = task.Result;
            //Debug.Log("receiving CompPoint : " + CPQuerySnapshot.Count);
            int idx = 1;
            foreach (DocumentSnapshot doc in CPQuerySnapshot.Documents)
            {
                Dictionary<string, object> CompPoint = doc.ToDictionary();

                foreach (KeyValuePair<string, object> pair in CompPoint)
                {
                    //Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                    //Debug.Log("디버깅 : " + CompPoint[pair.Key]);
                }

                int point = System.Int32.Parse(CompPoint["포인트"].ToString());
                //Debug.Log("receiveCompPoint 포인트 파싱");
                //Debug.Log("level : "+level + ", point : " + point);

                ChildDataController.CPresult.Add("포인트_" + idx, point);
                idx++;
                /*Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "isChecked", true }
                };
                CPRef.UpdateAsync(updates);*/

            }
            updateReward();
        });


    }

    static public void receiveCompPoint_str(updateDelegate updateReward)
    {
        CPresult_str.Clear();
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        Query CPquery = db.Collection("ParentUsers").Document(parentID).Collection("Point").WhereEqualTo("type", "card");
        CPquery = CPquery.WhereEqualTo("isChecked_str", false);
        CPquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            //Debug.Log("receiveCompPoint_str_1");
            QuerySnapshot CPQuerySnapshot = task.Result;
            //Debug.Log("receiveCompPoint_str_2");
            int idx = 1;
            foreach (DocumentSnapshot doc in CPQuerySnapshot.Documents)
            {
                Dictionary<string, object> CompPoint = doc.ToDictionary();

                foreach (KeyValuePair<string, object> pair in CompPoint)
                {
                    //Debug.Log("receiveCompPoint_str_3");
                    //Debug.Log("디버깅 : " + CompPoint[pair.Key]);
                }

                string str = CompPoint["내용"].ToString();
                //Debug.Log("receiveCompPoint_str_4");
                //Debug.Log("level : "+level + ", 내용 : " + str);

                ChildDataController.CPresult_str.Add("내용_" + idx, str);
                idx++;

            }
            updateReward();
        });


    }



    /// <summary>
    /// scheduleInformationList를 비우고 ParentUsers/{parentId}/Schdule에 견본을 제외한 모든 문서를 가져와서 scheduleInformationList에 넣음. 
    /// </summary>
    /// <param name="updatePoint"></param>
    public static void ReceiveScheduleInfo(updateDelegate updateSchedule)
    {
        scheduleInformationList.Clear();


        Debug.Log("ReceiveScheduleInfo 1");
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        string today=dayOfToday();

        Query scheduleQuery = db.Collection("ParentUsers").Document(parentID).Collection("Schedule").WhereEqualTo("요일", today+"요일");

        scheduleQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            foreach (var docSnapshot in task.Result.Documents)
            {
                scheduleInformationList.Add(docSnapshot.ConvertTo<ScheduleInformation>());
            }
            updateSchedule();
        });
    }




    public void UpdateData()
    {
        DocumentReference docRef = db.Collection("users").Document("aturing");
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "First", "Alan" },
                { "Middle", "Mathison" },
                { "Last", "Turing" },
                { "Born", 1912 }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the aturing document in the users collection.");
        });
    }

    public void ReadData()
    {
        Debug.Log("SSSS Firebase Read Data.");
        CollectionReference usersRef = db.Collection("users");
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log(String.Format("User: {0}", document.Id));
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Debug.Log(String.Format("First: {0}", documentDictionary["First"]));
                if (documentDictionary.ContainsKey("Middle"))
                {
                    Debug.Log(String.Format("Middle: {0}", documentDictionary["Middle"]));
                }

                Debug.Log(String.Format("Last: {0}", documentDictionary["Last"]));
                Debug.Log(String.Format("Born: {0}", documentDictionary["Born"]));
            }

            Debug.Log("Read all data from the users collection.");
        });
    }

    // Start is called before the first frame update
    void Start()
    {        
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        
        var objList = FindObjectsOfType<ChildDataController>();
        if (objList.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    static string dayOfToday()
    {
        DateTime nowDt = DateTime.Now;

        string day = "";
        if (nowDt.DayOfWeek == DayOfWeek.Monday)
            day = "월";
        else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
            day = ("화");
        else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
            day = ("수");
        else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
            day = ("목");
        else if (nowDt.DayOfWeek == DayOfWeek.Friday)
            day = ("금");
        else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
            day = ("토");
        else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
            day = ("일");

        return day;
    }
}
