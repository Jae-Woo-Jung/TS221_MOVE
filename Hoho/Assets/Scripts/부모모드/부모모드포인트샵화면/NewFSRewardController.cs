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

    public GameObject 보상목록배경;
    public GameObject talk;

    // Start is called before the first frame update
    public void SendRewardList()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ChildrenUsers").Document("001").Collection("Point").Document("CurrentPoint");
        DocumentReference docRef2 = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_1");


        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {
            { "제목", titleInput.text },
            { "내용", contentInput.text },
            { "포인트", System.Int32.Parse(pointInput.text) },
            { "완성여부", false }, //AddRewardController에 완성여부관련 변수 추가해야 할 듯.
            { "type", "list" },
            { "현재포인트", System.Int32.Parse("0") }
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
            AndroidBLEPluginStart.CallByAndroid("제목을 입력해주세요.");
            return;
        }
        if (contentInput.text == "")
        {
            AndroidBLEPluginStart.CallByAndroid("세부 내용을 입력해주세요.");
            return;
        }
        if (pointInput.text == "")
        {
            AndroidBLEPluginStart.CallByAndroid("포인트를 입력해주세요.");
            return;
        }

        talk.SetActive(true);
        //SendRewardList();
        보상목록배경.SetActive(false);

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
