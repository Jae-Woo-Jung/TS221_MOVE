using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentProgressController : MonoBehaviour
{
    /// <summary>
    /// ��ü���� ���̴� point
    /// </summary>
    public static int point;
    /// <summary>
    /// ��ü���� ���̴� point
    /// </summary>
    public static int goalPoint_static;
    /// <summary>
    /// ���� ���� ���� �ܰ�. PointShop���� ������ ���� ����.
    /// </summary>
    public static int level;
    /// <summary>
    /// ���� ������ �� ���� ����
    /// </summary>
    public static int totalLevel;
    /// <summary>
    /// �� �ȿ� �ִ� �ؽ�Ʈ. ����, <1>2370P ���̰���.  
    /// </summary>
    public static string totalPointContent;
    /// <summary>
    /// ���� ����/��ǥ ����
    /// </summary>
    public static float progressRatio;

    [Tooltip("�������� int")]
    public int pointTest;
    [Tooltip("���� level int")]
    public int levelTest;
    [Tooltip("�� ���� ���� string")]
    public string totalPointContentTest;

    [Tooltip("����Ʈ���׶��")]
    public GameObject pointCircle;
    [Tooltip("�����")]
    public TextMeshProUGUI pointRatio;
    [Tooltip("��������")]
    public TextMeshProUGUI currentPoint;
    [Tooltip("��ǥ����")]
    public TextMeshProUGUI goalPointText;
    [Tooltip("1~6�ܰ�, ����")]
    public GameObject progressLevel;
    [Tooltip("���鰣 ������")]
    public GameObject verticalLine;
    [Tooltip("��prefab")]
    public GameObject dotPrefab;


    /// <summary>
    /// pointShop�� point�� ����. static ������ ����. ���߿� ���̾�̽��� �����⵵ �ؾ���. 
    /// </summary>
    /// <param name="point"></param>
    public void addPoint(int point)
    {
        point += parsePoint(currentPoint);
        ParentProgressController.point = point;
        currentPoint.text = "����  "+point;        
        updateProgress();
    }

    public int getCurrentPoint()
    {
        return parsePoint(currentPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        //������ ��������.
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
    /// ���� ����, ���� ���� ��Ȳ, �����, 
    /// </summary>
    private void initProgress()
    {
        Debug.Log("point : "+point+", level : "+level);
        point = (int) ParentDataController.getValues()["point"];
        level = (int) ParentDataController.getValues()["level"];
        //totalLevel
        goalPoint_static = (int) ParentDataController.getValues()["goalPoint"];

        totalPointContent = GameObject.Find("��ü����Ʈ").GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + point + "P\n" + ParentDataController.getValues()["rewardTitle"];

        progressRatio = (float)point / goalPoint_static;
        Debug.Log("progressRatio : " + progressRatio);
        currentPoint.text = "����  " + ParentProgressController.point;
        ParentProgressController.level = Math.Max(1, level);

        generateLevel(level);
        GameObject totalPoint = GameObject.Find("��ü����Ʈ");

        if (level > totalLevel)
        {
            Debug.Log("���� ���� ����");
        }
        
        //totalPointContent = totalPoint.GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + parsePoint(currentPoint) + "P\n" + totPointContent;
        goalPointText.text = "�ϼ�  " + goalPoint_static;
        Debug.Log("��ǥ���� : " + goalPoint_static);

        Debug.Log(goalPointText.text);
        updateProgress();        
    }

    /// <summary>
    /// �� ���� ����
    /// </summary>
    /// <param name="level"></param>
    private void generateLevel(int level)
    {
        Transform[] transformList = progressLevel.transform.GetComponentsInChildren<Transform>();
        foreach (Transform dot in transformList)
        {
            if ((dot.name).Contains("��"))
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
    /// �� ��ũ��Ʈ�� ���ǵ� Text ������Ʈ�� �־�����, �� text���� int�� �� P���� ��ȯ. (����, "1143P"�� 1143 ��ȯ.)
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
    /// progressLevel�� ���� �� lv ������ ������ ��������, �������� �Ͼ������ �ٲ�.
    /// </summary>
    /// <param name="lv"></param>
    private void updateLevel(int lv)
    {
        for (int i=0; i<progressLevel.transform.childCount; i++)
        {
            if (i < lv)
            {
                progressLevel.transform.GetChild(i).GetComponent<Image>().color = Color.black; //lv ������ ������ ��������
            }
            else
            {
                progressLevel.transform.GetChild(i).GetComponent<Image>().color = Color.white;  //lv ������ ������ �Ͼ������
            }
        }
    }

    /// <summary>
    /// progressLevel�� ǥ�õ� ����(���� �� ����)�� ��ȯ��.
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
                level = i+1;  //0��°�� ���, ����1
            }
        }

        Debug.Log("current level is "+level);
        return level;
    }

    /// <summary>
    /// ���׶�� �� text�� point �κ��� ���� ������ ���� ������Ʈ��.
    /// </summary>
    private void updateTotalPoint()
    {
        GameObject totalPoint = GameObject.Find("��ü����Ʈ");  //��ü����Ʈ�� �����Ȳ����/����Ʈ���׶��/innerBoder/ContentArea�� ���� ���.
        string currentText = totalPoint.GetComponent<TextMeshProUGUI>().text;
        string changeText = currentText.Substring(currentText.IndexOf("\n") + 1);
        changeText=changeText.Substring(0, changeText.IndexOf("P")+1);

        totalPointContent=totalPoint.GetComponent<TextMeshProUGUI>().text = currentText.Replace(changeText, getCurrentPoint()+"P");
    }

    /// <summary>
    /// ���� ������ �������� lv(�� ����), ���� ��ĥ�� ����, ������� ����. ������� 100% �̻��̸� (������ ���) �״��� ���� �ܰ�� �Ѿ.
    /// </summary>
    private void updateProgress()
    {
        pointRatio.text = parsePoint(currentPoint)*100 / parsePoint(goalPointText)+"%";
        progressRatio = pointCircle.GetComponent<Slider>().value = parsePoint(pointRatio) / 100.0f;

        updateTotalPoint();
    }

}
