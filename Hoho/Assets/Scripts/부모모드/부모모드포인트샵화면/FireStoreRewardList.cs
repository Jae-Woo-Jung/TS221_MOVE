using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class FireStoreRewardList : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField contentInput;
    public TMP_InputField pointInput;
    // Start is called before the first frame update
    public void SendRewardList(int level)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("Point").Document("RewardList_"+level);

   
        Dictionary<string, object> RewardList = new Dictionary<string, object>
        {       
            { "제목", titleInput.text },
            { "내용", contentInput.text },
            { "포인트", System.Int32.Parse(pointInput.text) },
            { "완성여부", false }, //AddRewardController에 완성여부관련 변수 추가해야 할 듯.
            { "레벨", level },
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
