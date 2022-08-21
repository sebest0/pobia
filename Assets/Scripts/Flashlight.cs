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
    public Transform itemContainer;

    void Start()
    {
        lus = gameObject.GetComponentInChildren<Light>();
    }

    void Update()
    {
        //si tiene padre
        if (itemContainer != null)
        {
            transform.SetPositionAndRotation(itemContainer.position, itemContainer.rotation);
        }
    }
    public void PickUp(Transform Parent)
    {
        itemContainer = Parent;
        Debug.Log("Padre puesto");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void Drop()
    {
        itemContainer = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //Lo tira para lante (left es adelante)
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 2, ForceMode.Impulse);
        Debug.Log("Padre ido");
    }
    public void Use() {
        lus.enabled = !lus.isActiveAndEnabled;
    }
}
