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

        ChildDataController.fishGameResultList.Sort((result1, result2) => compareStringTime(result1.시작날짜, result2.시작날짜));


        int refH = -1;
        int refM = -1;

        if (ChildDataController.fishGameResultList.Count> 0)
        {
            var LastResult=ChildDataController.fishGameResultList.FindLast(x=>true);

            DateTime RefDate = Convert.ToDateTime(LastResult.시작날짜);

            //마지막으로 호흡한 시간. 
            refH = RefDate.Hour;
            refM = RefDate.Minute;
        }
        Debug.Log("initializeSchedule. refH : " + refH + ", refM : " + refM);


        int scheduleNum = 0;
        int finishedScheduleNum = 0;

        foreach (var schedule in ChildDataController.scheduleInformationList)
        {
            scheduleNum++;

            int nowH = new DateTime().Hour;
            int nowM = new DateTime().Minute;

            Transform table = scheduleContent.transform;
            int siblingIndex = 0;


            Debug.Log("initializeSchedul : "+schedule.요일+schedule.제목);

            string title = schedule.제목;
            string hour = (schedule.시 < 10 ? "0" : "") + schedule.시.ToString();
            string minute = (schedule.분 < 10 ? "0" : "") + schedule.분.ToString();

            Debug.Log(schedule.요일);

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.모드.Contains("표준") ? 0 : schedule.모드.Contains("집중") ? 1 : schedule.모드.Contains("안정") ? 2 : 3;

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];

            //추가할 스케줄
            int hAfter;
            Int32.TryParse(hour, out hAfter);
            int minAfter;
            Int32.TryParse(minute, out minAfter);

            


            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (hAfter<refH || (hAfter == refH && minAfter <= refM))
                {
                    newTime.transform.Find("완료표시").gameObject.SetActive(true);
                    hAfter += 24;
                    minAfter += 60;
                    finishedScheduleNum++;
                }
                else
                {
                    newTime.transform.Find("완료표시").gameObject.SetActive(false);
                }



                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                Debug.Log(currentTime.parent.name + ", " + currentTime.name);

                //기존의 스케줄
                string timeBefore = currentTime.Find("시간").GetComponent<TextMeshProUGUI>().text;
                int hBefore;
                Int32.TryParse(timeBefore.Substring(0, 2), out hBefore);
                int minBefore;
                Int32.TryParse(timeBefore.Substring(3, 2), out minBefore);

                //뒤에 있어야 할 조건 : 
                if (hBefore < hAfter || (hBefore == hAfter && minBefore < minAfter))
                {
                    siblingIndex++;
                }
            }                        

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];
            newTime.transform.SetSiblingIndex(siblingIndex);
        }

        gameProgress.value = scheduleNum==0? 0 : finishedScheduleNum/(float) scheduleNum;

    }

    int compareStringTime(string time1, string time2)
    {
        DateTime date1 = Convert.ToDateTime(time1);
        DateTime date2 = Convert.ToDateTime(time2);

        if (date1.Hour < date2.Hour || (date1.Hour==date2.Hour && date1.Minute < date2.Minute))
        {
            return -1;
        }
        else if (date1.Hour == date2.Hour && date1.Minute == date2.Minute)
        {
            return 0;
        }

        return 1;
    }
}
