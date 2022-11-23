using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;

public class SensorTracker : MonoBehaviour
{
    public Button ScanBtn;

    [Header("연결 확인을 위한 임시값.")]
    public TextMeshProUGUI UUID;
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
            ScanBtn.enabled = false;
            ScanBtn.GetComponentInChildren<TextMeshProUGUI>().text = "연결 성공";

            //Value.text = "Value : " + (Characteristic.isValueNull ? "No Value" : Characteristic.value);
            //UUID.text = "UUID : " + Characteristic.realUuid;
        }
        else if (AndroidBLEPluginStart.isScanning || AndroidBLEPluginStart.isConnecting)
        {
            ScanBtn.enabled = false;
            ScanBtn.GetComponentInChildren<TextMeshProUGUI>().text = "연결 중....";
        }
        else if (!AndroidBLEPluginStart.isConnected)
        {
            ScanBtn.enabled = true;
            ScanBtn.GetComponentInChildren<TextMeshProUGUI>().text = "벨트 연결";
        }
    }
}
