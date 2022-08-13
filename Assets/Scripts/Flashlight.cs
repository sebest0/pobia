using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour, IItem
{
    [SerializeField]
    Light lus;
    // Start is called before the first frame update
    void Start()
    {
        lus = gameObject.GetComponentInChildren<Light>();
        TurnOff();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn()
    {
        lus.enabled = true;
    }

    public void TurnOff()
    {
        lus.enabled = false;
    }

    public void PickUp(Transform a) {
        transform.SetParent(a);
        //Lo de quaternion es algo turbio q usa unity, pero es rotarlo 90 grados en el eje Y
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
    }

    public void Drop() { }
    public void Use() {
        if (lus.enabled)
        {
            TurnOff();
        }
        else {
            TurnOn();
        }
    }
}
