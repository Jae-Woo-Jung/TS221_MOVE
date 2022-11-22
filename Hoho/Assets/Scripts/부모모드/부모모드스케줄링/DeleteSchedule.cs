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

        if (titleText != null)  //�ð� ����
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
        Button deleteBtn=deletePannel.transform.Find("����").GetComponent<Button>();
        Button deleteAllBtn= deletePannel.transform.Find("��ü����").GetComponent<Button>();

        deletePannel.transform.Find("����").GetComponent<TextMeshProUGUI>().text = 
            this.transform.parent.parent.parent.parent.name + "���� " + titleText.text + ",\n" + timeText.text + "�� ������\n�����Ͻðڽ��ϱ�?";

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
            x.���� == transform.parent.parent.name+"����" && x.���� == titleText.text && 
            x.��==Int32.Parse(timeText.text.Substring(0, 2)) && x.�� == Int32.Parse(timeText.text.Substring(3, 2)));

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

        GameObject weekTable=GameObject.Find("�ְ��ð�ǥ");

        foreach (var currentTime in weekTable.GetComponentsInChildren<Transform>())
        {
            if (currentTime.tag != "TimeOrAim")
            {
                continue;
            }
            Debug.Log(currentTime.parent.name + ", " + currentTime.name);

            string day = currentTime.parent.parent.name;
            string title = currentTime.Find("����").GetComponent<TextMeshProUGUI>().text;

            string timeBefore = currentTime.Find("�ð�").GetComponent<TextMeshProUGUI>().text;
            int hour;
            Int32.TryParse(timeBefore.Substring(0, 2), out hour);
            int minute;
            Int32.TryParse(timeBefore.Substring(3, 2), out minute);

            Sprite mode = currentTime.GetComponent<Image>().sprite;

            //���� ����
            if ( (refTitle, refH, refM, refMode) == (title, hour, minute, mode))
            {

                int index = ParentDataController.scheduleInformationList.FindIndex(x =>
                                x.���� == day + "����" && x.���� == title &&
                                x.�� == hour && x.�� == minute);

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
