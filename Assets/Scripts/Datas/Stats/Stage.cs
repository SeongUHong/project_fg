using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    //��ƼƼ Ŭ����
    [Serializable]
    public class Stage
    {
        public List<StageSpawnMonster> spawn_monsters;
    }

    [Serializable]
    public class StageSpawnMonster
    {
        //����ID CharacterConf.Monster(��Ű ����)
        public int monster_id;
        // ���� ����
        public int level;
        // ��ȯ ����
        public int limit_num;
    }

}
