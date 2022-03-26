using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Maria5
{
    public MySqlConnection sqlconn = null;
    public string ip = null;
    public string DB_Name = null;
    public string id = null;
    public string pw = null;
    public string strConn = null;
    public Maria5()
    {

    }
    public Maria5(string ip,string DB_Name,string id,string pw )
    {
        this.ip = ip;
        this.DB_Name = DB_Name;
        this.id = id;
        this.pw = pw;
        sqlconn = new MySqlConnection(get_strConn());
    }
    public string get_strConn()
    {
        strConn = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;",
                               ip, id, pw, DB_Name);
        return strConn;
    }
    private void start_command(string quary)
    {
        sqlconn.Open();
        MySqlCommand command = new MySqlCommand(quary, sqlconn);
        command.ExecuteNonQuery();
        sqlconn.Close();
    }
    public void Add_field(string table_name, string field_name)
    {
        start_command(string.Format("ALTER TABLE {0} ADD {1} varchar(20)", table_name, field_name));
    }
    public void Insert(string table_name,string field_name,string field_Value)
    {
        start_command(string.Format("INSERT INTO {0} ({1}) VALUES({2})", table_name, field_name, "\""+field_Value+"\""));
    }
    public void Delete(string table_name, string field_name, string field_Value)
    {
        start_command(string.Format("INSERT INTO {0} ({1}) VALUES({2})", table_name, field_name, "\"" + field_Value + "\""));
    }
    public void Update(string table_name, string field_name, string field_Value, string where ,string where_Value)
    {
        start_command(string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4} ", table_name, field_name, "\"" + field_Value + "\"" , where, "\"" + where_Value + "\""));
    }
    public void create_Table(string table_create)
    {
        //테이블 생성 예시  https://server-talk.tistory.com/279
        start_command(table_create);
    }
    public void chage_field()
    {
        start_command("ALTER TABLE 회원정보 CHANGE 회원번호 아이디 VARCHAR(10)");
    }
    public void User_State(string UserNum,string pw,string adress, string email)
    {
        start_command(string.Format("INSERT INTO 회원정보 (아이디,패스워드,주소,이메일) VALUES({0},{1},{2},{3})", "\""+UserNum+ "\"", "\""+pw+ "\"", "\""+ adress+ "\"", "\""+ email+ "\""));
    }
    public void User_State(string id, string pw, string nickName)
    {
        start_command(string.Format("INSERT INTO 회원정보 (닉네임,아이디,패스워드) VALUES({0},{1},{2})", "\"" + nickName + "\"", "\"" + id + "\"", "\"" + pw + "\""));
    }
    public void Save_Notice_Board(string User_ID, string title, string Contents)
    {
        start_command(string.Format("INSERT INTO 게시판 (작성자아이디,제목,내용,작성_날짜) VALUES({0},{1},{2},{3})", "\"" + User_ID + "\"", "\"" + title + "\"", "\"" + Contents + "\"", "\"" + DateTime.Now.ToString() + "\""));
    }
    public void Select_User_State()
    {
        
    }
    public void SendTime(string Time,int Num)
    {
        start_command(string.Format("INSERT INTO LateTimeTest (SendTime,Num) VALUES({0},{1})", "\"" + Time + "\"", "\"" + Num + "\""));
    }
    public void insert_Postion(string object_Name)
    {
        start_command(string.Format("INSERT INTO ObjectPostion (Object_Name,X,Y,Z) VALUES({0},0,0,0)", "\"" + object_Name + "\""));
    }
    public void postion_Update(string table_Name, Vector3 Point, string object_Name)
    {
        start_command(string.Format("UPDATE {0} SET X = '{1}',Y = '{2}', Z = '{3}'  WHERE Object_Name = '{4}' ", table_Name,Point.x.ToString(),Point.y.ToString(),Point.z.ToString(),object_Name));
    }
    public void Add_CoffeeList(string Continent, string Country,string Name,string Variety, string Processing, string Note,string Rostery,string RostDay,string FirstDay,string weight,string ID)
    {
        start_command(string.Format("INSERT INTO {0} (대륙,나라,이름,품종,가공법,노트,로스터리,로스팅일,최초개봉일,무게) VALUES({1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", "User_"+ID, "\"" + Continent + "\"", "\""+Country+ "\"", "\""+ Name+ "\"", "\""+Variety+ "\"", "\""+Processing+ "\"", "\""+ Note+ "\"", "\"" + Rostery+ "\"", "\""+ RostDay+ "\"", "\""+FirstDay+ "\"", "\""+ weight+ "\""));
    }
    public DataSet SelectUsingAdapter(string quary,string table_name)
    {
        DataSet ds = new DataSet();

        using (MySqlConnection conn = new MySqlConnection(strConn))
        {
            //MySqlDataAdapter 클래스를 이용하여
            //비연결 모드로 데이타 가져오기
            //string quary = "SELECT * FROM 회원정보    WHERE 번호!=0";
            MySqlDataAdapter adpt = new MySqlDataAdapter(quary, conn);
            adpt.Fill(ds, table_name);
        }
        return ds;
        /*foreach (DataRow r in ds.Tables[0].Rows)
        {
            Debug.Log("아이디 : "+r["아이디"]+" 패스워드 : "+ r["패스워드"] + " 주소 : " + r["주소"] + " 이메일 : " + r["이메일"]);
        }*/

    }
    public DataSet SelectUsingAdapter(string quary)
    {
        DataSet ds = new DataSet();

        using (MySqlConnection conn = new MySqlConnection(strConn))
        {
            //MySqlDataAdapter 클래스를 이용하여
            //비연결 모드로 데이타 가져오기
            //string quary = "SELECT * FROM 회원정보 WHERE 번호!=0";
            MySqlDataAdapter adpt = new MySqlDataAdapter(quary, conn);
            adpt.Fill(ds);
        }
        return ds;
    }
    

}
