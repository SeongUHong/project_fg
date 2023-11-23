using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class SpawningPool : MonoBehaviour
{
    //���� �����ϴ� ���� ��
    [SerializeField]
    protected int _unitCount = 0;

    //���� �����ϴ� ���� ��
    [SerializeField]
    protected int _monsterCount = 0;

    //���� ����� ������Ʈ
    protected int _reserveCount = 0;

    //������ ������Ʈ�� ��
    [SerializeField]
    protected int _keepObjectCount = 0;

    //���� ����� ���ͼ�
    protected int _reservedMonsterCount = 0;

    //������ ������Ʈ�� ��
    [SerializeField]
    protected int _keepMonsterCount = 0;

    //���� ���
    [SerializeField]
    protected Vector3 _spawnPos;

    //���� ��
    [SerializeField]
    protected float _spawnTime = 2.0f;

    protected const string RESOURCE_ROOT = "Characters/";


    public void AddUnitCount(int value) { _unitCount += value; }
    public void MinusUnitCount(int value) { _unitCount -= value; }
    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void MinusMonsterCount(int value) { _monsterCount -= value; }
    public void SetKeepEnemyCount(int count) { _keepObjectCount = count; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    //ī��Ʈ Ȯ���� ���� ����
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
