using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalingOnGaming : MonoBehaviour
{

    public static float xScreenHalfSizeBase= 8.888889f;
    public static float yScreenHalfSizeBase = 5f;
    public static float xScaler;
    public static float yScaler;

    public RectTransform clockRect;

    public GameObject background;
    public GameObject boat;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
    // Start is called before the first frame update
    void Start()
    {

        Invoke("scaling", 0.3f);

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void scaling()
    {
        float yScreenHalfSize = Camera.main.orthographicSize;
        float xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        xScaler = xScreenHalfSize / xScreenHalfSizeBase;
        yScaler = yScreenHalfSize / yScreenHalfSizeBase;

        Vector3 clockPos = new Vector3((clockRect.position.x / Screen.width - 0.5f) * xScreenHalfSize, (clockRect.position.y / Screen.height - 0.5f) * yScreenHalfSize, 0);

        Debug.Log("y is " + yScreenHalfSize);
        Debug.Log("x is " + xScreenHalfSize);

        boat.transform.localScale = new Vector3(xScreenHalfSize / 8.890215f * 1.5f, yScreenHalfSize / 5.0f * 1.5f, 1);
        boat.transform.position = new Vector3(xScreenHalfSize / 8.890215f * -7.15f, yScreenHalfSize / 5.0f * 3.4f, 0);

        background.transform.localScale = new Vector3(xScreenHalfSize / xScreenHalfSizeBase * 1.579981f, yScreenHalfSize / yScreenHalfSizeBase * 1.888991f, 1);
    }
}
