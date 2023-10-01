using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class FarmingManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Grid grid;
    [SerializeField] private PlaceableSO initialSelection;
    [SerializeField] private GameObject emptyObject;

    private PlaceableSO currentSelectionSO;
    private GameObject currentSelection;
    private GraphicRaycaster gr;

    void Start()
    {
        gr = this.GetComponent<GraphicRaycaster>();
    }

    void OnEnable()
    {
        if (currentSelection == null) SetSelection(currentSelectionSO);
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
        if (currentSelection != null)
        {
            currentSelection.transform.position =
                grid.GetCellCenterWorld(
                    grid.WorldToCell(Camera.main.ScreenToWorldPoint(loc))
                    );
        }
    }

    void PlaceCurrentSelection(bool place)
    {
        if (place && CanPlace())
        {
            GameObject temp = GameObject.Instantiate(currentSelectionSO.gameObject);
            try {
                temp.GetComponent<Placeable>()
                            .TryPlace(
                                grid.WorldToCell(currentSelection.transform.position),
                                currentSelectionSO
                            );
            } catch (System.Exception e) {
                Debug.LogError(e);
            } finally {
                Destroy(temp);
            }
        }
    }
    
    public void SetSelection(PlaceableSO selection)
    {
        if (selection == null) selection = initialSelection;
        if (currentSelection != null) Destroy(currentSelection);
        currentSelectionSO = selection;
        currentSelection = GameObject.Instantiate(emptyObject);
        currentSelection.GetComponent<SpriteRenderer>().sprite = selection.sprite;
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
