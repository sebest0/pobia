using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FugaDeCilindro : MonoBehaviour
{
    [SerializeField]
    float i = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Mathf.Sin(Time.time + i) * 0.01f;
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.position = new Vector3(0,0,0);
        }
    }
}
