using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FarmingManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Grid grid;
    [SerializeField] private PlaceableSO initialSelection;

    private GameObject currentSelection;
    private GraphicRaycaster gr;

    void Start()
    {
        gr = this.GetComponent<GraphicRaycaster>();
        currentSelection = GameObject.Instantiate(initialSelection.prefab);
    }

    void OnEnable()
    {
        inputReader.PointEvent += SnapCurrentSelectionToGrid;
        inputReader.ClickEvent += PlaceCurrentSelection;
    }

    void OnDisable()
    {
        Destroy(currentSelection);
        inputReader.PointEvent -= SnapCurrentSelectionToGrid;
        inputReader.ClickEvent -= PlaceCurrentSelection;
    }

    void SnapCurrentSelectionToGrid(Vector2 loc)
    {
        Vector3Int cellLoc = grid.WorldToCell(Camera.main.ScreenToWorldPoint(loc));

        currentSelection.transform.position = grid.GetCellCenterWorld(cellLoc);
    }

    void PlaceCurrentSelection(bool place)
    {
        if (place && CanPlace())
        {
            if (currentSelection.GetComponent<Placeable>().TryPlace(grid.WorldToCell(currentSelection.transform.position)))
            {
                currentSelection = GameObject.Instantiate(currentSelection);
            }
        }
    }
    
    public void SetSelection(PlaceableSO selection)
    {
        Destroy(currentSelection);
        currentSelection = GameObject.Instantiate(selection.prefab);
        currentSelection.transform.position = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public bool CanPlace()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        return results.Count == 0;
    }
}
