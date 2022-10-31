using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class ResultController : MonoBehaviour
{
    /// <summary>
    /// ���� ���뵵 0, 1, 2, 3
    /// </summary>
    static int achievement=0;

    /// <summary>
    /// ���� �� �� �� ǥ�õǴ� ����
    /// </summary>
    static int gamePoint=0;

    /// <summary>
    /// ������ �� ����Ʈ.
    /// </summary>
    static int nowPoint=0;

    /// <summary>
    /// ���� ����
    /// </summary>
    static int level=1;

    public TextMeshProUGUI levelText;
    public List<GameObject> stars = new List<GameObject>();

    public TextMeshProUGUI pointText;
    public TextMeshProUGUI nowPointText;
    
   

    /// <summary>
    /// achievement : ���� ���뵵�� ����. 0~3 ������ ��.
    /// </summary>
    /// <param name="a"></param>
    public static void setAchievement(int a)
    {
        achievement = Math.Clamp(a, 0, 3);
    }

    /// <summary>
    /// achievement : ���� ���뵵, 0~3
    /// </summary>
    /// <returns></returns>
    public static int getAchievement()
    {
        return achievement;
    }

    /// <summary>
    /// ���� �� �� �� ǥ�õǴ� ����
    /// </summary>
    public static void setGamePoint(int point)
    {
        gamePoint = point;
    }
    /// <summary>
    /// ���� �� �� �� ǥ�õǴ� ����
    /// </summary>
    public static int getGamePoint()
    {
        return gamePoint;
    }

    /// <summary>
    /// ������ �� ����Ʈ.
    /// </summary>
    public static void setNowPoint(int point)
    {
        nowPoint = point;
        ChildDataController.setPoint(point);
    }

    /// <summary>
    /// ������ �� ����Ʈ.
    /// </summary>
    public static int getNowPoint()
    {
        return nowPoint;
    }

    public static void setLevel(int level)
    {
        ResultController.level = level;
    }

    public static int getLevel()
    {
        return level;
    }


    // Start is called before the first frame update
    void Start()
    {
        pointText.text = gamePoint.ToString();
        nowPointText.text = nowPoint.ToString();
        levelText.text = level.ToString();
        showStars(achievement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �� 0~3���� ������.
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
