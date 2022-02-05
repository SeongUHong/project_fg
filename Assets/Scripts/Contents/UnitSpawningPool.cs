using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawningPool : SpawningPool
{
    protected override void Init()
    {
        base.Init();
        //���� ���� �ʱ�ȭ
        _spawnPos = Managers.Game.UnitSpawnPos;

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
        GameObject character = Managers.Game.Spawn(Define.Layer.Unit, "Units/FitnessGirlSniper");
        character.transform.position = Managers.Game.CreatePos(Define.Layer.Unit);
    }

}
