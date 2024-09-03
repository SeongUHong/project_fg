using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseController
{
    //플레이푳E컨트롤러 UI가 들어있는 컴포넌트
    UIScene _uiScene;

    //스킬 발사 지점
    Transform _launchPoint;

    //공격대상 레이어
    int[] _layers;

    //스킬 이름
    string SKILL_NAME = "PlayerAttack";

    //게임오버 판넬초기화
    Panel_GameOver panel_GameOver;

    public override void Init()
    {
        //상태 초기화
        State = Define.State.Idle;

        //클리어 플래그초기화
        Conf.Main.CLEAR_FLAG = false;

        //스텟 초기화
        _stat = gameObject.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel("PlayerStat", 1));

        //애니메이터
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Can't Load Animator Component");
        }

        //리지드바디
        _rig = gameObject.GetComponent<Rigidbody>();
        if (_rig == null)
        {
            Debug.Log("Can't Load Rigidbody Component");
        }

        //컨트롤러UI 초기화
        _uiScene = Managers.UI.UIScene;

        if (_uiScene == null || _uiScene.JoyStickHandler == null)
        {
            Debug.Log("Not Exist Player Controller UI");
        }

        //HP바 추가
        if (gameObject.GetComponentInChildren<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        panel_GameOver = Managers.Game.Panel;
        //게임오버 판넬추가
        if (panel_GameOver == null)
        {
            Debug.Log("Not Exist GameOver Panel");
        }


        //스킬 발사 지점
        _launchPoint = Util.FindChild<Transform>(gameObject, "LaunchPoint", true);

        //공격대상 레이어 지정
        _layers = new int[]{ (int)Define.Layer.Monster, (int)Define.Layer.EnemyStaticObject};

        //버튼 액션 추가
        AddAction();
    }

    //Invoke로 사용할 수 있게 각종 버튼에 액션 추가
    void AddAction()
    {
        _uiScene.JoyStickHandler.OnDragHandler -= OnJoyStickDragEvent;
        _uiScene.JoyStickHandler.OnDragHandler += OnJoyStickDragEvent;
        _uiScene.JoyStickHandler.OnUpHandler -= OnJoyStickUpEvent;
        _uiScene.JoyStickHandler.OnUpHandler += OnJoyStickUpEvent;
        _uiScene.OnAttackBtnDownHandler -= OnAttackBtnDownEvent;
        _uiScene.OnAttackBtnDownHandler += OnAttackBtnDownEvent;
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
            State = Define.State.Attack;
            _attackFlag = false;
            StartCoroutine(AttackCoolTime());
        }

    }

    //공격 쿨타임 동안 공격 플레그를 false
    protected IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(_stat.AttackSpeed);
        _attackFlag = true;
    }

    void OnAttack()
    {
        Managers.Skill.SpawnSkill(SKILL_NAME, _launchPoint.position, _dir, _stat.AttackDistance, _stat.ProjectileSpeed, _stat.Offence, _layers);
    }

    void EndAttack()
    {
        State = Define.State.Idle;
    }

    protected override void UpdateMoving()
    {
        transform.position += _dir * Time.deltaTime * _stat.MoveSpeed;
        _dir.y = 0;
        if (_dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), 10 * Time.deltaTime);

    }

    protected override void UpdateDie()
    {
        _aliveFlag = false;
        StartCoroutine(Despwn());
        panel_GameOver.Show();
    }

    protected override void UpdateClear()
    {
        State = Define.State.Clear;
        base.UpdateClear();
    }

    protected override IEnumerator Despwn()
    {
        yield return new WaitForSeconds(Define.DESPAWN_DELAY_TIME);
        Managers.Game.Despawn(Define.Layer.Player, gameObject);
    }

    public void Stop()
    {
        StopAllCoroutines();
    }
}
