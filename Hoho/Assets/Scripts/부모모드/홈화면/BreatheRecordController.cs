using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BreatheRecordController : MonoBehaviour
{
    [Tooltip("ȣ�� ������ ����׷����� prefab")]
    public GameObject barPrefab;
    [Tooltip("�ϰ�ȣ����â�� content")]
    public GameObject content;

    [Tooltip("ȣ�����г�")]
    public GameObject breathePannel;


    public int percent;
    public int hour;
    public int minute;
    public bool isAM;

    /// <summary>
    /// ���븦 ������. percent�� 69%
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="isAM"></param>
    
    //public void makeBar(int percent, int hour, int minute, bool isAM=false)
    public void makeBar(Dictionary<int, float> data1, Dictionary<int, float> data2)    
    {
        DateTime time = DateTime.Parse(ParentDataController.fishGameResult.���۽ð�);
        Debug.Log("���� ���� �ð� : "+time);
        hour = time.Hour;
        minute = time.Minute;
        isAM = (time.ToString("tt") == "AM");
        percent = ParentDataController.fishGameResult.�ϼ���;

        GameObject bar=Instantiate(barPrefab, content.transform);
        
        GameObject ratio=bar.transform.Find("�޼���").gameObject;        
        GameObject button= bar.transform.Find("Button").gameObject;
        GameObject icon = bar.transform.Find("������").gameObject;
        TextMeshProUGUI timeText = bar.transform.Find("�ð�").gameObject.GetComponent<TextMeshProUGUI>();

        //icon ����
        icon.GetComponent<Image>();

        //��ư ����
        button.GetComponent<Button>().onClick.AddListener(makeGraph);
        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (GameObject.Find("Y��").GetComponent<RectTransform>().sizeDelta.y-50f)*percent/100f);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, GameObject.Find("X��").GetComponent<RectTransform>().rect.yMax);

        //�޼��� ����
        ratio.GetComponent<TextMeshProUGUI>().text = percent + "%";
        ratio.GetComponent<RectTransform>().anchoredPosition=new Vector2(ratio.GetComponent<RectTransform>().anchoredPosition.x, button.GetComponent<RectTransform>().rect.yMax+50f);

        //�ð� ����
        timeText.text = hour + ":" + minute + (isAM ? "AM" : "PM");         
        
        void makeGraph()
        {
            breathePannel.SetActive(true);
            breathePannel.GetComponent<GraphGenerator>().drawGraph(data1, data2);
            GameObject.Find("Ȩȭ���г�").SetActive(false);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ParentDataController.ReceiveBreath(makeBar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
