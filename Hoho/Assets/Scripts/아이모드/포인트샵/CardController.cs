using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;


public class CardController : MonoBehaviour
{

    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public TextMeshProUGUI point;
    public Button receive;
    public Button cpmsg;
    public Image bell;

    public ProgressController progressController;
    public PointListController pointListController;


    static FirebaseFirestore db;

    public void setCardInfo(string _title, string _content, int _point)
    {
        title.text = _title;
        content.text = _content;
        point.text = _point.ToString();
    }

    public void updateCard()
    {
        string parentID = ChildDataController.parentID;


        int CPPoint = System.Int32.Parse(pointListController.CPpointQueue.Dequeue());
        string CPPoint_str = pointListController.CPpointQueue_str.Dequeue();

        
        if (db == null)
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        DocumentReference CPRef = db.Collection("ParentUsers").Document(parentID).Collection("Point").Document(pointListController.CPpointQueue.Dequeue());
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "isChecked", true },
            {"isChecked_str", true }
        };

        CPRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log(
                    "Updated isCheckedand isChecked_str to true for the poped element.");
        });

        setCardInfo("ºÎ¸ð´ÔÀÇ ÄªÂùÄ«µå!", CPPoint_str, CPPoint);
        //Debug.Log("getPoint : " + CPPoint);
        //if (pointListController.CPpointStack.Count == 0)
        //{
        //    cpmsg.GetComponent<Button>().interactable = false;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        receive.onClick.AddListener(getPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void getPoint()
    {
        int p = System.Int32.Parse(point.text.Length>0? point.text : "0");
        progressController.addPoint(p);
        pointListController.updateList(pointListController.spriteList[0], "ÄªÂùÆ÷ÀÎÆ®", p);
        ChildDataController.SendPoint();
    }

}
