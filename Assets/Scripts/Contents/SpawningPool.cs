using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class SpawningPool : MonoBehaviour
{
    //현재 존재하는 유닛 수
    protected int _unitCount = 0;

    //현재 존재하는 몬스터 수
    protected int _monsterCount = 0;

    //생성 예약된 유닛의 수
    protected int _reserveCount = 0;

    //유지할 유닛의 수
    protected int _keepObjectCount = 0;


    //생성 예약된 몬스터 수
    protected int _reservedMonsterCount = 0;

    //유지할 몬스터 수
    protected int _keepMonsterCount = 0;

    //라운드별 최대 몬스터 수
    protected int _maxMonsterCount = 0;

    //라운드별 최대 유닛 수
    protected int _maxUnitCount = 0;

    //스폰 장소
    [SerializeField]
    protected Vector3 _spawnPos;

    //스폰 텀
    [SerializeField]
    protected float _spawnTime = 0.2f;

    protected const string RESOURCE_ROOT = "Characters/";


    public void AddUnitCount(int value) { _unitCount += value; }
    public void MinusUnitCount(int value) { _unitCount -= value; }
    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void MinusMonsterCount(int value) { _monsterCount -= value; }
    public void SetKeepObjectCount(int count) { _keepObjectCount = count; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    public void SetMaxMonsterCount(int count) { _maxMonsterCount = count;  }
    public void SetMaxUnitCount(int count) { _maxUnitCount = count; }

    //카운트 확인을 위해 생성
    public int GetKeepMonsterCount() { return _keepMonsterCount; }
    public int GetKeepObjectCount() { return _keepObjectCount; }
    public int GetUnitCount() { return _unitCount; }
    public int GetMonsterCount() { return _monsterCount; }
    public int GetMaxMonsterCount() { return _maxMonsterCount; }
    public int GetMaxUnitCount() { return _maxUnitCount; }

    private void Start()
    {
        Init();
    }

    private void Update()
    {


    }

    protected virtual void Init()
    {
    }
}
