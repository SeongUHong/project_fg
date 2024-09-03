using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : ManagerBase
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
    //float _positionVar = 3.0f;

    //스폰되어 있는 캐릭터
    List<GameObject> _units = new List<GameObject>();
    //스폰되어 있는 적
    List<GameObject> _monsters = new List<GameObject>();

    //스테이지별 최대 소환가능수
    public int _summonedUnitCount;


    public GameObject Player { get { return _player; } }
    public GameObject MonsterCrystal { get { return _monsterCrystal; } }
    public List<GameObject> Units { get { return _units; } }
    public List<GameObject> Monsters { get { return _monsters; } }

    public GameScene_Panel StartPanel { get { return Managers.UI.MakePopUp<GameScene_Panel>(); } }
    public Panel_GameOver Panel { get { return Managers.UI.MakePopUp<Panel_GameOver>(); } }
    public Panel_NextStage NextPanel { get { return Managers.UI.MakePopUp<Panel_NextStage>(); } }
    public All_Clear_Panel ClearPanel { get { return Managers.UI.MakePopUp<All_Clear_Panel>(); } }
    public Main_Panel Main_Panel{ get { return Managers.UI.MainSceneUI<Main_Panel>(); } }



    //스폰 되는 지점
    public Vector3 UnitSpawnPos { get { return _unitSpawnPos; } }
    public Vector3 MonsterSpawnPos { get { return _monsterSpawnPos; } }

    //스폰 이벤트
    public Action<int> AddSqawnAction;

    public override void Init()
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

    public GameObject InstantiateCrystal()
    {
        GameObject monsterCrystal = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal));
        if (monsterCrystal == null)
        {
            Debug.Log("Failed Load MonsterCrystal");
            //eturn;
        }
        _monsterCrystal = monsterCrystal;

        return monsterCrystal;
    }

    public GameObject InstantiatePlayer()
    {


        GameObject player = Managers.Resource.Instantiate("Characters/Player");
        if(player == null)
        {
            Debug.Log("Failed Load Player");
            //return null;
        }
        _player = player;
        
        player.transform.position = _unitSpawnPos;

        return player;
    }

    //캐릭터가 생성되는 위치를 반환
    public Vector3 CreatePos(Define.Layer layer)
    {
        Vector3 basePos;

        float spawnRange = 0;

        switch (layer)
        {
            case Define.Layer.Unit:
                basePos = _unitSpawnPos;

                spawnRange = Conf.Main.UNIT_SPAWN_RANGE;

                break;
            case Define.Layer.Monster:
                basePos = _monsterSpawnPos;

                spawnRange = Conf.Main.MONSTER_SPAWN_RANGE;

                break;
            default:
                Debug.Log($"Undifned Case : Layer {Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal)}");
                return Vector3.zero;
        }

        //랜덤 위치 생성
        Vector3 newPos = new Vector3(
            UnityEngine.Random.Range(basePos.x - spawnRange, basePos.x + spawnRange),
            basePos.y,
            UnityEngine.Random.Range(basePos.z - spawnRange, basePos.z + spawnRange)
        );

        return newPos;
    }

    public GameObject Spawn(Define.Layer layer, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate($"Characters/{path}", parent);
        UnitSpawningPool uSp = GameObject.Find("EnemySpawningPool").GetComponent<UnitSpawningPool>();
        MonsterSpawningPool mSp = GameObject.Find("MonsterSpawningPool").GetComponent<MonsterSpawningPool>();
        switch (layer)
        {
            case Define.Layer.Unit:
                _units.Add(go);
                uSp.AddUnitCount(1);
                _summonedUnitCount++;
                if (AddSqawnAction != null)
                    AddSqawnAction.Invoke(1);
                break;
            case Define.Layer.Monster:
                
                _monsters.Add(go);
                mSp.AddMonsterCount(1);
                if (AddSqawnAction != null)
                    AddSqawnAction.Invoke(1);
                break;
        }
        
        
        //위치 설정
        go.transform.position = CreatePos(layer);

        return go;
    }

    public void Despawn(Define.Layer layer, GameObject go)
    {
        SpawningPool sp = go.GetComponent<SpawningPool>();
        UnitSpawningPool uSp = GameObject.Find("EnemySpawningPool").GetComponent<UnitSpawningPool>();
        MonsterSpawningPool mSp = GameObject.Find("MonsterSpawningPool").GetComponent<MonsterSpawningPool>();
        switch (layer)
        {
            case Define.Layer.Unit:
                if (_units.Contains(go))
                {
                    _units.Remove(go);
                    uSp.MinusUnitCount(1);
                    if (AddSqawnAction != null)
                        AddSqawnAction.Invoke(-1);
                }
                else
                {
                    Debug.Log($"Not Spawned Object ObjectName:{go.name}, Layer{Enum.GetName(typeof(Define.Layer), layer)}");
                    return;
                }
                break;
            case Define.Layer.Monster:
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);

                    mSp.MinusMonsterCount(1);
                    if (AddSqawnAction != null)
                        AddSqawnAction.Invoke(-1);
                }
                else
                {
                    Debug.Log($"Not Spawned Object ObjectName:{go.name}, Layer{Enum.GetName(typeof(Define.Layer), layer)}");
                    return;
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }

    public void Clear() {
        _units.Clear();
        _monsters.Clear();
    }
}
