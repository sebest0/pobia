using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchingLightTest : MonoBehaviour
{
    Light lus;
    float ran;
    // Start is called before the first frame update
    void Start()
    {
        lus = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        ran = Random.Range(0.0f, 1.0f);
        if (ran > 0.1) {
            lus.intensity = 4;
        }
        else
        {
            lus.intensity = 1f;
        }
    }
}
