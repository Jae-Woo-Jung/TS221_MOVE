using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileController : MonoBehaviour
{

    public Image profileImage;
    public TextMeshProUGUI pw;
    public TextMeshProUGUI id;
    public TMP_InputField pwInput;    

    // Start is called before the first frame update
    void Start()
    {
        //pwButton�� Listener�� Inspectorâ���� ����.

        pwInput.GetComponentInChildren<Button>().onClick.AddListener(modifyPW);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void modifyPW()
    {
        if (pwInput.text.Length > 3)
        {
            pw.text = pwInput.text;
        }
        pwInput.text = "";
        pwInput.gameObject.SetActive(false);
    }

}
