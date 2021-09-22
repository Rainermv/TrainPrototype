using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItemReference : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public WagonComponent WagonComponentReference { get; set; }
    public Image ItemImageReference { get; set; }
    public Vector3 DestinationPosition { get; set; }

    public Action<PointerEventData, DragItemReference> OnPointerClickAction;
    public Action<PointerEventData, DragItemReference> OnBeginDragAction;
    public Action<PointerEventData, DragItemReference> OnDragAction;
    public Action<PointerEventData, DragItemReference> OnEndDragAction;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnPointerClickAction?.Invoke(pointerEventData, this);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        OnBeginDragAction?.Invoke(pointerEventData, this);

    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        OnDragAction?.Invoke(pointerEventData, this);

    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        OnEndDragAction?.Invoke(pointerEventData, this);

    }
}
