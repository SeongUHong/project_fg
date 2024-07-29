using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawningPool : MonoBehaviour
{
    //������ ������Ʈ�� ��
    [SerializeField]
    protected int _keepObjectCount = 0;
    //���� ���
    [SerializeField]
    protected Vector3 _spawnPos;
    //���� ��
    [SerializeField]
    protected float _spawnTime = 5.0f;

    //���� ����� ������Ʈ
    protected int _spawnedCount = 0;

    private void Start()
    {
        Init();
    }
    
    protected virtual void Init()
    {
        // ���� ���� �ʱ�ȭ
        _spawnPos = SpawnPos();

        // ���� ����
        StartCoroutine(ReserveSpawn());
    }

    public void SetKeepEnemyCount(int count)
    {
        _keepObjectCount = count;
    }

    // ����� �ð� ��ŭ �ڿ� ������Ʈ ����
    protected virtual IEnumerator ReserveSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

            // ���ѱ��� ��ȯ�� ��� ��ŵ
            if (Managers.Game.MonsterNum >= _keepObjectCount)
                continue;

            GameObject enemy = Managers.Game.Spawn(CharacterPath());
            enemy.transform.position = _spawnPos;
        }
    }

    protected abstract Vector3 SpawnPos();
    protected abstract string CharacterPath();
}
