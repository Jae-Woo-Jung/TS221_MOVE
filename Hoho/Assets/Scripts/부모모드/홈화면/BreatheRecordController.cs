using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BreatheRecordController : MonoBehaviour
{
    [Tooltip("호흡 측정용 막대그래프의 prefab")]
    public GameObject barPrefab;
    [Tooltip("일간호흡기록창의 content")]
    public GameObject content;

    [Tooltip("호흡기록패널")]
    public GameObject breathePannel;


    public int percent;
    public int hour;
    public int minute;
    public bool isAM;

    /// <summary>
    /// 막대를 생성함. percent는 69%
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    /// <param name="isAM"></param>
    
    //public void makeBar(int percent, int hour, int minute, bool isAM=false)
    public void makeBar(Dictionary<int, float> data1, Dictionary<int, float> data2)    
    {
        DateTime time = DateTime.Parse(ParentDataController.fishGameResult.시작시간);
        Debug.Log("게임 시작 시간 : "+time);
        hour = time.Hour;
        minute = time.Minute;
        isAM = (time.ToString("tt") == "AM");
        percent = ParentDataController.fishGameResult.완성률;

        GameObject bar=Instantiate(barPrefab, content.transform);
        
        GameObject ratio=bar.transform.Find("달성률").gameObject;        
        GameObject button= bar.transform.Find("Button").gameObject;
        //GameObject icon = bar.transform.Find("아이콘").gameObject;
        TextMeshProUGUI timeText = bar.transform.Find("시간").gameObject.GetComponent<TextMeshProUGUI>();

        //icon 변경
        //icon.GetComponent<Image>();

        //버튼 조절
        button.GetComponent<Button>().onClick.AddListener(makeGraph);
        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (GameObject.Find("Y축").GetComponent<RectTransform>().sizeDelta.y-50f)*percent/100f);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, GameObject.Find("X축").GetComponent<RectTransform>().rect.yMax);

        //달성률 변경
        ratio.GetComponent<TextMeshProUGUI>().text = percent + "%";
        ratio.GetComponent<RectTransform>().anchoredPosition=new Vector2(ratio.GetComponent<RectTransform>().anchoredPosition.x, button.GetComponent<RectTransform>().rect.yMax+50f);

        //시간 변경
        timeText.text = hour + ":" + minute + (isAM ? "AM" : "PM");         
        
        void makeGraph()
        {
            breathePannel.SetActive(true);
            breathePannel.GetComponent<GraphGenerator>().drawGraph(data1, data2);
            GameObject.Find("홈화면패널").SetActive(false);
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
