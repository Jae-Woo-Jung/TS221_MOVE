using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [Tooltip("게임 진행 시간. 게임 진행에 따라 줄어듦.")]
    public TextMeshProUGUI timeText;

    [Tooltip("점수")]
    public TextMeshProUGUI point;

    [Tooltip("게임 시작 후 시간에 따라 서서히 줄어들고 색깔 바뀜.")]
    public GameObject timeBar;

    /// <summary>
    /// 게임 전체 시간. setTime 함수로 초기화.
    /// </summary>
    [Tooltip("게임의 전체 시간")]
    public float fullTime;
    
    /// <summary>
    /// 게임 전체 시간을 미리 결정.
    /// </summary>
    public static float fullTimeStatic = 100;


    [SerializeField][Tooltip("게임 진행 시간")]
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
            SceneLoader.LoadScene("낚시결과");
            ChildDataController.fishGameResult.별개수 = getStarNum();
            ChildDataController.fishGameResult.플레이시간 = (int) fullTime;
            ChildDataController.fishGameResult.훈련시간 = (int) (fullTime - FishArrivalTime.getArrivalTime());
            ChildDataController.fishGameResult.완성률 = System.Int32.Parse(point.text) / FishArrivalTime.getFishNum();
            PointListController.pointContent cont = new PointListController.pointContent();
            cont.content = "낚시 게임";
            cont.point = FishArrivalTime.getFishNum() * 10;//System.Int32.Parse(point.text)/10;
            ChildDataController.addPoint(cont.point);   //아이 모드의 포인트 더하기.
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
