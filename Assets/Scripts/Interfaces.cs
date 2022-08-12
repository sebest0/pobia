using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    // Start is called before the first frame update
    void PickUp(Transform itemContainer);
    void Use();
    void Drop();
}
