using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

using UnityEngine.Android;
using Android.Data;
using TMPro;

public class AndroidBLEPluginStart : MonoBehaviour
{

    /// <summary>
    /// static 변수, 연결됐는지 여부.
    /// </summary>
    public static bool isConnected=false;

    public static List<BleScannedDevice> scannedDevices = new List<BleScannedDevice>();
    public static List<BleCharacteristicData> characteristicDatas = new List<BleCharacteristicData>();
    public static Dictionary<string, List<BleCharacteristicKeyInfo>> characteristicKeyInfos = new Dictionary<string, List<BleCharacteristicKeyInfo>>();
    public static AndroidJavaClass _bleControlCls;
    public static AndroidJavaObject _bleControlObj;
   
    [Tooltip("스캔 중 해당 시간(초)마다 스캔 결과를 업데이트함.")]
    public float updateTimeLimit = 0.5f;
    [Tooltip("스캔 지속 시간. 스캔은 배터리 소모를 많이 함.")]
    public float scanTimeLimit = 5.0f;
    [Tooltip("연결할 대상의 주소")]
    public string targetDevice= "14:90:BC:8A:2D:06";
    [Tooltip("연결된 장치의 주소")]
    public string connectedDevice;

    private float updateTime = 0.0f;
    private float scanTime = 0.0f;

    public static bool isConnecting = false;

    public static bool isScanning = false;


    /// <summary>
    /// bleControl 인스턴스 설정, activity, context 설정, 
    /// </summary>
    public static void bleInit()
    {
        _bleControlCls = new AndroidJavaClass("com.example.bleunityplugin.BLEControl");

        _bleControlObj = _bleControlCls.CallStatic<AndroidJavaObject>("getInstance");

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        _bleControlObj.Call("setActivity", activity, context);

        String initResult=_bleControlObj.Call<String>("init");

        if (initResult == "initialized")
        {
            //CallByAndroid("bleInit done.");
        }
        else
        {
            //CallByAndroid("bleInit failed.");
        }
    }

    // makeToast. 안드로이드 화면 상에 Text를 잠깐 띄워줌.
    public static void CallByAndroid(string message)
    {
        if (_bleControlCls == null)
        {
            bleInit();
        }
        
        Debug.Log("CallByAndroid : " + message);
#if PLATFORM_ANDROID && !UNITY_EDITOR
            _bleControlObj.Call("showText", message);
#endif        
    }

    public static void startScan()
    {        

        Debug.LogError("permission checking.");
        foreach (string permission in new string[] { Permission.FineLocation, Permission.CoarseLocation, "android.permission.BLUETOOTH_SCAN", "android.permission.BLUETOOTH_CONNECT" })
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Debug.LogError("Not yet permitted : " + permission);
                Debug.LogError("permitted : " + permission);
                Permission.RequestUserPermission(permission);
            }
            else
            {
                Debug.LogError("permitted : " + permission);
            }
        }


        _bleControlObj.Call<String>("init");
        _bleControlObj.Call("disconnectGattServer");
        _bleControlObj.Call("startScan");
        
        //CallByAndroid("BLEPluginSample.scannedNum : " + scannedDevices.Count.ToString());
        scannedDevices.Clear();

        isScanning = true;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {
        targetDevice="14:90:BC:8A:2D:06";
        bleInit();

        isScanning = false;
    }

    // Update is called once per frame
    void Update()
    {
        //연결이 끊긴 경우에 connecting=false;
        if (isConnected)
        {
            isConnected = _bleControlObj.Call<bool>("isConnected");
            if (!isConnected)
            {
                isConnecting = false;
            }
        }
        isConnected = _bleControlObj.Call<bool>("isConnected");

        updateTime +=Time.deltaTime;
        if (updateTime > updateTimeLimit)
        {

            updateDevices();
            
            if (isConnected)
            {                
                updateCharacteristicDatasAndKeyInfos();
            }
            updateTime = 0.0f;
        }

        if (isScanning || isConnecting)
        {
            scanTime += Time.deltaTime;
            if (scanTime > scanTimeLimit) 
            {
                AndroidBLEPluginStart._bleControlObj.Call("tidyUpScanned_devices");                
                stopScan(); 
            }
        }
        else if (!isConnected)
        {
            scanTime = 0.0f;            
            if (scannedDevices.Count>0){
                if (!isConnecting)
                {
                    bool targetExists = false;
                    foreach(BleScannedDevice device in scannedDevices)
                    {
                        //Debug.LogError(device.address);
                        if (device.address == targetDevice)
                        {
                            Debug.LogError("device name : " + device.name);
                            CallByAndroid("connecting to device name : " + device.name);
                            AndroidBLEPluginStart._bleControlObj.Call<string>("connectExternal", device.address);
                            isConnecting = true;
                            Invoke("checkConnecting", 15f);
                            targetExists = true;
                        }
                    }
                    /*
                    if (!targetExists)
                    {
                    //스캔은 됐으나 연결이 안 된 경우 자동으로 연결 시도. 5초 후에 다시 시도.
                    Debug.LogError("device name : "+scannedDevices[0].name+", device address : " + scannedDevices[0].address);
                    CallByAndroid("connecting to device name : " + scannedDevices[0].name);
                    AndroidBLEPluginStart._bleControlObj.Call<string>("connectExternal", scannedDevices[0].address);
                    isConnecting = true;
                    Invoke("checkConnecting", 15f);
                    }
                    */
                }
            }
        }
        else
        {            
            scanTime = 0.0f;
        }

    }

