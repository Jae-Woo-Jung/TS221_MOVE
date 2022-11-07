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
            //ȣ�� �ٽ� ����.
            Debug.Log("�ٽ� �������ּ���.");
            SceneLoader.LoadScene("���̸���׽�Ʈȭ��");
            return;
        }

        if (delta >= 50)
        {
            Debug.Log("���� ������ �������ּ���.");

        }

        if (breathingMin < 100)
        {
            Debug.Log("�ʹ� �混");
            tooLoosePannel.SetActive(true);
        }

        if (breathingMax > 900)
        {
            Debug.Log("�ʹ� ����");
            tooTightPannel.SetActive(true);
        }


        isTested = true;
        isTesting = false;
        SceneLoader.LoadScene("���ý���ȭ��");
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
