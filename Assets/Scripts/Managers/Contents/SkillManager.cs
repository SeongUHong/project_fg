using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    public GameObject SpawnPlayerAttack(Vector3 pos, Vector3 dir, float distance, float speed, int damage, Define.Layer[] layers, Transform parent = null)
    {
        return SpawnLaunchSkill(0, pos, dir, distance, speed, damage, layers, parent);
    }

    //인수:스킬이름, 좌표, 방향, 레이어, 스킬 타입
    public GameObject SpawnLaunchSkill(SkillConf.LaunchSkill skill, Vector3 pos, Vector3 dir, float distance, float speed, int damage, Define.Layer[] layers, Transform parent = null)
    {
        // 스킬명 취득
        // 스킬ID가 없으면 플레이어의 기본 공격
        string skillName = (skill > 0)
            ? Util.NumToEnumName<SkillConf.LaunchSkill>((int)skill)
            : "Attack";

        GameObject go = Managers.Resource.Instantiate($"Effects/{skillName}", parent);
        if (go == null)
        {
            Debug.Log($"Not Exist Skill:{skillName}");
            return null;
        }

        go.transform.position = pos;

        LaunchSkillController skillController = go.GetComponent<LaunchSkillController>();
        if (skillController == null)
        {
            Debug.Log($"Failed to load LaunchSkillController");
            return null;
        }

        skillController.SetSkillStatus(pos, dir, distance, speed, damage, layers);
        skillController.StartLaunch();

        return go;
    }

    public GameObject SpawnRnageSkill(SkillConf.RangeSkill skill, GameObject owner, float activeTime, float tickInterval, int damage, Define.Layer[] layers, Transform parent = null)
    {
        string skillName = Util.NumToEnumName<SkillConf.RangeSkill>((int)skill);
        GameObject go = Managers.Resource.Instantiate($"Effects/{skillName}", parent);
        if (go == null)
        {
            Debug.Log($"Not Exist Skill:{skillName}");
            return null;
        }

        go.transform.position = owner.transform.position;

        RangeSkillController skillController = go.GetComponent<RangeSkillController>();
        if (skillController == null)
        {
            Debug.Log($"Failed to load RangeSkillController");
            return null;
        }

        skillController.SetSkillStatus(owner, activeTime, tickInterval, damage, layers);
        skillController.ActiveSkill();

        return go;
    }
}
