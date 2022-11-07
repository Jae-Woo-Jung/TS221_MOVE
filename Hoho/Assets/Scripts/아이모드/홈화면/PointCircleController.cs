using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointCircleController : MonoBehaviour
{
    public void updateCircle()
    {

<<<<<<< Updated upstream
        TextMeshProUGUI rewardTitle = GameObject.Find("보상제목텍스트").GetComponent<TextMeshProUGUI>();
=======
        TextMeshProUGUI rewardTitle = GameObject.Find("제목text").GetComponent<TextMeshProUGUI>();
>>>>>>> Stashed changes
        TextMeshProUGUI currentLevel = GameObject.Find("단계Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pointText = GameObject.Find("현재포인트Text").GetComponent<TextMeshProUGUI>();

        rewardTitle.text = ProgressController.rewardTitle;
        currentLevel.text = ProgressController.level.ToString();
        pointText.text = ProgressController.pointString;

        GetComponent<Slider>().value = ProgressController.progressRatio;


    }

    // Start is called before the first frame update
    void Start()
    {
        if ((bool) ChildDataController.getValues()["isReceived"])
        {
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
}
