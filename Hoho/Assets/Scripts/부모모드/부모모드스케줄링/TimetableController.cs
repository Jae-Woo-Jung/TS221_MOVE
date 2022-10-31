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
            GameObject day=daysTimeList.Find(x => x.name == dayToggle.name);
            

        }
    }

    public void addAim(List<Toggle> daysList, string title, string hour, string minute, Toggle mode)
    {
        foreach (var dayToggle in daysList)
        {
            GameObject day = daysTimeList.Find(x => x.name == dayToggle.name);


        }
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
           Image dayImage=day.transform.Find("Image").GetComponent<Image>();
           
            if (day.name != dayOfToday())
            {
                dayImage.color = Color.white;
            }
            else
            {
                dayImage.color = new Color(255/255f, 197/255f, 0);
            }
        }

        //Firebase���� ������ �ҷ��ͼ� �ð�ǥ �����ϱ�.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //transform.SetSiblingIndex �� �̿��ؼ� ���� ���� ����.






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
