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

    //Ÿ�� �� �Լ� ���� �÷���
    protected bool _onLockTargetFlag = true;

    protected override void Init()
    {
        SetCreatureDefault();

        //�� �ĺ� �ڷ�ƾ ����
        StartCoroutine(TargetLockCoroutine());
    }

    protected override void UpdateIdle()
    {
        if (_lockTarget == null || _lockTarget.activeSelf == false) return;
        if (_attackFlag == false) return;
        State = Define.State.Moving;
    }

    protected override void UpdateAttack()
    {
        //Ÿ���� ���� ��� ������ ����
        if (_lockTarget == null || _lockTarget.activeSelf == false)
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

    protected override void UpdateDie() 
    {
        if (_aliveFlag)
        {
            _aliveFlag = false;
            StartCoroutine(Despawn());
        }
    }

    //����� �����ð� �� ��Ȱ��ȭ
    protected IEnumerator Despawn()
    {
        yield return new WaitForSeconds(Define.DESPAWN_DELAY_TIME);
        Managers.Game.Despawn(Layer(), gameObject);
    }

    //������ �ð���ŭ Ÿ�� ����
    protected IEnumerator TargetLockCoroutine()
    {
        while (true)
        {
            // Ÿ�� ������ ������ ��츸 ����
            if (_onLockTargetFlag)
            {
                OnLockTarget();
            }
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
        {
            return;
        }
        else
        {
            foreach (GameObject go in targetList)
            {
                distance = (go.transform.position - transform.position).sqrMagnitude;
                if (minDis <= distance) continue;

                minDis = distance;
                _lockTarget = go;
            }
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

    // ���� Ÿ�� 
    protected abstract GameObject MainTarget();
    // ���� �̿��� ��� Ÿ�ٵ�
    protected abstract List<GameObject> Targets();
    // ������Ʈ Layer
    protected abstract int Layer();
}
