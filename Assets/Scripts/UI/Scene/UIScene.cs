using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScene : UIBase
{
    //���̽�ƽ �ڵ鷯
    JoyStickHandler _joyStickHandler;

    public JoyStickHandler JoyStickHandler { get { return _joyStickHandler; } }

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

    //���̽�ƽ, �ڵ��� �μ��� ����
    //���̽�ƽ�� �ڵ鷯 ������Ʈ ����
    protected void BindJoyStickEvent(GameObject joyStick, GameObject handle)
    {
        JoyStickHandler joyStickHandler = joyStick.AddComponent<JoyStickHandler>();
        _joyStickHandler = joyStickHandler;
        joyStickHandler.Handle = Util.GetOrAddComponent<RectTransform>(handle);
    }

    //���ݹ�ư
    protected void BindAttackEvent(PointerEventData data)
    {
        Debug.Log("Attack Button Pressed");
    }

}
