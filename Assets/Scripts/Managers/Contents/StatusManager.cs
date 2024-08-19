using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    // 스테이지ID
    int _stageId;
    // 스킬 업그레이드 포인트
    int _point;
    // 유닛ID 별 레벨
    Dictionary<int, int> _unitLevels = new Dictionary<int, int>();

    public int StageId { get { return _stageId; } }
    public int Point { get { return _point; } }

    public void Init()
    {
        _stageId = Define.FIRST_STAGE_ID;
        _point = Define.POINT_PER_STAGE;
    }

    public void Reset()
    {
        _stageId = Define.FIRST_STAGE_ID;
        _point = Define.POINT_PER_STAGE;
        _unitLevels.Clear();
    }

    // 포인트 소비
    public void DecreasePoint()
    {
        _point--;
    }

    // 포인트 추가
    public void IncreasePoint()
    {
        _point++;
    }

    // 스테이지ID 증가
    public void IncreaseStageId()
    {
        _stageId++;
    }

    // 사용 가능한 유닛인가
    public bool IsAvailableUnit(int unitId)
    {
        if (_unitLevels.ContainsKey(unitId))
            return true;

        return false;
    }

    // 유닛 레벨 취득
    public int GetUnitLevel(int unitId)
    {
        int level = 0;
        if (!_unitLevels.TryGetValue(unitId, out level))
        {
            Debug.Log($"Inactive Unit. (unitId: {unitId})");
        }

        return level;
    }

    // 유닛 레벨업
    public int LevelUpUnit(int unitId)
    {
        // 비활성 유닛이면 가장 낮은 레벨 취득
        if (!IsAvailableUnit(unitId))
        {
            _unitLevels[unitId] = CharacterConf.MIN_LEVEL;
            DecreasePoint();

            return _unitLevels[unitId];
        }

        int level = _unitLevels[unitId];
        // 이미 최대 레벨에 도달한 경우 레벨업 불가
        if (IsMaxLevelUnit(unitId))
        {
            Debug.Log($"Unit is aleady max level. (unitId: {unitId})");
            return level;
        }

        // 레벨업
        _unitLevels[unitId] = ++level;
        DecreasePoint();

        return level;
    }

    // 최고 레벨의 유닛인가
    public bool IsMaxLevelUnit(int unitId)
    {
        if (_unitLevels[unitId] >= Managers.Data.GetUnitMaxLevel(unitId))
            return true;

        return false;
    }

    // 활성된 유닛ID 리스트
    public List<int> GetAvailableUnitIds()
    {
        return new List<int>(_unitLevels.Keys);
    }

    // 최종 스테이지인가
    public bool IsFinalStage()
    {
        return _stageId == Managers.Data.GetFinalStageId();
    }
}
