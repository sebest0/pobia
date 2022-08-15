using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Door : NetworkBehaviour, IHold
{
    [Command]
    public void Hold(Vector3 vector)
    {
        GetComponent<Rigidbody>().AddForce(vector);
    }

    [Command]
    public void Release()
    {
        netIdentity.RemoveClientAuthority();
    }
}