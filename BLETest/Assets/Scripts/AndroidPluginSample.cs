using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginSample : MonoBehaviour
{
    [SerializeField] Text _text;

    [SerializeField] Button _button;

    static AndroidJavaObject _pluginInstance;

    // Start is called before the first frame update
    void Awake()
    {
        var pluginClass = new AndroidJavaClass("com.example.pluginsample1.UnityPluginSample1");

        _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");

        _text.text = _pluginInstance.Call<string>("getPackageName");
    }

    // Update is called once per frame
    public static void CallByAndroid(string message)
    {
        _pluginInstance.Call("showToast", message);
    }
}
