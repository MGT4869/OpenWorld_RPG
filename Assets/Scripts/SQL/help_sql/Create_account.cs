using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class Create_account : MonoBehaviour
{
    public InputField ID, PW, ADRESS, EMAIL;
    [HideInInspector] public Maria5 maria = new Maria5("database-1.cfjahhyrrxrc.ap-northeast-2.rds.amazonaws.com", "Unity", "hanginn", "rudxor686!");
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void create_account()
    {
        int count = 0;
        int count1 = 0;
        DataSet ds = maria.SelectUsingAdapter(string.Format("SELECT * FROM 회원정보 WHERE 아이디 != {0}", "\""+ID.text+"\""),"회원정보");
        DataSet ds1 = maria.SelectUsingAdapter("SELECT * FROM 회원정보 WHERE 번호 !=0","회원정보");
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            count++;
        }
        foreach (DataRow r in ds1.Tables[0].Rows)
        {
            count1++;
        }
        Debug.Log(count);
        Debug.Log(count1);
        if (count == count1)
        {
            maria.User_State(ID.text, PW.text, ADRESS.text, EMAIL.text);
        }
        else
        {
            Debug.Log("동일한 아이디가 이미 있습니다.");
        }
    }
}
