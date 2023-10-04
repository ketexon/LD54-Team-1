using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private string description;

    private bool hovering;

    void Update()
    {
        if (hovering) textbox.transform.position = Input.mousePosition;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        textbox.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = description;
        textbox.transform.position = e.position;
        textbox.SetActive(true);
        hovering = true;
    }

    public void OnPointerExit(PointerEventData e)
    {
        textbox.SetActive(false);
        hovering = false;
    }
}
