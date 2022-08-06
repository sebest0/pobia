using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 8f;
    public float sensitivity = 100f;
    public Camera cam;

    float xRotation = 0;

    private void Start()
    {
        if(isLocalPlayer)
        {
            //Cuando aparece el jugador le saco el mouse dea
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //busco la camara cuando prenda el juego
            cam = Camera.FindObjectOfType<Camera>();
            //Pongo la camara encima del jugador y le digo que herbert es el papa
            cam.transform.SetPositionAndRotation(transform.localPosition, transform.localRotation);
            cam.transform.SetParent(transform);
        }
    }
    private void Update()
    {
        Movimiento();
    }

    void Movimiento()
    {
        if (isLocalPlayer)
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity * 10f;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity * 10f ;

            //El movimiento vertical del mouse hace girar en torno al eje X
            xRotation -= mouseY;
            //Para que no puedas girar
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            //Roto hacia la izquierda o derecha (sobre el eje Y)
            transform.Rotate(Vector3.up * mouseX);

            //Movimiento en funcion de la orientacion
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            transform.position = Time.deltaTime * speed * move + transform.position;
        }
    }
}


