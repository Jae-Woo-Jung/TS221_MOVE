using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class ParentCardController : MonoBehaviour
{

    public TMP_InputField cardMessage;
    public TMP_InputField cardPoint;

    public static string parentID="001";

    /// <summary>
    /// 파이어베이스에 메시지와 포인트 정보를 보냄.
    /// </summary>
    public void sendMessage()
    {



        string content = cardMessage.text;

        int point = -1;
        if (!System.Int32.TryParse(cardPoint.text, out point))
        {
            AndroidBLEPluginStart.CallByAndroid("포인트를 입력해주세요.");
        }
        else
        {

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            Query query = db.Collection("ParentUsers").Document(parentID).Collection("Point").WhereEqualTo("type", "card");
            query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
            {
                QuerySnapshot CardQuerySnapshot = querySnapshotTask.Result;
                string DocName = "CompPoint_" + (CardQuerySnapshot.Count + 1);

                DocumentReference docRef = db.Collection("ParentUsers").Document(parentID).Collection("Point").Document(DocName);



                Dictionary<string, object> RewardList = new Dictionary<string, object>
                {
                    { "내용", cardMessage.text },
                    { "포인트", cardPoint.text },
                    {"type", "card" },
                    {"isChecked", false },
                    {"isChecked_str", false }
                };
                string filePath = "ParentUsers/" + parentID + "/Point/CompPoint";

                docRef.SetAsync(RewardList).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added data to" + filePath + " document in the Point collection.");
                });

                docRef.UpdateAsync("Timestamp", FieldValue.ServerTimestamp)
                        .ContinueWithOnMainThread(task =>
                        {
                            Debug.Log(
                        "Updated the Timestamp field of the CompPoint document in the Point "
                        + "collection.");
                        });
                cardPoint.text = cardMessage.text = "";
            });
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
