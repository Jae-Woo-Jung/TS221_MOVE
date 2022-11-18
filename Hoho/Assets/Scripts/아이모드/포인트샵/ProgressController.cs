using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressController : MonoBehaviour
{
    /// <summary>
    /// 전체에서 쓰이는 point
    /// </summary>
    public static int point { 
        get { 
            return (int) ChildDataController.getValues()["point"]; 
        }
        set
        {
            ChildDataController.setPoint(value);
        }    
    }
    /// <summary>
    /// 현재 진행 보상 단계. PointShop에서 검은색 점의 개수.
    /// </summary>
    public static int level
    {
        get
        {
            return (int)ChildDataController.getValues()["level"];
        }
        set
        {
            ChildDataController.setCanSend(true);
            ChildDataController.setLevel(value);
        }
    }
    /// <summary>
    /// 원 안에 있는 텍스트. 가령, "놀이공원".  
    /// </summary>
    public static string rewardTitle
    {
        get
        {
            return (string) ChildDataController.getValues()["rewardTitle"];
        }
        set
        {
            ChildDataController.setRewardTitle(value);
        }
    }

    public static List<string> rewardTitleList
    {
        get
        {
            return (List<string>) ChildDataController.getValues()["rewardTitleList"];
        }
        set
        {
            ChildDataController.setRewardTitleList(value);
        }
    }

    /// <summary>
    /// 원 안에 있는 포인트 string. 가령 "<U>1</U> <U>2</U>"  
    /// </summary>
    public static string pointString;

    /// <summary>
    /// 현재 점수/목표 점수
    /// </summary>
    public static float progressRatio
    {
        get
        {
            return (float) ChildDataController.getValues()["progressRatio"];
        }
        set
        {
            ChildDataController.setProgressRatio(value);
        }
    }

    public int pointTest;
    public int levelTest;
    public string rewardTitleTest;



    [Tooltip("포인트동그라미")]
    public GameObject pointCircle;
    [Tooltip("진행률")]
    public TextMeshProUGUI pointRatio;
    [Tooltip("현재점수")]
    public TextMeshProUGUI currentPoint;
    [Tooltip("목표점수")]
    public TextMeshProUGUI goalPoint;
    [Tooltip("1~6단계, 점들")]
    public GameObject progressLevel;
    [Tooltip("보상목록표")]
    public GameObject rewardTable;


    /// <summary>
    /// pointShop의 point를 더함. static 변수에 저장. 나중에 파이어베이스에 보내기도 해야함. 
    /// </summary>
    /// <param name="point"></param>
    public void addPoint(int point)
    {
        point += parsePoint(currentPoint);
        ChildDataController.setPoint(ProgressController.point = point);
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
        initProgress();
    }

    // Update is called once per frame
    void Update()
    {
        pointTest=point; 
        levelTest=level;
        rewardTitleTest=rewardTitle;
    }

    /// <summary>
    /// 현재 점수, 레벨 진행 상황, 진행률, 
    /// </summary>
    private void initProgress()
    {
        currentPoint.text = ProgressController.point.ToString();
        ProgressController.level = Math.Max(1, level);
        updateLevel(level);
        GameObject rewardTitle = GameObject.Find("제목text");
        //TextMeshProUGUI currentLevel = GameObject.Find("단계Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pointText = GameObject.Find("현재포인트Text").GetComponent<TextMeshProUGUI>();
        

        GameObject.Find("Canvas").transform.Find("보상목록").gameObject.SetActive(true);
        var rows = GameObject.FindGameObjectsWithTag("row");
        GameObject.Find("Canvas").transform.Find("보상목록").gameObject.SetActive(false);

        if (level >= rows.Length)
        {
            level = rows.Length-1;
            Debug.Log("레벨 상한 도달");
        }


        //보상 목록 업데이트.

        List<string> rewardList = new List<string>();
        for (int i=0; i<rows.Length; i++)
        {
            rewardList.Add(getContentfromRow(rows[i]));
        }
        ChildDataController.setRewardTitleList(rewardList);
        

        //currentLevel.GetComponent<TextMeshProUGUI>().text = level.ToString();

        ProgressController.pointString = pointText.text = pointToString(parsePoint(currentPoint));
        ProgressController.rewardTitle=rewardTitle.GetComponent<TextMeshProUGUI>().text = getContentfromRow(rows[level-1]);
        goalPoint.text = "완성  " + getGoalfromRow(rows[level-1]);
        ChildDataController.setGoalPoint(getGoalfromRow(rows[level - 1]));

        Debug.Log(goalPoint.text);

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

    private string pointToString(int point)
    {
        
        string tempPoint = null;
        for (int i = 0; i < point.ToString().Length; i++)
        {
            tempPoint += "<U>" + point.ToString()[i] + "</U> ";
        }
        return tempPoint;
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
    /// 보상 목록의 행에서 내용을 반환.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private string getContentfromRow(GameObject row)
    {
        return row.transform.Find("내용/내용text").GetComponent<TextMeshProUGUI>().text;
    }

    /// <summary>
    /// 보상 목록의 행에서 point 목표를 int로 반환.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private int getGoalfromRow(GameObject row)
    {
        return parsePoint(row.transform.Find("포인트/포인트text").GetComponent<TextMeshProUGUI>());
    }

    /// <summary>
    /// 동그라미 속 text의 point 부분을 현재 점수에 따라 업데이트함.
    /// </summary>
    private void updateTotalPoint()
    {
        TextMeshProUGUI currentPoint = GameObject.Find("현재포인트Text").GetComponent<TextMeshProUGUI>();  //현재포인트Text는 진행상황영역/포인트동그라미/innerBoder/ContentArea의 하위 요소.

        ProgressController.pointString=currentPoint.text=pointToString(getCurrentPoint());
    }

    /// <summary>
    /// 현재 점수를 기준으로 lv(점 개수), 원의 색칠된 비율, 진행률을 변경. 진행률이 100% 이상이면 (가능한 경우) 그다음 보상 단계로 넘어감.
    /// </summary>
    private void updateProgress()
    {
        pointRatio.text = parsePoint(currentPoint)*100 / parsePoint(goalPoint)+"%";
        progressRatio = pointCircle.GetComponent<Slider>().value = parsePoint(pointRatio) / 100.0f;

        if (pointCircle.GetComponent<Slider>().value >= 1.0f)
        {
            levelUp();
        }

        updateTotalPoint();

    }

    /// <summary>
    /// //그 다음 level에 해당하는 보상 목록의 내용을 가져와서 진행률, 현재점수, 목표점수, TotalPoint의 text을 변경함.
    /// </summary>
    private void levelUp()
    {
        GameObject.Find("Canvas").transform.Find("보상목록").gameObject.SetActive(true);
        var rows = GameObject.FindGameObjectsWithTag("row");
        GameObject.Find("Canvas").transform.Find("보상목록").gameObject.SetActive(false);  

        if (level >= rows.Length)   
        {
            Debug.Log("레벨 상한 도달");
        }
        else    //level이 1인 경우, rows의 0번째에 해당.
        {
            GameObject rewardTitle = GameObject.Find("보상제목");
            TextMeshProUGUI currentLevel = GameObject.Find("단계Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI pointText = GameObject.Find("현재포인트Text").GetComponent<TextMeshProUGUI>();

            currentPoint.text = "0";

            level += 1;

            currentLevel.text = level.ToString();
            pointString = pointText.text = pointToString(parsePoint(currentPoint));
            ProgressController.rewardTitle = rewardTitle.GetComponent<TextMeshProUGUI>().text = getContentfromRow(rows[level-1]);

            goalPoint.text = "완성  " + getGoalfromRow(rows[level-1]);
            ChildDataController.setGoalPoint(getGoalfromRow(rows[level - 1]));
            pointRatio.text = "0%";
            pointCircle.GetComponent<Slider>().value = 0f;
            ProgressController.point = 0;
            updateLevel(level);
        }
    }
}
