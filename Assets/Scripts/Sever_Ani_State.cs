using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Sever_Ani_State : MonoBehaviour
{
    DataSet severPlayerDataset;

    string Anistate;
    [HideInInspector] public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestAni());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator TestAni()
    {
        do
        {
            severPlayerDataset = maria.SelectUsingAdapter("SELECT * FROM Player_Data Where Player_ID = '" + "AniTest" + "\'");
            Anistate = JsonUtility.FromJson<PlayerClass>(severPlayerDataset.Tables[0].Rows[0]["data"].ToString()).AniState;
            Debug.Log(Anistate);
            yield return new WaitForSecondsRealtime(0.01f);
        } while(true);
    }
}
