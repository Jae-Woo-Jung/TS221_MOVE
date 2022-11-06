using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;

public class ParentDataController : MonoBehaviour
{

    public delegate void updateDelegate();
    public delegate void makeBarDelegate(Dictionary<int, float> data1, Dictionary<int, float> data2);

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

    static FirebaseFirestore db;

    /// <summary>
    /// 보낼 수 있는지 여부.
    /// </summary>
    static bool canSend = false;

    /// <summary>
    /// 포인트 정보를 담음.
    /// </summary>
    static PointInformation pointInfo = new PointInformation();

    /// <summary>
    /// 유저 ID.
    /// </summary>
    static string childID = "001";


    /// <summary>
    /// 부모 ID
    /// </summary>
    static string parentID = "001";

    /// <summary>
    /// 시작날짜 = "", 시작시간 = "", 레벨 = 0, 별개수 = starNum, 플레이시간 = 0, 호흡기록, 예상호흡기록
    /// </summary>
    public static GameResult fishGameResult = new GameResult();

    public static List<ScheduleInformation> scheduleInformationList = new List<ScheduleInformation>();


    /// <summary>
    /// 호흡 결과는 여기에 기록해서 SendGameResult로 보낼 것. timestamp와 0~1 사이의 넣으면 됨. 
    /// </summary>
    public static Dictionary<string, float> BreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// correctHookPos는 여기에 기록해서 SendGameResult로 보낼 것. timestamp와 0~1 사이의 넣으면 됨. 
    /// </summary>
    public static Dictionary<string, float> ExpectedBreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// canSend(bool), point (int), level(int), rewardTitle(string), goalPoint(int), progressRatio(float), childID(string), parentID(string)를 담은 Dictionary 반환.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, object> getValues()
    {

        Dictionary<string, object> A = new Dictionary<string, object>();
        A.Add("canSend", canSend);
        A.Add("point", pointInfo.현재포인트);
        A.Add("level", pointInfo.레벨);
        A.Add("goalPoint", pointInfo.목표점수);
        A.Add("rewardTitle", pointInfo.보상제목);        
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
        ParentDataController.canSend = canSend;
    }

    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    public static void setPoint(int pt)
    {
        pointInfo.현재포인트 = pt;
    }


    /// <summary>
    /// 현재 보상을 얻기 위한 목표 점수.
    /// </summary>
    public static void setGoalPoint(int pt)
    {
        pointInfo.목표점수 = pt;
    }


    /// <summary>
    /// 현재 진행 보상 단계. PointShop에서 검은색 점의 개수.
    /// </summary>
    public static void setLevel(int lv)
    {
        pointInfo.레벨 = lv;
    }

    public static void setRewardTitle(string title)
    {
        pointInfo.보상제목 = title;
    }

    /// <summary>
    /// 유저 ID.
    /// </summary>
    public static void setChildID(string id)
    {
        childID = id;
    }

