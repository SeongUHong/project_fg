using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    public GameObject SpawnPlayerAttack(Vector3 pos, Vector3 dir, float distance, float speed, int damage, Define.Layer[] layers, Transform parent = null)
    {
        return SpawnLaunchSkill(0, pos, dir, distance, speed, damage, layers, parent);
    }

    //�μ�:��ų�̸�, ��ǥ, ����, ���̾�, ��ų Ÿ��
    public GameObject SpawnLaunchSkill(SkillConf.LaunchSkill skill, Vector3 pos, Vector3 dir, float distance, float speed, int damage, Define.Layer[] layers, Transform parent = null)
    {
        // ��ų�� ���
        // ��ųID�� ������ �÷��̾��� �⺻ ����
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
