using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    //GameManager이름을 사용할 수 없는 관계로 Ex를 붙임

    //플레이어
    GameObject _player;

    //적 넥서스
    GameObject _enemyCore;

    //스폰 되는 지점
    Vector3 _unitSpawnPos;
    Vector3 _monsterSpawnPos;

    //스폰위치 랜덤 범위
    float _positionVar = 1.0f;

    //스폰되어 있는 캐릭터
    List<GameObject> _units = new List<GameObject>();
    //스폰되어 있는 적
    List<GameObject> _monsters = new List<GameObject>();

    public GameObject Player { get { return _player; } }
    public GameObject EnemyCore { get { return _enemyCore; } }
    public List<GameObject> Units { get { return _units; } }
    public List<GameObject> Monsters { get { return _monsters; } }

    //스폰 되는 지점
    public Vector3 UnitSpawnPos { get { return _unitSpawnPos; } }
    public Vector3 MonsterSpawnPos { get { return _monsterSpawnPos; } }

    public void Init()
    {
        GameObject CharacterSpawnSpot = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.UnitSpawnSpot));
        if (CharacterSpawnSpot == null)
        {
            Debug.Log("Failed Load UnitSpawnSpot");
            return;
        }
        _unitSpawnPos = CharacterSpawnSpot.transform.position;

        GameObject EnemySpawnSpot = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterSpawnSpot));
        if (CharacterSpawnSpot == null)
        {
            Debug.Log("Failed Load MonsterSpawnSpot");
            return;
        }
        _monsterSpawnPos = EnemySpawnSpot.transform.position;

        GameObject EnemyCore = GameObject.Find(Enum.GetName(typeof(Define.SceneLocateObject), Define.SceneLocateObject.MonsterCrystal));
        if (EnemyCore == null)
        {
            Debug.Log("Failed Load MonsterCrystal");
            return;
        }
        _enemyCore = EnemyCore;
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
        
        player.transform.position = _unitSpawnPos;

        return player;
    }

    //캐릭터가 생성되는 위치를 반환
    public Vector3 CreatePos(Define.Layer layer)
    {
        Vector3 basePos;
        
        switch (layer)
        {
            case Define.Layer.Unit:
                basePos = _unitSpawnPos;
                break;
            case Define.Layer.Monster:
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

    public GameObject Spawn(Define.Layer layer, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate($"Characters/{path}", parent);

        switch (layer)
        {
            case Define.Layer.Unit:
                _units.Add(go);
                break;
            case Define.Layer.Monster:
                _monsters.Add(go);
                break;
        }

        return go;
    }

    public void Despawn(Define.Layer layer, GameObject go)
    {
        switch (layer)
        {
            case Define.Layer.Unit:
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
            case Define.Layer.Monster:
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

        Managers.Resource.Destroy(go);
    }
}
