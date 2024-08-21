using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    //���̽�ƽ �ڵ鷯
    JoyStickHandler _joyStickHandler;
    //���ݹ�ư �ڵ鷯
    Action _attackEvent;

    public JoyStickHandler JoyStickHandler
    {
        get
        {
            if (_joyStickHandler == null)
                Debug.Log("JoyStickHandler is not assigned");

            return _joyStickHandler;
        }
        set
        {
            _joyStickHandler = value;
        }
    }

    public Action AttackEvent { get { return _attackEvent; } set { _attackEvent = value; } }

    //���̽�ƽ, �ڵ��� �μ��� ����
    public void BindJoyStickEvent(GameObject joyStick, GameObject handle)
    {
        JoyStickHandler joyStickHandler = joyStick.AddComponent<JoyStickHandler>();
        _joyStickHandler = joyStickHandler;
        joyStickHandler.Handle = Util.GetOrAddComponent<RectTransform>(handle);
    }

    //���ݹ�ư
    public void ExecAttackEvent(PointerEventData data)
    {
        if (_attackEvent != null)
        {
            _attackEvent.Invoke();
        }
    }
}
