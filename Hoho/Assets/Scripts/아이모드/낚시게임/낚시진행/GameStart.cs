using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStart : MonoBehaviour
{

    /// <summary>
    /// ���� ���� �� false, 3�� �� start�� �ٲ�. 
    /// </summary>
    public static bool isStarted=false;

    [Tooltip("ȭ�� �߾��� 3, 2, 1, start �۾�")]
    public TextMeshProUGUI startText;
    
    private float countTime;

    /// <summary>
    /// 3, 2, 1, start ī��Ʈ �ٿ� ���� �� ���� �簳.
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
    /// �ð��� ���� 3, 2, 1, Start! �����ְ� ���� ����. (isStarte=true, isPaused=false)
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
            ChildDataController.fishGameResult.���۽ð� =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            ChildDataController.fishGameResult.���۳�¥ = DateTime.Now.ToString("d");
        }

        return; 
    }

    /// <summary>
    /// text �� ���̰� �ϱ�.
    /// </summary>
    private void disableText()
    {
        startText.gameObject.SetActive(false);
    }
}
