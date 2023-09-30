using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all objects that can be placed on the grid
public class Placeable : MonoBehaviour
{
    public GridCell Cell { get; private set; } = null;

    public void AttachToCell(GridCell cell)
    {
        Cell = cell;
    }
}
