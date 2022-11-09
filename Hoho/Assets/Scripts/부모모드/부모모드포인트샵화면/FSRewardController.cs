using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class FSRewardController : MonoBehaviour
{
    public TMP_InputField titleInput_1;
    public TMP_InputField pointInput_1;
    public TMP_InputField titleInput_2;
    public TMP_InputField pointInput_2;
    public TMP_InputField titleInput_3;
    public TMP_InputField pointInput_3;
    public TMP_InputField titleInput_4;
    public TMP_InputField pointInput_4;
    public TMP_InputField titleInput_5;
    public TMP_InputField pointInput_5;
    public TMP_InputField titleInput_6;
    public TMP_InputField pointInput_6;
    // Start is called before the first frame update
    public void SendRewardList_1()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_1");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_1.text },
            { "����Ʈ", System.Int32.Parse(pointInput_1.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 1 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    public void SendRewardList_2()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_2");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_2.text },
            { "����Ʈ", System.Int32.Parse(pointInput_2.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 2 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    public void SendRewardList_3()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_3");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_3.text },
            { "����Ʈ", System.Int32.Parse(pointInput_3.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 3 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    public void SendRewardList_4()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_4");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_4.text },
            { "����Ʈ", System.Int32.Parse(pointInput_4.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 4 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    public void SendRewardList_5()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_5");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_5.text },
            { "����Ʈ", System.Int32.Parse(pointInput_5.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 5 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    public void SendRewardList_6()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_6");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput_6.text },
            { "����Ʈ", System.Int32.Parse(pointInput_6.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "����", 6 },
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
