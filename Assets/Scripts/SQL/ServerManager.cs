using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Random = UnityEngine.Random;
public class ServerManager : MonoBehaviour
{
    public GameObject temp,temp2;
    public MonsterInfo[] MonsterList;
    public List<DefaultMobClass> MonsterData;
    public AllMonsterData tempData;
    public AllMonsterData ClientMonsterData;
    [HideInInspector]public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    public bool SeverMode;
    // Start is called before the first frame update
    void Start()
    {
        if(SeverMode)
        {
            SeverDefaultSetting();
        }
        else
        {
            ClientDefaultSetting();
        }
    }
    public void SeverDefaultSetting()
    {
        MonsterList = Transform.FindObjectsOfType<MonsterInfo>();
        foreach (MonsterInfo temp in MonsterList)
        {
            tempData.data.Add(new DefaultMobClass(temp.gameObject));
        }
        string send_json = JsonUtility.ToJson(tempData);
        maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Map_Num", "\'" + 1 + "\'"));

        StartCoroutine(ServerPostion_Control());
    }
    public void ClientDefaultSetting()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_Data Where Map_Num = '1'");
        ClientMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
        for(int i=0;i< ClientMonsterData.data.Count;i++)
        {
            GameObject tempObject = Instantiate(temp, temp.transform.position, Quaternion.identity);
            tempObject.name = ClientMonsterData.data[i].Mob_name;
            tempObject.transform.position = ClientMonsterData.data[i].MobPostion;
            tempObject.AddComponent<MonsterControl>();
        }

        StartCoroutine(ClientPostion_Control());
    }
    public IEnumerator ServerPostion_Control()
    {
        do
        {
            tempData.data.Clear();
            yield return new WaitForSecondsRealtime(0.4f);
            foreach (MonsterInfo temp in MonsterList)
            {
                tempData.data.Add(new DefaultMobClass(temp.gameObject));
            }
            string send_json = JsonUtility.ToJson(tempData);
            maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Map_Num", "\'" + 1 + "\'"));
        } while (true);
    }

    public IEnumerator ClientPostion_Control()
    {
        do
        {
            yield return new WaitForSecondsRealtime(1f);
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_Data Where Map_Num = '1'");
            ClientMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
            for (int i = 0; i < ClientMonsterData.data.Count; i++)
            {
                if(GameObject.Find(ClientMonsterData.data[i].Mob_name) == null)
                {
                    GameObject tempObject = Instantiate(temp, temp.transform.position, Quaternion.identity);
                    tempObject.name = ClientMonsterData.data[i].Mob_name;
                    tempObject.transform.position = ClientMonsterData.data[i].MobPostion;
                    tempObject.AddComponent<MonsterControl>();
                }
            }

        } while (true);
    }

    public AllMonsterData return_AllMonsterData()
    {
        return ClientMonsterData;
    }
}

/*string send_json = JsonUtility.ToJson(new DefaultMobClass(temp.gameObject));
foreach (DataRow temp_string in ds_json.Tables[0].Rows)
{
    if (temp_string["Monster_Name"].ToString() == temp.gameObject.name)
    {
        count++;
        break;
    }
}
if (count == 0)
{
    maria.create_Table(string.Format("INSERT INTO {0} (Monster_Name,data) VALUES({1},{2})", "Map1_Monster_Data", "\'" + temp.gameObject.name + "\'", "\'" + send_json + "\'"));
}
else
{
    maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Monster_Name", "\'" + temp.gameObject.name + "\'"));
}*/
