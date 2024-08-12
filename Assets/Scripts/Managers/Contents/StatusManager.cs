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
    // ����ID �� ����
    Dictionary<int, int> _unitLevels = new Dictionary<int, int>();

    public int StageId { get { return _stageId; } }
    public int Point { get { return _point; } }

    public void Init()
    {
        _stageId = Define.FIRST_STAGE_ID;
        _point = Define.POINT_PER_STAGE;
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
}
