using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    Transform itemContainer;

    IItem item;
    public float speed = 8f;
    public float jumpSpeed = 6f;
    public float gravity = 10f;
    public float sensitivity = 100f;

    public Camera cam;
    public CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;
    float xRotation = 0;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //Cuando aparece el jugador le saco el mouse dea
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Creo un GameObject vacio (local del metodo Start)
            GameObject camara = new GameObject("Camara" + gameObject.name);
            //Le agrego una camara
            camara.AddComponent<Camera>();
            //Lo instancio y utilizo el viejo GameObject para referenciar la camara
            camara = Instantiate(camara, Vector3.zero, Quaternion.Euler(Vector3.zero));
            //En cam guardo el componente "camara"
            cam = camara.GetComponent<Camera>();
            //Pongo la camara encima del jugador y le digo que herbert es el papa
            cam.transform.SetPositionAndRotation(transform.localPosition + Vector3.up * 0.8f, transform.localRotation);
            cam.transform.SetParent(transform);

        }
    }
    private void Update()
    {
        Movimiento();
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;

                Vector3 p1 = itemContainer.position;

                // Cast a sphere wrapping character controller 10 meters forward
                // to see if it is about to hit anything.
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.GetComponentInParent<IItem>() != null)
                    {
                        hit.transform.GetComponentInParent<IItem>().PickUp(itemContainer);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                item = gameObject.GetComponentInChildren<IItem>();
                if (item != null )
                {
                    item.Use();
                }
            }
        }

    }

    void Movimiento()
    {
        if (isLocalPlayer)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * 10f;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 10f;

            //El movimiento vertical del mouse hace girar en torno al eje X
            xRotation -= mouseY;
            //Para que no puedas girar
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            //Roto hacia la izquierda o derecha (sobre el eje Y)
            transform.Rotate(Vector3.up * mouseX);

            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

        }
    }
}


