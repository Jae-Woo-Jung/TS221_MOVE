using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class AddSchedule : MonoBehaviour
{


    public List<Toggle> daySelectList = new List<Toggle>();
    public List<Toggle> daySelectAimList = new List<Toggle>();

    public TMP_InputField titleText;
    public TMP_InputField hourInput;
    public TMP_InputField minuteInput;
    public List<Toggle> modeList = new List<Toggle>();

    public TMP_InputField aimInput;


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

        aimInput.onEndEdit.AddListener(aimChecker);
        cancelWithAimBtn.onClick.AddListener(cancelWithAim);
        applyWithAimBtn.onClick.AddListener(applyWithAim);

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

    /// <summary>
    /// 0~20 사이의 값으로 조정.
    /// </summary>
    /// <param name="times"></param>
    void aimChecker(string times)
    {
        int t = Int32.Parse(times);
        t = Mathf.Clamp(t, 0, 20);

        hourInput.text = (t < 10 ? "0" : "") + t.ToString();
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
            return;
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

        //this==GameManager
        GetComponent<TimetableController>().addSchedule(trueDayList, titleText.text, hourInput.text, minuteInput.text, modeList[0]);

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


    void applyWithAim()
    {
        List<Toggle> trueDayList = daySelectList.FindAll(x => x.isOn);        

        //Check the legibility of inputs.
        if (trueDayList.Count == 0)
        {
            Debug.LogError("No days are selected.");
            return;
        }
        if (aimInput.text.Length == 0)
        {
            Debug.LogError("No aims are set");
            return;
        }

        //this==GameManager
        GetComponent<TimetableController>().addAim(trueDayList, aimInput.text);

        cancelWithTime();
    }

    /// <summary>
    /// 
    /// </summary>
    void cancelWithAim()
    {
        foreach (Toggle day in daySelectAimList)
        {
            day.isOn = false;
        }
        aimInput.text = "";

        addAimPannel.SetActive(false);
        addSchedulePannel.SetActive(false);

    }

}
