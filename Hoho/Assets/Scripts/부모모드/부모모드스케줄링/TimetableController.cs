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
    /// �־��� ������ �־��� �ð��� �ð�ǥ�� �����ϰ� ������ ������. 
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
            Transform table = dayColumn.transform.Find("���Ͻð�ǥ");

            int siblingIndex = 0;
            foreach(var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                Debug.Log(currentTime.parent.name+", "+currentTime.name);

                string timeBefore = currentTime.Find("�ð�").GetComponent<TextMeshProUGUI>().text;
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
            newTime.transform.Find("����").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("�ð�").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = mode.gameObject.name.Contains("ǥ��") ? 0 : mode.gameObject.name.Contains("����") ? 1 : 2;
            
            newTime.GetComponent<Image>().sprite=modeImages[imageIdx];

            Debug.Log(siblingIndex);
            newTime.transform.SetSiblingIndex(siblingIndex);

            ParentDataController.ScheduleInformation schedule = new ParentDataController.ScheduleInformation();
            schedule.��� = mode.gameObject.name;
            schedule.�� = Int32.Parse(minute);
            schedule.�� = Int32.Parse(hour);
            schedule.���� = dayToggle.name + "����";
            schedule.���� = title;

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
            Transform table = dayColumn.transform.Find("���Ͻð�ǥ");

            int siblingIndex = 0;
            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }

                string timeBefore = currentTime.Find("�ð�").GetComponent<TextMeshProUGUI>().text;
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
            newTime.transform.Find("����").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("�ð�").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = 3;

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];

            newTime.transform.SetSiblingIndex(siblingIndex);


            ParentDataController.ScheduleInformation schedule = new ParentDataController.ScheduleInformation();
            schedule.�����ð� = Int32.Parse(exhale);
            schedule.���������½ð� = Int32.Parse(exhaleSustain);
            schedule.����ð� = Int32.Parse(inhale);
            schedule.��������½ð� = Int32.Parse(inhaleSustain);
            schedule.��� = "��������Ǹ��";
            schedule.�ݺ�Ƚ�� = Int32.Parse(repeat);
            schedule.�� = Int32.Parse(minute);
            schedule.�� = Int32.Parse(hour);
            schedule.���� = dayToggle.name + "����";
            schedule.���� = title;

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
        date.text = nowTime.Year + "�� " + nowTime.Month + "�� " + nowTime.Day + "�� " + dayOfToday() + "����";

        //���� ��¥�� �ش��ϴ� ���� ���� ǥ��. 
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

        //Firebase���� ������ �ҷ��ͼ� �ð�ǥ �����ϱ�.
        ParentDataController.ReceiveScheduleInfo(initializeSchedule);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //transform.SetSiblingIndex �� �̿��ؼ� ���� ���� ����.


    void initializeSchedule()
    {
        foreach(var schedule in ParentDataController.scheduleInformationList)
        {
            GameObject dayColumn = daysTimeList.Find(x => x.name == schedule.����[0].ToString());
            Transform table = dayColumn.transform.Find("���Ͻð�ǥ");
            int siblingIndex = 0;

            string title = schedule.����;
            string hour = (schedule.�� < 10 ? "0" : "") + schedule.��.ToString();
            string minute = (schedule.�� < 10 ? "0" : "") + schedule.��.ToString();

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("����").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("�ð�").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.���.Contains("ǥ��") ? 0 : schedule.���.Contains("����") ? 1 : schedule.���.Contains("����")? 2 : 3;

            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                Debug.Log(currentTime.parent.name + ", " + currentTime.name);

                string timeBefore = currentTime.Find("�ð�").GetComponent<TextMeshProUGUI>().text;
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
            day="��";
        else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
            day=("ȭ");
        else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
            day=("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
            day=("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Friday)
            day=("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
            day=("��");
        else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
            day=("��");

        return day;
    }
}
