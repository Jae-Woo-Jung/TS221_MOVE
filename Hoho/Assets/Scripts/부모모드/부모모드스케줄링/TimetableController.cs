using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class TimetableController : MonoBehaviour
{



    public GameObject weekTable;
    public TextMeshProUGUI date;

    public List<GameObject> daysTimeList = new List<GameObject>();

    public GameObject timePrefab;
    public GameObject aimPrefab;

    public List<Sprite> modeImages = new List<Sprite>();

    public GameObject deletePannel;

    /// <summary>
    /// 주어진 요일의 주어진 시간에 시간표를 설정하고 데이터 보내기. 
    /// </summary>
    /// <param name="daysList"></param>
    /// <param name="title"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="mode"></param>
    
    public void addSchedule(List<Toggle> daysList, string title, string hour, string minute, Toggle mode)
    {
        foreach(var dayToggle in daysList)
        {
            GameObject dayColumn=daysTimeList.Find(x => x.name == dayToggle.name);
            Transform table = dayColumn.transform.Find("일일시간표");

            int siblingIndex = 0;
            foreach(var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                Debug.Log(currentTime.parent.name+", "+currentTime.name);

                string timeBefore = currentTime.Find("시간").GetComponent<TextMeshProUGUI>().text;
                int hBefore; 
                Int32.TryParse(timeBefore.Substring(0, 2), out hBefore);
                int minBefore;
                Int32.TryParse(timeBefore.Substring(3, 2), out minBefore);
                
                int hAfter;
                Int32.TryParse(hour, out hAfter);
                int minAfter;
                Int32.TryParse(minute, out minAfter);

                if (hBefore < hAfter || (hBefore==hAfter && minBefore<minAfter))
                {
                    siblingIndex++;
                }
            }

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = mode.gameObject.name.Contains("표준") ? 0 : mode.gameObject.name.Contains("집중") ? 1 : 2;
            
            newTime.GetComponent<Image>().sprite=modeImages[imageIdx];

            Debug.Log(siblingIndex);
            newTime.transform.SetSiblingIndex(siblingIndex);

            ParentDataController.ScheduleInformation schedule = new ParentDataController.ScheduleInformation();
            schedule.모드 = mode.gameObject.name;
            schedule.분 = Int32.Parse(minute);
            schedule.시 = Int32.Parse(hour);
            schedule.요일 = dayToggle.name + "요일";
            schedule.제목 = title;

            ParentDataController.scheduleInformationList.Add(schedule);

        }

        Debug.Log("addCustomSchedule 10");

        ParentDataController.SendScheduleInformation();

    }

    public void addCustomSchedule(List<Toggle> daysList, string title, string hour, string minute,
            string repeat, string inhale, string inhaleSustain, string exhale, string exhaleSustain)
    {
        foreach (var dayToggle in daysList)
        {
            GameObject dayColumn = daysTimeList.Find(x => x.name == dayToggle.name);
            Transform table = dayColumn.transform.Find("일일시간표");

            int siblingIndex = 0;
            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }

                string timeBefore = currentTime.Find("시간").GetComponent<TextMeshProUGUI>().text;
                int hBefore;
                Int32.TryParse(timeBefore.Substring(0, 2), out hBefore);
                int minBefore;
                Int32.TryParse(timeBefore.Substring(3, 2), out minBefore);

                int hAfter;
                Int32.TryParse(hour, out hAfter);
                int minAfter;
                Int32.TryParse(minute, out minAfter);

                if (hBefore < hAfter || (hBefore == hAfter && minBefore < minAfter))
                {
                    siblingIndex++;
                }
            }

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = 3;

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];

            newTime.transform.SetSiblingIndex(siblingIndex);


            ParentDataController.ScheduleInformation schedule = new ParentDataController.ScheduleInformation();
            schedule.날숨시간 = Int32.Parse(exhale);
            schedule.날숨후참는시간 = Int32.Parse(exhaleSustain);
            schedule.들숨시간 = Int32.Parse(inhale);
            schedule.들숨후참는시간 = Int32.Parse(inhaleSustain);
            schedule.모드 = "사용자정의모드";
            schedule.반복횟수 = Int32.Parse(repeat);
            schedule.분 = Int32.Parse(minute);
            schedule.시 = Int32.Parse(hour);
            schedule.요일 = dayToggle.name + "요일";
            schedule.제목 = title;

            ParentDataController.scheduleInformationList.Add(schedule);
        }

        Debug.Log("addCustomSchedule 10");

        ParentDataController.SendScheduleInformation();
    }




    // Start is called before the first frame update
    void Start()
    {
        //Change date to today
        DateTime nowTime = DateTime.Today;
        date.text = nowTime.Year + "년 " + nowTime.Month + "월 " + nowTime.Day + "일 " + dayOfToday() + "요일";

        //오늘 날짜에 해당하는 요일 강조 표시. 
        foreach(var day in daysTimeList)
        {
           Image dayImage=day.GetComponent<Image>();
           
            if (day.name != dayOfToday())
            {
                dayImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                dayImage.color = new Color(255/255f, 197/255f, 0);
            }
        }

        //Firebase에서 데이터 불러와서 시간표 정리하기.
        ParentDataController.ReceiveScheduleInfo(initializeSchedule);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //transform.SetSiblingIndex 를 이용해서 순서 변경 가능.


    void initializeSchedule()
    {
        foreach(var schedule in ParentDataController.scheduleInformationList)
        {
            GameObject dayColumn = daysTimeList.Find(x => x.name == schedule.요일[0].ToString());
            Transform table = dayColumn.transform.Find("일일시간표");
            int siblingIndex = 0;

            string title = schedule.제목;
            string hour = (schedule.시 < 10 ? "0" : "") + schedule.시.ToString();
            string minute = (schedule.분 < 10 ? "0" : "") + schedule.분.ToString();

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.모드.Contains("표준") ? 0 : schedule.모드.Contains("집중") ? 1 : schedule.모드.Contains("안정")? 2 : 3;

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

                if (hBefore < hAfter || (hBefore == hAfter && minBefore < minAfter))
                {
                    siblingIndex++;
                }
            }

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];
            newTime.transform.SetSiblingIndex(siblingIndex);

        }

    }

    string dayOfToday()
    {
        DateTime nowDt = DateTime.Now;

        string day = "";
        if (nowDt.DayOfWeek == DayOfWeek.Monday)
            day="월";
        else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
            day=("화");
        else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
            day=("수");
        else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
            day=("목");
        else if (nowDt.DayOfWeek == DayOfWeek.Friday)
            day=("금");
        else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
            day=("토");
        else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
            day=("일");

        return day;
    }
}
