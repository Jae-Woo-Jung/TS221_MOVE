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
    [Tooltip("1~6단계, 점들")]
    public GameObject progressLevel;
    [Tooltip("점들간 검은선")]
    public GameObject verticalLine;
    [Tooltip("점prefab")]
    public GameObject dotPrefab;


    /// <summary>
    /// pointShop의 point를 더함. static 변수에 저장. 나중에 파이어베이스에 보내기도 해야함. 
    /// </summary>
    /// <param name="point"></param>
    public void addPoint(int point)
    {
        point += parsePoint(currentPoint);
        ParentProgressController.point = point;
        currentPoint.text = "현재  "+point;        
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
        Debug.Log("point : "+point+", level : "+level);
        point = (int) ParentDataController.getValues()["point"];
        level = (int) ParentDataController.getValues()["level"];
        //totalLevel
        goalPoint_static = (int) ParentDataController.getValues()["goalPoint"];

        totalPointContent = GameObject.Find("전체포인트").GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + point + "P\n" + ParentDataController.getValues()["rewardTitle"];

        progressRatio = (float)point / goalPoint_static;
        Debug.Log("progressRatio : " + progressRatio);
        currentPoint.text = "현재  " + ParentProgressController.point;
        ParentProgressController.level = Math.Max(1, level);

        generateLevel(level);
        GameObject totalPoint = GameObject.Find("전체포인트");

        if (level > totalLevel)
        {
            Debug.Log("레벨 상한 도달");
        }
        
        //totalPointContent = totalPoint.GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + parsePoint(currentPoint) + "P\n" + totPointContent;
        goalPointText.text = "완성  " + goalPoint_static;
        Debug.Log("목표점수 : " + goalPoint_static);

        Debug.Log(goalPointText.text);
        updateProgress();        
    }

    /// <summary>
    /// 점 개수 생성
    /// </summary>
    /// <param name="level"></param>
    private void generateLevel(int level)
    {
        Transform[] transformList = progressLevel.transform.GetComponentsInChildren<Transform>();
        foreach (Transform dot in transformList)
        {
            if ((dot.name).Contains("점"))
            {
                Destroy(dot.gameObject);
            }
        }

        List<GameObject> dots=new List<GameObject>();
        for (int i=0; i<level; i++)
        {
            Instantiate(dotPrefab, progressLevel.transform);
            dotPrefab.GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();
            dots.Add(dotPrefab);
        }


        verticalLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dots[0].GetComponent<RectTransform>().rect.yMin -dots[dots.Count-1].GetComponent<RectTransform>().rect.yMax);
        verticalLine.transform.position = new Vector2(dots[0].transform.position.x, (dots[0].transform.position.y + dots[dots.Count - 1].transform.position.y)/2 );
        
        if (totalLevel == 1)
        {
            verticalLine.SetActive(false);
        }
        updateLevel(level);
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
    /// progressLevel의 점들 중 lv 이하의 점들은 검정으로, 나머지는 하얀색으로 바꿈.
    /// </summary>
    /// <param name="lv"></param>
    private void updateLevel(int lv)
    {
        for (int i=0; i<progressLevel.transform.childCount; i++)
        {
            if (i < lv)
            {
                progressLevel.transform.GetChild(i).GetComponent<Image>().color = Color.black; //lv 이하의 점들은 검정으로
            }
            else
            {
                progressLevel.transform.GetChild(i).GetComponent<Image>().color = Color.white;  //lv 이후의 점들은 하얀색으로
            }
        }
    }

    /// <summary>
    /// progressLevel에 표시된 레벨(검은 점 개수)을 반환함.
    /// </summary>
    /// <returns></returns>
    private int getCurrentLevel()
    {
        int level = 0;
        Debug.Log("progressLevel's child# is "+progressLevel.transform.childCount);
        for (int i = 0; i < progressLevel.transform.childCount; i++)
        {
            if (progressLevel.transform.GetChild(i).GetComponent<Image>().color == Color.black)
            {
                level = i+1;  //0번째인 경우, 레벨1
            }
        }

        Debug.Log("current level is "+level);
        return level;
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

        updateTotalPoint();
    }

}
