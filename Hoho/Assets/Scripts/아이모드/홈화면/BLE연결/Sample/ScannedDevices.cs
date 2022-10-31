using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannedDevices : MonoBehaviour
{
    public GameObject deviceContent;
    public GameObject DevicePannelPrefab;
    public Button ScannedBtn;
    public GameObject ScannedDevicesObj;
    public GameObject DeviceProfilePannelObj;

    // Start is called before the first frame update
    void Start()
    {

        ScannedBtn.onClick.AddListener(() => {
            ScannedDevicesObj.SetActive(true);
            DeviceProfilePannelObj.SetActive(false);
            updateList();
        });
    }

    private void updateList()
    {
        //AndroidBLEPluginSample._bleControlObj.Call("tidyUpScanned_devices");

        Transform[] childList = deviceContent.GetComponentsInChildren<Transform>();
        if (childList.Length>0)
        {
            for (int i=1; i<childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        foreach (Android.Data.BleScannedDevice device in AndroidBLEPluginStart.scannedDevices)
        {            
            GameObject myInstance = Instantiate(DevicePannelPrefab, deviceContent.transform);
            DevicePannel pannelSetter = myInstance.GetComponent<DevicePannel>();
            pannelSetter.setAddr(device.address);
            pannelSetter.setName(device.name);
            pannelSetter.setRssi(device.rssi);
        }
    }

}
