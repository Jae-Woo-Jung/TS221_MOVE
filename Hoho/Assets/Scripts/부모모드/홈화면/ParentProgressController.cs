using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentProgressController : MonoBehaviour
{
    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    public static int point;
    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    public static int goalPoint_static;
    /// <summary>
    /// 현재 진행 보상 단계. PointShop에서 검은색 점의 개수.
    /// </summary>
    public static int level;
    /// <summary>
    /// 현재 설정된 총 보상 개수
    /// </summary>
    public static int totalLevel;
    /// <summary>
    /// 원 안에 있는 텍스트. 가령, <1>2370P 놀이공원.  
    /// </summary>
    public static string totalPointContent;
    /// <summary>
    /// 현재 점수/목표 점수
    /// </summary>
    public static float progressRatio;

    [Tooltip("현재점수 int")]
    public int pointTest;
    [Tooltip("현재 level int")]
    public int levelTest;
    [Tooltip("원 안의 내용 string")]
    public string totalPointContentTest;

    [Tooltip("포인트동그라미")]
    public GameObject pointCircle;
    [Tooltip("진행률")]
    public TextMeshProUGUI pointRatio;
    [Tooltip("현재점수")]
    public TextMeshProUGUI currentPoint;
    [Tooltip("목표점수")]
    public TextMeshProUGUI goalPointText;


    /// <summary>
    /// pointShop의 point를 더함. static 변수에 저장. 나중에 파이어베이스에 보내기도 해야함. 
    /// </summary>
    /// <param name="point"></param>
    public void addPoint(int point)
    {
        point += parsePoint(currentPoint);
        ParentProgressController.point = point;
        currentPoint.text = point.ToString();        
        updateProgress();
    }

    public int getCurrentPoint()
    {
        return parsePoint(currentPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        //데이터 가져오기.
        point = 0;
        level = 1;
        totalLevel = 6;
        goalPoint_static = 4000;

        ParentDataController.ReceivePoint(initProgress);
    }

    // Update is called once per frame
    void Update()
    {
        pointTest=point; 
        levelTest=level;        
        totalPointContentTest=totalPointContent;
    }

    /// <summary>
    /// 현재 점수, 레벨 진행 상황, 진행률, 
    /// </summary>
    private void initProgress()
    {
        Debug.Log("initProgress point : "+point);
        point = (int) ParentDataController.getValues()["point"];
        //totalLevel
        goalPoint_static = (int) ParentDataController.getValues()["goalPoint"];

        //totalPointContent = GameObject.Find("전체포인트").GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + point + "P\n" + ParentDataController.getValues()["rewardTitle"];

        progressRatio = (float)point / goalPoint_static;
        Debug.Log("progressRatio : " + progressRatio);
        currentPoint.text =  ParentProgressController.point.ToString();
        
        //totalPointContent = totalPoint.GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + parsePoint(currentPoint) + "P\n" + totPointContent;
        goalPointText.text =  goalPoint_static.ToString();
        Debug.Log("목표점수 : " + goalPoint_static);

        Debug.Log(goalPointText.text);

        updateProgress();        
    }



    /// <summary>
    /// 이 스크립트에 정의된 Text 오브젝트가 주어지면, 그 text에서 int로 몇 P인지 반환. (가령, "1143P"는 1143 반환.)
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private int parsePoint(TextMeshProUGUI point)
    {
        string tempNum = null;

        for (int i=0; i<point.text.Length; i++)
        {
            if (char.IsDigit(point.text[i]))
                tempNum += point.text[i];
        }
        return Int32.Parse(tempNum);
    }

    /// <summary>
    /// 동그라미 속 text의 point 부분을 현재 점수에 따라 업데이트함.
    /// </summary>
    private void updateTotalPoint()
    {
        GameObject totalPoint = GameObject.Find("전체포인트");  //전체포인트는 진행상황영역/포인트동그라미/innerBoder/ContentArea의 하위 요소.
        string currentText = totalPoint.GetComponent<TextMeshProUGUI>().text;
        string changeText = currentText.Substring(currentText.IndexOf("\n") + 1);
        changeText=changeText.Substring(0, changeText.IndexOf("P")+1);

        totalPointContent=totalPoint.GetComponent<TextMeshProUGUI>().text = currentText.Replace(changeText, getCurrentPoint()+"P");
    }

    /// <summary>
    /// 현재 점수를 기준으로 lv(점 개수), 원의 색칠된 비율, 진행률을 변경. 진행률이 100% 이상이면 (가능한 경우) 그다음 보상 단계로 넘어감.
    /// </summary>
    private void updateProgress()
    {
        pointRatio.text = parsePoint(currentPoint)*100 / parsePoint(goalPointText)+"%";
        progressRatio = pointCircle.GetComponent<Slider>().value = parsePoint(pointRatio) / 100.0f;

        GameObject totalPoint = GameObject.Find("제목text");  //전체포인트는 진행상황영역/포인트동그라미/innerBoder/ContentArea의 하위 요소.
        Debug.Log("updateProgress1");
        totalPoint.GetComponent<TextMeshProUGUI>().text = (string) ParentDataController.getValues()["rewardTitle"];
        Debug.Log("updateProgress2");
        //updateTotalPoint();
    }

}
