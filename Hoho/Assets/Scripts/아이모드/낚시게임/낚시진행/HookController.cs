using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class HookController : MonoBehaviour
{
    public static float idealTime = 0.0f;

    public float idealTime_temp = 0.0f;
    public int 완성률 = 0;
    
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


        //y = y < 1f ? -1f : 2.5f;

        Vector3 localPos= new Vector3(this.transform.parent.localPosition.x/ this.transform.parent.localScale.x, y/this.transform.parent.localScale.y);
        //Debug.Log("사슬의 예상 localPos :" + this.transform.parent.localPosition);
        //Debug.Log("사실의 Position : "+this.transform.parent.position);
        Vector3 worldPos = localPos + this.transform.parent.parent.transform.position;

        //Debug.Log("사슬의 최종 목표 : " + world_target);
        //Debug.Log("Vector3.Lerp(this.transform.parent.position, world_target, 0.1f) : " + Vector3.Lerp(this.transform.parent.position, world_target, 0.1f));

        this.transform.parent.position = Vector3.Lerp(this.transform.parent.position, worldPos, 0.1f);
        //this.transform.parent.localPosition = new Vector3(this.transform.parent.localPosition.x, y);
        //Debug.Log("사슬의 실제 pos :" + transform.parent.position);
        //Debug.Log("사슬의 y : "+y);
    }

    // Start is called before the first frame update
    void Start()
    {
        setPosition(-2.3f * ScalingOnGaming.yScaler);
        idealTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        idealTime_temp = idealTime;
        완성률 = (int)Mathf.Round((HookController.idealTime * 100f / (TimeController.progressedTime- FishArrivalTime.getArrivalTime())));
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject idealPosition = collision.gameObject;
        if (idealPosition.tag=="hookIdealPosition" && !Pause.isPaused && FishArrivalTime.getArrivalTime() > 0.0f)
        {
            idealTime += Time.deltaTime/4;  //낚시바늘의 collider가 2개임.
        }
    }

}
