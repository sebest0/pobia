using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : NetworkBehaviour, IItem
{
    [SerializeField]
    Light lus;
    // Start is called before the first frame update
    void Start()
    {
        lus = gameObject.GetComponentInChildren<Light>();
    }
    [Command]
    public void PickUp(Transform itemContainer) {
        transform.SetParent(itemContainer);
        //Lo de quaternion es algo turbio q usa unity, pero es rotarlo 90 grados en el eje Y
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
    }

    public void Drop() { }
    public void Use() {
        lus.enabled = !lus.isActiveAndEnabled;
    }
}
