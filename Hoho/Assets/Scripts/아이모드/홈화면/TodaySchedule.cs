using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class TodaySchedule : MonoBehaviour
{

    public GameObject timePrefab;
    public Slider gameProgress;
    public GameObject scheduleContent;

    public List<Sprite> modeImages = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        ChildDataController.ReceiveBreath( () => ChildDataController.ReceiveScheduleInfo(initializeSchedule));     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializeSchedule()
    {
        foreach (var schedule in ParentDataController.scheduleInformationList)
        {
            int nowH = new DateTime().Hour;
            int nowM = new DateTime().Minute;

            Transform table = scheduleContent.transform;
            int siblingIndex = 0;

            string title = schedule.제목;
            string hour = (schedule.시 < 10 ? "0" : "") + schedule.시.ToString();
            string minute = (schedule.분 < 10 ? "0" : "") + schedule.분.ToString();

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.모드.Contains("표준") ? 0 : schedule.모드.Contains("집중") ? 1 : schedule.모드.Contains("안정") ? 2 : 3;

            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                Debug.Log(currentTime.parent.name + ", " + currentTime.name);

                string timeBefore = currentTime.Find("시간").GetComponent<TextMeshProUGUI>().text;
                int hBefore;
                Int32.TryParse(timeBefore.Substring(0, 2), out hBefore);
                int minBefore;
                Int32.TryParse(timeBefore.Substring(3, 2), out minBefore);

                int hAfter;
                Int32.TryParse(hour, out hAfter);
                int minAfter;
                Int32.TryParse(minute, out minAfter);

                //뒤에 있어야 할 조건 : 
                if (hBefore < hAfter || (hBefore == hAfter && minBefore < minAfter))
                {
                    siblingIndex++;
                }
            }                        

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];
            newTime.transform.SetSiblingIndex(siblingIndex);
        }

    }
}
