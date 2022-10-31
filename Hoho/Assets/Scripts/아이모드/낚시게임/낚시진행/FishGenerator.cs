using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class FishGenerator : MonoBehaviour
{
    public static float arrivalTime;
    public float arrivalTimeStatic
    {
        get { return arrivalTime; }
        set { arrivalTime = value; }
    }

    [Tooltip("��� �ֱ�.")]
    public float recordingPeriod = 1.0f;
    public float recordingTime = 0.0f;

    [Tooltip("GameManager�� TimeController ������Ʈ")]
    public TimeController timeController;

    [Header("������")]
    public GameObject bubble;
    public GameObject wooper_looper;
    public GameObject fish1;

    [Header("���� ������ ���� ")]
    public float screenMin = -3.0f;
    public float screenMax = 2.0f;

    [Header("ȣ�� �׷���")]
    public static float upTime;
    public static float upWaitTime;
    public static float downTime;
    public static float downWaitTime;






    public float hookPos = 0.0f;

    [Tooltip("���̵���� �ؽ�Ʈ")]
    public TextMeshProUGUI guideText;

    public string[] fishList = { "bubble","angel", "arowana fish", "asian arowana fish", "betta fish", "calvary fish", "coelacanth fish",
        "discus", "flower fish", "golden archer fish", "guppy", "lnflatable molly fish", "Monodactylus",
        "piranha fish", "ramirezi", "silver shark fish", "sword tail", "wooper looper", "Yellow Cichlid" };

    [Tooltip("���� �ֱ�. ��, ������� �ƴ� ����� ���� �ÿ��� 0.5�� ������.")]
    public float respawnPeriod=0.7f;
    private float respawnTime = 0.0f;

    [SerializeField] private float yScreenHalfSize;
    [SerializeField] private float xScreenHalfSize;    


    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartFunction", 0.4f);
    }

    private void StartFunction()
    {        
        respawnTime = 0.0f;
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
        screenMin = yScreenHalfSize / 5.0f * -3.0f;
        screenMax = yScreenHalfSize / 5.0f * 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.isStarted && !Pause.isPaused)
        {
            respawnTime += Time.deltaTime;
            recordingTime += Time.deltaTime;
        }

        if (respawnTime > respawnPeriod)
        {
            int idx = UnityEngine.Random.Range(-10, 3);
            GameObject fish;
            switch (idx)
            {
                case 1:
                    fish = Instantiate(wooper_looper);
                    respawnTime = -0.5f;
                    break;
                case 2:
                    fish = Instantiate(fish1);
                    respawnTime = -0.5f;
                    break;
                default:
                    fish = Instantiate(bubble);
                    respawnTime = 0.0f;
                    break;
            }
            float yPos = breathPos(timeController.getProgressedTime());
            float xPos = (11f + ((idx < 1) ? 0.0f : 0.5f)) * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;

            fish.transform.position = new Vector3(xPos, yPos, 0.0f);
        }

        setGuideText(timeController.getProgressedTime());

        /*���߿� �����ؾ� ��.*/


        float correctHookPos=0.0f;
        if (FishArrivalTime.getArrivalTime() != 0.0f)
        {
            correctHookPos = breathPos(Mathf.Clamp(timeController.getProgressedTime() - FishArrivalTime.getArrivalTime(), 0f, timeController.getProgressedTime()));
            //Debug.Log("breathPos : " + hookPos);
            //hookPos = (hookPos - (screenMax + screenMin) / 2) / (screenMax - screenMin) * (3.75f + 2.3f) + (3.75f - 2.3f) / 2.0f * ScalingOnGaming.yScaler;            
            //ScreenMin~ScreenMax -> 0~1
            correctHookPos = (correctHookPos - screenMin) / (screenMax - screenMin);
            //Debug.Log("hookPos : " + hookPos);
        }
        else
        {
            correctHookPos = 0;
        }

        if (Characteristic.isValueNull)
        {
             hookPos = correctHookPos;
            //Debug.Log("�ٴ� : "+hookyPos);  
        }
        else
        {
            //500->0, 850 ->1

            hookPos = (Characteristic.value - 500) / 350.0f;
        }
        GameObject.Find("���˹ٴ�").GetComponent<HookController>().setPosition(hookPos);

        if (FishArrivalTime.getArrivalTime() != 0.0f && recordingTime>=recordingPeriod)
        {
            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            ChildDataController.BreatheResult.Add(nowTime, hookPos);
            ChildDataController.ExpectedBreatheResult.Add(nowTime, correctHookPos);            
            recordingTime = 0.0f;
        }

    }

    /// <summary>
    /// ��� �ð�, ���� �ð�, �� ���� �ð��� �����, �ð��� ���� �Լ��� ��ȯ. ������ ����.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private float breathPos(float x)
    {
        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;   
        float textPhase = (x + FishArrivalTime.getArrivalTime()) % period;

        float yPos;
        
        //phase�� ���� yPos ������ �ٷ�.
        if (phase<0.0f) 
        {
            Debug.Log("Wrong period.");
            return -100.0f;
        }
        if (phase < upTime)
        {
            yPos = -(screenMax-screenMin)/2.0f*Mathf.Cos(Mathf.PI / upTime * phase)+ (screenMax + screenMin) / 2.0f;
        }
        else if (phase < upTime+upWaitTime)
        {
            yPos = screenMax;
        }

        else if (phase < upTime+upWaitTime+downTime)
        {
            phase = phase - upTime - upWaitTime;
            yPos = (screenMax - screenMin) / 2.0f * Mathf.Cos(Mathf.PI / downTime * phase) + (screenMax + screenMin) / 2.0f;
        }

        else
        {
            yPos = screenMin;
        }

        return yPos;
    }


    private void setGuideText(float x)
    {
        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = (x - FishArrivalTime.getArrivalTime()) % period;

        Debug.Log(FishArrivalTime.getArrivalTime());
        string msg=null;

        if (FishArrivalTime.getArrivalTime() == 0.0f)
        {
            msg = "�غ����ּ���.";
        }
        else if (Pause.isPaused)
        {
            msg = "ȣ���� ���� ���ùٴ��� �ϴû� �簢�� ������ �������ּ���.";
        }
        /*
        else if (timeController.getRemainingTime() <= 0.0f)
        {
            msg = "�Ʒ��� �������ϴ�.";
        }*/
        //phase�� ���� yPos ������ �ٷ�.
        else if (phase <= 0.0f)
        {
            Debug.Log("Wrong period.");           
        }
        else if (phase < upTime)
        {
            float remainingPahse = upTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "�ʰ� ��� ���̸�������.";
        }
        else if (phase < upTime + upWaitTime)
        {
            float remainingPahse = upTime + upWaitTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "�ʰ� ���� ��������.";
        }

        else if (phase < upTime + upWaitTime + downTime)
        {
            phase = phase - upTime - upWaitTime;
            float remainingPahse = downTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "�ʰ� ���� õõ�� ��������.";
        }

        else
        {
            float remainingPahse = upTime + upWaitTime + downTime + downWaitTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "�ʰ� ���� ��������.";
        }

        guideText.text = msg;
    }

}
