using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScene : UIBase
{
    //조이스틱 핸들러
    JoyStickHandler _joyStickHandler;

    public JoyStickHandler JoyStickHandler { get { return _joyStickHandler; } }

    //공격버튼 핸들러
    public Action OnAttackBtnDownHandler = null;

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

    //조이스틱, 핸들을 인수로 받음
    //조이스틱에 핸들러 컴포넌트 적용
    public void BindJoyStickEvent(GameObject joyStick, GameObject handle)
    {
        JoyStickHandler joyStickHandler = joyStick.AddComponent<JoyStickHandler>();
        _joyStickHandler = joyStickHandler;
        joyStickHandler.Handle = Util.GetOrAddComponent<RectTransform>(handle);

    }

    //공격버튼
    protected void AttackEvent(PointerEventData data)
    {
        if (OnAttackBtnDownHandler != null)
        {
            OnAttackBtnDownHandler.Invoke();
        }
    }

}
