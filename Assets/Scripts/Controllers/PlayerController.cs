using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    //��ų �߻� ����
    Transform _launchPoint;

    //��ų �̸�
    string SKILL_NAME = "PlayerAttack";

    protected override void Init()
    {
        SetCreatureDefault();

        //��ų �߻� ����
        _launchPoint = Util.FindChild<Transform>(gameObject, "LaunchPoint", true);

        //��ư �׼� �߰�
        AddAction();
    }

    protected override string DieAnimName()
    {
        return $"Die{Random.Range(1, 5)}";
    }

    //Invoke�� ����� �� �ְ� ���� ��ư�� �׼� �߰�
    void AddAction()
    {
        Managers.Input.JoyStickHandler.OnDragHandler -= OnJoyStickDragEvent;
        Managers.Input.JoyStickHandler.OnDragHandler += OnJoyStickDragEvent;
        Managers.Input.JoyStickHandler.OnUpHandler -= OnJoyStickUpEvent;
        Managers.Input.JoyStickHandler.OnUpHandler += OnJoyStickUpEvent;
        Managers.Input.AttackEvent -= OnAttackBtnDownEvent;
        Managers.Input.AttackEvent += OnAttackBtnDownEvent;
    }

    //���̽�ƽ�� ������ ���ڷ� ����
    void OnJoyStickDragEvent(Vector3 diretion)
    {
        if (State != Define.State.Moving)
        {
            State = Define.State.Moving;
        }
        _dir = diretion;
    }

    void OnJoyStickUpEvent()
    {
        State = Define.State.Idle;
    }

    //���ݹ�ư Ŭ����
    void OnAttackBtnDownEvent()
    {
        if (_attackFlag)
        {
            State = Define.State.Attack;
            _attackFlag = false;
            StartCoroutine(AttackCoolTime());
        }
    }

    void OnAttack()
    {
        Managers.Skill.SpawnLaunchSkill(
            SKILL_NAME,
            _launchPoint.position,
            _dir,
            _stat.AttackDistance,
            _stat.ProjectileSpeed,
            _stat.Offence,
            new int[] { (int)Define.Layer.Monster, (int)Define.Layer.EnemyStaticObject });
    }

    protected override void UpdateMoving()
    {
            transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;
            if (_dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
    }

    public override void OnDie()
    {
        if (!_aliveFlag)
            return;

        State = Define.State.Die;
        _aliveFlag = false;

        // ��� �Ŀ��� ���� ĳ���Ϳ� ���ذ� ���� �ʵ��� �ݶ��̴��� ����
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        // ���ӿ��� �ǳ� Ȱ��
        Managers.Game.Gameover();
    }

}
