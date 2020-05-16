using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public StateMachinesTest stateMachines = new StateMachinesTest();

    public AnimationPlayer animationPlayer;

    [Range(0f, 100f)]
    public float moveeSpeed=10f, rotaSpeed = 10f;

    private void Awake()
    {
        animationPlayer = GetComponent<AnimationPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        State2 idle = new Idle(this.gameObject,1);
        State2 run = new Run(this.gameObject,1);
        State2 dance = new Dance(this.gameObject,1);
        State2 befreezed = new BeFreezed(this.gameObject,0);
        Translate tTIdle = new Translate(idle);
        Translate tTRun = new Translate(run);
        Translate tTDance = new Translate(dance);
        Translate tFreezed = new Translate(befreezed);
        
        stateMachines.AddTrans(nameof(idle),tTIdle);
        stateMachines.AddTrans(nameof(run),tTRun);
        stateMachines.AddTrans(nameof(dance),tTDance);
        stateMachines.AddTrans(nameof(befreezed), tFreezed);

        stateMachines.Start();

    }

    public void Idle()
    {
        animationPlayer.PlayAni(jinxmode.jinxIdle01.ToString());
    }
    public void Run()
    {
        animationPlayer.PlayAni(jinxmode.jinxRun.ToString());
    }
    public void Dance()
    {
        animationPlayer.PlayAni(jinxmode.jinxDance.ToString());
    }

  
    public void BeFreezed()
    {
        animationPlayer.PlayAni(jinxmode.jinxReCallIn.ToString());
    }

    public bool GetHealth()
    {
        if (stateMachines.otherInput.isFreeze)
        {
            return false;
        }
        else if (false)
        {
            return true;
        }
        else if (false)
        {
            return true;
        }
        else
        {
            return true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        stateMachines.TickState();
        stateMachines.smInput.UpdateInput();


        //if (Input.GetKeyDown("a"))
        //{
        //    Idle();
        //}
        //if (Input.GetKeyDown("b"))
        //{
        //    Run();
        //}
        //if (Input.GetKeyDown("c"))
        //{
        //    Dance();
        //}
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Sphere")
        {
            stateMachines.otherInput.Freeze(true);
            stateMachines.Enter("befreezed");
        }
    }
}
