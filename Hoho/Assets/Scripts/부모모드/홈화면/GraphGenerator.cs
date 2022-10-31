using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class GraphGenerator : MonoBehaviour
{
    [Tooltip("�׷����� ��")]
    public GameObject dotPrefab;
    [Tooltip("�׷��� �� ������ ��. ���� ȣ�� ���")]
    public UILineRenderer lineRenderer1;

    [Tooltip("�׷��� ��. ���� ȣ�� ���")]
    public UILineRenderer lineRenderer2;

    [Tooltip("ȣ��׷����� content")]
    public GameObject content;
   
    public GameObject xAxis;
    public GameObject yAxis;    

    [Tooltip("���̾�̽����� ������ ȣ�� ������")]
    public Dictionary<int, float> data=new Dictionary<int, float>();

    [Tooltip("�� ȭ�鿡 ���� ���� ����")]
    public int dotNum=10;


    /// <summary>
    /// �־��� �����Ϳ� ���� �׷��� �׸���.
    /// </summary>
    public void drawGraph(Dictionary<int, float> data1, Dictionary<int, float> data2)
    {
        //�׷��� �����.
        Transform[] transformList = content.transform.GetComponentsInChildren<Transform>();
        foreach (Transform dot in transformList)
        {
            if ((dot.name).Contains("time"))
            {
                Destroy(dot.gameObject);
            }
        }

        var rectTransform = content.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        
        List<GameObject> dots = new List<GameObject>();

        List<int> keys = data1.Keys.ToList();
        keys.Sort();

        //���� ȣ�� ��� �׷��� �׸���.
        foreach (int key in keys)
        {
            GameObject dot=Instantiate(dotPrefab, content.transform);
            dots.Add(dot);

            float xSpacing = content.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x/dotNum;
            Debug.Log("xSpacing : " + xSpacing);


            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(30f+yAxis.GetComponent<RectTransform>().anchoredPosition.x + key * xSpacing , xAxis.GetComponent<RectTransform>().anchoredPosition.y - 30f);
            dot.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, xAxis.GetComponent<RectTransform>().rect.yMax+yAxis.GetComponent<RectTransform>().rect.height*(0.01f+data1[key]));
           
           //Debug.Log("rectPosition of "+datum.Key+" : "+ dot.GetComponent<RectTransform>().anchoredPosition);
            Debug.Log((key, data1[key]) );

            dot.GetComponent<TextMeshProUGUI>().text = key.ToString();
            //dot.GetComponent<TextMeshProUGUI>().fontSize=
        }

        //draw lines
        List<Vector2> tempPoints =new List<Vector2>();

        for(int i=0; i<dots.Count; i++)
        {
            Debug.Log(i+"th dot :"+dots[i].GetComponent<TextMeshProUGUI>().text);
        }

        for(int i=0; i<dots.Count; i++)
        {
            tempPoints.Add(dots[i].GetComponent<RectTransform>().anchoredPosition+ dots[i].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition);
        }

        lineRenderer1.Points = tempPoints.ToArray();
        
        dots.Clear();

        keys = data2.Keys.ToList();
        keys.Sort();

        //���� ȣ�� ��� �׷��� �׸���.
        foreach (int key in keys)
        {
            GameObject dot = Instantiate(dotPrefab, content.transform);
            dots.Add(dot);

            float xSpacing = content.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x / dotNum;
            Debug.Log("xSpacing : " + xSpacing);


            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(30f + yAxis.GetComponent<RectTransform>().anchoredPosition.x + key * xSpacing, xAxis.GetComponent<RectTransform>().anchoredPosition.y - 30f);
            dot.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, xAxis.GetComponent<RectTransform>().rect.yMax + yAxis.GetComponent<RectTransform>().rect.height * (0.01f + data2[key]));

            //Debug.Log("rectPosition of "+datum.Key+" : "+ dot.GetComponent<RectTransform>().anchoredPosition);
            Debug.Log((key, data2[key]));

            dot.GetComponent<TextMeshProUGUI>().text = key.ToString();
            //dot.GetComponent<TextMeshProUGUI>().fontSize=
        }

        //draw lines
        tempPoints.Clear();

        for (int i = 0; i < dots.Count; i++)
        {
            Debug.Log(i + "th dot :" + dots[i].GetComponent<TextMeshProUGUI>().text);
        }

        for (int i = 0; i < dots.Count; i++)
        {
            tempPoints.Add(dots[i].GetComponent<RectTransform>().anchoredPosition + dots[i].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition);
        }

        lineRenderer2.Points = tempPoints.ToArray();

        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dots[dots.Count-1].GetComponent<RectTransform>().anchoredPosition.x);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i=0; i<100; i++)
        {
            data.Add(i, (Mathf.Sin(i)+1.0f)/2.0f);
        }
        drawGraph(data);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
