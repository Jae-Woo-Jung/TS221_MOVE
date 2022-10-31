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
        
        this.transform.parent.transform.localPosition = new Vector2(this.transform.parent.localPosition.x, y);
        //Debug.Log("�罽�� ���� pos :" + transform.parent.localPosition);
        //Debug.Log("�罽�� y : "+y);
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
}
