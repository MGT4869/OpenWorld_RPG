using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public GameObject temp,temp2;
    [HideInInspector]public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Test()
    {
        do
        {
            string send_json = JsonUtility.ToJson(new DefaultMobClass(temp));
            maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Mob_ID", "\'" + 1 + "\'"));
            //maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "X", "\'" + temp.transform.position.x + "\'", "Mob_ID", "\'" + 1 + "\'"));
            //maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "Y", "\'" + temp.transform.position.y + "\'", "Mob_ID", "\'" + 1 + "\'"));
            yield return new WaitForSecondsRealtime(0.001f);

            DataSet ds_json = maria.SelectUsingAdapter(string.Format("SELECT * FROM Map1_Monster_Data WHERE Mob_ID = {0}", "\"" + 1 + "\""));
            DefaultMobClass tempClass = JsonUtility.FromJson<DefaultMobClass>(ds_json.Tables[0].Rows[0]["data"].ToString());
            temp2.transform.position = tempClass.MobPostion+ new Vector3(10f,10f,0);
            //Vector3 tempvector = new Vector3(Convert.ToInt32(ds_json.Tables[0].Rows[0]["X"]), Convert.ToInt32(ds_json.Tables[0].Rows[0]["Y"])+4f,0f);
            //temp2.transform.position = tempvector;
            //Debug.Log(ds_json.Tables[0].Rows[0]["data"].ToString());
        } while (true);
    }
}
