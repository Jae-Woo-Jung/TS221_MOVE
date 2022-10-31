using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class SetGameTime : MonoBehaviour
{

    public TMP_InputField timeSetter;

    public static void setTime(string time)
    {
        float t = 0f; 
        
        if (float.TryParse(time, out t))
        {
            TimeController.fullTimeStatic = t;
        }
    }


    public static void setTime(float a)
    {
        TimeController.fullTimeStatic = a;
    }
    // Start is called before the first frame update
    void Start()
    {
        timeSetter.onEndEdit.AddListener(str => setTime(timeSetter.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
