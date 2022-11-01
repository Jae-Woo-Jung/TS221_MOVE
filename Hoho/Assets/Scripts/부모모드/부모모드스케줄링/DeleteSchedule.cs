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

    public TextMeshProUGUI aimText;

    private GameObject deletePannel;

    // Start is called before the first frame update
    void Start()
    {
        deletePannel = GameObject.Find("GameManager").GetComponent<TimetableController>().deletePannel;

        if (titleText != null)  //�ð� ����
        {
            this.GetComponent<Button>().onClick.AddListener(deleteTimeSchedule);
        }
        else //��ǥ ����
        {
            this.GetComponent<Button>().onClick.AddListener(deleteAim);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deleteTimeSchedule()
    {
        Button deleteBtn=deletePannel.transform.Find("����").GetComponent<Button>();

        deletePannel.transform.Find("����").GetComponent<TextMeshProUGUI>().text = 
            this.transform.parent.parent.name + "���� " + titleText.text + ",\n" + timeText.text + "�� ������\n�����Ͻðڽ��ϱ�?";

        deleteBtn.onClick.RemoveAllListeners();
        deleteBtn.onClick.AddListener(deleteClicked);
        
        deletePannel.SetActive(true);

    }

    void deleteClicked()
    {
        deletePannel.SetActive(false);
        Destroy(this.gameObject);
    }

    void deleteAim()
    {

        

    }
}
