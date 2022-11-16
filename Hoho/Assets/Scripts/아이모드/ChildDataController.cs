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
/// ���̸���� ������ �ۼ����� å����. 
/// </summary>
public class ChildDataController : MonoBehaviour
{
    [FirestoreData]
    public class GameResult
    {
        [FirestoreProperty]
        public string ���۳�¥ { get; set; } = "";

        [FirestoreProperty]
        public string ���۽ð� { get; set; } = "";

        [FirestoreProperty]
        public int ���� { get; set; } = 1;

        [FirestoreProperty]
        public int ������ { get; set; } = 0;

        [FirestoreProperty]
        public int �÷��̽ð� { get; set; } = 0;

        [FirestoreProperty]
        public int �Ʒýð� { get; set; } = 0;

        [FirestoreProperty]
        public int �ϼ��� { get; set; } = 0;

        [FirestoreProperty]
        public Dictionary<string, float> ȣ���� { get; set; } = ChildDataController.BreatheResult;

        [FirestoreProperty]
        public Dictionary<string, float> ����ȣ���� { get; set; } = ChildDataController.ExpectedBreatheResult;
    };


    [FirestoreData]
    public class PointInformation
    {
        [FirestoreProperty]
        public int ��������Ʈ { get; set; } = 0;

        [FirestoreProperty]
        public int ��ǥ���� { get; set; } = 1000;

        [FirestoreProperty]
        public int ���� { get; set; } = 1;

        [FirestoreProperty]
        public string �������� { get; set; } = "���̰���";
    }

    [FirestoreData]
    public class ScheduleInformation
    {

        [FirestoreProperty]
        public string ���� { get; set; } = "������";

        [FirestoreProperty]
        public string ���� { get; set; } = "";

        [FirestoreProperty]
        public int �� { get; set; } = 12;

        [FirestoreProperty]
        public int �� { get; set; } = 0;

        [FirestoreProperty]
        public string ��� { get; set; } = "ǥ�ظ��";

        [FirestoreProperty]
        public int ����ð� { get; set; } = 0;

        [FirestoreProperty]
        public int ��������½ð� { get; set; } = 0;

        [FirestoreProperty]
        public int �����ð� { get; set; } = 0;

        [FirestoreProperty]
        public int ���������½ð� { get; set; } = 0;

        [FirestoreProperty]
        public int �ݺ�Ƚ�� { get; set; } = 0;
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
    /// ��ü���� ���̴� point
    /// </summary>
    static int point=0;

    /// <summary>
    /// ���� ������ ��� ���� ��ǥ ����.
    /// </summary>
    static int goalPoint=1000;

    /// <summary>
    /// ���� ���� ���� �ܰ�. PointShop���� ������ ���� ����.
    /// </summary>
    static int level=1;
    /// <summary>
    /// �� �ȿ� �ִ� �ؽ�Ʈ. ����, "���̰���".  
    /// </summary>
    static string rewardTitle="���̰���";

    /// <summary>
    /// ������ ���� ���� �����.
    /// </summary>
    public static List<string> rewardTitleList = new List<string>();

    /// <summary>
    /// ���� ����/��ǥ ����
    /// </summary>
    static float progressRatio=point/(float) goalPoint;


    /// <summary>
    /// ���� ID.
    /// </summary>
    static string childID = "001";

    /// <summary>
    /// �θ� ID.
    /// </summary>
    public static string parentID = "001";

    /// <summary>
    /// ���۳�¥ = "", ���۽ð� = "", ���� = 0, ������ = starNum, �÷��̽ð� = 0, ȣ����, ����ȣ����
    /// </summary>
    public static GameResult fishGameResult = new GameResult();

    public static List<GameResult> fishGameResultList = new List<GameResult>();

    /// <summary>
    /// ȣ�� ����� ���⿡ ����ؼ� SendGameResult�� ���� ��. timestamp�� 0~1 ������ ������ ��. 
    /// </summary>
    public static Dictionary<string, float> BreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// correctHookPos�� ���⿡ ����ؼ� SendGameResult�� ���� ��. timestamp�� 0~1 ������ ������ ��. 
    /// </summary>
    public static Dictionary<string, float> ExpectedBreatheResult = new Dictionary<string, float> { { "0", 0 }, };


    public static List<ScheduleInformation> scheduleInformationList = new List<ScheduleInformation>();

    /// <summary>
    /// isReceived (bool), canSend(bool), point (int), level(int), rewardTitle (string), goalPoint (int), progressRatio (float), childID (string), parentID (string) �� ���� Dictionary ��ȯ.
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
    /// true�� �����ؾ� ���� �� ����. 
    /// </summary>
    /// <param name="canSend"></param>
    public static void setCanSend(bool canSend)
    {
       ChildDataController.canSend=canSend;
    }

    /// <summary>
    /// ��ü���� ���̴� point
    /// </summary>
    public static void setPoint(int pt)
    {
        point = pt;
    }

    /// <summary>
    /// ���� ����� point�� ����. ������, ���� ������� �˾Ƽ� ��������. ���� ������ ����Ʈ���� ���� �ʱ�ȭ��. 
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
    /// ���� ������ ��� ���� ��ǥ ����.
    /// </summary>
    public static void setGoalPoint(int pt)
    {
        goalPoint = pt;
    }


    /// <summary>
    /// ���� ���� ���� �ܰ�. PointShop���� ������ ���� ����.
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
    /// ���� ����/��ǥ ����
    /// </summary>
    public static void setProgressRatio(float ratio)
    {
        progressRatio = ratio;
    }


