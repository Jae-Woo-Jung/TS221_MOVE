using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class ResultController : MonoBehaviour
{
    /// <summary>
    /// 게임 성취도 0, 1, 2, 3
    /// </summary>
    static int achievement=0;

    /// <summary>
    /// 게임 한 판 후 표시되는 점수
    /// </summary>
    static int gamePoint=0;

    /// <summary>
    /// 적립된 총 포인트.
    /// </summary>
    static int nowPoint=0;

    /// <summary>
    /// 게임 모드.  표준 모드, 집중 모드, 안정 모드, 사용자정의모드
    /// </summary>
    static string mode="집중 모드";

    public TextMeshProUGUI modeText;
    public List<GameObject> stars = new List<GameObject>();

    public TextMeshProUGUI pointText;
    public TextMeshProUGUI nowPointText;
    
   

    /// <summary>
    /// achievement : 게임 성취도를 설정. 0~3 사이의 값.
    /// </summary>
    /// <param name="a"></param>
    public static void setAchievement(int a)
    {
        achievement = Math.Clamp(a, 0, 3);
    }

    /// <summary>
    /// achievement : 게임 성취도, 0~3
    /// </summary>
    /// <returns></returns>
    public static int getAchievement()
    {
        return achievement;
    }

    /// <summary>
    /// 게임 한 판 후 표시되는 점수
    /// </summary>
    public static void setGamePoint(int point)
    {
        gamePoint = point;
    }
    /// <summary>
    /// 게임 한 판 후 표시되는 점수
    /// </summary>
    public static int getGamePoint()
    {
        return gamePoint;
    }

    /// <summary>
    /// 적립된 총 포인트.
    /// </summary>
    public static void setNowPoint(int point)
    {
        nowPoint = point;
        ChildDataController.setPoint(point);
    }

    /// <summary>
    /// 적립된 총 포인트.
    /// </summary>
    public static int getNowPoint()
    {
        return nowPoint;
    }

    public static void setMode(string mode)
    {
        ResultController.mode = mode;
    }

    public static string getMode()
    {
        return mode;
    }


    // Start is called before the first frame update
    void Start()
    {
        pointText.text = gamePoint.ToString();
        nowPointText.text = nowPoint.ToString();
        modeText.text = mode;
        showStars(achievement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 별 0~3개를 보여줌.
    /// </summary>
    /// <param name="num"></param>
    private void showStars(int num)
    {
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }

        switch (num)
        {
            case 0:
                break;
            case 1:
                stars[0].SetActive(true);
                break;
            case 2:
                stars[0].SetActive(true);
                stars[2].SetActive(true);
                break;
            case 3:
                foreach (GameObject star in stars)
                {
                    star.SetActive(true);
                }
                break;
        }
    }

}
