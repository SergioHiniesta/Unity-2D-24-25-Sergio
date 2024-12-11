using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurpState : State
{
    public BurpState(BossController boss) : base(boss) {}

    public override void Entry()
    {
        base.Entry();
        Debug.Log("Burp State Entered");
        // activar animación follow
    }

    public override void Exit()
    {
        // terminar animación follow
    }

    public override void Update()
    {
        // buscar jugador y seguir
    }
}
