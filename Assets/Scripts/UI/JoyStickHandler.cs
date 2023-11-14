using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickHandler : MonoBehaviour,
    IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<Vector3> OnDragHandler = null;
    public Action OnUpHandler = null;

    //손잡이
    RectTransform _handle;
    //조이스틱
    RectTransform _joyStick;
    //조이스틱 반지름
    float _radius;
    //핸들 처음 위치
    Vector3 _handleFirstPos;

    public RectTransform Handle
    {
        set
        {
            _handle = value;
        }
    }

    private void Start()
    {
        _joyStick = Util.GetOrAddComponent<RectTransform>(gameObject);
        _handleFirstPos = _handle.position;
        _radius = _joyStick.sizeDelta.x * 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //클릭한 부분부터 드래그가 적용되도록함
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //벡터 계산을 위해 벡터2를 벡터3로 변환
        Vector3 downSpot = eventData.position;
        //방향을 구함
        Vector3 dir = (downSpot - _handleFirstPos).normalized;
        //거리를 구함
        float dis = (downSpot - _handleFirstPos).magnitude;
        //캐릭터 이동을 위한 방향
        Vector3 charDir = new Vector3(dir.x, 0.0f, dir.y);

        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(charDir);
        }

        if (dis > _radius)
        {
            _handle.position = _handleFirstPos + dir * _radius;
            
        }
        else
        {
            _handle.position = _handleFirstPos + dir * dis;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handle.position = _handleFirstPos;
        if(OnUpHandler != null)
        {
            OnUpHandler.Invoke();
        }
    }
}
