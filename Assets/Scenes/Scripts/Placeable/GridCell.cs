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
}
