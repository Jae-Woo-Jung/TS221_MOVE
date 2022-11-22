using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;
using Assets.SimpleAndroidNotifications;
using System.Linq;

public class TodaySchedule : MonoBehaviour
{
    
    public static string mode = "";

    public GameObject timePrefab;
    public Slider gameProgress;
    public GameObject scheduleContent;

    public Button startBtn;

    public List<Sprite> modeImages = new List<Sprite>();

    static float gameProgressRatio=0f;
    public static int compareSchedule(ChildDataController.ScheduleInformation info1, ChildDataController.ScheduleInformation info2)
    {
        int currentTime = DateTime.Now.Hour*60+DateTime.Now.Minute;
        int info1Time = info1.시 * 60 + info1.분;
        int info2Time = info2.시 * 60 + info2.분;

        if ((info1Time < currentTime - 5 && info2Time < currentTime - 5) || (info1Time > currentTime + 5 && info2Time > currentTime + 5))
        {
            if (info1Time<info2Time)
            {
                return -1;
            }
            else if (info1.시 == info2.시 && info1.분 == info2.분)
            {
                return 0;
            }

            return 1;
        }

        //info1이 과거.
        if (info1Time < currentTime - 5 && info2Time > currentTime + 5)
        {
            return 1;
        }

        if (info1Time < currentTime - 5 && info2Time >= currentTime - 5 && info2.완료==false)
        {
            return 1;
        }

        if (info1Time < currentTime - 5 && info2Time >= currentTime - 5 && info2.완료 == true)
        {
            return -1;
        }

        //info1이 미래. 둘 다 미래인 경우는 위에서 이미 처리.
        if (info1Time > currentTime + 5)
        {
            return -1;
        }

        //info1이 현재

        if (info1.완료)
        {
            return 1;
        }

        if (!info1.완료)
        {
            return -1;
        }

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameProgress.value = gameProgressRatio;
        ChildDataController.ReceiveBreath( () => ChildDataController.ReceiveScheduleInfo(initializeSchedule));

        Debug.Log(Convert.ToDateTime("2022 11 07 9:1"));
        Debug.Log(DateTime.Now.Year + " " + DateTime.Now.Month + " " + DateTime.Now.Day + " " + 22 + ":" + 0);
        Debug.Log("Start : "+Convert.ToDateTime(DateTime.Now.Year + " " + DateTime.Now.Month + " " + DateTime.Now.Day + " " + 22 + ":" + 0));


        var notificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(10),
            Title = "Custom notification",
            Message = "Message",
            Ticker = "Ticker",
            Sound = true,           
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        Debug.Log("Start : " + notificationParams.Delay);
        NotificationManager.SendCustom(notificationParams);


        startBtn.onClick.AddListener(checkTest);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializeSchedule()
    {

        int finishedScheduleNum = 0;

        ChildDataController.fishGameResultList.Sort((result1, result2) => compareStringTime(result1.시작날짜, result2.시작날짜));

        if (ChildDataController.fishGameResultList.Count> 0)
        {
            var LastResult=ChildDataController.fishGameResultList.FindLast(x=>true);

            DateTime RefDate = Convert.ToDateTime(LastResult.시작시간);
            Debug.Log("initializeSchedule Last Result : "+LastResult.시작시간);
        }

        Transform table = scheduleContent.transform;
        NotificationManager.CancelAll();

        foreach  (var schedule in ChildDataController.scheduleInformationList)
        {

            foreach (var result in ChildDataController.fishGameResultList)
            {
                if (result.스케줄적용 && schedule.분 == result.스케줄분 && schedule.시 == result.스케줄시)
                {
                    schedule.완료 = true;
                    finishedScheduleNum++;
                }
            }

            //notification 추가
#if PLATFORM_ANDROID && UNITY_EDITOR

            DateTime date1 = Convert.ToDateTime(DateTime.Now.Year + " " + DateTime.Now.Month + " " + DateTime.Now.Day + " " + schedule.시 + ":" + schedule.분);

            Debug.Log("initializeSchedule target schedule : " + date1);
            if (date1 > DateTime.Now+TimeSpan.FromMinutes(5))
            {
                Debug.Log("initializeSchedule : notification setting start.");
                TimeSpan delay = date1 - DateTime.Now- TimeSpan.FromMinutes(5);

                Debug.Log(delay.ToString());

                var notificationParams = new NotificationParams
                {
                    Id = UnityEngine.Random.Range(0, int.MaxValue),
                    Delay = delay,
                    Title = schedule.요일 + "_" + schedule.시 + "시" + schedule.분 + "분",
                    Message = schedule.요일 + "_" + schedule.시 + "시" + schedule.분 + "분에 " + schedule.모드 + " 호흡 훈련을 진행해주세요.",
                    Ticker = "Ticker",
                    Sound = true,
                    Vibrate = true,
                    Light = true,
                    SmallIcon = NotificationIcon.Heart,
                    SmallIconColor = new Color(0, 0.5f, 0),
                    LargeIcon = "app_icon"
                };

                NotificationManager.SendCustom(notificationParams);
            }
#endif

            string title = schedule.제목;
            string hour = (schedule.시 < 10 ? "0" : "") + schedule.시.ToString();
            string minute = (schedule.분 < 10 ? "0" : "") + schedule.분.ToString();

            Debug.Log(schedule.요일);

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("제목").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("시간").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.모드.Contains("표준") ? 0 : schedule.모드.Contains("집중") ? 1 : schedule.모드.Contains("안정") ? 2 : 3;

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];
            newTime.GetComponent<ScheduleContainer>().schedule = schedule;

            if (schedule.완료)
            {
                newTime.transform.Find("완료표시").gameObject.SetActive(true);
                newTime.transform.Find("미완료표시").gameObject.SetActive(false);
            }
        }

        gameProgressRatio = gameProgress.value = ChildDataController.scheduleInformationList.Count == 0 ? 0 : finishedScheduleNum / (float) ChildDataController.scheduleInformationList.Count;
        orderSchedule();
    }

