using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    //스킬 발사 지점
    Transform _launchPoint;
    Action _activeSkillEvent;

    // スキルクールタイムフラグ
    bool _flameBallFlag = true;
    [SerializeField]
    float _flameBallCoolTime = SkillConf.FLAME_BALL_COOLTIME;

    bool _rangeSkillFlag = true;
    [SerializeField]
    float _rangeSkillCoolTime = SkillConf.DUMMY_RANGE_COOLTIME;

    protected override void Init()
    {
        SetCreatureDefault();

        //스킬 발사 지점
        _launchPoint = Util.FindChild<Transform>(gameObject, "LaunchPoint", true);

        //버튼 액션 추가
        AddAction();
    }

    protected override string DieAnimName()
    {
        return $"Die{UnityEngine.Random.Range(1, 5)}";
    }

    //Invoke로 사용할 수 있게 각종 버튼에 액션 추가
    void AddAction()
    {
        Managers.Input.JoyStickHandler.OnDragHandler -= OnJoyStickDragEvent;
        Managers.Input.JoyStickHandler.OnDragHandler += OnJoyStickDragEvent;
        Managers.Input.JoyStickHandler.OnUpHandler -= OnJoyStickUpEvent;
        Managers.Input.JoyStickHandler.OnUpHandler += OnJoyStickUpEvent;
        Managers.Input.AttackEvent -= OnAttackBtnDownEvent;
        Managers.Input.AttackEvent += OnAttackBtnDownEvent;
        Managers.Input.FlameBallEvent -= OnFlameBallSkillBtnDownEvent;
        Managers.Input.FlameBallEvent += OnFlameBallSkillBtnDownEvent;
        Managers.Input.RangeSkillEvent -= OnRangeSkillBtnDownEvent;
        Managers.Input.RangeSkillEvent += OnRangeSkillBtnDownEvent;
    }

    //조이스틱의 방향을 인자로 받음
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

    //공격버튼 클릭시
    void OnAttackBtnDownEvent()
    {
        if (_attackFlag)
        {
            _activeSkillEvent = AttackEvent;
            State = Define.State.Attack;
        }
    }

    void OnFlameBallSkillBtnDownEvent()
    {
        if (_flameBallFlag)
        {
            _activeSkillEvent = FlameBallSkillEvent;
            State = Define.State.Attack;
        }
    }

    void OnRangeSkillBtnDownEvent()
    {
        if (_rangeSkillFlag)
        {
            _activeSkillEvent = RangeSkillEvent;
            State = Define.State.Attack;
        }
    }

    void OnAttack()
    {
        _activeSkillEvent.Invoke();
    }

    void AttackEvent()
    {
        Managers.Skill.SpawnLaunchSkill(
            SkillConf.LaunchSkill.Attack,
            _launchPoint.position,
            _dir,
            _stat.AttackDistance,
            _stat.ProjectileSpeed,
            _stat.Offence,
            new Define.Layer[] { Define.Layer.Monster, Define.Layer.EnemyStaticObject });

        _attackFlag = false;
        StartCoroutine(AttackCoolTime());
    }

    void FlameBallSkillEvent()
    {
        Managers.Skill.SpawnLaunchSkill(
            SkillConf.LaunchSkill.FlameBall,
            _launchPoint.position,
            _dir,
            _stat.AttackDistance,
            _stat.ProjectileSpeed,
            _stat.Offence,
            new Define.Layer[] { Define.Layer.Monster, Define.Layer.EnemyStaticObject });

        _flameBallFlag = false;
        StartCoroutine(AttackCoolTime());
    }

    void RangeSkillEvent()
    {
        Managers.Skill.SpawnRnageSkill(
            SkillConf.RangeSkill.DummyRange,
            gameObject,
            SkillConf.DUMMY_RANGE_ACTIVE_TIME,
            SkillConf.DUMMY_RANGE_DAMAGE_TICK_INTERVAL,
            8,
            new Define.Layer[] { Define.Layer.Monster });

        _rangeSkillFlag = false;
        StartCoroutine(RangeSkillCoolTime());
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

        // 사망 후에는 뒤의 캐릭터에 방해가 되지 않도록 콜라이더를 해제
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        // 게임오버 판넬 활성
        Managers.Game.Gameover();
    }

    IEnumerator RangeSkillCoolTime()
    {
        yield return new WaitForSeconds(_rangeSkillCoolTime);
        _rangeSkillFlag = true;
    }

}
