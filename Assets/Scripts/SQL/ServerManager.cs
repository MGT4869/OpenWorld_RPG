using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class ServerManager : MonoBehaviour
{
    public GameObject temp,temp2;
    public MonsterInfo[] MonsterList;
    public List<DefaultMobClass> MonsterData;
    public List<GameObject> Client_MonsterList = new List<GameObject>();
    public AllMonsterData tempData;
    public AllMonsterData ClientMonsterData;
    [HideInInspector]public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    public bool SeverMode;
    public Text Text_Temp;
    // Start is called before the first frame update
    public void Start()
    {
        Text_Temp.text = "진입";
        if (SeverMode)
        {
            SeverDefaultSetting();
            Text_Temp.text = "진입1";
        }
        else
        {
            Text_Temp.text = "진입2";
            StartCoroutine(GetAllMonsterData());
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
            Client_MonsterList.Add(tempObject);
        }

        StartCoroutine(ClientMonsterUpdate());
        StartCoroutine(ClientUpdateMonsterPotion());
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

    public IEnumerator ClientMonsterUpdate()
    {
        do
        {
            yield return new WaitForSecondsRealtime(1f);
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
    public IEnumerator ClientUpdateMonsterPotion()
    {
        do
        {
            yield return new WaitForSecondsRealtime(0.1f);
            for (int i = 0; i < ClientMonsterData.data.Count; i++)
            {
                Client_MonsterList.Find(x => x.gameObject.name == ClientMonsterData.data[i].Mob_name).transform.position = ClientMonsterData.data[i].MobPostion;
                Text_Temp.text = ClientMonsterData.data[i].MobPostion.ToString();
            }
        } while (true);
    }
    public IEnumerator GetAllMonsterData()
    {
        do
        {
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_Data Where Map_Num = '1'");
            ClientMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
            yield return new WaitForSecondsRealtime(0.1f);
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
