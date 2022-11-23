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
    public Button applyBtn;

    public GameObject �����Ϲ��;
    public GameObject talk;

    // Start is called before the first frame update
    public void SendRewardList()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ChildrenUsers").Document("001").Collection("Point").Document("CurrentPoint");
        DocumentReference docRef2 = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_1");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "����", titleInput.text },
            { "����", contentInput.text },
            { "����Ʈ", System.Int32.Parse(pointInput.text) },
            { "�ϼ�����", false }, //AddRewardController�� �ϼ����ΰ��� ���� �߰��ؾ� �� ��.
            { "type", "list" },
            { "��������Ʈ", System.Int32.Parse("0") }
        };
        docRef.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });

        docRef2.SetAsync(RewardList).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the RewardList document in the Point collection.");
        });



        titleInput.text = "";
        contentInput.text = "";
        pointInput.text = "";
    }


    public void newAddReward()
    {
        if (titleInput.text == "")
        {
            AndroidBLEPluginStart.CallByAndroid("������ �Է����ּ���.");
            return;
        }
        if (contentInput.text == "")
        {
            AndroidBLEPluginStart.CallByAndroid("���� ������ �Է����ּ���.");
            return;
        }
        if (pointInput.text == "")
        {
            AndroidBLEPluginStart.CallByAndroid("����Ʈ�� �Է����ּ���.");
            return;
        }

        talk.SetActive(true);
        //SendRewardList();
        �����Ϲ��.SetActive(false);

    }

    private void Start()
    {
        pointInput.onEndEdit.AddListener(x => pointInput.text = pointChecker(x));

    }

    // Update is called once per frame
    void Update()
    {

    }

    string pointChecker(string pt)
    {
        int p = 0;
        System.Int32.TryParse(pt, out p);
        p = Mathf.Clamp(p, 1, 9999);

        return p.ToString();
    }
}
