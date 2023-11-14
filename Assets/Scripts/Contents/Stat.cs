using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _offence;
    [SerializeField]
    protected int _defence;
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected float _attackDistance;
    [SerializeField]
    protected float _attackSpeed;
    [SerializeField]
    protected float _projectileSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Offence { get { return _offence; } set { _offence = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float AttackDistance { get { return _attackDistance; } set { _attackDistance = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public float ProjectileSpeed { get { return _projectileSpeed; } set { _projectileSpeed = value; } }

    public void SetStat(data.Stat stat)
    {
        _level = stat.level;
        _hp = stat.hp;
        _maxHp = stat.hp;
        _offence = stat.offence;
        _defence = stat.defence;
        _moveSpeed = stat.move_speed;
        _attackDistance = stat.attack_distance;
        _attackSpeed = stat.attack_speed;
        _projectileSpeed = stat.projectile_speed;
    }

    public virtual bool OnAttacked(int pureDamage)
    {
        int damage = Mathf.Max(0, pureDamage - Defence);
        if (Hp <= 0) return false;
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }


        return true;
    }

    protected virtual void OnDead()
    {
        gameObject.GetComponent<BaseController>().State = Define.State.Die;
    }
}