    void orderSchedule()
    {
        int scheduleNum = 0;
        int finishedScheduleNum = 0;

        Transform table = scheduleContent.transform;

        int currentTime=DateTime.Now.Hour*60+DateTime.Now.Minute;

        List<Transform> scheduleList = table.GetComponentsInChildren<Transform>().Where(x=>x.GetComponent<ScheduleContainer>()!=null).ToList<Transform>();

        scheduleList.Sort((s1, s2) => compareSchedule(s1.GetComponent<ScheduleContainer>().schedule, s2.GetComponent<ScheduleContainer>().schedule));

        for (int i=0; i<scheduleList.Count; i++)
        {
            Transform newTime = scheduleList[i];
            var info = newTime.GetComponent<ScheduleContainer>().schedule;
            newTime.SetSiblingIndex(i);
            

            if (info.시*60+info.분 < currentTime-5  && !info.완료)
            {
                newTime.transform.Find("완료표시").gameObject.SetActive(false);
                newTime.transform.Find("미완료표시").gameObject.SetActive(true);
            }
        }

    }


    int compareStringTime(string time1, string time2)
    {

        DateTime date1 = Convert.ToDateTime(time1);
        DateTime date2 = Convert.ToDateTime(time2);

        if (date1.Hour < date2.Hour || (date1.Hour==date2.Hour && date1.Minute < date2.Minute))
        {
            return -1;
        }
        else if (date1.Hour == date2.Hour && date1.Minute == date2.Minute)
        {
            return 0;
        }

        return 1;
    }





    void checkTest()
    {

        if (!AndroidBLEPluginStart.isConnected)
        {
            AndroidBLEPluginStart.CallByAndroid("왼쪽 위의 버튼을 눌러 벨트랑 연결해주세요.");
            return;
        }

        //SceneLoader.LoadScene("낚시시작화면");
        if (BreathingTest.isTested)
        {
            SceneLoader.LoadScene("낚시시작화면");
        }
        else
        {
            SceneLoader.LoadScene("아이모드테스트화면");
        }
        
    }
}
