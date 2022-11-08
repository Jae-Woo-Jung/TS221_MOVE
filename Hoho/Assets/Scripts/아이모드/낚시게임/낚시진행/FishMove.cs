using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public string fishName;
    public static float speed=1.0f;

    [Tooltip("static 변수로서 오브젝트끼리 공유함.")]
    public float speedStatic
    {
        get { return speed; }    // static speed 반환
        set { speed = value; }   // value 키워드 사용
    }

    public bool isArrived; 

    /// <summary>
    /// move만큼 x축으로 이동시킴. 오른쪽에서 왼쪽 이동은 음수.
    /// </summary>
    /// <param name="move"></param>
    public void setMove(float move)
    {
        transform.Translate(Vector3.left*move);
    }

    /// <summary>
    /// 초속 speed 설정.
    /// </summary>
    /// <param name="speed"></param>
    public void setSpeed(float speed)
    {
        FishMove.speed = speed;
    }

    /// <summary>
    /// 물고기 종류 설정하기.
    /// </summary>
    /// <param name="name"></param>
    public void setKind(string name)
    {
        this.name = name;
        //this.GetComponent<Animator>().runtimeAnimatorController=animationMap[name];
    }

    /// <summary>
    /// 오브젝트가 카메라 안에 있는지 확인. checkRight이 false면, 오른쪽으로 벗어나 있는 건 신경 안 씀.
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="checkRight"></param>
    /// <param name="checkLeft"></param>
    /// <returns></returns>
    public bool CheckObjectIsInCamera(GameObject _target, bool checkRight=false, bool checkLeft=true)
    {
        Camera selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 screenPoint = selectedCamera.WorldToViewportPoint(_target.transform.position);
        bool onScreen = (checkLeft ? screenPoint.x > 0 : true) && (checkRight ? screenPoint.x < 1 : true); // && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }


    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.isStarted && !Pause.isPaused)
        {
            setMove(speed * Time.deltaTime);
        }


        if (!CheckObjectIsInCamera(gameObject))
        {
            Destroy(gameObject);
        }
    }
}
