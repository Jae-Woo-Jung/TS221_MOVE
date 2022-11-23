using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class HookController : MonoBehaviour
{
    public static float idealTime = 0.0f;

    public float idealTime_temp = 0.0f;
    public int �ϼ��� = 0;
    
    public TextMeshProUGUI point;

    public float chainYmin=-1.0f;
    public float chainYmax=2.5f;

    

    /// <summary>
    /// ���˴��� y��ǥ�� ����. y���� �ʹ� �۰ų� ū ��쿡 ����� Mathf.Clamp ���.
    /// y=0~1�� �θ� ��. 
    /// </summary>
    /// <param name="y"></param>
    public void setPosition(float y)
    {
        //�罽�� y��ǥ�� -1~2.5.
        //0~1 -> -1~2.5�� ������.
        y = Mathf.Clamp(y, 0f, 1f);
        y = ((chainYmax-chainYmin) * y + chainYmin) * ScalingOnGaming.yScaler;


        //y = y < 1f ? -1f : 2.5f;

        Vector3 localPos= new Vector3(this.transform.parent.localPosition.x/ this.transform.parent.localScale.x, y/this.transform.parent.localScale.y);
        //Debug.Log("�罽�� ���� localPos :" + this.transform.parent.localPosition);
        //Debug.Log("����� Position : "+this.transform.parent.position);
        Vector3 worldPos = localPos + this.transform.parent.parent.transform.position;

        //Debug.Log("�罽�� ���� ��ǥ : " + world_target);
        //Debug.Log("Vector3.Lerp(this.transform.parent.position, world_target, 0.1f) : " + Vector3.Lerp(this.transform.parent.position, world_target, 0.1f));

        this.transform.parent.position = Vector3.Lerp(this.transform.parent.position, worldPos, 0.1f);
        //this.transform.parent.localPosition = new Vector3(this.transform.parent.localPosition.x, y);
        //Debug.Log("�罽�� ���� pos :" + transform.parent.position);
        //Debug.Log("�罽�� y : "+y);
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
        �ϼ��� = (int)Mathf.Round((HookController.idealTime * 100f / (TimeController.progressedTime- FishArrivalTime.getArrivalTime())));
    }

    /// <summary>
    /// collision object���� rigidBody, collider ������Ʈ�� �־�� ��. �׸��� OnTriggerEnter2D�� 2�� ȣ���.
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
            idealTime += Time.deltaTime/4;  //���ùٴ��� collider�� 2����.
        }
    }

}
