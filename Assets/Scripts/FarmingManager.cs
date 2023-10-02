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
    [SerializeField] private GraphicRaycaster gr;
    [SerializeField] private PlaceableDatabaseSO placeableDatabase;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private GameObject shopItemParent;

    private PlaceableSO currentSelectionSO;
    private GameObject currentSelection;

    Vector2 point;

    void Reset()
    {
        gr = GetComponentInParent<GraphicRaycaster>();
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

    void Start()
    {
        // clear shopItemParent
        foreach(Transform child in shopItemParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(var placeable in placeableDatabase.Placeables)
        {
            var shopItemGO = Instantiate(shopItemPrefab, shopItemParent.transform);
            var shopItem = shopItemGO.GetComponent<ShopItem>();
            shopItem.Initialize(this, placeable);
        }
    }

    void SnapCurrentSelectionToGrid(Vector2 loc)
    {
        point = loc;
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
        if (place && CanPlace() && ResourceManager.Instance.CanAfford(currentSelectionSO) )
        {
            Debug.Log(currentSelectionSO);
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
        currentSelection = Instantiate(currentSelectionSO.SpritePrefab);
        //currentSelection.GetComponent<SpriteRenderer>().sprite = selection.sprite;
        currentSelection.transform.position = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public bool CanPlace()
    {
        PointerEventData ped = new PointerEventData(null) { position = point };
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        return results.Count == 0;
    }
}
