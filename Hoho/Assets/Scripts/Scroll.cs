using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Scroll : MonoBehaviour {


    public RectTransform List;
    public int count;
    private float pos;
    private float movepos;
    private bool IsScroll = false;
    public int page;

    // Use this for initialization
	void Start () 
    {
        
        page = 1;
        pos = List.localPosition.x;
        movepos = List.rect.xMax - List.rect.xMax / count;
        Debug.Log("List xMax : " + List.rect.xMax);
        Debug.Log("List xMin : "+List.rect.xMin);
        Debug.Log(List.rect.xMax - List.rect.xMax / count + "|" + pos);
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Right()
    {
        if(page == count)
        {
            Debug.Log("오른쪽 끝");
        }
        else
        {
            page += 1;
            IsScroll = true;
            movepos = pos - List.rect.width / count;
            pos = movepos;
            StartCoroutine(scroll());
        }
    }

    public void Left()
    {
        //if(List.rect.xMax - List.rect.xMax/count == movepos)
        if (page==1)
        {
            Debug.Log("왼쪽 끝");
        }
        else
        {
            page -= 1;
            IsScroll = true;
            movepos = pos + List.rect.width / count;
            pos = movepos;
            StartCoroutine(scroll());
        }
    }

    IEnumerator scroll()
    {
        while (IsScroll)
        {
            List.localPosition = Vector2.Lerp(List.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
            if (Vector2.Distance(List.localPosition, new Vector2(movepos, 0)) < 0.1f)
            {
                IsScroll = false;
            }
            yield return null;
        }
    }
}
