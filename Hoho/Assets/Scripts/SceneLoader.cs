using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadScene(string sceneName, bool isAdditive)
    {
        SceneManager.LoadScene(sceneName, isAdditive? LoadSceneMode.Additive: LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!SceneManager.GetActiveScene().name.Contains("낚시"))
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }

        if (GameObject.Find("벨트버튼") != null)
        {
            GameObject.Find("벨트버튼").GetComponent<Button>().onClick.AddListener(AndroidBLEPluginStart.startScan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
