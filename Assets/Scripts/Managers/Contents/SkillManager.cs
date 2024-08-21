using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    //�μ�:��ų�̸�, ��ǥ, ����, ���̾�, ��ų Ÿ��
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
