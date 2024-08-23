using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    //인수:스킬이름, 좌표, 방향, 레이어, 스킬 타입
    public GameObject SpawnLaunchSkill(SkillConf.LaunchSkill skill, Vector3 pos, Vector3 dir, float distance, float speed, int damage, Define.Layer[] layers, Transform parent = null)
    {
        string skillName = Util.NumToEnumName<SkillConf.LaunchSkill>((int)skill);
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
}
