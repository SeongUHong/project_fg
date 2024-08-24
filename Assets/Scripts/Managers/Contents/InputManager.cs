﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    //조이스틱 핸들러
    JoyStickHandler _joyStickHandler;
    // デフォ攻撃 スキルイベント
    Action _attackEvent;
    // FlameBall スキルイベント
    Action _flameBallEvent;
    // RangeSkill スキルイベント
    Action _rangeSkillEvent;

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
    public Action FlameBallEvent { get { return _flameBallEvent; } set { _flameBallEvent = value; } }
    public Action RangeSkillEvent { get { return _rangeSkillEvent; } set { _rangeSkillEvent = value; } }

    //조이스틱, 핸들을 인수로 받음
    public void BindJoyStickEvent(GameObject joyStick, GameObject handle)
    {
        JoyStickHandler joyStickHandler = joyStick.AddComponent<JoyStickHandler>();
        _joyStickHandler = joyStickHandler;
        joyStickHandler.Handle = Util.GetOrAddComponent<RectTransform>(handle);
    }

    //공격버튼
    public void ExecAttackEvent(PointerEventData data)
    {
        if (_attackEvent != null)
        {
            _attackEvent.Invoke();
        }
    }

    public void ExecFlameBallEvent(PointerEventData data)
    {
        if (_flameBallEvent == null)
            return;

        _flameBallEvent.Invoke();
    }

    public void ExecRangeSkillEvent(PointerEventData data)
    {
        if (_rangeSkillEvent == null)
            return;

        _rangeSkillEvent.Invoke();
    }
}
