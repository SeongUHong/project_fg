using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningPool : SpawningPool
{
    protected override void Init()
    {
        base.Init();
        //���� ���� �ʱ�ȭ
        _spawnPos = Managers.Game.EnemySpawnPos;

        //���� �׼� �߰�
        SpawnAction -= AddSqawnAction;
        SpawnAction += AddSqawnAction;
    }

    protected override void AddSqawnAction()
    {
        while (_reserveCount < _keepObjectCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    //����� �ð� ��ŭ �ڿ� ������Ʈ ����
    protected virtual IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));
        GameObject enemy = Managers.Game.Spawn(Define.Layer.Enemy, Enum.GetName(typeof(Define.Enemy), Define.Enemy.Enemy));
        enemy.transform.position = _spawnPos;
        enemy.GetComponent<BaseController>().State = Define.State.Idle;
    }

}
