using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointListController : MonoBehaviour
{
    public class pointContent
    {
        /// <summary>
        /// icon ����. ������ ǥ��.  0~2
        /// </summary>
        public int icon=0;

        /// <summary>
        /// ���� ��� ����.  ��¥�� ǥ�� �� ���൵ ��.
        /// </summary>
        public string content="";

        /// <summary>
        /// ���� ����Ʈ
        /// </summary>
        public int point=0;
    }


    //��Ͽ� ����� �����ܵ�.
    public List<Sprite> spriteList;

    public ProgressController progressController;

    public static List<pointContent> pointContentList = new List<pointContent> { new pointContent(), new pointContent(), new pointContent(), new pointContent() };

    //���1, ���2, ���3, ���4
    private List<GameObject> pointList = new List<GameObject>();


    /// <summary>
    /// CP = Compensation Point, CPpointStack�� Ī��ī���� ����Ʈ�� �׾Ƶδ� ����
    /// </summary>
    public Stack<String> CPpointStack = new Stack<String>();


    public static List<TextMeshProUGUI> guideTexts = new List<TextMeshProUGUI>();

    public void updateList(Sprite icon, string content, int point)
    {
        if (pointContentList.Count>10)
        {
            //�ʱ�ȭ.
            pointContentList.RemoveRange(0, pointContentList.Count - 3);
            Debug.Log("pointContentList num : "+pointContentList.Count);
        }

        pointContent cont = new pointContent();
        cont.icon = 0;
        cont.content = content;
        cont.point = point;


        pointContentList.Add(cont);
        /*
        for (int i = pointList.Count - 2; i >=0; i--)
        {
            var currentObject = pointList[i];
            var targetObject = pointList[i + 1];
            
            setIcon(targetObject, currentObject.transform.GetChild(0).GetComponent<Image>().sprite);
            setContent(targetObject, currentObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            setPoint(targetObject, currentObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text);
        }

        setIcon(pointList[0], icon);
        setContent(pointList[0], content);
        setPoint(pointList[0], point);
        */
        setList();
    }

    

    public void GetStackP()
    {
        Debug.Log("GetStakP : "+ (ChildDataController.CPresult.Count));
        
        foreach(KeyValuePair<string, int> pair in ChildDataController.CPresult)
        {
            Debug.Log( "pair : "+(pair.Key, pair.Value));
        }
        
        for (int i = 1; i < ChildDataController.CPresult.Count+1; i++)
        {
            try
            {

                Debug.Log("GetStackP iteration");
                string msg = ChildDataController.CPresult["����Ʈ_" + i].ToString();
                //guideTexts[i - 1].text = msg;
                Debug.Log(msg);
                CPpointStack.Push(msg);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public static Stack<String> GetStackC()
    {
        Stack<String> stack = new Stack<String>();
        for (int i = 1; i < guideTexts.Count + 1; i++)
        {
            string msg = ChildDataController.CPresult["����Ʈ_" + i].ToString() + "P";
            guideTexts[i - 1].text = msg;
            Debug.Log(msg);
            stack.Push(msg);

        }
        return stack;
    }





    // Start is called before the first frame update
    void Start()
    {
        //���� �� pointList�� ��ϵ� �ֱ�.
       Transform[] childList = GetComponentsInChildren<RectTransform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i].name.Contains("���"))
                {
                    pointList.Add(childList[i].gameObject);
                }
            
            }
        }
        setList();

        Debug.Log("start of point list controller");
        ChildDataController.receiveCompPoint(GetStackP);
    }

    // Update is called once per frame
    void Update()
    {
         
    }


    /// <summary>
    // pointContentList�� ���� Ȯ��. ������ 4���� ����. 
    /// </summary>
    private void setList()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i >= pointContentList.Count)
            {

            }

            int index = pointContentList.Count - 1 - i;
            pointContent cont = pointContentList[index];
            setIcon(pointList[i], spriteList[cont.icon]);
            setContent(pointList[i], cont.content);
            setPoint(pointList[i], cont.point);
        }
    }


    /// <summary>
    /// �������� �ش� sprite�� ��������.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="icon"></param>
    private void setIcon(GameObject target, Sprite icon)
    {
        if (icon != null)
        {
            target.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        }
    }

    /// <summary>
    /// content�� Ī������Ʈ,�⼮����Ʈ. �ڿ� ��¥�� )�� �������� �ƴ��� Ȯ���ؼ� �˾Ƽ� �ٿ���.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="content"></param>
    private void setContent(GameObject target, string content)
    {
        if (!content.EndsWith(")"))
        {
            content += "(" + DateTime.Now.Month + "/" + DateTime.Now.Day + ")";
        }
        target.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = content;
    }

    /// <summary>
    /// ������ ǥ������. ���� ����
    /// </summary>
    /// <param name="target"></param>
    /// <param name="point"></param>
    private void setPoint(GameObject target, int point)
    {
        int accumPoint=progressController.getCurrentPoint();
        string content = "+" + point; //+"\n"+"("+accumPoint+"P)";
        target.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = content;
    }

    /// <summary>
    /// ������ string���� ���޵� ���, �״�� ������.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="content"></param>
    private void setPoint(GameObject target, string content)
    {
        target.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = content;
    }
}
