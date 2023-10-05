using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    //�� �ν� ���� �ð�
    [SerializeField]
    float _updateLockOnInterval = 1.0f;

    //Ÿ�� �� �Լ� ���� �÷���
    bool _onLockTargetFlag = true;

    //���� ����Ʈ
    List<GameObject> targetList = Managers.Game.Units;

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
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel($"{gameObject.name}Stat", 1));

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

        if(Managers.Game.Player.GetComponent<Stat>().Hp == 0)
        {
            State = Define.State.Idle;
            _attackFlag = false;
            _continuedFlag = false;
            return;
        }



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
        _dir.y = 0;
        if (dis < _stat.AttackDistance)
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
    void OnAttack()
    {
        _attackFlag = false;
        GameObject go = _lockTarget;
        //Ÿ���� ��Ȱ��ȭ�Ǿ��� ��� ��ŵ
        if (!(_lockTarget == null || _lockTarget.activeSelf == false))
        {
            _lockTarget.GetComponent<Stat>().OnAttacked(_stat.Offence);
        }


        //���� ��Ÿ��
        StartCoroutine(AttackCoolTime());
    }

    void EndAttack()
    {
        State = Define.State.Idle;
    }

    protected IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(_stat.AttackSpeed);
        _attackFlag = true;
    }

    protected override void UpdateDie()
    {
        _onLockTargetFlag = false;
        _monsterFlag = false;
        base.UpdateDie();
    }

    //������ �ð���ŭ Ÿ�� ����
    protected IEnumerator TargetLockCoroutine()
    {
        OnLockTarget();
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


        foreach (GameObject go in targetList)
        {
            distance = (go.transform.position - transform.position).sqrMagnitude;
            if (minDis > distance)
            {
                minDis = distance;
                _lockTarget = go;
            }
        }
        
        if (State != Define.State.Die && _lockTarget.GetComponent<Stat>().Hp != 0) {
            _onLockTargetFlag = true;
        } 
    }

    protected override void UpdateClear()
    {

        State = Define.State.Clear;
        base.UpdateClear();
    }

    protected override IEnumerator Despwn()
    {
        yield return new WaitForSeconds(Define.DESPAWN_DELAY_TIME);
        Managers.Game.Despawn(Define.Layer.Monster, gameObject);
    }
}

