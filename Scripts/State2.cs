using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态，这里我们只表现行为结果，我们的状态是基于某个角色或系统的，
/// 分两种状态：
/// 一种是瞬间触发的
/// 一种是持续触发的
/// </summary>
public abstract  class State2 
{
    protected GameObject stateMan;

    public int permission { get;protected set; }
    public State2(GameObject stateMan,int permission)
    {
        this.stateMan = stateMan;
        this.permission = permission;
    }
    public virtual void Trigger()
    {

    }
    public virtual void Tick()
    {

    }
}
public class JinxState : State2
{
    protected TestPlayer character;
    public JinxState(GameObject stateMan,int permission) : base(stateMan, permission)
    {
        character = stateMan.GetComponent<TestPlayer>();
    }

    public override void Tick()
    {
        if (character.GetHealth()) IsHealthTick();

        else  IsNotHealthTick();

    }


    protected virtual void IsHealthTick()
    {

    }

    protected virtual void IsNotHealthTick()
    {

    }
}
public class Run : JinxState
{
 
    public Run(GameObject stateMan,int permission) : base(stateMan, permission)
    {
         
    }

    public override void Trigger()
    {
        character.Run();

    }
    protected override void IsHealthTick()
    {
        if (character.stateMachines.smInput.Vertical == 0 && character.stateMachines.smInput.Horizontal == 0)
        {
            character.stateMachines.Enter("idle");
            // character.stateMachines.Exit("run");
        }

        stateMan.transform.position += stateMan.transform.forward * character.moveeSpeed * Time.deltaTime * character.stateMachines.smInput.Vertical;

        float y = stateMan.transform.eulerAngles.y;
        y += character.rotaSpeed * Time.deltaTime * character.stateMachines.smInput.Horizontal;
        stateMan.transform.rotation = Quaternion.Euler(0f, y, 0f);
    }
}
public class Idle : JinxState
{
   
    public Idle(GameObject stateMan,int permission) : base(stateMan, permission)
    {
         
    }

    public override void Trigger()
    {
        character.Idle();
    }

    protected override void IsHealthTick()
    {
        if (character.stateMachines.smInput.Vertical != 0 || character.stateMachines.smInput.Horizontal != 0)
        {
            character.stateMachines.Enter("run");
        }
        if (character.stateMachines.smInput.Dance)
        {
            character.stateMachines.Enter("dance");
        }
        if (!character.animationPlayer.isPlaying)
        {
            character.Idle();
        }
        Debug.Log("Idle ...");
    }
}

public class Dance : JinxState
{
    public Dance(GameObject stateMan,int permission) : base(stateMan, permission)
    {
        character = stateMan.GetComponent<TestPlayer>();
    }

    public override void Trigger()
    {
        character.Dance();
    }

    protected override void IsHealthTick()
    {
        if (!character.animationPlayer.isPlaying)
        {
            character.stateMachines.Enter("idle");
        }
        Debug.Log("dance ...");
    }
}


public class BeFreezed : JinxState
{
 
    public BeFreezed(GameObject stateMan,int permission) : base(stateMan, permission)
    {
        
    }

    public override void Trigger()
    {
        character.BeFreezed();
    }
    protected override void IsNotHealthTick()
    {
        if (!character.animationPlayer.isPlaying)
        {
            character.stateMachines.otherInput.Freeze(false);
        }
        if (character.stateMachines.smInput.Release)
        {
            character.stateMachines.otherInput.Freeze(false);
        }
        Debug.Log("Being Freeeeeezzzzzzzzzzzzzzzzzzzzzze......");
    }
}
public  class Translate
{
    private State2 state;
    public Translate(State2 state)
    {
        this.state = state;
    }
    public State2 GetState()
    {
        return state;
    }
    public void Trigger (){

        state.Trigger();
    }

    public void Tick()
    {
        state.Tick();
    }
}
