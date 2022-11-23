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
        videoRect.anchoredPosition = new Vector2(backgroundRect.rect.width * 0*63f / 1000f, backgroundRect.rect.height * 0*47.1151f / 1000f);
        float length = Mathf.Min(backgroundRect.rect.height * (575.7883f - 59f) / 1000f, backgroundRect.rect.width * (851.8f - 59f) / 1000f);
        videoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length);
        videoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
