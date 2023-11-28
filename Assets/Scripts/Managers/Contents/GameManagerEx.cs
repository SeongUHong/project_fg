using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : ManagerBase
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
    //float _positionVar = 3.0f;

    //�����Ǿ� �ִ� ĳ����
    List<GameObject> _units = new List<GameObject>();
    //�����Ǿ� �ִ� ��
    List<GameObject> _monsters = new List<GameObject>();


    public GameObject Player { get { return _player; } }
    public GameObject MonsterCrystal { get { return _monsterCrystal; } }
    public List<GameObject> Units { get { return _units; } }
    public List<GameObject> Monsters { get { return _monsters; } }

    public GameScene_Panel StartPanel { get { return Managers.UI.MakePopUp<GameScene_Panel>(); } }
    public Panel_GameOver Panel { get { return Managers.UI.MakePopUp<Panel_GameOver>(); } }
        
    public Panel_NextStage NextPanel { get { return Managers.UI.MakePopUp<Panel_NextStage>(); } }

    public Main_Panel Main_Panel{ get { return Managers.UI.MainSceneUI<Main_Panel>(); } }

    //���� �Ǵ� ����
    public Vector3 UnitSpawnPos { get { return _unitSpawnPos; } }
    public Vector3 MonsterSpawnPos { get { return _monsterSpawnPos; } }

    //���� �̺�Ʈ
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

    //ĳ���Ͱ� �����Ǵ� ��ġ�� ��ȯ
    public Vector3 CreatePos(Define.Layer layer)
    {
        Vector3 basePos;

        float spawnRange = 0;

        switch (layer)
        {
            case Define.Layer.Unit:
                basePos = _unitSpawnPos;

                spawnRange = Conf.UNIT_SPAWN_RANGE;

                break;
            case Define.Layer.Monster:
                basePos = _monsterSpawnPos;

                spawnRange = Conf.MONSTER_SPAWN_RANGE;

                break;
            default:
                Debug.Log($"Undifned Case : Layer {Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal)}");
                return Vector3.zero;
        }

        //���� ��ġ ����
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

        switch (layer)
        {
            case Define.Layer.Unit:
                _units.Add(go);
                go.AddComponent<SpawningPool>().AddUnitCount(1);
                if (AddSqawnAction != null)
                    AddSqawnAction.Invoke(1);
                break;
            case Define.Layer.Monster:
                _monsters.Add(go);
                go.AddComponent<SpawningPool>().AddMonsterCount(1);
                if (AddSqawnAction != null)
                    AddSqawnAction.Invoke(1);
                break;
        }
        
        //��ġ ����
        go.transform.position = CreatePos(layer);

        return go;
    }

    public void Despawn(Define.Layer layer, GameObject go)
    {
        SpawningPool sp = go.GetComponent<SpawningPool>();

        switch (layer)
        {
            case Define.Layer.Unit:
                if (_units.Contains(go))
                {
                    _units.Remove(go);

                    sp.MinusUnitCount(1);
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

                    sp.MinusMonsterCount(1);
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
}
