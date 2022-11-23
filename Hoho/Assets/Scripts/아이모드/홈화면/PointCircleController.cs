using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointCircleController : MonoBehaviour
{
    //public ProgressController progressController;
    public void updateCircle()
    {
        //progressController.initProgress();

        Debug.Log("updateCircle 0");
        TextMeshProUGUI rewardTitle = GameObject.Find("제목text").GetComponent<TextMeshProUGUI>();
        Debug.Log("updateCircle 1");
        //TextMeshProUGUI currentLevel = GameObject.Find("단계Text").GetComponent<TextMeshProUGUI>();
        Debug.Log("updateCircle 2");
        //TextMeshProUGUI pointText = GameObject.Find("현재포인트Text").GetComponent<TextMeshProUGUI>();
        Debug.Log("updateCircle 3");

        rewardTitle.text = (string) ChildDataController.getValues()["rewardTitle"];
        Debug.Log("updateCircle 4");
        //currentLevel.text = ((int)ChildDataController.getValues()["level"]).ToString();
        Debug.Log("updateCircle 5");
        
        //pointText.text = underlinePoint( ((int) ChildDataController.getValues()["point"]).ToString() );

        Debug.Log("updateCircle 6");
        /*
        level = pointInfo.����;
        goalPoint = pointInfo.��ǥ����;
        rewardTitle = pointInfo.��������;
        point = pointInfo.��������Ʈ;
        */

        GetComponent<Slider>().value = (float) ChildDataController.getValues()["progressRatio"];

        //Debug.Log("updateCircle progressRatio : " + (float)ChildDataController.getValues()["progressRatio"]);

    }

    // Start is called before the first frame update
    void Start()
    {
        if ( !(bool) ChildDataController.getValues()["isReceived"])
        {
            //Debug.Log("PointCircleControl start");
            ChildDataController.ReceivePoint(updateCircle);

        }        
        else
        {
            updateCircle();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    //<U>1</U> <U>2</U> <U>3</U> <U>4</U>
    string underlinePoint(string text)
    {
        string result = "";

        foreach(char a in text)
        {
            result += "<U>" + a + "</U> ";
        }
        //Debug.Log("underlinePoint : "+result);
        return result;

    }
}
