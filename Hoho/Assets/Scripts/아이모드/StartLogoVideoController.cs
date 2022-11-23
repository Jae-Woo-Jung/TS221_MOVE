using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StartLogoVideoController : MonoBehaviour
{
    public VideoPlayer logo;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("logo length : "+logo.length);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("logo time : " + logo.time);
        if (logo.time/logo.length > 0.8f)
        {
            logo.Stop();
        }        
    }
}
