using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    // ��������ID
    int _stageId;
    // ��ų ���׷��̵� ����Ʈ
    int _point;
    // �÷��̾� ����
    int _playerLevel;
    // ����ID �� ����
    Dictionary<int, int> _unitLevels = new Dictionary<int, int>();
    // ��ųID �� ����
    Dictionary<int, int> _skillLevels = new Dictionary<int, int>();

    public int StageId { get { return _stageId; } }
    public int Point { get { return _point; } }
    public int PlayerLevel { get { return _playerLevel; } }

    public void Init()
    {
        _stageId = Define.FIRST_STAGE_ID;
        _point = Define.POINT_PER_STAGE;
        _playerLevel = Define.MIN_PLAYER_LEVEL;
    }

    public void Reset()
    {
        _stageId = Define.FIRST_STAGE_ID;
        _point = Define.POINT_PER_STAGE;
        _unitLevels.Clear();
        _playerLevel = Define.MIN_PLAYER_LEVEL;
    }

    // ����Ʈ �Һ�
    public void DecreasePoint()
    {
        _point--;
    }

    // ����Ʈ �߰�
    public void IncreasePoint()
    {
        _point++;
    }

    // ��������ID ����
    public void IncreaseStageId()
    {
        _stageId++;
    }

    // �÷��̾� ���� ����
    public void IncreasePlayerLevel()
    {
        _playerLevel++;
    }

    // �÷��̾� ���� ��
    public void LevelUpPlayer()
    {
        DecreasePoint();
        IncreasePlayerLevel();
    }

    // �÷��̾� ������ �ִ�ġ�ΰ�
    public bool IsMaxPlayerLevel()
    {
        if (_playerLevel >= Managers.Data.GetPlayerMaxLevel())
        {
            return true;
        }

        return false;
    }

    // ��� ������ �����ΰ�
    public bool IsAvailableUnit(int unitId)
    {
        if (_unitLevels.ContainsKey(unitId))
            return true;

        return false;
    }

    // ���� ���� ���
    public int GetUnitLevel(int unitId)
    {
        int level = 0;
        if (!_unitLevels.TryGetValue(unitId, out level))
        {
            Debug.Log($"Inactive Unit. (unitId: {unitId})");
        }

        return level;
    }

    // ���� ������
    public int LevelUpUnit(int unitId)
    {
        // ��Ȱ�� �����̸� ���� ���� ���� ���
        if (!IsAvailableUnit(unitId))
        {
            _unitLevels[unitId] = CharacterConf.MIN_LEVEL;
            DecreasePoint();

            return _unitLevels[unitId];
        }

        int level = _unitLevels[unitId];
        // �̹� �ִ� ������ ������ ��� ������ �Ұ�
        if (IsMaxLevelUnit(unitId))
        {
            Debug.Log($"Unit is aleady max level. (unitId: {unitId})");
            return level;
        }

        // ������
        _unitLevels[unitId] = ++level;
        DecreasePoint();

        return level;
    }

    // �ְ� ������ �����ΰ�
    public bool IsMaxLevelUnit(int unitId)
    {
        if (_unitLevels[unitId] >= Managers.Data.GetUnitMaxLevel(unitId))
            return true;

        return false;
    }

    // Ȱ���� ����ID ����Ʈ
    public List<int> GetAvailableUnitIds()
    {
        return new List<int>(_unitLevels.Keys);
    }

    // ���� ���������ΰ�
    public bool IsFinalStage()
    {
        return _stageId == Managers.Data.GetFinalStageId();
    }

    public int GetSkillLevel(int skillId)
    {
        int level = 0;
        if (!_skillLevels.TryGetValue(skillId, out level))
        {
            Debug.Log($"Inactive Skill. (skillId: {skillId})");
        }

        return level;
    }

    // ��� ������ ��ų�ΰ�
    public bool IsAvailableSkill(int skillId)
    {
        if (_skillLevels.ContainsKey(skillId))
            return true;

        return false;
    }

    // �ְ� ������ ��ų�ΰ�
    public bool IsMaxLevelSkill(int skillId)
    {
        if (_skillLevels[skillId] >= Managers.Data.GetSkillMaxLevel(skillId))
            return true;

        return false;
    }

    // ��ų ������
    public int LevelUpSkill(int skillId)
    {
        // ��Ȱ�� ��ų�̸� ���� ���� ���� ���
        if (!IsAvailableSkill(skillId))
        {
            _skillLevels[skillId] = SkillConf.MIN_LEVEL;
            DecreasePoint();

            return _skillLevels[skillId];
        }

        int level = _skillLevels[skillId];
        // �̹� �ִ� ������ ������ ��� ������ �Ұ�
        if (IsMaxLevelSkill(skillId))
        {
            Debug.Log($"Skill is aleady max level. (skillId: {skillId})");
            return level;
        }

        // ������
        _skillLevels[skillId] = ++level;
        DecreasePoint();

        return level;
    }
}
