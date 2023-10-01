using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : Placeable
{
    public override bool ValidatePlace(Vector3Int loc)
    {
        // TOOD: somethign with hammers idk
        return true;
    }

    public override void UpdateResources()
    {
        // TOOD: somethign with hammers idk
    }

}
