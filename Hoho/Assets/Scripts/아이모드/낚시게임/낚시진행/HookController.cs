using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class HookController : MonoBehaviour
{

    public TextMeshProUGUI point;

    public float chainYmin=-1.0f;
    public float chainYmax=2.5f;

    /// <summary>
    /// 낚싯대의 y좌표를 조절. y값이 너무 작거나 큰 경우에 대비해 Mathf.Clamp 사용.
    /// y=0~1로 두면 됨. 
    /// </summary>
    /// <param name="y"></param>
    public void setPosition(float y)
    {
        //사슬의 y좌표는 -1~2.5.
        //0~1 -> -1~2.5로 보내기.
        y = Mathf.Clamp(y, 0f, 1f);
        y = ((chainYmax-chainYmin) * y + chainYmin) * ScalingOnGaming.yScaler;
        
        this.transform.parent.transform.localPosition = new Vector2(this.transform.parent.localPosition.x, y);
        //Debug.Log("사슬의 실제 pos :" + transform.parent.localPosition);
        //Debug.Log("사슬의 y : "+y);
    }

    // Start is called before the first frame update
    void Start()
    {
        setPosition(-2.3f * ScalingOnGaming.yScaler);
    }

    // Update is called once per frame
    void Update()
    {        

    }

    /// <summary>
    /// collision object에는 rigidBody, collider 컴포넌트가 있어야 함. 그리고 OnTriggerEnter2D는 2번 호출됨.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameObject fish = collision.gameObject;
        if (fish.tag == "Fish" || fish.tag=="fish")
        {
            if (fish.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static) return;
            fish.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            int a=Convert.ToInt32(point.text.Split("P")[0])+100;
            point.text = a.ToString();
            GameObject.Destroy(fish);
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        }
        //handle point
    }
}
