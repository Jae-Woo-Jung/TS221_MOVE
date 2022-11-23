using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Pause : MonoBehaviour
{
    public static bool isPaused=false;

    public GameObject connectionChecker;
    public Button pauseBtn;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public GameStart gameStart;

    // Start is called before the first frame update
    void Start()
    {
        pauseBtn.onClick.AddListener(pauseGame);
    }

    // Update is called once per frame
    void Update()
    {
/*
        if (AndroidBLEPluginStart.isConnected == false)
        {
            isPaused = true;
            connectionChecker.SetActive(true);
        }
*/

        if (GameStart.isStarted && heart3.activeSelf) // ||(pauseBtn.GetComponentInChildren<TextMeshProUGUI>().text == "¿Á∞≥")
        {
            pauseBtn.enabled = true;
        }
        else
        {
            pauseBtn.enabled = false;
        }
        
    }

    private void pauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            foreach (var heart in new GameObject[]{ heart1, heart2, heart3})
            {
                if (heart.activeSelf == true)
                {
                    //pauseBtn.GetComponentInChildren<TextMeshProUGUI>().text="¿Á∞≥";
                    heart.SetActive(false);
                    FishArrivalTime.guideSquare.SetActive(true);
                    return;
                }
            }
            isPaused = false;
        }
        else
        {
            //pauseBtn.GetComponentInChildren<TextMeshProUGUI>().text = "∏ÿ√„";
            FishArrivalTime.guideSquare.SetActive(false);
            gameStart.startGame();        
        }
     
    }


}
