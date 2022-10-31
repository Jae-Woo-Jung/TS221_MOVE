using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Android;
using Android.Data;

public class ServicePannel : MonoBehaviour
{
    public Text ServiceName;
    public Text UUID;
    public GameObject characteristicPrefab;
    public Button Service;

    private string _addr;
    private string _uuid;
    private bool isFolded=true;


    public void SetUuid(string deviceId, string serviceId)
    {
        _addr = deviceId;
        _uuid = serviceId;
        UUID.text = "UUID : " + _uuid;
    }

    private void unfold()
    {
        if (isFolded)
        {
            List<BleCharacteristicKeyInfo> keyInfos = new List<BleCharacteristicKeyInfo>();

            if (AndroidBLEPluginSample.characteristicKeyInfos.TryGetValue(_addr, out keyInfos))
            {
              foreach (BleCharacteristicKeyInfo info in keyInfos)
              {
                if (info.serviceUUID == _uuid)
                {
                    GameObject myCharacteristic = Instantiate(characteristicPrefab, this.transform);
                    Characteristic generator= myCharacteristic.GetComponent<Characteristic>();
                    generator.setUuid(_addr, _uuid, info.characteristicUUID);

                    RectTransform rectTransform = this.transform.GetComponent<RectTransform>();
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height + myCharacteristic.GetComponent<RectTransform>().rect.height);
                }
              }
            }
            isFolded = false;
        }

    }

    private void fold()
    {
        if (!isFolded)
        {
            Transform[] transformList=this.GetComponentsInChildren<Transform>();
            foreach (Transform trans in transformList)
            {
                if ( (trans.name).Contains("Characteristic"))
                {
                    RectTransform rectTransform = this.transform.GetComponent<RectTransform>();
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height - trans.gameObject.GetComponent<RectTransform>().rect.height);

                    Destroy(trans.gameObject);
                }
            }
            isFolded = true;
        }


    }

    public void ServiceBtnController()
    {
        if (isFolded)
        {
            unfold();
        }
        else
        {
            fold();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isFolded = false;
        Service.onClick.AddListener(ServiceBtnController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
