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

    public GameObject deletePannel;

    // Start is called before the first frame update
    void Start()
    {
        /*deletePannel = GameObject.Find("GameManager").GetComponent<TimetableController>().deletePannel;

        if (titleText != null)  //시간 설정
        {
            this.GetComponent<Button>().onClick.AddListener(deleteTimeSchedule);
        }
        else //목표 설정
        {
            this.GetComponent<Button>().onClick.AddListener(deleteAim);
        }        
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deleteTimeSchedule()
    {
        Button deleteBtn=deletePannel.transform.Find("삭제").GetComponent<Button>();


        deletePannel.SetActive(true);        deleteBtn.onClick.RemoveAllListeners();
        deleteBtn.onClick.AddListener( () => Destroy(this.gameObject));


    }


    void deleteAim()
    {

        

    }
}
