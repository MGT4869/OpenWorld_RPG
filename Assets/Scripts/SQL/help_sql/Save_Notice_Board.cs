using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_Notice_Board : MonoBehaviour
{
    public InputField title, Contents;
    [HideInInspector] public Maria5 maria = new Maria5("database-1.cfjahhyrrxrc.ap-northeast-2.rds.amazonaws.com", "Unity", "hanginn", "rudxor686!");
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Save_Contents()
    {
        maria.Save_Notice_Board("MGT", title.text, Contents.text);
    }
}
