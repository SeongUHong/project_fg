using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerEx
{
    //GameManager이름을 사용할 수 없는 관계로 Ex를 붙임

    //플레이어
    GameObject _player;

    //적 넥서스
    GameObject _monsterCrystal;

    //스폰 되는 지점
    Vector3 _unitSpawnPos;
    Vector3 _monsterSpawnPos;

    //스폰위치 랜덤 범위
    float _positionVar = 1.0f;

    //스폰되어 있는 캐릭터
    List<GameObject> _units = new List<GameObject>();
    //스폰되어 있는 적
    List<GameObject> _monsters = new List<GameObject>();

    // 소환 게이지
    SummonGauge _summonGauge;

    // 스포닝 풀
    public List<SpawningPool> _spawningPools = new List<SpawningPool>();

    public GameObject Player { get { return _player; } }
    public GameObject MonsterCrystal { get { return _monsterCrystal; } }
    public List<GameObject> Units { get { return _units; } }
    public List<GameObject> Monsters { get { return _monsters; } }
    public int MonsterNum { get { return _monsters.Count; } }

    //스폰 되는 지점
    public Vector3 UnitSpawnPos { get { return _unitSpawnPos; } }
    public Vector3 MonsterSpawnPos { get { return _monsterSpawnPos; } }

    public void Init()
    {
        GameObject unitSpawnPos = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.UnitSpawnSpot));
        if (unitSpawnPos == null)
        {
            Debug.Log("Failed Load UnitSpawnSpot");
            return;
        }
        _unitSpawnPos = unitSpawnPos.transform.position;

        GameObject monsterSpawnSpot = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterSpawnSpot));
        if (monsterSpawnSpot == null)
        {
            Debug.Log("Failed Load MonsterSpawnSpot");
            return;
        }
        _monsterSpawnPos = monsterSpawnSpot.transform.position;

        GameObject monsterCrystal = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal));
        if (monsterCrystal == null)
        {
            Debug.Log("Failed Load MonsterCrystal");
            return;
        }
        _monsterCrystal = monsterCrystal;
    }

    public void Clear()
    {
        _units.Clear();
        _monsters.Clear();
        _spawningPools.Clear();
    }

    public GameObject InstantiatePlayer()
    {
        GameObject player = Managers.Resource.Instantiate("Characters/Player");
        if(player == null)
        {
            Debug.Log("Failed Load Player");
            return null;
        }
        _player = player;
        
        player.GetComponent<NavMeshAgent>().Warp(_unitSpawnPos);

        return player;
    }

    //캐릭터가 생성되는 위치를 반환
    public Vector3 CreatePos(int layer)
    {
        Vector3 basePos;
        
        switch (layer)
        {
            case (int)Define.Layer.Unit:
                basePos = _unitSpawnPos;
                break;
            case (int)Define.Layer.Monster:
                basePos = _monsterSpawnPos;
                break;
            default:
                Debug.Log($"Undifned Case : Layer {Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal)}");
                return Vector3.zero;
        }

        //랜덤 위치 생성
        Vector3 newPos = new Vector3(
            UnityEngine.Random.Range(basePos.x - _positionVar, basePos.x + _positionVar),
            basePos.y,
            UnityEngine.Random.Range(basePos.z - _positionVar, basePos.z + _positionVar)
        );

        return newPos;
    }

    public GameObject Spawn(string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate($"Characters/{path}", parent);
        int layer = go.layer;

        switch (layer)
        {
            case (int)Define.Layer.Unit:
                _units.Add(go);
                break;
            case (int)Define.Layer.Monster:
                _monsters.Add(go);
                break;
        }
        
        //위치 설정
        go.GetComponent<NavMeshAgent>().Warp(CreatePos(layer));

        return go;
    }

    // 게이지를 소비하여 유닛 소환
    public void SummonUnit(int unitId)
    {
        if (!IsEnoughSummonCost())
            return;

        ConsumeSummonGauge(Define.SUMMON_COST);
        Spawn($"Units/{Util.NumToEnumName<CharacterConf.Unit>(unitId)}");
    }

    // 오브젝트 비활성
    public void Despawn(GameObject go)
    {
        RemoveFromSpawnList(go);
        Managers.Resource.Destroy(go);
    }

    // 스폰된 오브젝트 리스트에서 대상 삭제
    public void RemoveFromSpawnList(GameObject go)
    {
        int layer = go.layer;

        switch (layer)
        {
            case (int)Define.Layer.Unit:
                if (_units.Contains(go))
                {
                    _units.Remove(go);
                }
                else
                {
                    Debug.Log($"Not Spawned Object ObjectName:{go.name}, Layer{Enum.GetName(typeof(Define.Layer), layer)}");
                    return;
                }
                break;
            case (int)Define.Layer.Monster:
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                }
                else
                {
                    Debug.Log($"Not Spawned Object ObjectName:{go.name}, Layer{Enum.GetName(typeof(Define.Layer), layer)}");
                    return;
                }
                break;
        }
    }

    // 스테이지ID로 몬스터 스포닝풀 가동
    public void StartSpawningPool()
    {
        // 스테이지 몬스터 정보 취득
        List<data.StageSpawnMonster> spawnMonsters = Managers.Data.GetStageSpawnMonsterByStageId(Managers.Status.StageId);

        foreach (data.StageSpawnMonster spawnMonster in spawnMonsters)
        {
            // 몬스터 이름 취득
            CharacterConf.Monster monster = (CharacterConf.Monster)spawnMonster.monster_id;
            string monsterName = monster.ToString();

            // 몬스터 이름의 스포닝풀 오브젝트 생성
            GameObject SpawningPool = new GameObject($"{monsterName}SpawningPool");
            // 스포닝풀 셋팅
            MonsterSpawningPool monsterSpawningPool = Util.GetOrAddComponent<MonsterSpawningPool>(SpawningPool);
            monsterSpawningPool.Name = monsterName;
            monsterSpawningPool.SetKeepEnemyCount(spawnMonster.limit_num);

            _spawningPools.Add(monsterSpawningPool);
        }
    }

    // 소환 게이지 증가 시작
    public void StartSummonGaugeIncreasing()
    {
        GameObject go = new GameObject("SummonGauge");
        SummonGauge summonGauge = Util.GetOrAddComponent<SummonGauge>(go);
        summonGauge.StartGaugeIncreasing();
        _summonGauge = summonGauge;
    }

    public float GetSummonGauge()
    {
        if (_summonGauge == null)
            return 0;

        return _summonGauge.Gauge;
    }

    // 유닛을 소환할 만큼의 게이지가 있는가
    public bool IsEnoughSummonCost()
    {
        return GetSummonGauge() >= Define.SUMMON_COST;
    }

    // 소환 게이지 소비
    public void ConsumeSummonGauge(float value)
    {
        if (_summonGauge == null)
        {
            Debug.Log("SummonGage is not initialized");
            return;
        }

        _summonGauge.ConsumeGauge(value);
    }

    // 패배 처리
    public void Gameover()
    {
        // 게임오버 판넬 활성
        Managers.UI.ShowPopupUI<UIPopupGameover>();
        // 상태 초기화
        Managers.Status.Reset();
    }

    // 스테이지 클리어 처리
    public void StageClear()
    {
        // 몬스터 스폰 정지
        foreach(SpawningPool pool in _spawningPools)
        {
            pool.StopSpawn();
        }

        // 전 몬스터 디스폰
        foreach (GameObject monster in _monsters) {
            monster.GetComponent<BaseController>().SetDieState();
        }

        // 마지막 스테이지인 경우
        if (Managers.Status.IsFinalStage())
        {
            // 상태 초기화
            Managers.Status.Reset();
            // 팝업 활성
            Managers.UI.ShowPopupUI<UIPopupGameClear>();
        }
        else
        {
            // 스테이지ID 증가
            Managers.Status.IncreaseStageId();
            // 스킬 포인트 추가
            Managers.Status.IncreasePoint();

            // 팝업 활성
            Managers.UI.ShowPopupUI<UIPopupStageClear>();
        }
    }
}
