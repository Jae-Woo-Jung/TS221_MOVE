using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;
using Assets.SimpleAndroidNotifications;

public class TodaySchedule : MonoBehaviour
{
    
    public static string mode = "";

    public GameObject timePrefab;
    public Slider gameProgress;
    public GameObject scheduleContent;

    public Button startBtn;

    public List<Sprite> modeImages = new List<Sprite>();

    static float gameProgressRatio=0f;

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

        ChildDataController.fishGameResultList.Sort((result1, result2) => compareStringTime(result1.���۳�¥, result2.���۳�¥));


        int refH = -1;
        int refM = -1;

        if (ChildDataController.fishGameResultList.Count> 0)
        {
            var LastResult=ChildDataController.fishGameResultList.FindLast(x=>true);

            DateTime RefDate = Convert.ToDateTime(LastResult.���۳�¥);

            //���������� ȣ���� �ð�. 
            refH = RefDate.Hour;
            refM = RefDate.Minute;
        }
        Debug.Log("initializeSchedule. refH : " + refH + ", refM : " + refM);


        int scheduleNum = 0;
        int finishedScheduleNum = 0;

        foreach (var schedule in ChildDataController.scheduleInformationList)
        {
            scheduleNum++;

            int nowH = new DateTime().Hour;
            int nowM = new DateTime().Minute;

            Transform table = scheduleContent.transform;
            int siblingIndex = 0;


            Debug.Log("initializeSchedul : "+schedule.����+", ���� : "+schedule.����);

            string title = schedule.����;
            string hour = (schedule.�� < 10 ? "0" : "") + schedule.��.ToString();
            string minute = (schedule.�� < 10 ? "0" : "") + schedule.��.ToString();

            Debug.Log(schedule.����);

            GameObject newTime = Instantiate(timePrefab, table);
            newTime.transform.Find("����").GetComponent<TextMeshProUGUI>().text = title;
            newTime.transform.Find("�ð�").GetComponent<TextMeshProUGUI>().text = hour + ":" + minute;
            int imageIdx = schedule.���.Contains("ǥ��") ? 0 : schedule.���.Contains("����") ? 1 : schedule.���.Contains("����") ? 2 : 3;

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];

            //�߰��� ������
            int hAfter;
            Int32.TryParse(hour, out hAfter);
            int minAfter;
            Int32.TryParse(minute, out minAfter);

            


            foreach (var currentTime in table.GetComponentsInChildren<Transform>())
            {

                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }

                int modeIndex=0;
                modeIndex = modeImages.FindIndex(x => x == currentTime.GetComponent<Image>().sprite);

                Debug.Log("initializeSchedule : modeIndex " + modeIndex);

                if (hAfter<refH || (hAfter == refH && minAfter <= refM))
                {
                    newTime.transform.Find("�Ϸ�ǥ��").gameObject.SetActive(true);
                    hAfter += 24;
                    minAfter += 60;
                    finishedScheduleNum++;
                }
                else
                {
                    newTime.transform.Find("�Ϸ�ǥ��").gameObject.SetActive(false);

                    //notificatoin �߰�.
#if PLATFORM_ANDROID && UNITY_EDITOR

                    DateTime date1 = Convert.ToDateTime(DateTime.Now.Year + " " + DateTime.Now.Month + " " + DateTime.Now.Day + " " + schedule.�� + ":" + schedule.��);

                    if (date1 > DateTime.Now)
                    {
                    TimeSpan delay = date1 - DateTime.Now;

                    var notificationParams = new NotificationParams{
                            Id = UnityEngine.Random.Range(0, int.MaxValue),
                            Delay = delay,
                            Title = schedule.���� + "_" + schedule.�� + "��" + schedule.�� + "��",
                            Message = schedule.���� + "_" + schedule.�� + "��" + schedule.�� + "�п� " + schedule.��� + " ȣ�� �Ʒ��� �������ּ���.",
                            Ticker = "Ticker",
                            Sound = true,           
                            Vibrate = true,
                            Light = true,
                            SmallIcon = NotificationIcon.Heart,
                            SmallIconColor = new Color(0, 0.5f, 0),
                            LargeIcon = "app_icon"
                        };

                    Debug.Log(DateTime.Now.Year + " " + DateTime.Now.Month + " " + DateTime.Now.Day + " "+  schedule.�� + ":" + schedule.��);

                    NotificationManager.SendCustom(notificationParams);
                    }
#endif
                }



                if (currentTime.tag != "TimeOrAim")
                {
                    continue;
                }
                //Debug.Log(currentTime.parent.name + ", " + currentTime.name);

                //������ ������
                string timeBefore = currentTime.Find("�ð�").GetComponent<TextMeshProUGUI>().text;
                int hBefore;
                Int32.TryParse(timeBefore.Substring(0, 2), out hBefore);
                int minBefore;
                Int32.TryParse(timeBefore.Substring(3, 2), out minBefore);

                //�ڿ� �־�� �� ���� : 
                if (hBefore < hAfter || (hBefore == hAfter && minBefore < minAfter))
                {
                    siblingIndex++;
                    
                    mode = modeIndex == 0 ? "ǥ�ظ��" : modeIndex == 1 ? "���߸��" : modeIndex == 2 ? "�������" : "��������Ǹ��";
                }
            }                        

            newTime.GetComponent<Image>().sprite = modeImages[imageIdx];
            newTime.transform.SetSiblingIndex(siblingIndex);
            
        }

        //Debug.Log("Current mode : " + mode);
        gameProgressRatio= gameProgress.value = scheduleNum==0? 0 : finishedScheduleNum/(float) scheduleNum;
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
            AndroidBLEPluginStart.CallByAndroid("���� ���� ��ư�� ���� ��Ʈ�� �������ּ���.");
        }

        //SceneLoader.LoadScene("���ý���ȭ��");
        if (BreathingTest.isTested)
        {
            SceneLoader.LoadScene("���ý���ȭ��");
        }
        else
        {
            SceneLoader.LoadScene("���̸���׽�Ʈȭ��");
        }
        
    }
}
