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
            ChildDataController.fishGameResult.스케줄적용 = false;
            SceneManager.LoadScene("낚시모드선택");
        }
        else
        {
            ChildDataController.fishGameResult.스케줄적용 = true;
            ChildDataController.fishGameResult.스케줄분 = ChildDataController.scheduleInformationList[0].분;
            ChildDataController.fishGameResult.스케줄시 = ChildDataController.scheduleInformationList[0].시;
            ChildDataController.fishGameResult.모드 = TodaySchedule.mode;
            SceneManager.LoadScene("낚시진행");
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
        int modeIndex = TodaySchedule.mode.Contains("표준") ? 0 : TodaySchedule.mode.Contains("집중") ? 1 : TodaySchedule.mode.Contains("안정") ? 2 : TodaySchedule.mode.Contains("사용자") ? 3 : TodaySchedule.mode.Contains("자유") ? 4 : -1;

        if (modeIndex >= 0)
        {
            modeIcon.sprite = modeIconList[modeIndex];
        }
    }

    void updateMode()
    {
        if (ChildDataController.scheduleInformationList.Count == 0)
        {
            TodaySchedule.mode = "자유모드";
            return;
        }

        ChildDataController.scheduleInformationList.Sort(TodaySchedule.compareSchedule);
        var info = ChildDataController.scheduleInformationList[0];

        if (info.완료 || Math.Abs(info.시*60+info.분 - DateTime.Now.Hour*60+DateTime.Now.Minute )>5)
        {
            TodaySchedule.mode = "자유모드";
        }
        if (!info.완료)
        {
            TodaySchedule.mode = info.모드;            
        }
    }

}
