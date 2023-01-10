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
        a.BeforeBattle?.Invoke(); //
        b.BeforeBattle?.Invoke(); //���� ���� �ߵ��ϴ� Ʈ����

        if (a.lastVelocity.magnitude > b.lastVelocity.magnitude)
        {
            a.BeforeAttack?.Invoke();
            b.BeforeDefence?.Invoke();
            if (DamageCalculate())
            {
                var reflect = Vector2.Reflect(a.lastVelocity.normalized, aNomal);
                a.rigid.velocity = (reflect);
                b.rigid.velocity = (-reflect + a.lastVelocity / 2);
            }
            a.AfterAttack?.Invoke();
            b.AfterDefence?.Invoke();
        }
        else if (a.lastVelocity.magnitude < b.lastVelocity.magnitude)
        {
            b.BeforeAttack?.Invoke();
            a.BeforeDefence?.Invoke();
            if (DamageCalculate())
            {
                var reflect = Vector2.Reflect(b.lastVelocity.normalized, bNomal);
                b.rigid.velocity = (reflect);
                a.rigid.velocity = (-reflect + b.lastVelocity / 2);
            }
            b.AfterAttack?.Invoke();
            a.AfterDefence?.Invoke();
        }
        a.AfterBattle?.Invoke(); //
        b.AfterBattle?.Invoke(); // ���� ���� �ߵ��ϴ� Ʈ����
        a.AttackFinish();
        b.AttackFinish();
        a = null;
        b = null;
    }

    private bool DamageCalculate()
    {
        float aFinalDamage = a.ATK; //
        float bFinalDamage = b.ATK; //���⿡ �нú꽺ų ������ ���ϴ� ������ ���
        if(a.isDoubleAttack)
        {
            if(b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
            }
            else if(b.isFirstAttack)
            {
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
                a.HP -= bFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //���⿡ �ӵ���� ���� ������
            }
            else
            {
                b.HP -= aFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
            }
        }
        else if(a.isFirstAttack)
        {
            if (b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //���⿡ �ӵ���� ���� ������
            }
            else if (b.isFirstAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
            }
            else
            {
                b.HP -= aFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //���⿡ �ӵ���� ���� ������
            }
        }
        else
        {
            if (b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; //���⿡ �ӵ���� ���� ������
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
            }
            else if (b.isFirstAttack)
            {
                a.HP -= bFinalDamage; // ���⿡ �ӵ���� ���� ������
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                b.HP -= aFinalDamage; // 
            }
            else
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //���⿡ �ӵ���� ���� ������
            }
        }
        if (a.HP <= 0 || b.HP <= 0)
        {
            return false;
        }
        return true;
    }
}
