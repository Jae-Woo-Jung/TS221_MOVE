using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

using Android.Data;
using UnityEngine.Android;

public class AndroidBLEPluginSample : MonoBehaviour
{
    public Button ScanBtn;

    public static List<BleScannedDevice> scannedDevices = new List<BleScannedDevice>();
    public static List<BleCharacteristicData> characteristicDatas = new List<BleCharacteristicData>();
    public static Dictionary<string, List<BleCharacteristicKeyInfo>> characteristicKeyInfos = new Dictionary<string, List<BleCharacteristicKeyInfo>>();
    public static AndroidJavaObject _bleControlCls;
    public static AndroidJavaObject _bleControlObj;
    
    public float updateTimeLimit = 0.05f;
    public float scanTimeLimit = 5.0f;
    public string connectedDevice;
    
    private float updateTime = 0.0f;
    private float scanTime = 0.0f;

    private static bool isScanning = false;



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
            AndroidPluginSample.CallByAndroid("bleInit done.");
        }
        else
        {
            AndroidPluginSample.CallByAndroid("bleInit failed.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        bleInit();

        ScanBtn.onClick.AddListener(startScan);

        isScanning = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateTime+=Time.deltaTime;
        if (updateTime > updateTimeLimit)
        {

            updateDevices();
            bool isConnected = _bleControlObj.Call<bool>("isConnected");
            if (isConnected)
            {                
                updateCharacteristicDatasAndKeyInfos();
            }
            updateTime = 0.0f;
        }

        if (isScanning)
        {
            ScanBtn.enabled = false;
            ScanBtn.GetComponentInChildren<Text>().text="Scanning....";
            scanTime += Time.deltaTime;
            if (scanTime > scanTimeLimit) 
            {
                AndroidBLEPluginSample._bleControlObj.Call("tidyUpScanned_devices");
                stopScan(); 
            }
        }
        else
        {
            ScanBtn.enabled = true;
            ScanBtn.GetComponentInChildren<Text>().text = "Scan";
            scanTime = 0.0f;
        }

    }

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
    private void startScan()
    {
        

        Debug.LogError("permission checking.");
        foreach (string permission in new string[] { Permission.FineLocation, Permission.CoarseLocation, "android.permission.BLUETOOTH_SCAN", "android.permission.BLUETOOTH_CONNECT", Permission.ExternalStorageWrite, Permission.ExternalStorageRead})
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
        
        AndroidPluginSample.CallByAndroid("BLEPluginSample.scannedNum : " + scannedDevices.Count.ToString());
        scannedDevices.Clear();
        
        //AndroidPluginSample.CallByAndroid("BLEPluginSample.scan start");
        isScanning = true;
    }

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

}