    /// <summary>
    /// ParentUsers/{parentId}/Schdule에 견본을 제외한 모든 문서를 삭제 후 scheduleInformationList의 모든 예약 내용을 각각 "요일_시_분"의 id로 문서화함.
    /// </summary>
    public static void SendScheduleInformation()
    {

        Debug.Log("Schedule Nums : " + scheduleInformationList.Count);

        if (!canSend)
        {
            Debug.Log("Not yet prepared.");
        }

        Debug.Log("Send Schedule Information");

        Query scheduleQuery = db.Collection("ParentUsers").Document(parentID).Collection("Schedule").WhereNotEqualTo("요일", "?");

        //전체 삭제.        
        scheduleQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            foreach(DocumentSnapshot docSnap in task.Result.Documents)
            {
                docSnap.Reference.DeleteAsync();                
            }
        });

        //수정.
        foreach (ScheduleInformation schedule in scheduleInformationList)
        {
            string day = schedule.요일;
            Debug.Log(day);            

            scheduleQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                string documentName = day + "_" + schedule.시 + "시" + schedule.분 + "분";

                DocumentReference docRef = db.Collection("ParentUsers").Document(parentID).Collection("Schedule").Document(documentName);
                Debug.Log(documentName);

                docRef.SetAsync(schedule).ContinueWithOnMainThread(task => {

                    if (task.IsCompleted)
                    {
                        Debug.Log("Added data to the document " + documentName + " in the parents collection.");
                    }
                    else { Debug.Log("Failed"); }
                });
            });
        }
    }

    /*
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
                { "현재 총 포인트", point},
                { "현재 레벨", level },
                { "보상 제목", rewardTitle },
                { "목표 점수", goalPoint },
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

        Debug.Log("Keys : " + fishGameResult.호흡기록.Keys.ToString());

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
                    Debug.Log("Added data to the document " + documentName + " in the users collection.");
                }
                else { Debug.Log("Failed"); }
            });
        });
    }*/


    /// <summary>
    /// 아이 모드의 포인트를 읽음.  path = ChildrenUsers/ChildID/Point/CurrentPoint
    /// </summary>
    public static void ReceivePoint(updateDelegate updatePoint)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        DocumentReference pointDoc = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("CurrentPoint");

        pointDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            { // document가 없으면 false
              //snapshot.ID
                Debug.Log("123");
                pointInfo = snapshot.ConvertTo<PointInformation>();
                Debug.Log("456");
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }

            updatePoint();
        });
        
    }

    /// <summary>
    /// 호흡 기록을 가져와서 생성함.
    /// </summary>
    /// <param name="updateRecord"></param>
    public static void ReceiveBreath(makeBarDelegate updateRecord)
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        Debug.Log("Receive point for GameResult");

        string today = DateTime.Now.ToString("d");
        Debug.Log(today);
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("시작날짜", today);

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot todayQuerySnapshot = task.Result;
            
            foreach(DocumentSnapshot doc in todayQuerySnapshot.Documents)
            {

                fishGameResult = doc.ConvertTo<GameResult>();

                Debug.Log( "완성률 : "+fishGameResult.완성률);

                Dictionary<int, float> data1 = new Dictionary<int, float>();
                Dictionary<int, float> data2 = new Dictionary<int, float>();
                Debug.Log("레벨 : "+fishGameResult.레벨);
                try
                {
                    Debug.Log("시작 시간 : "+fishGameResult.호흡기록.First().Key);
                    DateTime startTime = DateTime.Parse(fishGameResult.예상호흡기록.Keys.Min());


                    List<string> keys=fishGameResult.호흡기록.Keys.ToList();
                    keys.Sort();
                    
                    Debug.Log("시작 시간 파싱 : " + startTime);
                    foreach (string key in keys)
                    {
                        DateTime time = DateTime.Parse(key);

                        int sec=(int) Math.Round( (time - startTime).TotalSeconds );
                        data1.Add(sec, fishGameResult.호흡기록[key]);
                        Debug.Log("호흡기록 : " + sec);
                    }

                    Debug.Log("호흡기록 done.");


                    keys = fishGameResult.예상호흡기록.Keys.ToList();
                    keys.Sort();

                    foreach (string key in keys)
                    {
                        DateTime time = DateTime.Parse(key);

                        int sec = (int) Math.Round( (time - startTime).TotalSeconds);
                        Debug.Log("시작 시간 : "+startTime+", 기록 시간 : "+time+", 예상 호흡 기록 경과 시간 (초):" + sec);
                        data2.Add(sec, fishGameResult.예상호흡기록[key]);
                    }

                    Debug.Log("예상 호흡기록 done.");

                    updateRecord(data1, data2);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

            }

            //Debug.Log(documentName+"\n시작날짜 : " + fishGameResult.시작날짜 + "\n" + "시작시간 : " + fishGameResult.시작시간 + "\n" + "레벨 : "+ fishGameResult.레벨 + "\n별개수 : " + fishGameResult.별개수 + "\n플레이시간 : " + fishGameResult.플레이시간+"\n호흡기록 : "+fishGameResult.호흡기록.Keys.Count);
        });



    }


    /// <summary>
    /// scheduleInformationList를 비우고 ParentUsers/{parentId}/Schdule에 견본을 제외한 모든 문서를 가져와서 scheduleInformationList에 넣음. 
    /// </summary>
    /// <param name="updatePoint"></param>
    public static void ReceiveScheduleInfo(updateDelegate updatePoint)
    {
        scheduleInformationList.Clear();
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }

        Query scheduleQuery = db.Collection("ParentUsers").Document(parentID).Collection("Schedule").WhereNotEqualTo("요일", "?");

        scheduleQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            foreach(var docSnapshot in task.Result.Documents)
            {
                scheduleInformationList.Add(docSnapshot.ConvertTo<ScheduleInformation>());
            }
            updatePoint();
        });
    }



    // Start is called before the first frame update
    void Start()
    {
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
