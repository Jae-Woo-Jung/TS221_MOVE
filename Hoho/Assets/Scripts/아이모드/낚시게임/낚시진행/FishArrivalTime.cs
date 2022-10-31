using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어선/빈사각형의 script.
/// </summary>
public class FishArrivalTime : MonoBehaviour
{
    public static GameObject guideSquare;

    private static float arrivalTime = 0.0f;
    [Tooltip("도착 시간")]
    public float arrivalTimeStatic;

    [Tooltip("GameManager의 TimeController 컴포넌트")]
    public TimeController timeController;
    public FishGenerator fishGenerator;

    private static int fishNum = 0;

    /// <summary>
    /// 물고기의 생성 후 도착 시간을 알려줌. 초기값은 0.0f. 한 번 설정되면 게임 중 안 바뀜.
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
        guideSquare = GameObject.Find("가이드라인사각형");
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
