using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStart : MonoBehaviour
{

    /// <summary>
    /// 게임 시작 시 false, 3초 후 start로 바뀜. 
    /// </summary>
    public static bool isStarted=false;

    [Tooltip("화면 중앙의 3, 2, 1, start 글씨")]
    public TextMeshProUGUI startText;
    
    private float countTime;

    /// <summary>
    /// 3, 2, 1, start 카운트 다운 시작 후 게임 재개.
    /// </summary>
    public void startGame()
    {
        countTime = 0.0f;
        startText.text = "3";
        startText.gameObject.SetActive(true);
        isStarted = false;
    }


    // Start is called before the first frame update
    void Awake()
    {
        isStarted = false;
        ChildDataController.BreatheResult.Clear();
        ChildDataController.ExpectedBreatheResult.Clear();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStarted)
        {
            countTime += Time.deltaTime;
            updateTime();
        }
    }

    /// <summary>
    /// 시간에 따라 3, 2, 1, Start! 보여주고 게임 시작. (isStarte=true, isPaused=false)
    /// </summary>
    private void updateTime()
    {
        if (countTime < 1.0f)
        {
            startText.text = "3";
        }

        else if (countTime < 2.0f)
        {
            startText.text = "2";
        }
        else if (countTime < 3.0f)
        {
            startText.text = "1";
        }

        else
        {
            startText.text = "Start!";
            Invoke("disableText", 1.0f);
            isStarted = true;
            Pause.isPaused = false;
            ChildDataController.fishGameResult.시작시간 =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            ChildDataController.fishGameResult.시작날짜 = DateTime.Now.ToString("d");
        }

        return; 
    }

    /// <summary>
    /// text 안 보이게 하기.
    /// </summary>
    private void disableText()
    {
        startText.gameObject.SetActive(false);
    }
}
