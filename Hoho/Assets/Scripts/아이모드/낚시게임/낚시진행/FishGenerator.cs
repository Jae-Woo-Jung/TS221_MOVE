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

    /// <summary>
    /// 0->표준, 1->집중, 2->안정 3->사용자정의
    /// </summary>
    public static int modeIndex;

    [Tooltip("기록 주기.")]
    public float recordingPeriod = 1.0f;
    public float recordingTime = 0.0f;

    [Tooltip("GameManager의 TimeController 컴포넌트")]
    public TimeController timeController;

    [Header("물고기들")]
    public GameObject bubble;
    public GameObject fish2;
    public GameObject fish3;
    public GameObject fish4;
    public GameObject fish5;
    public GameObject fish6;

    [Header("호흡막대")]
    public GameObject breathLine;


    [Header("생성 높낮이 최저 ")]
    public float screenMin = -3.0f;
    public float screenMax = 2.0f;

    [Header("호흡 그래프")]
    public static float upTime;
    public static float upWaitTime;
    public static float downTime;
    public static float downWaitTime;

    [Header("호흡 그래프 매터리얼")]
    public Material standardInhale;
    public Material standardExhale;

    public Material attentionInhale;
    public Material attentionInhaleSustain;
    public Material attentionExhale;

    public Material antiStressInhale;
    public Material antiStressExhale;
    public Material antiStressExhaleSustain;


    //표준 : 3 0 9 0, 집중모드 : 3 6 6 0, 안정: 3 0 6 6

    [Header("오디오")]
    public AudioSource audioSource; 
    [Tooltip("3 0 9 0 : 표준 들숨, 표준 날숨")]
    public List<AudioClip> audioStandard = new List<AudioClip>();
    [Tooltip("3 6 6 0 : 집중 들숨, 집중 유지, 집중 날숨")]
    public List<AudioClip> audioAttention = new List<AudioClip>();
    [Tooltip("3 0 6 6 : 안정 들숨, 안정 날숨, 안정 날숨 유지")]
    public List<AudioClip> audioAntiStress = new List<AudioClip>();

    public float hookPos = 0.0f;

    [Tooltip("가이드라인 텍스트")]
    public TextMeshProUGUI guideText;

    public string[] fishList = { "bubble","angel", "arowana fish", "asian arowana fish", "betta fish", "calvary fish", "coelacanth fish",
        "discus", "flower fish", "golden archer fish", "guppy", "lnflatable molly fish", "Monodactylus",
        "piranha fish", "ramirezi", "silver shark fish", "sword tail", "wooper looper", "Yellow Cichlid" };

    [Tooltip("생성 주기. 단, 물방울이 아닌 물고기 생성 시에는 0.5초 딜레이.")]
    public float respawnPeriod=0.5f;
    private float respawnTime = 0.0f;

    [SerializeField] private float yScreenHalfSize;
    [SerializeField] private float xScreenHalfSize;



    [Header("막대를 한 번만 생성하기 위해 필요.")]
    public bool needInhaleLine = true;
    public bool needInhaleSustainLine = true;
    public bool needExhaleLine = true;
    public bool needExhaleSustainLine = true;


    [Header("소리를 한 번만 생성하기 위해 필요.")]
    public bool needInhaleSound = true;
    public bool needInhaleSustainSound = true;
    public bool needExhaleSound = true;
    public bool needExhaleSustainSound = true;


    //막대를 제대로 생성하기 위해 필요.
    private BreathLineController pastLine=null;



    public Transform triangle;
    public Transform idealPosition;

    //표준 : 3 0 9 0, 집중모드 : 3 6 6 0, 안정: 3 0 6 6
    // Start is called before the first frame update
    void Start()
    {
        switch (modeIndex)
        {
            case 0:
                upTime = 3;
                upWaitTime = 0;
                downTime = 9;
                downWaitTime = 0;
                break;
            case 1:
                upTime = 3;
                upWaitTime = 6;
                downTime = 6;
                downWaitTime = 0;
                break;
            case 2:
                upTime = 3;
                upWaitTime = 0;
                downTime = 6;
                downWaitTime = 6;
                break;
            case 3:
                upTime = 3;
                upWaitTime = 0;
                downTime = 6;
                downWaitTime = 6;
                break;
        }

        

        pastLine = null;

        Invoke("StartFunction", 0.4f);
    }

    private void StartFunction()
    {        
        respawnTime = 0.0f;
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
        screenMin = yScreenHalfSize / 5.0f * -3.0f;
        screenMax = yScreenHalfSize / 5.0f * 2.0f;
        
        respawnTime = respawnPeriod-1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.isStarted && !Pause.isPaused)
        {
            respawnTime += Time.deltaTime;
            recordingTime += Time.deltaTime;

            triangle.position = new Vector3((11f) * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase, breathPos(timeController.getProgressedTime()), 0);
        }

        if (respawnTime > respawnPeriod)
        {

            int idx =  UnityEngine.Random.Range(1, 6);
            float correction = 0.0f;

            if (timeController.getProgressedTime() < 0.3)
            {
                idx = -1;  //초반에는 물방울 나오게.
            }

            respawnTime = -1f;
            GameObject fish;
            switch (idx)
            {
                
                case 1:
                    fish = Instantiate(fish2);
                    correction = 0.5f;
                    break;
                case 2:
                    fish = Instantiate(fish3);
                    correction = 0.4f;
                    break;
                case 3:
                    fish = Instantiate(fish4);
                    correction = 0.3f;
                    break;
                case 4:
                    fish = Instantiate(fish5);
                    correction = 0.5f;
                    break;
                case 5:
                    fish = Instantiate(fish6);
                    correction = 0.1f;
                    break;                
                default:
                    fish = Instantiate(bubble);
                    respawnTime = 0.0f;
                    break;
            }
            float yPos = breathPos(timeController.getProgressedTime());
            
            float xPos = (11f + correction) * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;

            fish.transform.position = new Vector3(xPos, yPos, 0f);
        }



        makeModeLine(timeController.getProgressedTime());
   
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
            idealPosition.position = new Vector3(idealPosition.position.x, breathPos(Mathf.Clamp(timeController.getProgressedTime() - FishArrivalTime.getArrivalTime(), 0f, timeController.getProgressedTime())), 0);
            playSound(Mathf.Clamp(timeController.getProgressedTime() - FishArrivalTime.getArrivalTime(), 0f, timeController.getProgressedTime()));
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

            hookPos = (float) (Characteristic.value - BreathingTest.breathingMin) /(float) (BreathingTest.breathingMax - BreathingTest.breathingMin);
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

        float yPos;

        Material material= standardInhale;
        
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

    /// <summary>
    /// 막대 생성.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private void makeModeLine(float x)
    {
        if (x == 0)  //아직 게임 시작 안 함.
        {
            return;
        }

        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;

        Material material = standardInhale;

        GameObject line=null;


        float xPos = (pastLine != null) ? pastLine.vertex2.transform.position.x : 11f * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;
        //phase에 따른 yPos 생성값 다룸.
        if (phase < 0.0f)
        {
            Debug.Log("Wrong period.");
            return;
        }
        if (phase < upTime)
        {
            needInhaleSustainLine = needExhaleLine = needExhaleSustainLine = true;

            if (needInhaleLine)
            {
                Debug.Log(" makeModeLine : inhaleLine");

                needInhaleLine = false;
                Vector3 pos1 = new Vector3(xPos, screenMin, -0.5f);
                Vector3 pos2 = new Vector3(pos1.x + FishMove.speed * upTime, screenMax, -0.5f);
                line = generateLine(pos1, pos2);
                switch (modeIndex)
                {
                    case 0:
                        material = standardInhale;                        
                        break;
                    case 1:
                        material = attentionInhale;
                        break;
                    case 2:
                        material = antiStressInhale;
                        break;
                    case 3:
                        break;
                }
                line.GetComponent<LineRenderer>().material = material;
            }
        }
        else if (phase < upTime + upWaitTime)
        {
            needInhaleLine = needExhaleLine = needExhaleSustainLine = true;

            if (needInhaleSustainLine)
            {
                Debug.Log(" makeModeLine : inhaleSustainLine");
                needInhaleSustainLine = false;
                Vector3 pos1 = new Vector3(xPos, screenMax, -0.5f);
                Vector3 pos2 = new Vector3(pos1.x + FishMove.speed * upWaitTime, screenMax, -0.5f);
                line = generateLine(pos1, pos2);
                switch (modeIndex)
                {
                    case 0:
                        //material = standardInhale;
                        break;
                    case 1:
                        material = attentionInhaleSustain;
                        break;
                    case 2:
                        //material = antiStressInhale;
                        break;
                    case 3:
                        break;
                }
                line.GetComponent<LineRenderer>().material = material;
            }
        }

        else if (phase < upTime + upWaitTime + downTime)
        {   
            needInhaleLine = needInhaleSustainLine = needExhaleSustainLine = true;

            if (needExhaleLine)
            {
                Debug.Log(" makeModeLine : exhaleLine");
                needExhaleLine = false;
                Vector3 pos1 = new Vector3(xPos, screenMax, -0.5f);
                Vector3 pos2 = new Vector3(pos1.x + FishMove.speed * downTime, screenMin, -0.5f);
                line = generateLine(pos1, pos2);
                switch (modeIndex)
                {
                    case 0:
                        material = standardExhale;
                        break;
                    case 1:
                        material = attentionExhale;
                        break;
                    case 2:
                        material = antiStressExhale;
                        break;
                    case 3:
                        break;
                }
                line.GetComponent<LineRenderer>().material = material;
            }
        }

        else
        {
            needInhaleLine = needInhaleSustainLine = needExhaleLine = true;

            if (needExhaleSustainLine)
            {
                Debug.Log(" makeModeLine : exhaleSustainLine");

                needExhaleSustainLine = false;
                Vector3 pos1 = new Vector3(xPos, screenMin, -0.5f);
                Vector3 pos2 = new Vector3(pos1.x + FishMove.speed * downWaitTime, screenMin, -0.5f);
                line = generateLine(pos1, pos2);
                switch (modeIndex)
                {
                    case 0:
                        material = standardExhale;
                        break;
                    case 1:
                        material = attentionExhale;
                        break;
                    case 2:
                        material = antiStressExhale;
                        break;
                    case 3:
                        break;
                }
                line.GetComponent<LineRenderer>().material = material;
            }
        }

        if (line != null)
        {
            pastLine = line.GetComponent<BreathLineController>();
        }       
    }



    /// <summary>
    /// 막대 생성.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private void playSound(float x)
    {
        if (x == 0)  //아직 게임 시작 안 함.
        {
            return;
        }

        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;

        Material material = standardInhale;

        GameObject line = null;


        float xPos = (pastLine != null) ? pastLine.vertex2.transform.position.x : 11f * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;
        //phase에 따라 다름.
        if (phase < 0.0f)
        {
            Debug.Log("Wrong period.");
            return;
        }
        if (phase < upTime)
        {

            audioSource.volume = (1 + phase / upTime) / 2;
            needInhaleSustainSound = needExhaleSound = needExhaleSustainSound = true;

            if (needInhaleSound)
            {
                Debug.Log(" Sound : inhaleSound");

                needInhaleSound = false;
                changeAudioClip(modeIndex, 0);
            }
        }
        else if (phase < upTime + upWaitTime)
        {
            audioSource.volume = 1;
            needInhaleSound = needExhaleSound = needExhaleSustainSound= true;

            if (needInhaleSustainSound)
            {
                Debug.Log(" Sound : inhaleSustainSound");
                needInhaleSustainSound = false;
                changeAudioClip(modeIndex, 1);
            }
        }

        else if (phase < upTime + upWaitTime + downTime)
        {
            audioSource.volume = 1 - ((phase - upTime-downTime) / downTime / 2f);
            needInhaleSound = needInhaleSustainSound = needExhaleSustainSound = true;

            if (needExhaleSound)
            {
                Debug.Log(" Sound : exhaleSound");
                needExhaleSound = false;
                changeAudioClip(modeIndex, 2);
            }
        }

        else
        {
            audioSource.volume = 0.5f;
            needInhaleSound = needInhaleSustainSound = needExhaleSound = true;

            if (needExhaleSustainLine)
            {
                Debug.Log(" Sound : exhaleSustainSound");
                needExhaleSustainSound = false;
                changeAudioClip(modeIndex, 3);
            }
        }
    }



    private void setGuideText(float x)
    {
        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = (x - FishArrivalTime.getArrivalTime()) % period;

        //Debug.Log(FishArrivalTime.getArrivalTime());
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

    GameObject generateLine(Vector3 pos1, Vector3 pos2)
    {

        GameObject line = Instantiate(breathLine);
        BreathLineController lineController = line.GetComponent<BreathLineController>();

        lineController.vertex1.transform.position = pos1;
        lineController.vertex2.transform.position = pos2;
        return line;
    }

    /// <summary>
    /// mode : 0->표준, 1->집중, 2->안정 3->사용자정의
    /// phase : 0->들숨, 1->들숨 후 유지, 2->날숨, 3->날숨 후 유지
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="phase"></param>
    void changeAudioClip(int mode, int phase)
    {
        switch (mode)
        {
            case 0:
                switch (phase)
                {
                    case 0:
                        if (audioSource.clip!= audioStandard[0])
                        {
                            audioSource.clip = audioStandard[0];
                            audioSource.Play();
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        if (audioSource.clip != audioStandard[1])
                        {
                            audioSource.clip = audioStandard[1];
                            audioSource.Play();
                        }
                        break;
                    case 3:
                        break;
                }
                break;
            case 1:

                Debug.Log("집중모드 소리");
                switch (phase)
                {
                    case 0:
                        if (audioSource.clip != audioAttention[0])
                        {
                            audioSource.clip = audioAttention[0];
                            audioSource.Play();
                        }
                        break;
                    case 1:
                        if (audioSource.clip != audioAttention[1])
                        {
                            audioSource.clip = audioAttention[1];
                            audioSource.Play();
                        }
                        break;
                    case 2:
                        if (audioSource.clip != audioAttention[2])
                        {
                            audioSource.clip = audioAttention[2];
                            audioSource.Play();
                        }
                        break;
                    case 3:
                        break;
                }
                break;
            case 2:
                switch (phase)
                {
                    case 0:
                        if (audioSource.clip != audioAntiStress[0])
                        {
                            audioSource.clip = audioAntiStress[0];
                            audioSource.Play();
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        if (audioSource.clip != audioAntiStress[1])
                        {
                            audioSource.clip = audioAntiStress[1];
                            audioSource.Play();
                        }
                        break;
                    case 3:
                        if (audioSource.clip != audioAntiStress[2])
                        {
                            audioSource.clip = audioAntiStress[2];
                            audioSource.Play();
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }
}
