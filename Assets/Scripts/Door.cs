using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Door : NetworkBehaviour, IHold
{
    private void Start()
    {
        
    }
    [Command]
    public void Hold(float x, float z, NetworkIdentity player)
    {
        GetComponent<Rigidbody>().AddForce(x, 0, z);
    }
}