using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    //현재 상태
    [SerializeField]
    private Define.State _state = Define.State.Idle;

    //방향
    protected Vector3 _dir = Vector3.forward;
    //리지드바디
    protected Rigidbody _rig;
    //애니메이터
    protected Animator _anim;
    //스텟
    protected Stat _stat;
    // 쿨타임 플래그
    protected bool _attackFlag = true;
    // 생존 플래그
    protected bool _aliveFlag = true;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            // 생존 플래그가 false면 Die이외의 상태로 변경 불가능
            if (!_aliveFlag && value != Define.State.Die) return;

            _state = value;

            // 애니메이터가 없다면 애니메이션 재생 스킵
            if (_anim == null) return;

            switch (_state)
            {
                //애니메이션 재생
                case Define.State.Idle:
                    _anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.Die:
                    _anim.CrossFade(DieAnimName(), 0.5f);
                    break;
                case Define.State.Attack:
                    _anim.CrossFade("Attack", 0.1f);
                    break;
                case Define.State.Moving:
                    _anim.CrossFade("Walk", 0.1f);
                    break;
                case Define.State.Skill:
                    break;
            }
        }
    }

    // ================================
    // 애니메이션 관련
    // ================================
    // Die 상태의 애니메이션명
    protected virtual string DieAnimName() { return "Die"; }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;

        }
        UpdateAlways();
    }

    // 애니메이션이 있는 오브젝트 디폴트 설정
    protected virtual void SetCreatureDefault()
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
        _stat.SetStat(Managers.Data.GetStatByLevel($"{gameObject.name}Stat", GetLevel()));

        //HP바 추가
        if (gameObject.GetComponentInParent<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }
    }


    protected virtual void UpdateAlways() 
    {
        _rig.velocity = Vector3.zero; 
    }

    // 쿨타임 경과후 공격가능 플래그 활성
    protected IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(_stat.AttackSpeed);
        _attackFlag = true;
    }

    // 공격 후의 처리
    // 애니메이션에서 호출되는 경우가 있음
    void EndAttack()
    {
        State = Define.State.Idle;
    }

    // 재활성 되었을 때의 처리
    protected virtual void OnEnable()
    {
        _aliveFlag = true;
        _attackFlag = true;

        // 상태 초기화
        State = Define.State.Idle;

        // 체력회복
        if (_stat != null)
        {
            _stat.Hp = _stat.MaxHp;
        }

        // 콜라이더 활성
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    public virtual void OnDie()
    {
        if (!_aliveFlag)
            return;

        SetDieState();

        // 사망 처리
        Managers.Game.Despawn(gameObject);
    }

    public virtual void SetDieState()
    {
        State = Define.State.Die;
        _aliveFlag = false;

        // 사망 후에는 뒤의 캐릭터에 방해가 되지 않도록 콜라이더를 해제
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    // ====================================
    // 추상 메서드
    // ====================================
    protected abstract void Init();
    // 레벨을 반환
    protected abstract int GetLevel();

    // ====================================
    // 필수는 아니지만
    // 오버라이드 해야만 사용가능한 메서드
    // ====================================
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDie() { }
}
