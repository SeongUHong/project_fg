using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : BaseController
{
    //����
    [SerializeField]
    Stat _stat;

    //�� �ν� ���� �ð�
    [SerializeField]
    float _updateLockOnInterval = 2.0f;

    //Ÿ�� �� �Լ� ���� �÷���
    bool _onLockTargetFlag = false;

    public override void Init()
    {
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
        //Ÿ�� ���� �Լ��� ����
        if (_onLockTargetFlag == false)
        {
            _onLockTargetFlag = true;
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

        Vector3 lockTargetPos = _lockTarget.transform.position;
        lockTargetPos.y = transform.position.y;
        _destPos = lockTargetPos - transform.position;

    }

    protected override void UpdateIdle()
    {
        if (_lockTarget == null || _lockTarget.activeSelf == false) return;
        State = Define.State.Moving;
    }

    protected override void UpdateAttack()
    {
        State = Define.State.Idle;
    }

    protected override void UpdateDie()
    {
        _onLockTargetFlag = false;
    }

    //������ �ð���ŭ Ÿ�� ����
    protected IEnumerator TargetLockCoroutine()
    {
        OnLockTarget();
        yield return new WaitForSeconds(_updateLockOnInterval);
        if (_lockTarget != null || _lockTarget.activeSelf)
        {
            Debug.Log($"Locked Target:{_lockTarget.name}");
        }
        _onLockTargetFlag = false;
    }

    //Ÿ�� ���� �Լ�
    protected void OnLockTarget()
    {
        float minDis;
        float distance;

        //�⺻ Ÿ������ �÷��̾ ����
        if (Managers.Game.EnemyCore == null) return;
        _lockTarget = Managers.Game.EnemyCore;
        minDis = (Managers.Game.EnemyCore.transform.position - transform.position).sqrMagnitude;

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
    }
}

