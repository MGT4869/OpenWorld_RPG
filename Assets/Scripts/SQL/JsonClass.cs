using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class DefaultMobClass//�⺻ ��Ŭ���� �ʿ��� ������ Ư���� �ɷ��ִ� �����ϰ�� ��ӹ޾� ���
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
