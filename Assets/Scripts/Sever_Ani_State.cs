using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Sever_Ani_State : MonoBehaviour
{
    Animator animator;
    DataSet severPlayerDataset;

    string Anistate;
    [HideInInspector] public Maria5 maria = new Maria5("220.149.12.209", "OpenWorld_MapData", "nagne", "123");
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TestAni());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public IEnumerator TestAni()
    {
        do
        {
            severPlayerDataset = maria.SelectUsingAdapter("SELECT * FROM Player_Data Where Player_ID = '" + "288e8376f4309c2d61d7324709b7139597c2db15" + "\'");
            Anistate = JsonUtility.FromJson<PlayerClass>(severPlayerDataset.Tables[0].Rows[0]["data"].ToString()).AniState;
            Debug.Log(Anistate);
            yield return new WaitForSecondsRealtime(0.01f);
        } while(true);
    }

    public void Move()
    {
        if (Anistate == "1")
        {
            animator.SetBool("Run", true);
        }
        else if (Anistate == "!1")
        {
            animator.SetBool("Run", false);
        }
        
        if (Anistate == "2")
        {
            animator.SetBool("Walk", true);
        }
        else if (Anistate == "!2")
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }

        if (Anistate == "3")
        {
            animator.SetBool("Jump", true);
        }
        else if (Anistate == "!3")
        {
            animator.SetBool("Jump", false);
        }
    }
}
