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

        int modeIndex = TodaySchedule.mode.Contains("ǥ��") ? 0: TodaySchedule.mode.Contains("����") ? 1 : TodaySchedule.mode.Contains("����") ? 2 : TodaySchedule.mode.Contains("�����") ? 3 : -1;


        if (modeIndex >= 0)
        {
            modeIcon.sprite = modeIconList[modeIndex];

        }


        DateTime nowDt = DateTime.Now;
        if ((nowDt.DayOfWeek == DayOfWeek.Saturday)||(nowDt.DayOfWeek == DayOfWeek.Sunday))
        {
            SceneManager.LoadScene("���ø�弱��");
        }
        else
        {
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
        
    }
}
