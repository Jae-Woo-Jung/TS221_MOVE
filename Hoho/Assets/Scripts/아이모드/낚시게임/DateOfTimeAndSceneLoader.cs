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
            SceneManager.LoadScene("낚시모드선택");
        }
        else
        {
            SceneManager.LoadScene("낚시진행");
        }*/

        if (TodaySchedule.mode.Contains("자유"))
        {
            SceneManager.LoadScene("낚시모드선택");
        }
        else
        {
            SceneManager.LoadScene("낚시진행");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int modeIndex = TodaySchedule.mode.Contains("표준") ? 0 : TodaySchedule.mode.Contains("집중") ? 1 : TodaySchedule.mode.Contains("안정") ? 2 : TodaySchedule.mode.Contains("사용자") ? 3 : -1;


        if (modeIndex >= 0)
        {
            modeIcon.sprite = modeIconList[modeIndex];

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
