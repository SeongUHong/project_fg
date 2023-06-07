using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    //�� �ν� ���� �ð�
    [SerializeField]
    float _updateLockOnInterval = 2.0f;

    //Ÿ�� �� �Լ� ���� �÷���
    bool _onLockTargetFlag = true;

    public override void Init()
    {
        //���� �ʱ�ȭ
        State = Define.State.Idle;

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

        //���� �߰�
        _stat = transform.GetComponent<Stat>();

        //HP�� �߰�
        if (gameObject.GetComponentInParent<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        //�� �ĺ� �ڷ�ƾ ����
        StartCoroutine(TargetLockCoroutine());
    }

    protected override void UpdateAlways()
    {
        base.UpdateAlways();

        //Ÿ�� ���� �Լ��� ����
        if (_onLockTargetFlag)
        {
            StartCoroutine(TargetLockCoroutine());
        }
    }

    protected override void UpdateMoving()
    {
        //���µ� Ÿ�ٰ��� �Ÿ� ���
        if (_lockTarget == null || _lockTarget.activeSelf == false)
        {
            State = Define.State.Idle;
            return;
        }

        _destPos = _lockTarget.transform.position - transform.position;
        float dis = _destPos.magnitude;
        _dir = _destPos.normalized;
        if(dis < _stat.AttackDistance)
        {
            State = Define.State.Attack;
            return;
        }
        transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;
        if (_dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
    }

    protected override void UpdateIdle()
    {
        if (_lockTarget == null || _lockTarget.activeSelf == false) return;
        State = Define.State.Moving;
    }

    protected override void UpdateAttack()
    {
        return;
    }

    protected override void UpdateDie()
    {
        _onLockTargetFlag = false;
    }

    //������ �ð���ŭ Ÿ�� ����
    protected IEnumerator TargetLockCoroutine()
    {
        OnLockTarget();
        _onLockTargetFlag = false;
        yield return new WaitForSeconds(_updateLockOnInterval);
    }

    //Ÿ�� ���� �Լ�
    protected void OnLockTarget()
    {
        float minDis;
        float distance;

        //�⺻ Ÿ������ �÷��̾ ����
        if (Managers.Game.Player == null) return;
        _lockTarget = Managers.Game.Player;
        minDis = (Managers.Game.Player.transform.position - transform.position).sqrMagnitude;

        //�÷��̾� ���� ��� ĳ���Ϳ� �Ÿ��� ���ϰ� ���� ����� ĳ���͸� Ÿ������ ����
        List<GameObject> targetList = Managers.Game.Monsters;
        if (targetList.Count == 0)
        {
            return;
        }
        else
        {
            foreach (GameObject go in targetList)
            {
                distance = (go.transform.position - transform.position).sqrMagnitude;
                if (minDis > distance)
                {
                    minDis = distance;
                    _lockTarget = go;
                }
            }
        }
        if (State != Define.State.Die) _onLockTargetFlag = true;
    }
}

