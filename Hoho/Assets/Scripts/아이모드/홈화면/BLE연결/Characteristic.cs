using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Android.Data;

public class Characteristic : MonoBehaviour
{

    /// <summary>
    /// updateTimeLimit
    /// </summary>
    public float updateTimeLimit = 0f;
    /// <summary>
    /// isValueNull이 false인지 확인할 것. value
    /// </summary>
    public static int value=0;
    /// <summary>
    /// isValueNull이 false인지 확인할 것. 실제로 연결된 characteristic uuid
    /// </summary>
    public static string realUuid = "";
    /// <summary>
    /// isValueNull이 true면 값을 못 받고 있다는 뜻.
    /// </summary>
    public static bool isValueNull = true;

    private float updateTime = 0.0f;
    private string serviceUuid= "180c";
    private string uuid = "6eea5885-ed94-4731-b81a-00eb60d93b49";

    private List<string> notifiedList = new List<string>();

    public void setUuid(string DeviceAddress, string ServiceUuid, string Uuid)
    {
        serviceUuid = ServiceUuid;
        uuid = Uuid;        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void Update()
    {
        if (!AndroidBLEPluginStart.isConnected) { return; }

        updateTime += Time.deltaTime;

        if(serviceUuid==null || uuid == null) { return; }

        AndroidBLEPluginStart.isConnected = AndroidBLEPluginStart._bleControlObj.Call<bool>("isConnected");

        if (updateTime > updateTimeLimit)
        {
            Debug.LogError("characteristic update start");
            List<BleCharacteristicData> characteristicDatas = AndroidBLEPluginStart.characteristicDatas;

            BleCharacteristicData characteristic = characteristicDatas.Find(x => (x.serviceUuid.Contains(serviceUuid) && (x.characteristicUuid == uuid)));
            foreach(BleCharacteristicData ch in characteristicDatas)
            {                
                if (!notifiedList.Contains(ch.serviceUuid+ch.characteristicUuid))
                {
                    Debug.Log("serviceUuid : " + ch.serviceUuid + ", " + "characteristicUuid : " + ch.characteristicUuid+"\n");
                    AndroidBLEPluginStart._bleControlObj.Call<bool>("setNotification", ch.serviceUuid, ch.characteristicUuid, true);
                    notifiedList.Add(ch.serviceUuid+ch.characteristicUuid);
                    Debug.Log("notification done.");                  
                }
            }

            //Find가 실패할 수 있음.           
            try 
            {                
                characteristic.serviceUuid.Contains("");
            }
            catch
            {
                Debug.LogError("characteristic was null");
                characteristic = characteristicDatas.Find(x => x.hasData);
            }

            if (characteristic.serviceUuid != null)
            {
                if (!characteristic.hasData)
                {
                    Debug.Log("service : "+characteristic.serviceUuid+", character : "+characteristic.characteristicUuid);
                    isValueNull = true;
                    realUuid = "";
                    AndroidBLEPluginStart._bleControlObj.Call<bool>("setNotification", characteristic.serviceUuid, characteristic.characteristicUuid, true);
                    return;
                }
                
                Debug.Log(characteristic.characteristicUuid+ ":\nthe value : " + value.ToString());
                isValueNull = false;
                value = characteristic.intData;
                realUuid = characteristic.characteristicUuid;
                AndroidBLEPluginStart.isConnecting = false;
            }
            Debug.LogError("characteristic update Done.");
            updateTime = 0f;
        }
    }
#endif
}
