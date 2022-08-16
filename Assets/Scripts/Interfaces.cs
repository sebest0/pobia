using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public interface IItem
{
    // Start is called before the first frame update
    void PickUp(Transform hand);
    void Use();
    void Drop();
}

public interface IHold
{
    void Hold(Vector3 vector);
}
