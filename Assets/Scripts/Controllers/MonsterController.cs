using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    //적 인식 갱신 시간
    [SerializeField]
    float _updateLockOnInterval = 1.0f;

    //타겟 락 함수 실행 플래그
    bool _onLockTargetFlag = true;

    //유닛 리스트
    List<GameObject> targetList = Managers.Game.Units;

    public override void Init()
    {
        //상태 초기화
        State = Define.State.Idle;

        //애니메이터
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Can't Load Animator Component");
        }

        //리지드바디
        _rig = gameObject.GetComponent<Rigidbody>();
        if (_anim == null)
        {
            Debug.Log("Can't Load Rigidbody Component");
        }

        //스텟 추가
        _stat = transform.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel($"{gameObject.name}Stat", 1));

        //HP바 추가
        if (gameObject.GetComponentInParent<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        //적 식별 코루틴 실행
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



        //타겟 락온 함수를 실행
        if (_onLockTargetFlag)
        {
            StartCoroutine(TargetLockCoroutine());
        }
    }

    protected override void UpdateMoving()
    {
        //락온된 타겟과의 거리 계산
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
        //타겟이 없을 경우 움직임 멈춤
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
        //타겟이 비활성화되었을 경우 스킵
        if (!(_lockTarget == null || _lockTarget.activeSelf == false))
        {
            _lockTarget.GetComponent<Stat>().OnAttacked(_stat.Offence);
        }


        //공격 쿨타임
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

    //지정된 시간만큼 타겟 갱신
    protected IEnumerator TargetLockCoroutine()
    {
        OnLockTarget();
        yield return new WaitForSeconds(_updateLockOnInterval);
    }

    //타겟 갱신 함수
    protected void OnLockTarget()
    {
        float minDis;
        float distance;

        //기본 타겟으로 플레이어를 지정
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

