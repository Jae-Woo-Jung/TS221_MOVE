using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;

public class FireStoreTimeCustom : MonoBehaviour
{
    public TMP_InputField inhaleText;
    public TMP_InputField inhaleStopText;
    public TMP_InputField exhaleText;
    public TMP_InputField exhaleStopText;
    public TMP_InputField totalTimesText;
    // Start is called before the first frame update
    public void SendTimeCustom()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ParentUsers").Document("001").Collection("TimeCustom").Document("TimeCustom");
        Dictionary<string, object> TimeCustom = new Dictionary<string, object>
        {
            { "TotalTime", System.Int32.Parse(totalTimesText.text) },//일단 str이긴 한데 필요시 int로 변환할 것.
            { "Inhale", System.Int32.Parse(inhaleText.text) },
            { "InhaleStop", System.Int32.Parse(inhaleStopText.text) },
            { "Exhale", System.Int32.Parse(exhaleText.text) } ,
            { "ExhaleStop", System.Int32.Parse(exhaleStopText.text) }
        };
        docRef.SetAsync(TimeCustom).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the TimeCustom document in the TimeCustom collection.");
        });
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
