using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawningPool : MonoBehaviour
{
    //���� ����� ������Ʈ
    protected int _reserveCount = 0;

    //������ ������Ʈ�� ��
    [SerializeField]
    protected int _keepObjectCount = 0;

    //���� ���
    [SerializeField]
    protected Vector3 _spawnPos;

    //���� ��
    [SerializeField]
    protected float _spawnTime = 5.0f;

    protected const string RESOURCE_ROOT = "Characters/";

    public void SetKeepEnemyCount(int count) { _keepObjectCount = count; }

    //���� ����
    protected Action SpawnAction = null;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (SpawnAction == null) return;
        SpawnAction.Invoke();
    }

    //�߰��� �׼�
    protected abstract void AddSqawnAction();

    protected virtual void Init()
    {
    }
}
