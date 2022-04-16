using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class DefaultMobClass//기본 몹클래스 필요한 데이터 특수한 능력있는 몬스터일경우 상속받아 사용
{
    public string Mob_name;
    public Vector3 MobPostion;
    public int MonsterType;
    public int NowHP;

    public DefaultMobClass(GameObject Mob_Object)
    {
        Mob_name = Mob_Object.gameObject.name;
        MobPostion = Mob_Object.transform.position;
        MonsterType = Mob_Object.GetComponent<MonsterInfo>().MonsterType;
        NowHP = Mob_Object.GetComponent<MonsterInfo>().NowHp;
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
[Serializable]
public class PlayerClass
{
    public string Player_name;
    public int Hp;
    public Vector3 PlayerPos;
    public string AniState;
    public string AniRun;
    public PlayerClass(GameObject Charctor_Object)
    {
        Player_name = Charctor_Object.gameObject.name;
        PlayerPos = Charctor_Object.transform.position;
        Hp = Charctor_Object.GetComponent<PlayerInfo>().Hp;
        AniState = Charctor_Object.GetComponent<PlayerInfo>().AniState;
        AniRun = Charctor_Object.GetComponent<PlayerInfo>().AniRun;
    }
}
[Serializable]
public class AllPlayerData
{
    public List<PlayerClass> data;

    public AllPlayerData(List<PlayerClass> data)
    {
        data = this.data;
    }
}
[Serializable]
public class JsonVector3
{
    public Vector3 Pos;

    public JsonVector3(GameObject temp)
    {
        Pos = temp.transform.position;
    }
}
