using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
public class ServerManager : MonoBehaviour
{
    public GameObject temp;
    public MonsterInfo[] MonsterList;
    public List<DefaultMobClass> MonsterData = new List<DefaultMobClass>();
    public List<GameObject> Client_MonsterList = new List<GameObject>();
    public List<GameObject> Server_PlayerList = new List<GameObject>();

    public AllPlayerData SeverAllPlayerData;
    public AllMonsterData SeverMonsterData;
    public AllMonsterData ClientMonsterData;
    [HideInInspector]public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    public bool SeverMode;
    public GameObject PlayerCharctor;
    public GameObject PlayerPrefab;
    // Start is called before the first frame update
    public void Start()
    {
        if (SeverMode)
        {
            SeverDefaultSetting();
        }
        else
        {
            PlayerDefaultChecking(); 
            StartCoroutine(Client_UpdateAllData());
            ClientDefaultSetting();
        }
    }
    public void PlayerDefaultChecking()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data Where Player_ID = '"+SystemInfo.deviceUniqueIdentifier+"\'");
        if(ds_json.Tables[0].Rows.Count == 0)
        {
            maria.create_Table(string.Format("INSERT INTO Player_Data(Player_ID) VALUES({0})", "\'"+ SystemInfo.deviceUniqueIdentifier + "\'"));
        }
        else
        {
            PlayerCharctor = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
            PlayerCharctor.AddComponent<PlayerInfo>();
            PlayerCharctor.name = JsonUtility.FromJson<PlayerClass>(ds_json.Tables[0].Rows[0]["data"].ToString()).Player_name;
            PlayerCharctor.transform.position = JsonUtility.FromJson<PlayerClass>(ds_json.Tables[0].Rows[0]["data"].ToString()).PlayerPos;
        }
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

    public IEnumerator ClientMonsterUpdate()
    {
        do
        {
            yield return new WaitForSecondsRealtime(1f);
            for (int i = 0; i < ClientMonsterData.data.Count; i++)
            {
                if(Client_MonsterList.Find(x => x.gameObject.name == ClientMonsterData.data[i].Mob_name) == null)
                {
                    GameObject tempObject = Instantiate(temp, temp.transform.position, Quaternion.identity);
                    tempObject.name = ClientMonsterData.data[i].Mob_name;
                    tempObject.transform.position = ClientMonsterData.data[i].MobPostion;
                    tempObject.AddComponent<MonsterControl>();
                    Client_MonsterList.Add(tempObject);
                }
            }

        } while (true);
    }
    public IEnumerator ClientUpdateMonsterPotion()
    {
        do
        {
            yield return new WaitForSecondsRealtime(0.05f);
            for (int i = 0; i < ClientMonsterData.data.Count; i++)
            {
                Client_MonsterList.Find(x => x.gameObject.name == ClientMonsterData.data[i].Mob_name).transform.position = ClientMonsterData.data[i].MobPostion;
            }
        } while (true);
    }
    public IEnumerator Client_UpdateAllData()
    {
        do
        {
            //DownLoad
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_Data Where Map_Num = '1'");
            ClientMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
            yield return new WaitForSecondsRealtime(0.1f);

            //Upload
            string send_json = JsonUtility.ToJson(new PlayerClass(PlayerCharctor));
            maria.create_Table(string.Format("UPDATE Player_Data SET data = \'{0}\' WHERE Player_ID = \'{1}\'", send_json, SystemInfo.deviceUniqueIdentifier));

        } while (true);
    }
    public void SeverDefaultSetting()
    {
        MonsterList = FindObjectsOfType<MonsterInfo>();
        foreach (MonsterInfo temp in MonsterList)
        {
            SeverMonsterData.data.Add(new DefaultMobClass(temp.gameObject));
        }
        string send_json = JsonUtility.ToJson(SeverMonsterData);
        maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Map_Num", "\'" + 1 + "\'"));

        StartCoroutine(Server_UpdateAllData());
        StartCoroutine(ServerPostion_Control());
        StartCoroutine(Server_PlayerPostion_Control());
    }
    public IEnumerator ServerPostion_Control()
    {
        do
        {
            SeverMonsterData.data.Clear();
            yield return new WaitForSecondsRealtime(0.1f);
            foreach (MonsterInfo temp in MonsterList)
            {
                SeverMonsterData.data.Add(new DefaultMobClass(temp.gameObject));
            }
            string send_json = JsonUtility.ToJson(SeverMonsterData);
            maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Map_Num", "\'" + 1 + "\'"));
        } while (true);
    }
    public IEnumerator Server_PlayerPostion_Control()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data");
        foreach (DataRow temp in ds_json.Tables[0].Rows)
        {
            SeverAllPlayerData.data.Add(JsonUtility.FromJson<PlayerClass>(temp["data"].ToString()));
        }
        Debug.Log(SeverAllPlayerData.data.Count);
        for(int i=0;i< SeverAllPlayerData.data.Count;i++)
        {
            GameObject tempObject = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
            tempObject.name = SeverAllPlayerData.data[i].Player_name;
            tempObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
            Server_PlayerList.Add(tempObject);
        }

        do
        {
            yield return new WaitForSecondsRealtime(0.1f);
            for(int i=0;i< SeverAllPlayerData.data.Count;i++)
            {
                if(Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name))
                {
                    Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name).gameObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
                }
            }


        } while (true);

    }
    public IEnumerator Server_UpdateAllData()
    {
        do
        {
            //UpLoad
            yield return new WaitForSecondsRealtime(0.1f);
            //DownLoad
            SeverAllPlayerData.data.Clear();
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data");
            foreach(DataRow temp in ds_json.Tables[0].Rows)
            {
                SeverAllPlayerData.data.Add(JsonUtility.FromJson<PlayerClass>(temp["data"].ToString()));
            }

        } while (true);
    }
    public AllMonsterData return_AllMonsterData()
    {
        return ClientMonsterData;
    }
}