    /// <summary>
    /// ���� ID.
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
    /// ������ ����Ʈ, level, rewardTitle, pointString, �����, childID�� ����. 
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
                { "��������Ʈ", point},
                { "����", level },
                { "��������", rewardTitle },
                { "��ǥ����", goalPoint },
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
        fishGameResult.ȣ���� = BreatheResult;
        fishGameResult.����ȣ���� = ExpectedBreatheResult;

        Debug.Log( "Keys : "+fishGameResult.ȣ����.Keys.ToString());

        if (!canSend)
        {
            Debug.Log("Not yet prepared.");
        }

        Debug.Log("Send point for GameResult");

        string today = fishGameResult.���۳�¥;       
        Debug.Log(today);
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("���۳�¥", today);        

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {     
            
            QuerySnapshot todayQuerySnapshot = task.Result;
            string documentName = today + "_" + (todayQuerySnapshot.Count + 1);            

            DocumentReference docRef = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").Document(documentName);

            //Debug.Log(documentName+"\n���۳�¥ : " + fishGameResult.���۳�¥ + "\n" + "���۽ð� : " + fishGameResult.���۽ð� + "\n" + "���� : "+ fishGameResult.���� + "\n������ : " + fishGameResult.������ + "\n�÷��̽ð� : " + fishGameResult.�÷��̽ð�+"\nȣ���� : "+fishGameResult.ȣ����.Keys.Count);

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
            { // document�� ������ false
              //snapshot.ID
                Debug.Log("ChildDataController.receivePoint");
                PointInformation pointInfo = snapshot.ConvertTo<PointInformation>();
                level = pointInfo.����;
                goalPoint=pointInfo.��ǥ����;
                rewardTitle=pointInfo.��������;
                point=pointInfo.��������Ʈ;

                progressRatio = pointInfo.��������Ʈ / (float)pointInfo.��ǥ����;

                Debug.Log("ReceivePoint 2 : "+pointInfo.���� + " " + pointInfo.��ǥ���� + ", " + pointInfo.�������� + ", " + pointInfo.��������Ʈ);

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
    /// ȣ�� ����� �����ͼ� ������ ��ġ.
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
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("���۳�¥", today);

        fishGameResultList.Clear();

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot todayQuerySnapshot = task.Result;

            foreach (DocumentSnapshot doc in todayQuerySnapshot.Documents)
            {
                Debug.Log("ReceiveBreath 2");
                fishGameResult = doc.ConvertTo<GameResult>();

                fishGameResultList.Add(fishGameResult);

                Debug.Log("�ϼ��� : " + fishGameResult.�ϼ���);

                Debug.Log("���� : " + fishGameResult.����);

                Debug.Log("���� ȣ���� done.");

                fishGameResultList.Add(fishGameResult);
                
            }
            
            updateRecord();
            //Debug.Log(documentName+"\n���۳�¥ : " + fishGameResult.���۳�¥ + "\n" + "���۽ð� : " + fishGameResult.���۽ð� + "\n" + "���� : "+ fishGameResult.���� + "\n������ : " + fishGameResult.������ + "\n�÷��̽ð� : " + fishGameResult.�÷��̽ð�+"\nȣ���� : "+fishGameResult.ȣ����.Keys.Count);
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
                    //Debug.Log(pair.Key == "����");
                    //Debug.Log("����� : "+RewardLists[pair.Key]);                 
                }

                //Debug.Log("���� : "+RewardLists["����"].ToString());

                int level = System.Int32.Parse(RewardLists["����"].ToString());

                //Debug.Log("level is " + level);
                //Debug.Log(level);
                int point = System.Int32.Parse(RewardLists["����Ʈ"].ToString());
                //Debug.Log("123");
                //Debug.Log("level : "+level + ", point : " + point);

                ChildDataController.RLresult.Add("����Ʈ_"+ level.ToString(), point);

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
                int level = System.Int32.Parse(RewardLists["����"].ToString());
                string str = RewardLists["����"].ToString();
                //Debug.Log("level : " + level + ", str : " + str);
                ChildDataController.RLresult_str.Add("����_" + level.ToString(), str);

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
                    //Debug.Log("����� : " + CompPoint[pair.Key]);
                }

                int point = System.Int32.Parse(CompPoint["����Ʈ"].ToString());
                //Debug.Log("receiveCompPoint ����Ʈ �Ľ�");
                //Debug.Log("level : "+level + ", point : " + point);

                ChildDataController.CPresult.Add("����Ʈ_" + idx, point);
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
                    //Debug.Log("����� : " + CompPoint[pair.Key]);
                }

                string str = CompPoint["����"].ToString();
                //Debug.Log("receiveCompPoint_str_4");
                //Debug.Log("level : "+level + ", ���� : " + str);

                ChildDataController.CPresult_str.Add("����_" + idx, str);
                idx++;

            }
            updateReward();
        });


    }



    /// <summary>
    /// scheduleInformationList�� ���� ParentUsers/{parentId}/Schdule�� �ߺ��� ������ ��� ������ �����ͼ� scheduleInformationList�� ����. 
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

        Query scheduleQuery = db.Collection("ParentUsers").Document(parentID).Collection("Schedule").WhereEqualTo("����", today+"����");

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
            day = "��";
        else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
            day = ("ȭ");
        else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
            day = ("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
            day = ("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Friday)
            day = ("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
            day = ("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
            day = ("��");

        return day;
    }
}
