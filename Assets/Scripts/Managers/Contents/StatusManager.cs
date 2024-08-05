using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    // ��������ID
    int _stageId;
    // ����ID �� ����
    Dictionary<int, int> _unitLevels = new Dictionary<int, int>();

    public int StageId { get { return _stageId; } }

    public void Init()
    {
        _stageId = Define.FIRST_STAGE_ID;
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

        // ������
        int level = _unitLevels[unitId];
        _unitLevels[unitId] = ++level;

        return level;
    }
}
