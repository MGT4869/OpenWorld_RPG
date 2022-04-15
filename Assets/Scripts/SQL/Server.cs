using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Server : MonoBehaviour
{
    [HideInInspector] public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");//서버인증

    public GameObject PlayerPrefab; //서버에 표시할 플레이어들의 기본오브젝트 프리팹

    public List<GameObject> Server_PlayerList = new List<GameObject>(); // 서버에서 가지고온 모든 플레이어 오브젝트 리스트

    public AllPlayerData SeverAllPlayerData; //서버에서 가져온 모든 플레이어 정보를 담아둔곳
    public AllMonsterData SeverMonsterData;// 서버에 올리는 모든 몬스터 정보를 담아둔 곳

    public MonsterInfo[] MonsterList;

    public float updateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        SeverDefaultSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SeverDefaultSetting()
    {
        MonsterList = FindObjectsOfType<MonsterInfo>();
        foreach (MonsterInfo temp in MonsterList)
        {
            string sql = string.Format("SELECT MonsterName FROM Map1_Monster_State where MonsterName = '{0}'",temp.gameObject.name);
            if(maria.SelectUsingAdapter(sql).Tables[0].Rows.Count == 0)
            {
                DefaultMobClass tempClass = new DefaultMobClass(temp.gameObject);
                string send_json = JsonUtility.ToJson(new JsonVector3(temp.gameObject));
                string sql1 = string.Format("INSERT INTO Map1_Monster_State(MonsterName,Hp,Pos) VALUES('{0}','{1}','{2}')", tempClass.Mob_name, tempClass.NowHP, send_json);
                maria.create_Table(sql1);
            }


        }
        StartCoroutine(Server_UpdateAllData());
        StartCoroutine(ServerPostion_Control());
        StartCoroutine(Server_PlayerPostion_Control());
    }

    public IEnumerator Server_UpdateAllData()
    {
        do
        {
            //UpLoad
            yield return new WaitForSecondsRealtime(updateSpeed);
            //DownLoad
            SeverAllPlayerData.data.Clear();
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data");
            foreach (DataRow temp in ds_json.Tables[0].Rows)
            {
                SeverAllPlayerData.data.Add(JsonUtility.FromJson<PlayerClass>(temp["data"].ToString()));
            }

        } while (true);
    }

    public IEnumerator ServerPostion_Control()
    {
        do
        {
            yield return new WaitForSecondsRealtime(updateSpeed);
            foreach (MonsterInfo temp in MonsterList)
            {
                string sql = string.Format("SELECT MonsterName FROM Map1_Monster_State where MonsterName = '{0}'", temp.gameObject.name);
                if (maria.SelectUsingAdapter(sql).Tables[0].Rows.Count == 0)
                {
                    DefaultMobClass tempClass = new DefaultMobClass(temp.gameObject);
                    string send_json = JsonUtility.ToJson(new JsonVector3(temp.gameObject));
                    string sql1 = string.Format("INSERT INTO Map1_Monster_State(MonsterName,Hp,Pos) VALUES('{0}','{1}','{2}')", tempClass.Mob_name, tempClass.NowHP, send_json);
                    maria.create_Table(sql1);
                }
                else
                {
                    string send_json = JsonUtility.ToJson(new JsonVector3(temp.gameObject));
                    Debug.Log(send_json);
                    string sql1 = string.Format("UPDATE Map1_Monster_State SET Pos = '{0}' where MonsterName = '{1}'", send_json, temp.gameObject.name);
                    maria.create_Table(sql1);
                }


            }
        } while (true);
    }

    public IEnumerator Server_PlayerPostion_Control()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT data FROM Player_Data");
        foreach (DataRow temp in ds_json.Tables[0].Rows)
        {
            SeverAllPlayerData.data.Add(JsonUtility.FromJson<PlayerClass>(temp["data"].ToString()));
        }
        Debug.Log(SeverAllPlayerData.data.Count);
        for (int i = 0; i < SeverAllPlayerData.data.Count; i++)
        {
            GameObject tempObject = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
            tempObject.name = SeverAllPlayerData.data[i].Player_name;
            tempObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
            Server_PlayerList.Add(tempObject);
        }

        do
        {
            yield return new WaitForSecondsRealtime(updateSpeed);
            for (int i = 0; i < SeverAllPlayerData.data.Count; i++)
            {
                if (Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name))
                {
                    Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name).gameObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
                }
            }


        } while (true);

    }
}












/*public void SeverDefaultSetting()
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

public IEnumerator Server_UpdateAllData()
{
    do
    {
        //UpLoad
        yield return new WaitForSecondsRealtime(updateSpeed);
        //DownLoad
        SeverAllPlayerData.data.Clear();
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data");
        foreach (DataRow temp in ds_json.Tables[0].Rows)
        {
            SeverAllPlayerData.data.Add(JsonUtility.FromJson<PlayerClass>(temp["data"].ToString()));
        }

    } while (true);
}

public IEnumerator ServerPostion_Control()
{
    do
    {
        SeverMonsterData.data.Clear();
        yield return new WaitForSecondsRealtime(updateSpeed);
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
    for (int i = 0; i < SeverAllPlayerData.data.Count; i++)
    {
        GameObject tempObject = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
        tempObject.name = SeverAllPlayerData.data[i].Player_name;
        tempObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
        Server_PlayerList.Add(tempObject);
    }

    do
    {
        yield return new WaitForSecondsRealtime(updateSpeed);
        for (int i = 0; i < SeverAllPlayerData.data.Count; i++)
        {
            if (Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name))
            {
                Server_PlayerList.Find(x => x.gameObject.name == SeverAllPlayerData.data[i].Player_name).gameObject.transform.position = SeverAllPlayerData.data[i].PlayerPos;
            }
        }


    } while (true);

}*/
