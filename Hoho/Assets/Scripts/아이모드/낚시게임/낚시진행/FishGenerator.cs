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
    /// 0->ǥ��, 1->����, 2->���� 3->���������
    /// </summary>
    public static int modeIndex;

    [Tooltip("��� �ֱ�.")]
    public float recordingPeriod = 1.0f;
    public float recordingTime = 0.0f;

    [Tooltip("GameManager�� TimeController ������Ʈ")]
    public TimeController timeController;

    [Header("������")]
    public GameObject bubble;
    public GameObject fish2;
    public GameObject fish3;
    public GameObject fish4;
    public GameObject fish5;
    public GameObject fish6;

    [Header("ȣ����")]
    public GameObject breathLine;


    [Header("���� ������ ���� ")]
    public float screenMin = -3.0f;
    public float screenMax = 2.0f;

    [Header("ȣ�� �׷���")]
    public static float upTime;
    public static float upWaitTime;
    public static float downTime;
    public static float downWaitTime;

    [Header("ȣ�� �׷��� ���͸���")]
    public Material standardInhale;
    public Material standardExhale;

    public Material attentionInhale;
    public Material attentionInhaleSustain;
    public Material attentionExhale;

    public Material antiStressInhale;
    public Material antiStressExhale;
    public Material antiStressExhaleSustain;


    //ǥ�� : 3 0 9 0, ���߸�� : 3 6 6 0, ����: 3 0 6 6

    [Header("�����")]
    public AudioSource audioSource; 
    [Tooltip("3 0 9 0 : ǥ�� ���, ǥ�� ����")]
    public List<AudioClip> audioStandard = new List<AudioClip>();
    [Tooltip("3 6 6 0 : ���� ���, ���� ����, ���� ����")]
    public List<AudioClip> audioAttention = new List<AudioClip>();
    [Tooltip("3 0 6 6 : ���� ���, ���� ����, ���� ���� ����")]
    public List<AudioClip> audioAntiStress = new List<AudioClip>();

    public float hookPos = 0.0f;

    [Tooltip("���̵���� �ؽ�Ʈ")]
    public TextMeshProUGUI guideText;

    public string[] fishList = { "bubble","angel", "arowana fish", "asian arowana fish", "betta fish", "calvary fish", "coelacanth fish",
        "discus", "flower fish", "golden archer fish", "guppy", "lnflatable molly fish", "Monodactylus",
        "piranha fish", "ramirezi", "silver shark fish", "sword tail", "wooper looper", "Yellow Cichlid" };

    [Tooltip("���� �ֱ�. ��, ������� �ƴ� ����� ���� �ÿ��� 0.5�� ������.")]
    public float respawnPeriod=0.5f;
    private float respawnTime = 0.0f;

    [SerializeField] private float yScreenHalfSize;
    [SerializeField] private float xScreenHalfSize;



    [Header("���븦 �� ���� �����ϱ� ���� �ʿ�.")]
    public bool needInhaleLine = true;
    public bool needInhaleSustainLine = true;
    public bool needExhaleLine = true;
    public bool needExhaleSustainLine = true;


    [Header("�Ҹ��� �� ���� �����ϱ� ���� �ʿ�.")]
    public bool needInhaleSound = true;
    public bool needInhaleSustainSound = true;
    public bool needExhaleSound = true;
    public bool needExhaleSustainSound = true;


    //���븦 ����� �����ϱ� ���� �ʿ�.
    private BreathLineController pastLine=null;



    public Transform triangle;
    public Transform idealPosition;

    //ǥ�� : 3 0 9 0, ���߸�� : 3 6 6 0, ����: 3 0 6 6
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
                idx = -1;  //�ʹݿ��� ����� ������.
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

        /*���߿� �����ؾ� ��.*/


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
            //Debug.Log("�ٴ� : "+hookyPos);  
        }
        else
        {
            //500->0, 850 ->1

            hookPos = (float) (Characteristic.value - BreathingTest.breathingMin) /(float) (BreathingTest.breathingMax - BreathingTest.breathingMin);
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

        float yPos;

        Material material= standardInhale;
        
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

    /// <summary>
    /// ���� ����.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private void makeModeLine(float x)
    {
        if (x == 0)  //���� ���� ���� �� ��.
        {
            return;
        }

        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;

        Material material = standardInhale;

        GameObject line=null;


        float xPos = (pastLine != null) ? pastLine.vertex2.transform.position.x : 11f * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;
        //phase�� ���� yPos ������ �ٷ�.
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
    /// ���� ����.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private void playSound(float x)
    {
        if (x == 0)  //���� ���� ���� �� ��.
        {
            return;
        }

        float period = upTime + upWaitTime + downTime + downWaitTime;
        float phase = x % period;

        Material material = standardInhale;

        GameObject line = null;


        float xPos = (pastLine != null) ? pastLine.vertex2.transform.position.x : 11f * xScreenHalfSize / ScalingOnGaming.xScreenHalfSizeBase;
        //phase�� ���� �ٸ�.
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

    GameObject generateLine(Vector3 pos1, Vector3 pos2)
    {

        GameObject line = Instantiate(breathLine);
        BreathLineController lineController = line.GetComponent<BreathLineController>();

        lineController.vertex1.transform.position = pos1;
        lineController.vertex2.transform.position = pos2;
        return line;
    }

    /// <summary>
    /// mode : 0->ǥ��, 1->����, 2->���� 3->���������
    /// phase : 0->���, 1->��� �� ����, 2->����, 3->���� �� ����
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

                Debug.Log("���߸�� �Ҹ�");
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
