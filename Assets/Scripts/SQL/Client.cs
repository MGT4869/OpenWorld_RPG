﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Client : MonoBehaviour
{
    public GameObject PlayerCharctor; //이계정 플레이어 게임오브젝트를 담아둔 게임오브젝트
    public GameObject PlayerPrefab;// 이계정에 접속한 플레이어를 나타내는 게임오브젝트 프리팹
    public GameObject MonsterPrefab; // 몬스터 프리팹

    public List<GameObject> Client_MonsterList = new List<GameObject>();// 모든 인게임존재하는 몬스터를 담아둔 클래스

    private DataSet MonsterDataList;// 모든 인게임존재하는 몬스터를 담아둔 클래스

    public float updateSpeed;

    [HideInInspector] public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    // Start is called before the first frame update
    void Start()
    {
        PlayerDefaultChecking();
        ClientDefaultSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDefaultChecking()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Player_Data Where Player_ID = '" + SystemInfo.deviceUniqueIdentifier + "\'");
        if (ds_json.Tables[0].Rows.Count == 0)
        {
            maria.create_Table(string.Format("INSERT INTO Player_Data(Player_ID) VALUES({0})", "\'" + SystemInfo.deviceUniqueIdentifier + "\'"));
            string send_json = JsonUtility.ToJson(new PlayerClass(PlayerPrefab));
            Debug.Log(send_json);
            maria.create_Table(string.Format("UPDATE Player_Data SET data = \'{0}\' WHERE Player_ID = \'{1}\'", send_json, SystemInfo.deviceUniqueIdentifier));

            DataSet ds_json_intfor = maria.SelectUsingAdapter("SELECT * FROM Player_Data Where Player_ID = '" + SystemInfo.deviceUniqueIdentifier + "\'");
            PlayerCharctor = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
            PlayerCharctor.AddComponent<PlayerInfo>();
            PlayerCharctor.name = JsonUtility.FromJson<PlayerClass>(ds_json_intfor.Tables[0].Rows[0]["data"].ToString()).Player_name;
            PlayerCharctor.transform.position = JsonUtility.FromJson<PlayerClass>(ds_json_intfor.Tables[0].Rows[0]["data"].ToString()).PlayerPos;
        }
        else
        {
            PlayerCharctor = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
            PlayerCharctor.AddComponent<PlayerInfo>();
            PlayerCharctor.name = JsonUtility.FromJson<PlayerClass>(ds_json.Tables[0].Rows[0]["data"].ToString()).Player_name;
            PlayerCharctor.transform.position = JsonUtility.FromJson<PlayerClass>(ds_json.Tables[0].Rows[0]["data"].ToString()).PlayerPos;
        }
        PlayerCharctor.transform.position = new Vector3(0, 0, 0);

        StartCoroutine(Client_UpdateAllData());
    }

    public IEnumerator Client_UpdateAllData()
    {
        do
        {
            //DownLoad
            MonsterDataList = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_State");
            yield return new WaitForSecondsRealtime(updateSpeed);

            //Upload
            string send_json = JsonUtility.ToJson(new PlayerClass(PlayerCharctor));
            maria.create_Table(string.Format("UPDATE Player_Data SET data = \'{0}\' WHERE Player_ID = \'{1}\'", send_json, SystemInfo.deviceUniqueIdentifier));

        } while (true);
    }

    public void ClientDefaultSetting()
    {
        DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_State");
        //ClientMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
        for (int i = 0; i < ds_json.Tables[0].Rows.Count; i++)
        {
            //Vector3 tempvector3 = new Vector3(ds_json.Tables[0].Rows[i]["Pos"].ToString());
            JsonVector3 tempVector3 = JsonUtility.FromJson<JsonVector3>(ds_json.Tables[0].Rows[i]["Pos"].ToString());
            GameObject tempObject = Instantiate(MonsterPrefab, tempVector3.Pos, Quaternion.identity);
            tempObject.name = ds_json.Tables[0].Rows[i]["MonsterName"].ToString();
            Client_MonsterList.Add(tempObject);
        }

        StartCoroutine(ClientMonsterUpdate());
        StartCoroutine(ClientUpdateMonsterPotion());
    }

    public IEnumerator ClientMonsterUpdate()
    {
        do
        {
            yield return new WaitForSecondsRealtime(updateSpeed);
            for (int i = 0; i < MonsterDataList.Tables[0].Rows.Count; i++)
            {
                if (Client_MonsterList.Find(x => x.gameObject.name == MonsterDataList.Tables[0].Rows[i]["MonsterName"].ToString()) == null)
                {
                    JsonVector3 tempVector3 = JsonUtility.FromJson<JsonVector3>(MonsterDataList.Tables[0].Rows[i]["Pos"].ToString());
                    GameObject tempObject = Instantiate(MonsterPrefab, tempVector3.Pos, Quaternion.identity);
                    tempObject.name = MonsterDataList.Tables[0].Rows[i]["MonsterName"].ToString();
                    Client_MonsterList.Add(tempObject);
                }
            }

        } while (true);
    }
    public IEnumerator ClientUpdateMonsterPotion()
    {
        do
        {
            yield return new WaitForSecondsRealtime(updateSpeed);
            for (int i = 0; i < MonsterDataList.Tables[0].Rows.Count; i++)
            {
                Client_MonsterList.Find(x => x.gameObject.name == MonsterDataList.Tables[0].Rows[i]["MonsterName"].ToString()).transform.position = JsonUtility.FromJson<JsonVector3>(MonsterDataList.Tables[0].Rows[i]["Pos"].ToString()).Pos;
            }
        } while (true);
    }
}
