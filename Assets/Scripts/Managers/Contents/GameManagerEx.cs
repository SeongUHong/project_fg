using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerEx
{
    //GameManager�̸��� ����� �� ���� ����� Ex�� ����

    //�÷��̾�
    GameObject _player;

    //�� �ؼ���
    GameObject _monsterCrystal;

    //���� �Ǵ� ����
    Vector3 _unitSpawnPos;
    Vector3 _monsterSpawnPos;

    //������ġ ���� ����
    float _positionVar = 1.0f;

    //�����Ǿ� �ִ� ĳ����
    List<GameObject> _units = new List<GameObject>();
    //�����Ǿ� �ִ� ��
    List<GameObject> _monsters = new List<GameObject>();

    // ��ȯ ������
    SummonGauge _summonGauge;

    // ������ Ǯ
    public List<SpawningPool> _spawningPools = new List<SpawningPool>();

    public GameObject Player { get { return _player; } }
    public GameObject MonsterCrystal { get { return _monsterCrystal; } }
    public List<GameObject> Units { get { return _units; } }
    public List<GameObject> Monsters { get { return _monsters; } }
    public int MonsterNum { get { return _monsters.Count; } }

    //���� �Ǵ� ����
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

    //ĳ���Ͱ� �����Ǵ� ��ġ�� ��ȯ
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

        //���� ��ġ ����
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
        
        //��ġ ����
        go.GetComponent<NavMeshAgent>().Warp(CreatePos(layer));

        return go;
    }

    // �������� �Һ��Ͽ� ���� ��ȯ
    public void SummonUnit(int unitId)
    {
        if (!IsEnoughSummonCost())
            return;

        ConsumeSummonGauge(Define.SUMMON_COST);
        Spawn($"Units/{Util.NumToEnumName<CharacterConf.Unit>(unitId)}");
    }

    // ������Ʈ ��Ȱ��
    public void Despawn(GameObject go)
    {
        RemoveFromSpawnList(go);
        Managers.Resource.Destroy(go);
    }

    // ������ ������Ʈ ����Ʈ���� ��� ����
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

    // ��������ID�� ���� ������Ǯ ����
    public void StartSpawningPool()
    {
        // �������� ���� ���� ���
        List<data.StageSpawnMonster> spawnMonsters = Managers.Data.GetStageSpawnMonsterByStageId(Managers.Status.StageId);

        foreach (data.StageSpawnMonster spawnMonster in spawnMonsters)
        {
            // ���� �̸� ���
            CharacterConf.Monster monster = (CharacterConf.Monster)spawnMonster.monster_id;
            string monsterName = monster.ToString();

            // ���� �̸��� ������Ǯ ������Ʈ ����
            GameObject SpawningPool = new GameObject($"{monsterName}SpawningPool");
            // ������Ǯ ����
            MonsterSpawningPool monsterSpawningPool = Util.GetOrAddComponent<MonsterSpawningPool>(SpawningPool);
            monsterSpawningPool.Name = monsterName;
            monsterSpawningPool.SetKeepEnemyCount(spawnMonster.limit_num);

            _spawningPools.Add(monsterSpawningPool);
        }
    }

    // ��ȯ ������ ���� ����
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

    // ������ ��ȯ�� ��ŭ�� �������� �ִ°�
    public bool IsEnoughSummonCost()
    {
        return GetSummonGauge() >= Define.SUMMON_COST;
    }

    // ��ȯ ������ �Һ�
    public void ConsumeSummonGauge(float value)
    {
        if (_summonGauge == null)
        {
            Debug.Log("SummonGage is not initialized");
            return;
        }

        _summonGauge.ConsumeGauge(value);
    }

    // �й� ó��
    public void Gameover()
    {
        // ���ӿ��� �ǳ� Ȱ��
        Managers.UI.ShowPopupUI<UIPopupGameover>();
        // ���� �ʱ�ȭ
        Managers.Status.Reset();
    }

    // �������� Ŭ���� ó��
    public void StageClear()
    {
        // ���� ���� ����
        foreach(SpawningPool pool in _spawningPools)
        {
            pool.StopSpawn();
        }

        // �� ���� ����
        foreach (GameObject monster in _monsters) {
            monster.GetComponent<BaseController>().SetDieState();
        }

        // ������ ���������� ���
        if (Managers.Status.IsFinalStage())
        {
            // ���� �ʱ�ȭ
            Managers.Status.Reset();
            // �˾� Ȱ��
            Managers.UI.ShowPopupUI<UIPopupGameClear>();
        }
        else
        {
            // ��������ID ����
            Managers.Status.IncreaseStageId();
            // ��ų ����Ʈ �߰�
            Managers.Status.IncreasePoint();

            // �˾� Ȱ��
            Managers.UI.ShowPopupUI<UIPopupStageClear>();
        }
    }
}
