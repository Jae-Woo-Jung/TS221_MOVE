using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class AddSchedule : MonoBehaviour
{


    public List<Toggle> daySelectList = new List<Toggle>();
    public List<Toggle> daySelectCustomList = new List<Toggle>();

    public TMP_InputField titleInput;
    public TMP_InputField hourInput;
    public TMP_InputField minuteInput;
    public List<Toggle> modeList = new List<Toggle>();

    public TMP_InputField titleCustomInput;
    public TMP_InputField hourCustomInput;
    public TMP_InputField minuteCustomInput;


    public TMP_InputField repeatInput;
    public TMP_InputField inhaleInput;
    public TMP_InputField inhaleSustainInput;
    public TMP_InputField exhaleInput;
    public TMP_InputField exhaleSustainInput;


    public GameObject addSchedulePannel;
    public GameObject addCustomPannel;
    public GameObject customBackground;


    public Toggle mode1;
    public Toggle mode2;
    public Toggle mode3;

    public Button applyWithTimeBtn;
    public Button cancelWithTimeBtn;

    public Button applyWithCustomBtn;

    // Start is called before the first frame update
    void Start()
    {
        applyWithTimeBtn.onClick.AddListener(applyWithTime);
        cancelWithTimeBtn.onClick.AddListener(cancelWithTime);
        
        hourInput.onEndEdit.AddListener(x=> hourInput.text=hourChecker(x));
        minuteInput.onEndEdit.AddListener(x=> minuteInput.text=minuteChecker(x));

        mode1.onValueChanged.AddListener((bool b) =>             
            {
                if (b)
                {
                    mode2.isOn = false;
                    mode3.isOn = false;
                }
            }        
        );

        mode2.onValueChanged.AddListener((bool b) =>
            {
                if (b)
                {
                    mode1.isOn = false;
                    mode3.isOn = false;
                }
            }
        );

        mode3.onValueChanged.AddListener((bool b) =>
            {
                if (b)
                {
                    mode1.isOn = false;
                    mode2.isOn = false;
                }
            }
        );


        repeatInput.onEndEdit.AddListener(x => repeatInput.text = breathTimeChecker(x));
        inhaleInput.onEndEdit.AddListener(x => inhaleInput.text = breathTimeChecker(x));
        inhaleSustainInput.onEndEdit.AddListener(x => inhaleSustainInput.text = breathTimeChecker(x));
        exhaleInput.onEndEdit.AddListener(x => exhaleInput.text = breathTimeChecker(x));
        exhaleSustainInput.onEndEdit.AddListener(x => exhaleSustainInput.text = breathTimeChecker(x));

        applyWithCustomBtn.onClick.AddListener(applyWithCustom);

    }

    // Update is called once per frame
    void Update()
    {
        
    }





    string hourChecker(string hr)
    {
        int h=0;
        Int32.TryParse(hr, out h);
        h = Mathf.Clamp(h, 0, 23);

        return (h < 10 ? "0" : "") + h.ToString();
    }

    string minuteChecker(string min)
    {

        int m=0;
        Int32.TryParse(min, out m);
        m = Mathf.Clamp(m, 0, 59);

        return  (m<10? "0":"")+m.ToString();
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

    string breathTimeChecker(string time)
    {
        int t = Int32.Parse(time);
        t=Mathf.Clamp(t, 0, 99);

        return t.ToString();
    }


    void applyWithTime()
    {
        List<Toggle> trueDayList = daySelectList.FindAll(x => x.isOn);
        List<Toggle> modeList = this.modeList.FindAll(x => x.isOn);
        
        //Check the legibility of inputs.
        if (trueDayList.Count == 0)
        {
            AndroidBLEPluginStart.CallByAndroid("요일을 선택해주세요.");
            //Debug.LogError("No days are selected.");
            return;
        }
        if (titleInput.text.Length ==0)
        {
            AndroidBLEPluginStart.CallByAndroid("제목을 입력해주세요.");
            //Debug.LogError("title is null.");
            return;
        }
        if (hourInput.text.Length == 0 || minuteInput.text.Length == 0)
        {
            AndroidBLEPluginStart.CallByAndroid("시간을 입력해주세요.");
            //Debug.LogError("time is null");
            return;
        }
        if (modeList.Count !=1)
        {
            AndroidBLEPluginStart.CallByAndroid("모드를 하나 선택해주세요.");
            //Debug.LogError(modeList.Count+" modes are selected");
            return;
        }


        //this==GameManager
        GetComponent<TimetableController>().addSchedule(trueDayList, titleInput.text, hourInput.text, minuteInput.text, modeList[0]);

        cancelWithTime();
    }

    void applyWithCustom()
    {
        List<Toggle> trueDayList = daySelectCustomList.FindAll(x => x.isOn);

        //Check the legibility of inputs.
        if (trueDayList.Count == 0)
        {
            AndroidBLEPluginStart.CallByAndroid("요일을 선택해주세요.");
            //Debug.LogError("No days are selected.");
            return;
        }
        if (titleCustomInput.text.Length == 0)
        {
            AndroidBLEPluginStart.CallByAndroid("제목을 입력해주세요.");
            //Debug.LogError("title is null.");
            return;
        }
        if (hourCustomInput.text.Length * minuteCustomInput.text.Length * inhaleInput.text.Length * inhaleSustainInput.text.Length * exhaleInput.text.Length * exhaleSustainInput.text.Length == 0)
        {
            AndroidBLEPluginStart.CallByAndroid("시간 및 반복횟수를 모두 입력해주세요.");
            //Debug.LogError("time is null");
            return;
        }

        //this==GameManager
        GetComponent<TimetableController>().addCustomSchedule(trueDayList, titleInput.text, hourInput.text, minuteInput.text, 
            repeatInput.text, inhaleInput.text, inhaleSustainInput.text, exhaleInput.text, exhaleSustainInput.text);

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

        foreach (Toggle day in daySelectCustomList)
        {
            day.isOn = false;
        }

        titleCustomInput.text = hourCustomInput.text = minuteCustomInput.text =titleInput.text = hourInput.text=minuteInput.text="";

        inhaleInput.text = inhaleSustainInput.text = exhaleInput.text = exhaleSustainInput.text = "";

        foreach (Toggle mode in modeList)
        {
            mode.isOn = false;
        }
        
        addCustomPannel.SetActive(false);
        addSchedulePannel.SetActive(false);
        customBackground.SetActive(false);

    }





}
