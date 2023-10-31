using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : BaseController
{
    //적 인식 갱신 시간
    [SerializeField]
    float UpdateLockOnInterval = 2.0f;

    //타겟 락 함수 실행 플래그
    bool _onLockTargetFlag = true;

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

        //플레이어 스테이트초기화
        //_playerState = Managers.Game.Player.GetComponent<PlayerController>().State;
        //적 식별 코루틴 실행
        StartCoroutine(TargetLockCoroutine());


    }


    protected override void UpdateAlways()
    {
        base.UpdateAlways();


        //플레이어가 죽으면 정지
        if (Managers.Game.Player.GetComponent<PlayerController>().State == Define.State.Die)
            UpdateDie();

        //타겟 락온 함수를 실행
        if (_onLockTargetFlag)
        {
            StartCoroutine(TargetLockCoroutine());
        }
    }

    protected override void UpdateIdle()
    {
        if (_lockTarget == null || _lockTarget.activeSelf == false) return;
        if (_attackFlag == false) return;
        State = Define.State.Moving;
    }

    protected override void UpdateMoving()
    {
        //타겟이 없을 경우 움직임 멈춤
        if (_lockTarget == null || _lockTarget.activeSelf == false)
        {
            State = Define.State.Idle;
            return;
        }

        //락온된 타겟과의 거리 계산
        _destPos = _lockTarget.transform.position - transform.position;
        float dis = _destPos.magnitude;
        _dir = _destPos.normalized;
        if(dis < _stat.AttackDistance)
        {
            
            //_stat.OnAttacked(500);
            //return;
            State = Define.State.Attack;
            return;
        }

        transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;
        if (_dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);
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

    protected override void UpdateDie()
    {
        _onLockTargetFlag = false;
        _unitFlag = false;
        StartCoroutine(Despwn());
        //base.UpdateDie();
    }

    void OnAttack()
    {
        _attackFlag = false;
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

    //지정된 시간만큼 타겟 갱신
    protected IEnumerator TargetLockCoroutine()
    {
        OnLockTarget();
        yield return new WaitForSeconds(UpdateLockOnInterval);
    }

    //타겟 갱신 함수
    protected void OnLockTarget()
    {
        float minDis;
        float distance;

        //기본 타겟으로 크리스탈을 지정
        if (Managers.Game.MonsterCrystal == null) return;
        _lockTarget = Managers.Game.MonsterCrystal;
        minDis = (Managers.Game.MonsterCrystal.transform.position - transform.position).sqrMagnitude;

        //플레이어 포함 모든 캐릭터와 거리를 비교하고 가장 가까운 캐릭터를 타겟으로 지정
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

    protected override void UpdateClear()
    {
        State = Define.State.Clear;
        base.UpdateClear();
    }

    protected override IEnumerator Despwn()
    {
        yield return new WaitForSeconds(Define.DESPAWN_DELAY_TIME);
        Managers.Game.Despawn(Define.Layer.Unit, gameObject);
    }
}

