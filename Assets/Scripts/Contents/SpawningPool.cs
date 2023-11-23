using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class SpawningPool : MonoBehaviour
{
    //현재 존재하는 유닛 수
    [SerializeField]
    protected int _unitCount = 0;

    //현재 존재하는 몬스터 수
    [SerializeField]
    protected int _monsterCount = 0;

    //생성 예약된 오브젝트
    protected int _reserveCount = 0;

    //유지할 오브젝트의 수
    [SerializeField]
    protected int _keepObjectCount = 0;

    //생성 예약된 몬스터수
    protected int _reservedMonsterCount = 0;

    //유지할 오브젝트의 수
    [SerializeField]
    protected int _keepMonsterCount = 0;

    //스폰 장소
    [SerializeField]
    protected Vector3 _spawnPos;

    //스폰 텀
    [SerializeField]
    protected float _spawnTime = 2.0f;

    protected const string RESOURCE_ROOT = "Characters/";


    public void AddUnitCount(int value) { _unitCount += value; }
    public void MinusUnitCount(int value) { _unitCount -= value; }
    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void MinusMonsterCount(int value) { _monsterCount -= value; }
    public void SetKeepEnemyCount(int count) { _keepObjectCount = count; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    //카운트 확인을 위해 생성
    public int GetKeepMonsterCount() { return _keepMonsterCount; }

    private void Start()
    {
        Managers.Game.AddSqawnAction -= AddMonsterCount;
        Managers.Game.AddSqawnAction += AddMonsterCount;
        Init();
    }

    private void Update()
    {
    }

    protected virtual void Init()
    {
    }
}
