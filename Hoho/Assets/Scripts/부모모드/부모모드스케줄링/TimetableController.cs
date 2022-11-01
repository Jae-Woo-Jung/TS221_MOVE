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

                //Ƚ���� ������ �ִ� ���.
                if (currentTime.Find("�ð�")== null)
                {
                    Destroy(currentTime.gameObject);
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

                if (hBefore < hAfter || (hBefore==hAfter && minBefore<minAfter))
                {
                    siblingIndex++;
                }
            }

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("����").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("�ð�").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            var sp = newTime.transform.Find("��������").GetComponent<Image>().sprite;
            sp=mode.transform.Find("Background").GetComponent<Image>().sprite;

            Debug.Log(siblingIndex);
            newTime.transform.SetSiblingIndex(siblingIndex);
        }
    }

    public void addAim(List<Toggle> daysList, string aim)
    {
        foreach (var dayToggle in daysList)
        {
            GameObject dayColumn = daysTimeList.Find(x => x.name == dayToggle.name);
            Transform table = dayColumn.transform.Find("���Ͻð�ǥ");

            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {
                if (currentTime.tag!="TimeOrAim")
                {
                    continue;
                }

                Destroy(currentTime);
            }

            GameObject newAim = Instantiate(aimPrefab, table);
            newAim.transform.Find("����").GetComponent<TextMeshProUGUI>().text = dayToggle.name + " �Ϸ�\n�����Ӱ�\n"+aim+"�� �̻�";
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
