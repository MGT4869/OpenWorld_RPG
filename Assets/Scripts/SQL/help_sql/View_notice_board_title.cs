using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class View_notice_board_title : MonoBehaviour
{
    public InputField title, Contents;
    public GameObject bt;
    public GameObject view_Contents;
    public Text Contents_text;
    public Text Contents_Title_text;
    private List<string> Contents_List = new List<string>();
    private List<string> Contents_title_List = new List<string>();
    public List<GameObject> Title_List = new List<GameObject>();
    [HideInInspector] public Maria5 maria = new Maria5("database-1.cfjahhyrrxrc.ap-northeast-2.rds.amazonaws.com", "Unity", "hanginn", "rudxor686!");
    // Start is called before the first frame update
    void Start()
    {
        Craete_Title();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Craete_Title()
    {
        int cnt = -1;
        int PosY = 450;
        DataSet ds = maria.SelectUsingAdapter("SELECT * FROM 게시판 WHERE 번호 != 0","게시판");
        Debug.Log(ds.Tables[0].Rows.Count);
        bt.transform.GetChild(0).gameObject.GetComponent<Text>().text = "temp";
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            GameObject temp = Instantiate(bt, new Vector3(0, 0, 0), Quaternion.identity);
            cnt++;
            Title_List.Add(temp);
            Contents_List.Add(r["내용"].ToString());
            Contents_title_List.Add(r["제목"].ToString());
            temp.name = ""+cnt;
            temp.transform.GetComponent<Button>().onClick.AddListener(View_Contents);
            temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = r["제목"]+"           "+"작성날짜"+r["작성_날짜"];
            temp.transform.position = new Vector3(500,PosY, 0);
            temp.transform.SetParent(GameObject.Find("Canvas").transform);
            PosY -= 25;
        }

    }
    public void View_Contents()
    {
        GameObject.Find("Create_notice_board_button").SetActive(false);
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        foreach (GameObject temp in Title_List)
        {
            Destroy(temp);
        }
        view_Contents.SetActive(true);
        Contents_text.text = Contents_List[Convert.ToInt32(ButtonName)];
        Contents_Title_text.text = Contents_title_List[Convert.ToInt32(ButtonName)];


    }
    public void back_tittle()
    {
        view_Contents.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Create_notice_board_button").gameObject.SetActive(true);
        Title_List.Clear();
        Contents_List.Clear();
        Craete_Title();
    }
    public void enter_create_notice_board()
    {
        foreach (GameObject temp in Title_List)
        {
            Destroy(temp);
        }
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        GameObject.Find(ButtonName).SetActive(false);
        GameObject.Find("Canvas").transform.Find("Create_notice_board").gameObject.SetActive(true);

    }
    public void Save_Contents()
    {
        maria.Save_Notice_Board("MGT", title.text, Contents.text);
        title.text = null;
        Contents.text = null;
        GameObject.Find("Create_notice_board").SetActive(false);
        GameObject.Find("Canvas").transform.Find("Create_notice_board_button").gameObject.SetActive(true);
        Title_List.Clear();
        Contents_List.Clear();
        Craete_Title();
    }
}
