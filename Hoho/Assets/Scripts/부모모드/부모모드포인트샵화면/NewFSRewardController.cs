using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class NewFSRewardController : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField contentInput;
    public TMP_InputField pointInput;
    public GameObject �����Ϲ��;
    // Start is called before the first frame update
    public void SendRewardList()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_1");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput.text },
            { "����", contentInput.text },
            { "����Ʈ", System.Int32.Parse(pointInput.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "type", "list" }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });
        titleInput.text = "";
        contentInput.text = "";
        pointInput.text = "";
    }


    public void newAddReward()
    {
        if (titleInput.text == "" || contentInput.text == "" || pointInput.text == "")
        {
            return;
        }
        SendRewardList();
        �����Ϲ��.SetActive(false);

    }




    // Update is called once per frame
    void Update()
    {

    }
}
