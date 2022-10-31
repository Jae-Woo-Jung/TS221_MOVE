using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using Android.Data;

public class Characteristic : MonoBehaviour
{


    public Text CharacteristicName;
    public Text UUID;
    public Text Value;
    public Text IsNotifying;
    public Text Property;
    public float updateTimeLimit = 0.05f;

    private float updateTime = 0.0f;
    private string deviceAddress;
    private string serviceUuid;
    private string uuid;

    private int value;

    public void setUuid(string DeviceAddress, string ServiceUuid, string Uuid)
    {
        deviceAddress = DeviceAddress;
        serviceUuid = ServiceUuid;
        uuid = Uuid;
        UUID.text = "UUID : " + uuid;
    }

    private void Start()
    {
        AndroidBLEPluginSample._bleControlObj.Call<bool>("setNotification", serviceUuid, uuid, true);

        IsNotifying.text = "Notifying : " + "False";
    }

    private void Update()
    {
        updateTime += Time.deltaTime;

        if (updateTime > updateTimeLimit)
        {
            List<BleCharacteristicData> characteristicDatas = AndroidBLEPluginSample.characteristicDatas;

            BleCharacteristicData characteristic = characteristicDatas.Find(x => (x.serviceUuid == serviceUuid) && (x.characteristicUuid == uuid));

            if (characteristic.serviceUuid != null)
            {
                if (!characteristic.hasData)
                {
                    Value.text = "Value : No value";
                    AndroidBLEPluginSample._bleControlObj.Call<bool>("setNotification", serviceUuid, uuid, true);
                    return;
                }

                updateValue(serviceUuid, uuid, characteristic.intData, characteristic.stringData);
                IsNotifying.text = "Notifying : " + characteristic.isNotify.ToString();
                Property.text = "Properties : " + characteristic.property.ToString();
            }
            updateTime = 0f;
        }
    }


    private void updateValue(string serviceId, string charId, int intData, string stringData)
    {
        Debug.Log(charId+":\nthe value : " + value.ToString());
        Value.text = "int Value : " + intData.ToString();
    }

}
