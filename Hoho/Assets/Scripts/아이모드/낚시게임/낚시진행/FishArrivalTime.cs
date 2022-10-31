using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �/��簢���� script.
/// </summary>
public class FishArrivalTime : MonoBehaviour
{
    public static GameObject guideSquare;

    private static float arrivalTime = 0.0f;
    [Tooltip("���� �ð�")]
    public float arrivalTimeStatic;

    [Tooltip("GameManager�� TimeController ������Ʈ")]
    public TimeController timeController;
    public FishGenerator fishGenerator;

    private static int fishNum = 0;

    /// <summary>
    /// ������� ���� �� ���� �ð��� �˷���. �ʱⰪ�� 0.0f. �� �� �����Ǹ� ���� �� �� �ٲ�.
    /// </summary>
    /// <returns></returns>
    public static float getArrivalTime()
    {
        return arrivalTime;
    }


    public static int getFishNum()
    {
        return fishNum;
    }

    // Start is called before the first frame update
    void Start()
    {
        guideSquare = GameObject.Find("���̵���λ簢��");
        arrivalTime = 0.0f;
        fishNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        arrivalTimeStatic = arrivalTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject fish = collision.gameObject;
        Debug.Log(fish);
        if (fish.tag == "Fish" || fish.tag == "fish")
        {
            //Debug.Log(arrivalTime);
            FishMove fishMove = fish.GetComponent<FishMove>();

            if (arrivalTime == 0.0f)
            {
                arrivalTime=timeController.getProgressedTime();
            }
            if (fishMove.isArrived)
            {
                return;
            }
            fishMove.isArrived = true;
            guideSquare.transform.position = new Vector2(guideSquare.transform.position.x, fish.transform.position.y);
            fishNum++;
        }
    }
}
