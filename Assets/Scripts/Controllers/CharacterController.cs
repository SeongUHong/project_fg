using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : BaseController
{
    //���µ� ������Ʈ
    [SerializeField]
    protected GameObject _lockTarget;

    //�� �ν� ���� �ð�
    [SerializeField]
    protected float _updateLockOnInterval = 2.0f;


    protected override void Init()
    {
        SetCreatureDefault();

        //�� �ĺ� �ڷ�ƾ ����
        StartCoroutine(TargetLockCoroutine());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(TargetLockCoroutine());
    }

    protected override void UpdateIdle()
    {
        if (!CanAttackTarget())
            return;

        if (_attackFlag == false)
            return;

        State = Define.State.Moving;
    }

    protected override void UpdateAttack()
    {
        //Ÿ���� ���� ��� ������ ����
        if (!CanAttackTarget())
        {
            State = Define.State.Idle;
            return;
        }

        if ((_lockTarget.transform.position - transform.position).magnitude > _stat.AttackDistance)
        {
            State = Define.State.Moving;
            return;
        }
    }

    //������ �ð���ŭ Ÿ�� ����
    protected IEnumerator TargetLockCoroutine()
    {
        while (true)
        {
            OnLockTarget();
            yield return new WaitForSeconds(_updateLockOnInterval);
        }
    }

    // Ÿ�� ���� ó��
    protected void OnLockTarget()
    {
        float minDis;
        float distance;

        // �⺻ Ÿ������ ũ����Ż�� ����
        GameObject mainTarget = MainTarget();
        if (mainTarget == null) return;

        _lockTarget = mainTarget;
        minDis = (_lockTarget.transform.position - transform.position).sqrMagnitude;

        // �÷��̾� ���� ��� ĳ���Ϳ� �Ÿ��� ���ϰ� ���� ����� ĳ���͸� Ÿ������ ����
        List<GameObject> targetList = Targets();
        if (targetList == null || targetList.Count <= 0)
            return;

        foreach (GameObject go in targetList)
        {
            if (go.GetComponent<Stat>().IsDefeated()) 
                continue;

            distance = (go.transform.position - transform.position).sqrMagnitude;
            if (minDis <= distance) continue;

            minDis = distance;
            _lockTarget = go;
        }
    }

    protected override void UpdateMoving()
    {
        //Ÿ���� ���� ��� ������ ����
        if (_lockTarget == null || _lockTarget.activeSelf == false)
        {
            State = Define.State.Idle;
            return;
        }

        //���µ� Ÿ�ٰ��� �Ÿ� ���
        _destPos = _lockTarget.transform.position - transform.position;
        float dis = _destPos.magnitude;
        _dir = _destPos.normalized;
        if (dis < _stat.AttackDistance)
        {
            State = Define.State.Attack;
            return;
        }

        // �̵� ó��
        transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;

        // ȸ�� ó��
        if (_dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
    }

    // ���� ó��
    // �ִϸ��̼ǿ��� ȣ��Ǵ� ��찡 ����
    void OnAttack()
    {
        _attackFlag = false;
        //Ÿ���� ��Ȱ��ȭ�Ǿ��� ��� ��ŵ
        if (!(_lockTarget == null || _lockTarget.activeSelf == false))
        {
            _lockTarget.GetComponent<Stat>().OnAttacked(_stat.Offence);
        }

        //���� ��Ÿ��
        StartCoroutine(AttackCoolTime());
    }

    public override void OnDie()
    {
        if (!_aliveFlag)
            return;

        _aliveFlag = false;
        State = Define.State.Die;

        // ��� �Ŀ��� ���� ĳ���Ϳ� ���ذ� ���� �ʵ��� �ݶ��̴��� ����
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        // ��� ó��
        Managers.Game.RemoveFromSpawnList(gameObject);
        StartCoroutine(Despawn());
    }

    //����� �����ð� �� ��Ȱ��ȭ
    protected IEnumerator Despawn()
    {
        yield return new WaitForSeconds(Define.DESPAWN_DELAY_TIME);
        Managers.Resource.Destroy(gameObject);
    }

    // Ÿ���� ������ �� �ִ°�
    protected bool CanAttackTarget()
    {
        if (_lockTarget == null)
            return false;

        if (_lockTarget.activeSelf == false)
            return false;

        // HP�� ���� ���
        if (_lockTarget.GetComponent<Stat>().IsDefeated())
            return false;

        return true;
    }

    // ���� Ÿ�� 
    protected abstract GameObject MainTarget();
    // ���� �̿��� ��� Ÿ�ٵ�
    protected abstract List<GameObject> Targets();
}
