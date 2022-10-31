using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Android.Data;

public class DeviceProfile : MonoBehaviour
{

    public GameObject ServicePannelPrefab;
    public Transform ProfileContent;

    [SerializeField] private Text DeviceNameText;
    [SerializeField] private Text AdressText;

    private static string DeviceName;
    private static string _addr;

    public void setAddr(string addr)
    {
        _addr = addr;
        AdressText.text = addr;
    }

    public void setName(string name)
    {
        DeviceName = name;
        DeviceNameText.text = name;
    }

    public void updateList(string addr, string name)
    {
        setAddr(addr);
        setName(name);

        AndroidBLEPluginStart.characteristicKeyInfos.TryGetValue(_addr, out List<BleCharacteristicKeyInfo> keyInfos);

        Transform[] childList = ProfileContent.GetComponentsInChildren<Transform>();
        if (childList.Length > 0)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        string prevServiceUuid = null;
        foreach (BleCharacteristicKeyInfo info in keyInfos)
        {
            if (prevServiceUuid != info.serviceUUID)
            {

                GameObject myService = Instantiate(ServicePannelPrefab, ProfileContent);
                ServicePannel generator = myService.GetComponent<ServicePannel>();
                generator.SetUuid(_addr, info.serviceUUID);
                prevServiceUuid = info.serviceUUID;
            }
            
        }
    }
}
