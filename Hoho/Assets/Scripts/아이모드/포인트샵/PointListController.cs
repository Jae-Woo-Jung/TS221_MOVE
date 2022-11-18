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
        /// icon 종류. 순서로 표시.  0~2
        /// </summary>
        public int icon=0;

        /// <summary>
        /// 보상 목록 내용.  날짜는 표기 안 해줘도 됨.
        /// </summary>
        public string content="";

        /// <summary>
        /// 얻은 포인트
        /// </summary>
        public int point=0;
    }


    //목록에 사용할 아이콘들.
    public List<Sprite> spriteList;

    public ProgressController progressController;

    public CardController cardController;

    public static List<pointContent> pointContentList = new List<pointContent> { new pointContent(), new pointContent(), new pointContent(), new pointContent() };

    //목록1, 목록2, 목록3, 목록4
    private List<GameObject> pointList = new List<GameObject>();


    /// <summary>
    /// CP = Compensation Point, CPpointStack은 칭찬카드의 포인트를 쌓아두는 스택
    /// </summary>

    public Queue<String> CPpointQueue = new Queue<string>();
    public Queue<String> CPpointQueue_str = new Queue<string>();


    public static List<TextMeshProUGUI> guideTexts = new List<TextMeshProUGUI>();
    public static List<TextMeshProUGUI> guideTexts_str = new List<TextMeshProUGUI>();

    public void updateList(Sprite icon, string content, int point)
    {
        if (pointContentList.Count>10)
        {
            //초기화.
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
        //Debug.Log("GetStakP : "+ (ChildDataController.CPresult.Count));
        
        foreach(KeyValuePair<string, string> pair in ChildDataController.CPresult)
        {
            Debug.Log( "GetStackP pair : "+(pair.Key, pair.Value));
        }
        
        for (int i = 1; i < ChildDataController.CPresult.Count+1; i++)
        {
            try
            {

                

                //Debug.Log("GetStackP iteration");
                string msg = ChildDataController.CPresult["포인트_" + i].ToString();
                //guideTexts[i - 1].text = msg;
                //Debug.Log(msg);
                CPpointQueue.Enqueue(msg);

                string ID = ChildDataController.CPresult["ID_" + i];
                CPpointQueue.Enqueue(ID);

            }
            catch (Exception e)
            {
                Debug.Log("GetStackP : "+e.Message);
            }
        }
        Debug.Log("GetStackP, CPpointStack.Count : "+ CPpointQueue.Count);
    }

    public void GetStackP_str()
    {
        //Debug.Log("GetStakP : "+ (ChildDataController.CPresult.Count));

        foreach (KeyValuePair<string, string> pair in ChildDataController.CPresult_str)
        {
            //Debug.Log( "pair : "+(pair.Key, pair.Value));
        }

        for (int i = 1; i < ChildDataController.CPresult_str.Count + 1; i++)
        {
            try
            {

                //Debug.Log("GetStackP iteration");
                string msg = ChildDataController.CPresult_str["내용_" + i].ToString();
                //guideTexts[i - 1].text = msg;
                //Debug.Log(msg);
                CPpointQueue_str.Enqueue(msg);
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
            string msg = ChildDataController.CPresult["포인트_" + i].ToString() + "P";
            guideTexts[i - 1].text = msg;
            Debug.Log(msg);
            stack.Push(msg);

        }
        return stack;
    }





    // Start is called before the first frame update
    void Start()
    {
        //시작 시 pointList에 목록들 넣기.
       Transform[] childList = GetComponentsInChildren<RectTransform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i].name.Contains("목록"))
                {
                    pointList.Add(childList[i].gameObject);
                }
            
            }
        }
        setList();

        Debug.Log("start of point list controller");
        ChildDataController.receiveCompPoint(GetStackP);
        ChildDataController.receiveCompPoint_str(GetStackP_str);
    }

    // Update is called once per frame
    void Update()
    {
        if (CPpointQueue.Count == 0)
        {            
            cardController.cpmsg.GetComponent<Button>().interactable = false;
            cardController.bell.GetComponent<Image>().gameObject.SetActive(false);
        }
        else
        {
            cardController.cpmsg.GetComponent<Button>().interactable = true;
            cardController.bell.GetComponent<Image>().gameObject.SetActive(true);
        }
    }


    /// <summary>
    // pointContentList에 따라 확인. 끝에서 4개를 추출. 
    /// </summary>
    private void setList()
    {
        for (int i = 0; i < 3; i++)
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
    /// 아이콘을 해당 sprite로 설정해줌.
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
    /// content는 칭찬포인트,출석포인트. 뒤에 날짜는 )로 끝나는지 아닌지 확인해서 알아서 붙여줌.
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
    /// 점수를 표시해줌. 현재 점수
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
    /// 점수가 string으로 전달된 경우, 그대로 적어줌.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="content"></param>
    private void setPoint(GameObject target, string content)
    {
        target.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = content;
    }
}
