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

    [Tooltip("기록 주기.")]
    public float recordingPeriod = 1.0f;
    public float recordingTime = 0.0f;

    [Tooltip("GameManager의 TimeController 컴포넌트")]
    public TimeController timeController;

    [Header("물고기들")]
    public GameObject bubble;
    public GameObject wooper_looper;
    public GameObject fish1;

    [Header("생성 높낮이 최저 ")]
    public float screenMin = -3.0f;
    public float screenMax = 2.0f;

    [Header("호흡 그래프")]
    public static float upTime;
    public static float upWaitTime;
    public static float downTime;
    public static float downWaitTime;






    public float hookPos = 0.0f;

    [Tooltip("가이드라인 텍스트")]
    public TextMeshProUGUI guideText;

    public string[] fishList = { "bubble","angel", "arowana fish", "asian arowana fish", "betta fish", "calvary fish", "coelacanth fish",
        "discus", "flower fish", "golden archer fish", "guppy", "lnflatable molly fish", "Monodactylus",
        "piranha fish", "ramirezi", "silver shark fish", "sword tail", "wooper looper", "Yellow Cichlid" };

    [Tooltip("생성 주기. 단, 물방울이 아닌 물고기 생성 시에는 0.5초 딜레이.")]
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

        /*나중에 수정해야 함.*/


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
            //Debug.Log("바늘 : "+hookyPos);  
        }
        else
        {
            //500->0, 850 ->1

            hookPos = (Characteristic.value - 500) / 350.0f;
        }
        GameObject.Find("낚싯바늘").GetComponent<HookController>().setPosition(hookPos);

        if (FishArrivalTime.getArrivalTime() != 0.0f && recordingTime>=recordingPeriod)
        {
            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            ChildDataController.BreatheResult.Add(nowTime, hookPos);
            ChildDataController.ExpectedBreatheResult.Add(nowTime, correctHookPos);            
            recordingTime = 0.0f;
        }

    }

    /// <summary>
    /// 들숨 시간, 날숨 시간, 숨 참는 시간을 고려한, 시간에 따른 함숫값 반환. 사인파 형태.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private float breathPos(float x)
    {
        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;   
        float textPhase = (x + FishArrivalTime.getArrivalTime()) % period;

        float yPos;
        
        //phase에 따른 yPos 생성값 다룸.
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
            msg = "준비해주세요.";
        }
        else if (Pause.isPaused)
        {
            msg = "호흡을 통해 낚시바늘을 하늘색 사각형 안으로 움직여주세요.";
        }
        /*
        else if (timeController.getRemainingTime() <= 0.0f)
        {
            msg = "훈련이 끝났습니다.";
        }*/
        //phase에 따른 yPos 생성값 다룸.
        else if (phase <= 0.0f)
        {
            Debug.Log("Wrong period.");           
        }
        else if (phase < upTime)
        {
            float remainingPahse = upTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "초간 깊게 들이마쉬세요.";
        }
        else if (phase < upTime + upWaitTime)
        {
            float remainingPahse = upTime + upWaitTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "초간 숨을 참으세요.";
        }

        else if (phase < upTime + upWaitTime + downTime)
        {
            phase = phase - upTime - upWaitTime;
            float remainingPahse = downTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "초간 숨을 천천히 내쉬세요.";
        }

        else
        {
            float remainingPahse = upTime + upWaitTime + downTime + downWaitTime - phase;
            msg = Mathf.CeilToInt(remainingPahse) + "초간 숨을 참으세요.";
        }

        guideText.text = msg;
    }

}
