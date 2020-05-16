using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


/// <summary>
/// 状态机，这里可以并存多种状态
/// 状态按先来先执行的规则表现结果
/// </summary>
public class StateMachinesTest 
{
   
    public Dictionary<string, Translate> trans = new Dictionary<string, Translate>();
 

    private List<string> transString = new List<string>();

    public SmInput smInput = new SmInput();
    public OtherInput otherInput = new OtherInput();
    public void AddTrans(string key,Translate tra)
    {
        if (trans.ContainsKey(key)) return;
        trans.Add(key,tra);
    }
    

    /// <summary>
    /// 切换新的状态，(在权限允许的情况下)并清除旧的状态
    /// </summary>
    /// <param name="n">如果输入多个状态则用；好隔开</param>
    public void Enter(string n)
    {
        var ns = n.Split(';');
        var permission = 0;
        foreach (var value in ns)
        {
            transString.Add(value);
            TriggerState();
            if (trans[value].GetState().permission==1)
            {
                permission = 1;
            }
            Debug.Log($"Enter state:{value}");
        }
        Debug.Log(transString.Count);
        if(permission!=0)
        Exit(n);
    }

    private void Exit(string n)
    {
        var ns = n.Split(';');
        List<string> temp = new List<string>();
        transString.FindAll((str)=> { temp.Add(str); return true; });
        if (temp.Count >= 2)
        {
            foreach (var item in ns)
            {
                temp.Remove(item);
            }
 
            transString.RemoveAll((str) => { if (temp.Contains(str)) { Debug.Log($"Exit state:{str}"); return true; } return false; });
            
        }
        else Debug.Log("Exit state: not state");



        Debug.Log(transString.Count);

        //if (transString.Count < 2) n = "not state";
        //else transString.RemoveAll((str) => { return str != n; });

        //Debug.Log($"Exit state:{n}");
    }
    public void Start()
    {
        Enter("idle");
    }
    public void TriggerState()
    {
        for (int i = 0; i < transString.Count; i++)
        {
            trans[transString[i]].Trigger();
        }
    }

    public void TickState()
    {
        for (int i = 0; i < transString.Count; i++)
        {
            trans[transString[i]].Tick();
        }
    }
}

public class SmInput{

    public float Horizontal { get; private set; } 
    public float  Vertical{ get; private set; }

    public bool Dance { get; private set; }


    public bool Release { get; private set; }
    public void UpdateInput()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        Dance = Input.GetKeyDown(KeyCode.F1);

        Release = Input.GetKeyDown("r");
    }

}

public class OtherInput
{

    public bool isFreeze { get; private set; }
 
    public void Freeze(bool freeze)
    {
        isFreeze = freeze;
    }

}
