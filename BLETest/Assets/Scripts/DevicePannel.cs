using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DevicePannel : MonoBehaviour
{
    public static float updateFrequency=0.5f;

    public Button ConnectBtn;
    public Button ProfileBtn;
    public Text AdressText;
    public Text DeviceNameText;
    public Text RssiText;
    public GameObject gameManager;

    public GameObject DeviceProfileObj;
    public GameObject ScannedDevicesObj;

    private DeviceProfile profileGenerator;

    private float updateTime = 0.0f;
    [SerializeField] private string addr;
    [SerializeField] private string DeviceName;
    [SerializeField] private int rssi;


    public void setAddr(string addr)
    {
        this.addr = addr;
        AdressText.text = "Address : "+addr;
    }

    public void setName(string name)
    {
        this.DeviceName = name;
        DeviceNameText.text = name;
    }

    public void setRssi(int Rssi)
    {
        this.rssi = Rssi;
        RssiText.text = "rssi : "+Rssi.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        
        
        ScannedDevicesObj = GameObject.Find("Canvas").transform.Find("ScannedDeivces").gameObject;
        DeviceProfileObj = GameObject.Find("Canvas").transform.Find("DeviceProfilePannel").gameObject;

        ConnectBtn.onClick.AddListener(ConnectBtnController);
        ProfileBtn.onClick.AddListener(seeProfile);

        ConnectBtn.GetComponentInChildren<Text>().text = "Connect";
        //ProfileBtn.gameObject.SetActive(false);

        profileGenerator = gameManager.GetComponent<DeviceProfile>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTime += Time.deltaTime;

        if (updateTime > updateFrequency)
        {
            bool isConnected = AndroidBLEPluginSample._bleControlObj.Call<bool>("isConnected");
            var datas = AndroidBLEPluginSample.characteristicDatas;
            if (datas.Count > 0 && datas[0].deviceAddr == addr && isConnected)
            {
                ConnectBtn.GetComponentInChildren<Text>().text = "Disconnect";
                ProfileBtn.gameObject.SetActive(true);
            }
            else
            {
                ConnectBtn.GetComponentInChildren<Text>().text = "Connect";
                ProfileBtn.gameObject.SetActive(false);
            }


            Android.Data.BleScannedDevice device = AndroidBLEPluginSample.scannedDevices.Find(x => (x.address == addr) && (x.name == DeviceName));
            if (device.name !=null)
            {
                setRssi(device.rssi);
            }
            else
            {
                Destroy(this);
            }
            updateTime = 0.0f;
        }        


    }

    private void ConnectBtnController()
    {
        if (ConnectBtn.GetComponentInChildren<Text>().text == "Connect")
        {
            Debug.LogError("Try Connect");
            AndroidBLEPluginSample._bleControlObj.Call<string>("connectExternal", addr);
        }
        else
        {
            AndroidBLEPluginSample._bleControlObj.Call("disconnectGattServer");
        }
    }

    private void seeProfile()
    {
        ScannedDevicesObj.SetActive(false);
        Debug.LogError("see Profile. addr: " +addr+", name : "+DeviceName);
        DeviceProfileObj.SetActive(true);        
        profileGenerator.updateList(addr, DeviceName);
    }

}
