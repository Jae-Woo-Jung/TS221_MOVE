using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class AddSchedule : MonoBehaviour
{


    public List<Toggle> daySelectList = new List<Toggle>();
    public TMP_InputField titleText;
    public TMP_InputField hourInput;
    public TMP_InputField minuteInput;
    public List<Toggle> modeList = new List<Toggle>();

    public GameObject addSchedulePannel;
    public GameObject addAimPannel;

    public Button applyWithTimeBtn;
    public Button cancelWithTimeBtn;

    public Button applyWithAimBtn;
    public Button cancelWithAimBtn;

    // Start is called before the first frame update
    void Start()
    {
        applyWithTimeBtn.onClick.AddListener(applyWithTime);
        cancelWithTimeBtn.onClick.AddListener(cancelWithTime);
        hourInput.onEndEdit.AddListener(hourChecker);
        minuteInput.onEndEdit.AddListener(minuteChecker);

    }

    // Update is called once per frame
    void Update()
    {
        
    }





    void hourChecker(string hr)
    {
        int h = Int32.Parse(hr);
        h = Mathf.Clamp(h, 0, 23);

        hourInput.text = (h < 10 ? "0" : "") + h.ToString();
    }

    void minuteChecker(string min)
    {
        int m = Int32.Parse(min);
        m = Mathf.Clamp(m, 0, 59);

        minuteInput.text=   (m<10? "0":"")+m.ToString();
    }


    void applyWithTime()
    {
        List<Toggle> trueDayList = daySelectList.FindAll(x => x.isOn);
        List<Toggle> modeList = this.modeList.FindAll(x => x.isOn);
        
        //Check the legibility of inputs.
        if (trueDayList.Count == 0)
        {
            Debug.LogError("No days are selected.");
            return;
        }
        if (modeList.Count !=1)
        {
            Debug.LogError(modeList.Count+" modes are selected");
        }
        if (titleText.text.Length ==0)
        {
            Debug.LogError("title is null.");
            return;
        }
        if (hourInput.text.Length == 0 || minuteInput.text.Length == 0)
        {
            Debug.LogError("time is null");
            return;
        }


        //addSchedule(trueDayList, titleText.text, hourInput.text, minuteInput.text, modeList[0]);

        cancelWithTime();
    }

    /// <summary>
    /// 
    /// </summary>
    void cancelWithTime()
    {
        foreach (Toggle day in daySelectList)
        {
            day.isOn = false;
        }
        titleText.text = hourInput.text=minuteInput.text="";
        foreach (Toggle mode in modeList)
        {
            mode.isOn = false;
        }
        
        addAimPannel.SetActive(false);
        addSchedulePannel.SetActive(false);

    }

}
