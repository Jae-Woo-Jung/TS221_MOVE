using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class DateOfTimeAndSceneLoader : MonoBehaviour
{
    public Image modeIcon;
    public List<Sprite> modeIconList = new List<Sprite>();

    public void SceneLoad()
    {

        /*DateTime nowDt = DateTime.Now;
        if ((nowDt.DayOfWeek == DayOfWeek.Saturday)||(nowDt.DayOfWeek == DayOfWeek.Sunday))
        {
            SceneManager.LoadScene("���ø�弱��");
        }
        else
        {
            SceneManager.LoadScene("��������");
        }*/

        if (TodaySchedule.mode.Contains("����"))
        {
            ChildDataController.fishGameResult.���������� = false;
            SceneManager.LoadScene("���ø�弱��");
        }
        else
        {
            ChildDataController.fishGameResult.���������� = true;
            ChildDataController.fishGameResult.�����ٺ� = ChildDataController.scheduleInformationList[0].��;
            ChildDataController.fishGameResult.�����ٽ� = ChildDataController.scheduleInformationList[0].��;
            ChildDataController.fishGameResult.��� = TodaySchedule.mode;
            SceneManager.LoadScene("��������");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        updateMode();
        int modeIndex = TodaySchedule.mode.Contains("ǥ��") ? 0 : TodaySchedule.mode.Contains("����") ? 1 : TodaySchedule.mode.Contains("����") ? 2 : TodaySchedule.mode.Contains("�����") ? 3 : TodaySchedule.mode.Contains("����") ? 4 : -1;

        if (modeIndex >= 0)
        {
            modeIcon.sprite = modeIconList[modeIndex];
        }
    }

    void updateMode()
    {
        if (ChildDataController.scheduleInformationList.Count == 0)
        {
            TodaySchedule.mode = "�������";
            return;
        }

        ChildDataController.scheduleInformationList.Sort(TodaySchedule.compareSchedule);
        var info = ChildDataController.scheduleInformationList[0];

        if (info.�Ϸ� || Math.Abs(info.��*60+info.�� - DateTime.Now.Hour*60+DateTime.Now.Minute )>5)
        {
            TodaySchedule.mode = "�������";
        }
        if (!info.�Ϸ�)
        {
            TodaySchedule.mode = info.���;            
        }
    }

}
