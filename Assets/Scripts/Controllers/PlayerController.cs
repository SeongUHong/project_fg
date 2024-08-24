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
    bool _rangeSkillFlag = true;

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
        data.Skill skill = Managers.Data.GetSKillBySkillIdAndLevel((int)SkillConf.Skill.FlameBall, 1);

        Managers.Skill.SpawnLaunchSkill(
            SkillConf.LaunchSkill.FlameBall,
            _launchPoint.position,
            _dir,
            skill.distance,
            skill.move_speed,
            skill.offence,
            new Define.Layer[] { Define.Layer.Monster, Define.Layer.EnemyStaticObject });

        _flameBallFlag = false;
        StartCoroutine(FlameBallCoolTime(skill.cooltime));
    }

    void RangeSkillEvent()
    {
        data.Skill skill = Managers.Data.GetSKillBySkillIdAndLevel((int)SkillConf.Skill.DummyRange, 1);

        Managers.Skill.SpawnRnageSkill(
            SkillConf.RangeSkill.DummyRange,
            gameObject,
            skill.active_time,
            skill.tick_interval,
            skill.offence,
            new Define.Layer[] { Define.Layer.Monster });

        _rangeSkillFlag = false;
        StartCoroutine(RangeSkillCoolTime(skill.cooltime));
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

    IEnumerator RangeSkillCoolTime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        _rangeSkillFlag = true;
    }

    IEnumerator FlameBallCoolTime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        _flameBallFlag = true;
    }
}
