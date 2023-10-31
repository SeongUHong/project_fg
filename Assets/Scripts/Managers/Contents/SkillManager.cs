using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : ManagerBase
{
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    //인수:스킬이름, 좌표, 방향, 레이어, 스킬 타입
    public GameObject SpawnSkill(
        string skillName, Vector3 pos, Vector3 dir, float distance, float speed, int damage, int[] layers, 
        Define.Skill skillType = Define.Skill.Launch, Transform parent = null)
    {
        GameObject skill = Managers.Resource.Instantiate($"Effects/{skillName}", parent);
        if (skill == null)
        {
            Debug.Log($"Not Exist Skill:{skillName}");
            return null;
        }

        skill.transform.position = pos;

        switch (skillType)
        {
            case Define.Skill.Launch:
                LaunchSkillController skillController = Util.GetOrAddComponent<LaunchSkillController>(skill);
                skillController.SetSkillStatus(pos, dir, distance, speed, damage, layers);
                skillController.StartLaunch();
                break;
        }

        return skill;
    }
}
