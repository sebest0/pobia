using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    Transform itemContainer;
    

    IItem item;
    IHold hold;
    public float speed = 8f;
    public float jumpSpeed = 6f;
    public float gravity = 10f;
    public float sensitivity = 100f;

    public Camera cam;
    public CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;
    float xRotation = 0;

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
        Movimiento();
        CheckHold();
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;

                // Cast a Ray
                // to see if it is about to hit anything.
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.GetComponentInParent<IItem>() != null)
                    {
                        CmdPickupItem(hit.transform.GetComponentInParent<NetworkIdentity>());
                        hit.transform.GetComponentInParent<IItem>().PickUp(itemContainer);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
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

    void CheckHold()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.GetComponent<IHold>() != null)
                    {
                        canMove = false;
                        hold = hit.transform.GetComponent<IHold>();
                        CmdPickupItem(hit.transform.GetComponent<NetworkIdentity>());
                    }
                }
            }

            if (Input.GetKey(KeyCode.Mouse0) && !canMove)
            {
                hold.Hold(transform.forward*Input.GetAxis("Mouse Y") + transform.right*Input.GetAxis("Mouse X"));
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && !canMove)
            {
                hold.Release();
                hold = null;
                canMove = true;
            }
        }
    }

    [Command]
    void CmdPickupItem(NetworkIdentity item)
    {
        item.AssignClientAuthority(connectionToClient);
        Debug.Log(item.assetId);
    }
}


