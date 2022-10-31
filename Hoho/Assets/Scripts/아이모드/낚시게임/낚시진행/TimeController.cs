using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [Tooltip("���� ���� �ð�. ���� ���࿡ ���� �پ��.")]
    public TextMeshProUGUI timeText;

    [Tooltip("����")]
    public TextMeshProUGUI point;

    [Tooltip("���� ���� �� �ð��� ���� ������ �پ��� ���� �ٲ�.")]
    public GameObject timeBar;

    /// <summary>
    /// ���� ��ü �ð�. setTime �Լ��� �ʱ�ȭ.
    /// </summary>
    [Tooltip("������ ��ü �ð�")]
    public float fullTime;
    
    /// <summary>
    /// ���� ��ü �ð��� �̸� ����.
    /// </summary>
    public static float fullTimeStatic=50;


    [SerializeField][Tooltip("���� ���� �ð�")]
    private float progressedTime;


    public void setTime(float time)
    {
        fullTime = time;
    }

    public float getProgressedTime()
    {
        return progressedTime;
    }

    public float getRemainingTime()
    {
        return fullTime - progressedTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        progressedTime = 0;
        fullTime = fullTimeStatic;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.isStarted && !Pause.isPaused)
        {
            progressedTime += Time.deltaTime;
            updateTime();
            updateBar();
        }

        if (GameStart.isStarted && getRemainingTime() <= 0f)
        {
            SceneLoader.LoadScene("���ð��");
            ChildDataController.fishGameResult.������ = getStarNum();
            ChildDataController.fishGameResult.�÷��̽ð� = (int) fullTime;
            ChildDataController.fishGameResult.�Ʒýð� = (int) (fullTime - FishArrivalTime.getArrivalTime());
            ChildDataController.fishGameResult.�ϼ��� = System.Int32.Parse(point.text) / FishArrivalTime.getFishNum();
            PointListController.pointContent cont = new PointListController.pointContent();
            cont.content = "���� ����";
            cont.point = FishArrivalTime.getFishNum() * 10;//System.Int32.Parse(point.text)/10;
            ChildDataController.addPoint(cont.point);   //���� ����� ����Ʈ ���ϱ�.
            PointListController.pointContentList.Add(cont);

            setResult();
            ChildDataController.setCanSend(ChildDataController.BreatheResult.Count >0);

            ChildDataController.SendGameResult();
            ChildDataController.SendPoint();
        }
    }

    private void updateTime()
    {
        int min;
        int second;
        if (fullTime > progressedTime) { 
            min = (int) (fullTime-progressedTime)/60;
            second = (int)(fullTime - progressedTime) - min * 60;
        }
        else
        {
            min = second = 0;
        }

        timeText.text = (min.ToString().Length == 2) ? min.ToString() + "  " + second.ToString() : "0"+min.ToString() + "  " + second.ToString();
    }

    private void updateBar()
    {
        Slider timeSlider = timeBar.GetComponent<Slider>();
        Image fill=GameObject.Find("Fill").GetComponent<Image>();

        timeSlider.value = (fullTime - progressedTime) / fullTime;
        if (timeSlider.value<0.3f)
        {
            fill.color = Color.red;
        }

        else if (timeSlider.value < 0.6f)
        {    
            fill.color = Color.yellow;
        }

        else
        {
            fill.color = Color.green;
        }
    }

    private int getStarNum()
    {
        float ratio = float.Parse(point.text) / FishArrivalTime.getFishNum();

        if (95f < ratio) return 3;
        if (85f < ratio) return 2;
        if (70f < ratio) return 1;
        return 0;
    }

    private void setResult()
    {
        int pt = System.Int32.Parse(point.text);
        ResultController.setAchievement(getStarNum());
        ResultController.setGamePoint(pt);
        ResultController.setNowPoint((int)ChildDataController.getValues()["point"] + FishArrivalTime.getFishNum()*10);
    }
}
