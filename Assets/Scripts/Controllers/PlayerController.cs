using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    //�÷��̾�E��Ʈ�ѷ� UI�� ����ִ� ������Ʈ
    UIScene _uiScene;

    //���̽�ƽ ����
    Vector3 _dir;

    //�÷��̾� ����
    Stat _stat;

    Rigidbody _rig;

    public override void Init()
    {
        //���� �ʱ�ȭ
        _state = Define.State.Idle;

        //���� �ʱ�ȭ
        _stat = gameObject.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }

        //�ִϸ�����
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Can't Load Animator Component");
        }

        //������ٵ�
        _rig = gameObject.GetComponent<Rigidbody>();
        if (_anim == null)
        {
            Debug.Log("Can't Load Rigidbody Component");
        }

        //��Ʈ�ѷ�UI �ʱ�ȭ
        _uiScene = Managers.UI.UIScene;

        if(_uiScene == null || _uiScene.JoyStickHandler == null)
        {
            Debug.Log("Not Exist Player Controller UI");
        }

        //HP�� �߰�
        if (gameObject.GetComponentInChildren<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        //��ư �׼� �߰�
        AddAction();
    }

    //Invoke�� ����� �� �ְ� ���� ��ư�� �׼� �߰�
    void AddAction()
    {
        _uiScene.JoyStickHandler.OnDragHandler -= OnJoyStickDragEvent;
        _uiScene.JoyStickHandler.OnDragHandler += OnJoyStickDragEvent;
        _uiScene.JoyStickHandler.OnUpHandler -= OnJoyStickUpEvent;
        _uiScene.JoyStickHandler.OnUpHandler += OnJoyStickUpEvent;
    }

    //���̽�ƽ�� ������ ���ڷ� ����
    void OnJoyStickDragEvent(Vector3 diretion)
    {
        
        State = Define.State.Moving;       
        _dir = diretion;
    }

    void OnJoyStickUpEvent()
    {
        State = Define.State.Idle;
    }

    protected override void UpdateMoving()
    {
        transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;
        if (_dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
    }

    protected override void UpdateAlways()
    {
        _rig.velocity = Vector3.zero;
    }
}
