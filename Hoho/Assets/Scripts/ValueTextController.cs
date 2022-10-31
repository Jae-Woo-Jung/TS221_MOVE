using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class ValueTextController : MonoBehaviour
{

    public TextMeshProUGUI Value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AndroidBLEPluginStart.isConnected)
        {
            Value.text = "Value : " + (Characteristic.isValueNull ? "No Value" : Characteristic.value);
        }
    }
}
