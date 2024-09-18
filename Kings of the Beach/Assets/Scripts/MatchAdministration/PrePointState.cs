using UnityEngine;
using KotB.StateMachine;

public class PrePointState : State
{
    public override void Enter()
    {
        Debug.Log("We're in the pre point state");
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
