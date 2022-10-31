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
    /// ���̾�̽��� �޽����� ����Ʈ ������ ����.
    /// </summary>
    public void sendMessage()
    {



        string content = cardMessage.text;

        int point = 0;
        if (System.Int32.TryParse(cardPoint.text, out point) && point != 0)
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
                    { "����", cardMessage.text },
                    { "����Ʈ", cardPoint.text },
                    {"type", "card" }
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
        else
        Debug.Log("Please set point");
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
