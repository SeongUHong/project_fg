using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderMonsterController : MonsterController
{
    // スキル発射元
    Transform _launchPoint;

    protected override void Init()
    {
        base.Init();

        // スキル発射元の初期化
        _launchPoint = Util.FindChild<Transform>(gameObject, "EyeCTRL", true);
    }
    

    void OnAttack()
    {
        // ターゲットが非活性になったときはReturn
        if (!CanAttackTarget())
            return;

        Managers.Skill.SpawnLaunchSkill(
            SkillConf.LaunchSkill.BeholderAttack,
            _launchPoint.position,
            _dir,
            _stat.AttackDistance,
            _stat.ProjectileSpeed,
            _stat.Offence,
            new Define.Layer[] { Define.Layer.Unit });

        _attackFlag = false;

        // クールタイム開始
        StartCoroutine(AttackCoolTime());
    }
}

