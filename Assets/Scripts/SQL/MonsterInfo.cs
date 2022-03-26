using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    public int MonsterType;
    public int Hp;
    public int NowHp;
    // Start is called before the first frame update
    void Start()
    {
        NowHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
