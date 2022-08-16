using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    //Para que el server lo vea y temas de autoridad
    [SyncVar]
    Transform mano;
    //Antes eran del tipo de la interfaz, hasta que tengamos inventario esto es para poder referenciarlo hasta tirarlo
    [SyncVar]
    GameObject item;
    //La puerta o cajon
    [SyncVar]
    GameObject hold;

    public float speed = 8f;
    public float jumpSpeed = 6f;
    public float gravity = 10f;
    public float sensitivity = 100f;

    public Camera cam;
    public CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;
    float xRotation = 0;

    //Mientras abra puertas no se va a poder mover, por ahora lo dejamos moverse dior
    private bool canMove = true;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //Cuando aparece el jugador le saco el mouse dea
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Activo la camara
            cam.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            Movimiento();
            //La funcion de abrir puerta o cajon (Es como las otras pero la hice funcion)
            CheckHold();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (item != null)
                {
                    Debug.Log("Ya tenes un item");
                    return;
                }
                RaycastHit hit;

                // Cast a Ray
                // to see if it is about to hit anything.
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<IItem>() != null)
                    {
                        item = hit.transform.gameObject;
                        CmdPickUp(item);
                        item.GetComponent<IItem>().PickUp(mano);
                        Debug.Log("Item agarrado");
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (item == null)
                {
                    Debug.Log("No tenes items");
                    return;
                }
                CmdDrop(item);
                item.GetComponent<IItem>().Drop();
                item = null;
                Debug.Log("Item suelto");
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (item != null )
                {
                    Debug.Log("Usado");
                    item.GetComponent<IItem>().Use();
                }
            }
            
        }

    }
    [Client]
    void Movimiento()
    {
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * 10f;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 10f;
            if (!canMove) { mouseX = 0; mouseY = 0; }
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
                if (!canMove) { moveDirection = Vector3.zero; }
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
            

        }
    }

    /*Es la funcion de las puertas, tiene 3 etapas, cuando haces click, asignas todo.
      Mientras mantenes, moves el mouse y mueve la puerta
      Y cuando soltas, le sacas autoridad y seguis con tu vida*/
    void CheckHold()
    {
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<IHold>() != null)
                    {
                        //Como estoy agarrando un agarrable, no me puedo mover y le asigno autoridad
                        hold = hit.transform.gameObject;
                        CmdPickUp(hold);
                        canMove = false;
                        Debug.Log("Agarrado");
                    }
                }
            }

            if (Input.GetKey(KeyCode.Mouse0) && !canMove)
            {
                //La funcion hold es la que mueve la puerta
                hold.GetComponent<IHold>().Hold(transform.forward*Input.GetAxis("Mouse Y") + transform.right*Input.GetAxis("Mouse X"));
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && !canMove)
            {
                CmdDrop(hold);
                hold = null;
                canMove = true;
                Debug.Log("Soltado");
            }
        }
    }

    //Funcion que asigna autoridad
    [Command]
    void CmdPickUp(GameObject pick)
    {
        pick.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }

    //Funcion que saca autoridad
    [Command]
    void CmdDrop(GameObject pick)
    {
        pick.GetComponent<NetworkIdentity>().RemoveClientAuthority();
    }
}


