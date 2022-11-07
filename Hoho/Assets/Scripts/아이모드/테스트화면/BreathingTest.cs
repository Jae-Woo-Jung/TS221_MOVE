using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class BreathingTest : MonoBehaviour
{
    public static bool isTested = false;
    public static bool isTesting = false;

    public static int breathingMax=1;
    public static int breathingMin=0;

    public List<Image> checkList = new List<Image>();

    public Sprite done;
    public Sprite notYet;

    public GameObject tooTightPannel;
    public GameObject tooLoosePannel;

    public VideoPlayer guideVideo;

    // Start is called before the first frame update
    void Start()
    {
        breathingMax = 1;
        breathingMin = 0;
        isTested = false;
        
        guideVideo.Stop();
        guideVideo.loopPointReached += endReached;
        
        foreach (var check in checkList)
        {
            check.sprite = notYet;
        }


        Invoke("startTesting", 1);

    }

    // Update is called once per frame
    void Update()
    {
        


        if (isTesting)
        {
            if (Characteristic.value > breathingMax)
            {
                Characteristic.value = breathingMax;
            }
            if (Characteristic.value < breathingMin)
            {
                Characteristic.value = breathingMin;
            }
        }

    }


    void startTesting()
    {
        
        isTesting = true;
        guideVideo.Play();
    }

    void showResult()
    {
        isTesting = false;


        float delta = (breathingMax - breathingMin);

        breathingMin = (int) (0.1f*delta +breathingMin);
        breathingMax =(int)  (0.9 * delta+ breathingMin);


        delta= (breathingMax - breathingMin);


        if (delta < 50)
        {
            //호흡 다시 진행.
            Debug.Log("다시 진행해주세요.");
            SceneLoader.LoadScene("아이모드테스트화면");
            return;
        }

        if (delta >= 50)
        {
            Debug.Log("낚시 게임을 진행해주세요.");

        }

        if (breathingMin < 100)
        {
            Debug.Log("너무 헐렁");
            tooLoosePannel.SetActive(true);
        }

        if (breathingMax > 900)
        {
            Debug.Log("너무 조임");
            tooTightPannel.SetActive(true);
        }


        isTested = true;
        isTesting = false;
        SceneLoader.LoadScene("낚시시작화면");
    }

    void endReached(VideoPlayer vp)
    {

        int idx = checkList.FindIndex(x => x.sprite == notYet);

        if (idx >= 0)
        {
            checkList[idx].sprite = done;
            vp.Play();
        }
        
        if (idx==checkList.Count-1)
        {
            vp.Stop();
            showResult();
        }
    }

}
