using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    //인수:스킬이름, 좌표, 방향, 레이어, 스킬 타입
    public GameObject SpawnLaunchSkill(string skillName, Vector3 pos, Vector3 dir, float distance, float speed, int damage, int[] layers, Transform parent = null)
    {
        GameObject skill = Managers.Resource.Instantiate($"Effects/{skillName}", parent);
        if (skill == null)
        {
            Debug.Log($"Not Exist Skill:{skillName}");
            return null;
        }

        skill.transform.position = pos;

        LaunchSkillController skillController = Util.GetOrAddComponent<LaunchSkillController>(skill);
        skillController.SetSkillStatus(pos, dir, distance, speed, damage, layers);
        skillController.StartLaunch();

        return skill;
    }
}
