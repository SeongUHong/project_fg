using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    //���� ����
    [SerializeField]
    private Define.State _state = Define.State.Idle;

    //����
    protected Vector3 _dir = Vector3.forward;
    //������ٵ�
    protected Rigidbody _rig;
    //�ִϸ�����
    protected Animator _anim;
    //����
    protected Stat _stat;
    //������
    protected Vector3 _destPos;
    // ��Ÿ�� �÷���
    protected bool _attackFlag = true;
    // ���� �÷���
    protected bool _aliveFlag = true;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            if (_anim == null) return;

            switch (_state)
            {
                //�ִϸ��̼� ���
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
    // �ִϸ��̼� ����
    // ================================
    // Die ������ �ִϸ��̼Ǹ�
    protected virtual string DieAnimName() { return "Die"; }

    private void Start()
    {
        Init();
    }

    void FixedUpdate()
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

    // �ִϸ��̼��� �ִ� ������Ʈ ����Ʈ ����
    protected void SetCreatureDefault()
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
    }

    protected virtual void UpdateAlways() 
    {
        _rig.velocity = Vector3.zero; 
    }

    protected virtual void UpdateDie() 
    {
        if (_aliveFlag)
        {
            _aliveFlag = false;
            Managers.Game.Despawn((int)Define.Layer.Unit, gameObject);
        }
    }

    // ��Ÿ�� ����� ���ݰ��� �÷��� Ȱ��
    protected IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(_stat.AttackSpeed);
        _attackFlag = true;
    }

    // ���� ���� ó��
    // �ִϸ��̼ǿ��� ȣ��Ǵ� ��찡 ����
    void EndAttack()
    {
        State = Define.State.Idle;
    }

    protected virtual void OnEnable()
    {
        //���� �ʱ�ȭ
        State = Define.State.Idle;

        //ü��ȸ��
        if (_stat != null)
        {
            _stat.Hp = _stat.MaxHp;
        }

        _aliveFlag = true;
    }

    public virtual void OnDie()
    {
        State = Define.State.Die;
    }

    // ====================================
    // �߻� �޼���
    // ====================================
    protected abstract void Init();

    // ====================================
    // �ʼ��� �ƴ�����
    // �������̵� �ؾ߸� ��밡���� �޼���
    // ====================================
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
}
