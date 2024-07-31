using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterController : BaseController
{
    //���µ� ������Ʈ
    [SerializeField]
    protected GameObject _lockTarget;

    //�� �ν� ���� �ð�
    [SerializeField]
    protected float _updateLockOnInterval = CharacterConf.UPDATE_LOCK_ON_INTERVAL;

    NavMeshAgent _nav;

    protected override void Init()
    {
        SetCreatureDefault();

        // NavMeshAgent ����
        _nav = gameObject.GetComponent<NavMeshAgent>();
        if (_nav == null)
        {
            Debug.Log("Can't Load NavMeshAgent Component");
        }
        // �̵� �ӵ�
        _nav.speed = _stat.MoveSpeed;
        // �̵��� ���ߴ� �Ÿ�
        _nav.stoppingDistance = _stat.AttackDistance;
        // �и� ����
        UpdateAvoidancePriority(CharacterConf.AvoidancePriority.Stop);

        //�� �ĺ� �ڷ�ƾ ����
        StartCoroutine(TargetLockCoroutine());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // navȰ��
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        StartCoroutine(TargetLockCoroutine());
    }

    protected override void UpdateIdle()
    {
        StopMoving();

        if (!CanAttackTarget())
            return;

        if (_attackFlag == false)
            return;

        State = Define.State.Moving;
    }

    protected override void UpdateAttack()
    {
        StopMoving();

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
        // Ÿ���� ���� ��� ������ ����
        if (!CanAttackTarget())
        {
            State = Define.State.Idle;
            return;
        }

        // ���µ� Ÿ�ٰ��� �Ÿ�
        Vector3 destPos = _lockTarget.transform.position;
        // ���� ����
        Vector3 toTarget = destPos - transform.position;
        // �̵����� �ʾƵ� ��ǥ�� ���� ������ �����ص�
        _dir = toTarget.normalized;

        // ���� ������ �Ÿ��� ���
        if (toTarget.magnitude < _stat.AttackDistance)
        {
            State = Define.State.Attack;
            return;
        }

        // �̵� ó��
        UpdateAvoidancePriority(CharacterConf.AvoidancePriority.Move);
        _nav.isStopped = false;
        _nav.SetDestination(destPos);

        if (_dir == Vector3.zero)
            return;

        // ȸ�� ó��
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
    }

    // ���� ó��
    // �ִϸ��̼ǿ��� ȣ��Ǵ� ��찡 ����
    void OnAttack()
    {
        // Ÿ���� ��Ȱ��ȭ�Ǿ��� ��� ��ŵ
        if (!CanAttackTarget())
            return;

        _lockTarget.GetComponent<Stat>().OnAttacked(_stat.Offence);

        _attackFlag = false;

        // ���� ��Ÿ��
        StartCoroutine(AttackCoolTime());
    }

    public override void OnDie()
    {
        if (!_aliveFlag)
            return;

        _aliveFlag = false;
        State = Define.State.Die;

        // navMeshAgent ��Ȱ��
        StopMoving();
        _nav.enabled = false;

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

    // navMeshAgent�� avoidancePriority����
    protected void UpdateAvoidancePriority(CharacterConf.AvoidancePriority priority)
    {
        if (_nav.avoidancePriority == (int)priority)
            return;

        _nav.avoidancePriority = (int)priority;
    }

    // �̵��� ����
    protected void StopMoving()
    {
        UpdateAvoidancePriority(CharacterConf.AvoidancePriority.Stop);

        if (_nav.isStopped == true)
            return;

        _nav.isStopped = true;
    }

    // ���� Ÿ�� 
    protected abstract GameObject MainTarget();
    // ���� �̿��� ��� Ÿ�ٵ�
    protected abstract List<GameObject> Targets();
}
