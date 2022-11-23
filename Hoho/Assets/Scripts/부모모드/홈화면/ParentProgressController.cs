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


    /// <summary>
    /// pointShop�� point�� ����. static ������ ����. ���߿� ���̾�̽��� �����⵵ �ؾ���. 
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
        Debug.Log("initProgress point : "+point);
        point = (int) ParentDataController.getValues()["point"];
        //totalLevel
        goalPoint_static = (int) ParentDataController.getValues()["goalPoint"];

        //totalPointContent = GameObject.Find("��ü����Ʈ").GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + point + "P\n" + ParentDataController.getValues()["rewardTitle"];

        progressRatio = (float)point / goalPoint_static;
        Debug.Log("progressRatio : " + progressRatio);
        currentPoint.text =  ParentProgressController.point.ToString();
        
        //totalPointContent = totalPoint.GetComponent<TextMeshProUGUI>().text = "<" + level + ">\n" + parsePoint(currentPoint) + "P\n" + totPointContent;
        goalPointText.text =  goalPoint_static.ToString();
        Debug.Log("��ǥ���� : " + goalPoint_static);

        Debug.Log(goalPointText.text);

        updateProgress();        
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

        GameObject totalPoint = GameObject.Find("����text");  //��ü����Ʈ�� �����Ȳ����/����Ʈ���׶��/innerBoder/ContentArea�� ���� ���.
        Debug.Log("updateProgress1");
        totalPoint.GetComponent<TextMeshProUGUI>().text = (string) ParentDataController.getValues()["rewardTitle"];
        Debug.Log("updateProgress2");
        //updateTotalPoint();
    }

}
