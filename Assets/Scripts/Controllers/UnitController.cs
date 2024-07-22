using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : CharacterController
{
    protected override GameObject MainTarget()
    {
        return Managers.Game.MonsterCrystal;
    }

    protected override List<GameObject> Targets()
    {
        return Managers.Game.Monsters;
    }

    // ������ 4���� ��� �ִϸ��̼��� ����
    protected override string DieAnimName() 
    {
        return $"Die{Random.Range(1, 5)}";
    }
}