#endif

    /*
    /// <summary>
    /// 스캔될 때마다 scanned Device에 업로드.
    /// </summary>
    private void addScannedDevice(string addr, string name, int rssi)
    {
        scannedDevices.Add(new BleScannedDevice(addr, name, rssi));
    }
    */




    /// <summary>
    /// 스캔 종료. ScannedDevices 초기화 및 스캔 재시작.
    /// </summary>
    private void stopScan()
    {
        isScanning = false;
        scanTime = 0.0f;
        _bleControlObj.Call("stopScan");
    }

    private void updateDevices()
    {

        int deviceNum = _bleControlObj.Call<int>("getScanned_devicesNum");
        for (int i=0; i<deviceNum; i++)
        {
            AndroidJavaObject javaDevice = _bleControlObj.Call<AndroidJavaObject>("getScanned_device", i);
            string address = javaDevice.Call<string>("getAddress");
            string name = javaDevice.Call<string>("getName");
            int rssi = _bleControlObj.Call<int>("getRssi", i);

            bool add = true;

            for (int j=0; j<scannedDevices.Count; j++)
            {
                if (scannedDevices[j].address==address)
                {
                    add = false;
                }
            }

            if (add) { 
                BleScannedDevice device = new BleScannedDevice(address, name, rssi);
                scannedDevices.Add(device);
            }
            
        }
    }

    private void updateCharacteristicDatasAndKeyInfos()
    {
        int chNum = _bleControlObj.Call<int>("getCharacteristicKeyInfoNum");
        List<BleCharacteristicKeyInfo> newInfoList = new List<BleCharacteristicKeyInfo>();
        for (int i = 0; i < chNum; i++)
        {
            
            AndroidJavaObject javaInfo = _bleControlObj.Call<AndroidJavaObject>("getCharacteristicKeyInfo", i);
            string address = javaInfo.Call<string>("getAddress");
            string name = javaInfo.Call<string>("getName");
            string serviceUuid = javaInfo.Call<string>("getServiceUuid");
            string characteristicUuid = javaInfo.Call<string>("getCharacteristicUuid");
            int intData = javaInfo.Call<int>("getDataAsInt");
            string stringData = javaInfo.Call<string>("getDataAsString");
            bool hasData = javaInfo.Call<int>("getHasData") == 1 ? true:false;
            bool isNotifying = javaInfo.Call<int>("getNotification")==1?true:false;
            int property = javaInfo.Call<int>("getProperties");

            BleCharacteristicData newData=new BleCharacteristicData(address, serviceUuid, characteristicUuid, intData, stringData, isNotifying, property, hasData);
            BleCharacteristicKeyInfo newInfo = new BleCharacteristicKeyInfo(address, serviceUuid, characteristicUuid);

            connectedDevice = address;
            int existingIndex = getCharacteristicDataIndex(characteristicDatas, serviceUuid, characteristicUuid);

            if (existingIndex == -1)
            {
                characteristicDatas.Add(newData);
            }
            else            
            {
                characteristicDatas[existingIndex] = newData;
            }
            newInfoList.Add(newInfo);
        }

        List<BleCharacteristicKeyInfo> dump=null;
        if (characteristicKeyInfos.TryGetValue(connectedDevice, out dump))
        {
            characteristicKeyInfos[connectedDevice] = newInfoList;  
        }
        else
        {
            characteristicKeyInfos.Add(connectedDevice, newInfoList);
        }          
    }

    private int getCharacteristicDataIndex(List<BleCharacteristicData> datas, string serviceUuid, string characteristicUuid)
    {
        for (int j=0; j<datas.Count; j++)
        {
            if ((datas[j].serviceUuid == serviceUuid) && (datas[j].characteristicUuid == characteristicUuid))
            {
                return j;
            }
        }
        return -1;
    }


    private void checkConnecting()
    {
            isConnecting = false;      
    }
}