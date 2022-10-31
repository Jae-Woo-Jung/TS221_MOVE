using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentCustomizer : MonoBehaviour
{

    public TMP_InputField inhaleText;
    public TMP_InputField inhaleStopText;
    public TMP_InputField exhaleText;
    public TMP_InputField exhaleStopText;
    public TMP_InputField totalTimesText;

    public FireStoreTimeCustom fireStroeTimeCustom;

    public void applyCustomizing()
    {


        if (totalTimesText.text.Length * inhaleText.text.Length * inhaleStopText.text.Length * exhaleText.text.Length * exhaleStopText.text.Length == 0)
        {
            Debug.LogError("Please write all");
            return;
        }

        if (totalTimesText.text == "0" || inhaleText.text == "0" || exhaleText.text == "0")
        {
            Debug.LogError("No 0 is acceptable.");
            return;
        }

        fireStroeTimeCustom.SendTimeCustom();
        totalTimesText.text = inhaleText.text = inhaleStopText.text = exhaleText.text = exhaleStopText.text = "";
    }

    private float clampTime(string time)
    {
        float temp = 0;
        if (float.TryParse(time, out temp))
        {
            return Mathf.Clamp(temp, 0f, 8f);
        }

        return 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //0~8을 넘어가는 값이 입력됐으면 
        foreach (TMP_InputField input in new TMP_InputField[] { inhaleText, inhaleStopText, exhaleText, exhaleStopText })
        {
            input.onEndEdit.AddListener((string time) => {
                input.text = clampTime(time).ToString();
            });
        }

        totalTimesText.onEndEdit.AddListener((string str) =>
        {
            if (str.StartsWith("-"))
            {
                totalTimesText.text = "0";
            }
        });


    }

    // Update is called once per frame
    void Update()
    {

    }

}
