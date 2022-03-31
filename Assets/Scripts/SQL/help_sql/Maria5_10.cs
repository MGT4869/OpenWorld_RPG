using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class Maria5_10 : MonoBehaviour
{
    private MySqlConnection sqlconn = null;
    private string ip = null;
    private string DB_Name = null;
    private string id = null;
    private string pw = null;
    private string strConn = null;

    private void start_command(string quary)
    {
        sqlconn.Open();
        MySqlCommand command = new MySqlCommand(quary, sqlconn);
        command.ExecuteNonQuery();
        sqlconn.Close();
    }
    public Maria5_10()
    {
        ip = "database - 1.cfjahhyrrxrc.ap - northeast - 2.rds.amazonaws.com";
        DB_Name = "Unity";
        id = "hanginn";
        pw = "rudxor686!";
        sqlconn = new MySqlConnection(get_strConn());
    }
    private string get_strConn()
    {
        strConn = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;",
                               ip, id, pw, DB_Name);
        return strConn;
    }
    public void insert_Postion(string object_Name, string field_Value)
    {
        start_command(string.Format("INSERT INTO ObjectPostion (Object_Name,Object_Postion) VALUES({0},{1})", "\"" + object_Name + "\"", "\"" + field_Value + "\"" ));
    }
    public void postion_Update(string table_Name,string field_Name, string field_Value,string object_Name)
    {
        start_command(string.Format("INSERT INTO {0} ({1}) VALUES({2}) WHERE Object_Name ={3}", table_Name, field_Name, "\"" + field_Value + "\"", "\"" + object_Name + "\""));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
