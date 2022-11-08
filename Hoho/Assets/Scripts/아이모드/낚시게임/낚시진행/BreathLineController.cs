using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathLineController : MonoBehaviour
{
    public GameObject vertex1;
    public GameObject vertex2;
    public LineRenderer line;

    public float scaler=0.95f;

    // Start is called before the first frame update
    void Start()
    {
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.isStarted && !Pause.isPaused)
        {
            setMove(vertex1.transform, FishMove.speed * Time.deltaTime);
            setMove(vertex2.transform, FishMove.speed * Time.deltaTime);
        }


        Vector3 pos1 = vertex1.transform.position;
        Vector3 pos2 = vertex2.transform.position;


        Vector3 direction = (pos2 - pos1).normalized;

        Vector3 startPos = scaler * pos1 + (1-scaler) * pos2;
        Vector3 endPos= (1-scaler) * pos1 + scaler * pos2;

        startPos.z = 1;
        endPos.z = 1;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);   

        if (!CheckObjectIsInCamera(vertex1) && !CheckObjectIsInCamera(vertex2))
        {
            Destroy(this.gameObject);
        }
    }


    bool CheckObjectIsInCamera(GameObject _target, bool checkRight = false, bool checkLeft = true)
    {
        Camera selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 screenPoint = selectedCamera.WorldToViewportPoint(_target.transform.position);
        bool onScreen = (checkLeft ? screenPoint.x > 0 : true) && (checkRight ? screenPoint.x < 1 : true); // && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    void setMove(Transform _target, float move)
    {
        _target.Translate(Vector3.left * move);
    }

}
