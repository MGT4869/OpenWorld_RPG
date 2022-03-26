using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    [HideInInspector] public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    private bool SeverMode;
    private AllMonsterData AllMonsterData;
    private ServerManager SM;
    // Start is called before the first frame update
    void Start()
    {
        SM = GameObject.Find("SeverManager").GetComponent<ServerManager>();
        SeverMode = SM.SeverMode;
        if (SeverMode)
        {
            //StartCoroutine(ServerPostion_Control());
        }
        else
        {
            StartCoroutine(ClientPostion_Control());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ServerPostion_Control()
    {
        do
        {
            yield return new WaitForSecondsRealtime(0.1f);
            string send_json = JsonUtility.ToJson(new DefaultMobClass(gameObject));
            DataSet ds_json = maria.SelectUsingAdapter(string.Format("SELECT * FROM Map1_Monster_Data WHERE Monster_Name = {0}", "\"" + gameObject.name + "\""));
            if (ds_json.Tables[0].Rows.Count == 0)
            {
                maria.create_Table(string.Format("INSERT INTO {0} (Monster_Name,data) VALUES({1},{2})", "Map1_Monster_Data", "\'" + gameObject.name + "\'", "\'" + send_json + "\'"));
            }
            else
            {
                maria.create_Table(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", "Map1_Monster_Data", "data", "\'" + send_json + "\'", "Monster_Name", "\'" + gameObject.name + "\'"));
            }

        } while (true);
    }

    public IEnumerator ClientPostion_Control()
    {
        do
        {
            int count = 0;
            yield return new WaitForSecondsRealtime(0.1f);
            DataSet ds_json = maria.SelectUsingAdapter("SELECT * FROM Map1_Monster_Data Where Map_Num = '1'");
            AllMonsterData = JsonUtility.FromJson<AllMonsterData>(ds_json.Tables[0].Rows[0]["data"].ToString());
            for (int i = 0; i < AllMonsterData.data.Count; i++)
            {
                if(AllMonsterData.data[i].Mob_name == gameObject.name)
                {
                    count++;
                    gameObject.transform.position = AllMonsterData.data[i].MobPostion;
                }

            }
            if (count == 0) Destroy(gameObject);

        } while (true);
    }
}
