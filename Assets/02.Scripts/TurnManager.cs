using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoSingleton<TurnManager>
{
    private Bullet[] bullets;
    public static int Turn { get; private set; }

    private void Start()
    {
        bullets = FindObjectsOfType<Bullet>();
    }

    public void AddTrun(int add = 1)
    {
        Turn += add;
        foreach (Bullet bullet in bullets)
            bullet.CoolDown();
    }
}
