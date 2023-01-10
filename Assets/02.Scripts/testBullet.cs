using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        //MouseUp.AddListener(Boost);
        BeforeAttack.AddListener(OnTrigger);
        AfterAttack.AddListener(OffTrigger);
        coolTime = 2;
    }

    private void OnTrigger()
    {
        if (coolTurn > 0) return;
        isDoubleAttack = true;
    }

    private void OffTrigger()
    {
        if (coolTurn > 0) return;
        isDoubleAttack = false;
        SetCool();
    }

    private void Boost()
    {
        if (coolTurn > 0) return;
        rigid.velocity *= 2;
    }
}
