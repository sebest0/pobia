using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : NetworkBehaviour, IItem
{
    [SerializeField]
    Light lus;

    //Desde player le podemos asignar el "parent" para poder agarrarlo al jugador
    [SyncVar]
    Transform itemContainer;

    void Start()
    {
        lus = gameObject.GetComponentInChildren<Light>();
    }

    void Update()
    {
        //si tiene padre
        if (itemContainer != null)
        {
            //Saque lo del quaternion porque es un falso padre, osea le copia la posicion y la rotacion, pero es un gameobject independiente
            transform.SetPositionAndRotation(itemContainer.position, itemContainer.rotation * Quaternion.Euler(0, 90, 0));
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void PickUp(Transform Parent)
    {
        itemContainer = Parent;
        Debug.Log("Padre puesto");
    }
    public void Drop()
    {
        itemContainer = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //Lo tira para lante
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2);
        Debug.Log("Padre ido");
    }
    public void Use() {
        lus.enabled = !lus.isActiveAndEnabled;
    }
}
