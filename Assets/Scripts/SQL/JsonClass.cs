using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class DefaultMobClass//기본 몹클래스 필요한 데이터 특수한 능력있는 몬스터일경우 상속받아 사용
{
    public string Mob_name;
    public Vector3 MobPostion;


    public DefaultMobClass(GameObject Mob_Object)
    {
        Mob_name = Mob_Object.gameObject.name;
        MobPostion = Mob_Object.transform.position;
    }
}
[Serializable]
public class AllMonsterData
{
    public List<DefaultMobClass> data;
    public AllMonsterData(List<DefaultMobClass> data)
    {
        data = this.data;
    }
}
