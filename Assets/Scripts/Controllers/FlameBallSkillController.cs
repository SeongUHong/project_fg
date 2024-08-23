using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBallSkillController : LaunchSkillController
{
    protected override void OnTriggerEnter(Collider other)
    {
        // 対象レイヤーでなければreturn
        if (!IsTarget(other.gameObject.layer))
            return;

        other.gameObject.GetComponent<Stat>().OnAttacked(_damage);
    }
}
