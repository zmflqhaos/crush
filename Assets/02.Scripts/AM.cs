using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AM : MonoSingleton<AM>
{
    private Bullet a = null, b = null;
    private Vector2 aNomal, bNomal;

    public void CrashSet(Bullet bullet, Vector2 nomal)
    {
        if (a == null)
        {
            a = bullet;
            aNomal = nomal;

        }
        else if (b == null)
        {
            b = bullet;
            bNomal = nomal;

            CrashResult();
        }
    }

    private void CrashResult()
    {
        a.BeforeAttack?.Invoke(); //
        b.BeforeAttack?.Invoke(); //���� ���� �ߵ��ϴ� Ʈ����

        if (a.lastVelocity.magnitude > b.lastVelocity.magnitude)
        {
            if (DamageCalculate())
            {
                var reflect = Vector2.Reflect(a.lastVelocity.normalized, aNomal);
                a.rigid.velocity = (reflect);
                b.rigid.velocity = (-reflect);
            }
        }
        else if (a.lastVelocity.magnitude < b.lastVelocity.magnitude)
        {
            if (DamageCalculate())
            {
                var reflect = Vector2.Reflect(b.lastVelocity.normalized, bNomal);
                b.rigid.velocity = (reflect);
                a.rigid.velocity = (-reflect);
            }
        }
        a.AfterAttack?.Invoke(); //
        b.AfterAttack?.Invoke(); // ���� ���� �ߵ��ϴ� Ʈ����
        a.AttackFinish();
        b.AttackFinish();
        a = null;
        b = null;
    }

    private bool DamageCalculate()
    {
        float aFinalDamage = a.ATK; //
        float bFinalDamage = b.ATK; //���⿡ �нú꽺ų ������ ���ϴ� ������ ���
        a.HP -= bFinalDamage; // 
        b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
        return (a.HP > 0 && b.HP > 0);
    }
}
