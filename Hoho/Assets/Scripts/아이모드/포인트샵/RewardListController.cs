using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardListController : MonoBehaviour
{

    public List<TextMeshProUGUI> guideTexts = new List<TextMeshProUGUI>();

    private void setGuideText()
    {
        for (int i=1; i<guideTexts.Count+1; i++)
        {
            string msg = ChildDataController.RLresult["Æ÷ÀÎÆ®_"+i].ToString() + "P";
            guideTexts[i-1].text = msg;
            Debug.Log(msg);
            

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChildDataController.receiveRewardList(setGuideText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
