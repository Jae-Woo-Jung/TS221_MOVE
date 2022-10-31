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

    static FirebaseFirestore db;

    /// <summary>
    /// ���� �� �ִ��� ����.
    /// </summary>
    static bool canSend = false;

    /// <summary>
    /// ����Ʈ ������ ����.
    /// </summary>
    static PointInformation pointInfo = new PointInformation();

    /// <summary>
    /// ���� ID.
    /// </summary>
    static string childID = "001";


    /// <summary>
    /// �θ� ID
    /// </summary>
    static string parentID = "001";

    /// <summary>
    /// ���۳�¥ = "", ���۽ð� = "", ���� = 0, ������ = starNum, �÷��̽ð� = 0, ȣ����, ����ȣ����
    /// </summary>
    public static GameResult fishGameResult = new GameResult();


    /// <summary>
    /// ȣ�� ����� ���⿡ ����ؼ� SendGameResult�� ���� ��. timestamp�� 0~1 ������ ������ ��. 
    /// </summary>
    public static Dictionary<string, float> BreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// correctHookPos�� ���⿡ ����ؼ� SendGameResult�� ���� ��. timestamp�� 0~1 ������ ������ ��. 
    /// </summary>
    public static Dictionary<string, float> ExpectedBreatheResult = new Dictionary<string, float> { { "0", 0 }, };

    /// <summary>
    /// canSend(bool), point (int), level(int), rewardTitle(string), goalPoint(int), progressRatio(float), childID(string), parentID(string)�� ���� Dictionary ��ȯ.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, object> getValues()
    {

        Dictionary<string, object> A = new Dictionary<string, object>();
        A.Add("canSend", canSend);
        A.Add("point", pointInfo.��������Ʈ);
        A.Add("level", pointInfo.����);
        A.Add("goalPoint", pointInfo.��ǥ����);
        A.Add("rewardTitle", pointInfo.��������);        
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
        ParentDataController.canSend = canSend;
    }

    /// <summary>
    /// ��ü���� ���̴� point
    /// </summary>
    public static void setPoint(int pt)
    {
        pointInfo.��������Ʈ = pt;
    }


    /// <summary>
    /// ���� ������ ��� ���� ��ǥ ����.
    /// </summary>
    public static void setGoalPoint(int pt)
    {
        pointInfo.��ǥ���� = pt;
    }


    /// <summary>
    /// ���� ���� ���� �ܰ�. PointShop���� ������ ���� ����.
    /// </summary>
    public static void setLevel(int lv)
    {
        pointInfo.���� = lv;
    }

    public static void setRewardTitle(string title)
    {
        pointInfo.�������� = title;
    }

    /// <summary>
    /// ���� ID.
    /// </summary>
    public static void setChildID(string id)
    {
        childID = id;
    }

    /*
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
                { "���� �� ����Ʈ", point},
                { "���� ����", level },
                { "���� ����", rewardTitle },
                { "��ǥ ����", goalPoint },
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

        Debug.Log("Keys : " + fishGameResult.ȣ����.Keys.ToString());

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
                    Debug.Log("Added data to the document " + documentName + " in the users collection.");
                }
                else { Debug.Log("Failed"); }
            });
        });
    }*/


    /// <summary>
    /// ���� ����� ����Ʈ�� ����.  path = ChildrenUsers/ChildID/Point/CurrentPoint
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
            { // document�� ������ false
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
    /// ȣ�� ����� �����ͼ� ������.
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
        Query todayQuery = db.Collection("ChildrenUsers").Document(childID).Collection("Point").Document("FishPoint").Collection("Results").WhereEqualTo("���۳�¥", today);

        todayQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot todayQuerySnapshot = task.Result;
            
            foreach(DocumentSnapshot doc in todayQuerySnapshot.Documents)
            {

                fishGameResult = doc.ConvertTo<GameResult>();

                Debug.Log( "�ϼ��� : "+fishGameResult.�ϼ���);

                Dictionary<int, float> data1 = new Dictionary<int, float>();
                Dictionary<int, float> data2 = new Dictionary<int, float>();
                Debug.Log("���� : "+fishGameResult.����);
                try
                {
                    Debug.Log("���� �ð� : "+fishGameResult.ȣ����.First().Key);
                    DateTime startTime = DateTime.Parse(fishGameResult.����ȣ����.Keys.Min());


                    List<string> keys=fishGameResult.ȣ����.Keys.ToList();
                    keys.Sort();
                    
                    Debug.Log("���� �ð� �Ľ� : " + startTime);
                    foreach (string key in keys)
                    {
                        DateTime time = DateTime.Parse(key);

                        int sec=(int) Math.Round( (time - startTime).TotalSeconds );
                        data1.Add(sec, fishGameResult.ȣ����[key]);
                        Debug.Log("ȣ���� : " + sec);
                    }

                    Debug.Log("ȣ���� done.");


                    keys = fishGameResult.����ȣ����.Keys.ToList();
                    keys.Sort();

                    foreach (string key in keys)
                    {
                        DateTime time = DateTime.Parse(key);

                        int sec = (int) Math.Round( (time - startTime).TotalSeconds);
                        Debug.Log("���� �ð� : "+startTime+", ��� �ð� : "+time+", ���� ȣ�� ��� ��� �ð� (��):" + sec);
                        data2.Add(sec, fishGameResult.����ȣ����[key]);
                    }

                    Debug.Log("���� ȣ���� done.");

                    updateRecord(data1, data2);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

            }

            //Debug.Log(documentName+"\n���۳�¥ : " + fishGameResult.���۳�¥ + "\n" + "���۽ð� : " + fishGameResult.���۽ð� + "\n" + "���� : "+ fishGameResult.���� + "\n������ : " + fishGameResult.������ + "\n�÷��̽ð� : " + fishGameResult.�÷��̽ð�+"\nȣ���� : "+fishGameResult.ȣ����.Keys.Count);
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
