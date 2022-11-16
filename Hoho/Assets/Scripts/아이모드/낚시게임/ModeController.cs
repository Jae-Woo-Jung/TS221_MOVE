using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    public Button button_0;
    public Button button_1;
    public Button button_2;
    
    

    void Mode0()
    {
        FishGenerator.modeIndex = 0;
    }

    void Mode1()
    {
        FishGenerator.modeIndex = 1;
    }

    void Mode2()
    {
        FishGenerator.modeIndex = 2;
    }


    // Start is called before the first frame update
    void Start()
    {
        button_0.onClick.AddListener(Mode0);
        button_1.onClick.AddListener(Mode1);
        button_2.onClick.AddListener(Mode2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
