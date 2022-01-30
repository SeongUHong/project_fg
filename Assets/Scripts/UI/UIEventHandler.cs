using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, 
    IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //UI�� ���� �׼��� �߰���
    //Action Invoke�� �߰��� �׼� ����

    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnDownHandler = null;
    public Action<PointerEventData> OnUpHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnDownHandler != null)
        {
            OnDownHandler.Invoke(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(OnUpHandler != null)
        {
            OnUpHandler.Invoke(eventData);
        }
    }
}
