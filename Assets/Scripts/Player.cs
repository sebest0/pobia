using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 8f;
    void Movimiento()
    {
        if(isLocalPlayer)
        {
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"); //Movimiento en funcion de la orientacion
            transform.position = Time.deltaTime * speed * move + transform.position;                                      //AAA
        }
    }

    void Update()
    {
        Movimiento();
    }
}
