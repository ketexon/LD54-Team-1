using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GridCellStatus
{
    Infertile,
    Fertile
}

// A cell that something can be placed on
public class GridCell : MonoBehaviour
{
    [SerializeField] public GridCellStatus Status;
    [SerializeField] public Placeable Placeable;

    Placeable placeable;

    void Awake()
    {
        // If we start with a placeable, attach it to this cell
        if(placeable != null)
        {
            placeable.AttachToCell(this);
        }
    }

    public void OnPlaceableDie()
    {
        Placeable = null;
    }
}
