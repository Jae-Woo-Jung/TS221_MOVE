using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scailing : MonoBehaviour
{
    public RectTransform backgroundRect;
    public RectTransform videoRect;

    // Start is called before the first frame update
    void Start()
    {
        videoRect.anchoredPosition = new Vector2(backgroundRect.rect.width * 60f / 1000f, backgroundRect.rect.height * 41f / 1000f);
        videoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, backgroundRect.rect.width * 880f / 1000f);
        videoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backgroundRect.rect.height * 579f / 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
