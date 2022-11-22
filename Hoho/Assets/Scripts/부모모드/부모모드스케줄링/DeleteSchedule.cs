using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class DeleteSchedule : MonoBehaviour
{

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;

    private GameObject deletePannel;

    // Start is called before the first frame update
    void Start()
    {
        deletePannel = GameObject.Find("GameManager").GetComponent<TimetableController>().deletePannel;

        if (titleText != null)  //시간 설정
        {
            this.GetComponent<Button>().onClick.AddListener(deleteTimeSchedule);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deleteTimeSchedule()
    {
        Button deleteBtn=deletePannel.transform.Find("삭제").GetComponent<Button>();
        Button deleteAllBtn= deletePannel.transform.Find("전체삭제").GetComponent<Button>();

        deletePannel.transform.Find("내용").GetComponent<TextMeshProUGUI>().text = 
            this.transform.parent.parent.parent.parent.name + "요일 " + titleText.text + ",\n" + timeText.text + "의 일정을\n삭제하시겠습니까?";

        deleteBtn.onClick.RemoveAllListeners();
        deleteBtn.onClick.AddListener(deleteClicked);
        deleteAllBtn.onClick.RemoveAllListeners();
        deleteAllBtn.onClick.AddListener(deleteAllClicked);

        deletePannel.SetActive(true);

    }

    void deleteClicked()
    {
        deletePannel.SetActive(false);
        Destroy(this.gameObject);

        int idx = ParentDataController.scheduleInformationList.FindIndex(x => 
            x.요일 == transform.parent.parent.name+"요일" && x.제목 == titleText.text && 
            x.시==Int32.Parse(timeText.text.Substring(0, 2)) && x.분 == Int32.Parse(timeText.text.Substring(3, 2)));

        if (idx != -1)
        {
            ParentDataController.scheduleInformationList.RemoveAt(idx);
        }
        else
        {
            Debug.Log("deleteClicked : no eleemnt?");
        }

        ParentDataController.SendScheduleInformation();
    }


    void deleteAllClicked()
    {

        string refDay = transform.parent.parent.name;
        string refTitle = titleText.text;
        int refH = Int32.Parse(timeText.text.Substring(0, 2));
        int refM = Int32.Parse(timeText.text.Substring(3, 2));
        Sprite refMode = GetComponent<Image>().sprite;

        GameObject weekTable=GameObject.Find("주간시간표");

        foreach (var currentTime in weekTable.GetComponentsInChildren<Transform>())
        {
            if (currentTime.tag != "TimeOrAim")
            {
                continue;
            }
            Debug.Log(currentTime.parent.name + ", " + currentTime.name);

            string day = currentTime.parent.parent.name;
            string title = currentTime.Find("제목").GetComponent<TextMeshProUGUI>().text;

            string timeBefore = currentTime.Find("시간").GetComponent<TextMeshProUGUI>().text;
            int hour;
            Int32.TryParse(timeBefore.Substring(0, 2), out hour);
            int minute;
            Int32.TryParse(timeBefore.Substring(3, 2), out minute);

            Sprite mode = currentTime.GetComponent<Image>().sprite;

            //요일 무시
            if ( (refTitle, refH, refM, refMode) == (title, hour, minute, mode))
            {

                int index = ParentDataController.scheduleInformationList.FindIndex(x =>
                                x.요일 == day + "요일" && x.제목 == title &&
                                x.시 == hour && x.분 == minute);

                if (index != -1)
                {
                    ParentDataController.scheduleInformationList.RemoveAt(index);
                }
                else
                {
                    Debug.Log("deleteClicked : no eleemnt?");
                }
                Destroy(currentTime.gameObject);
            }
        }

        deletePannel.SetActive(false);
        ParentDataController.SendScheduleInformation();
    }

}
