using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChangeLayerRecursively
{
    public static void ChangeLayersRecursively(this Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            child.ChangeLayersRecursively(name);
        }
    }

}
