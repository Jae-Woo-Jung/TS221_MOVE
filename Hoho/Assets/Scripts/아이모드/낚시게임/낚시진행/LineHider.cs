using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("LineHider : " + collision.name);
        if (collision.tag == "Vertex")
        {            
            Vector3 newPos= collision.transform.position;
            newPos.z = 1;
            collision.transform.position = newPos;
        }
    }
}
