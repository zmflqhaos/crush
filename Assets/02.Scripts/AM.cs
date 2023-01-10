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
        b.BeforeBattle?.Invoke(); //공격 직전 발동하는 트리거

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
        b.AfterBattle?.Invoke(); // 공격 직후 발동하는 트리거
        a.AttackFinish();
        b.AttackFinish();
        a = null;
        b = null;
    }

    private bool DamageCalculate()
    {
        float aFinalDamage = a.ATK; //
        float bFinalDamage = b.ATK; //여기에 패시브스킬 등으로 변하는 데미지 계산
        if(a.isDoubleAttack)
        {
            if(b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
            }
            else if(b.isFirstAttack)
            {
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
                a.HP -= bFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //여기에 속도비례 보정 들어가야함
            }
            else
            {
                b.HP -= aFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
            }
        }
        else if(a.isFirstAttack)
        {
            if (b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //여기에 속도비례 보정 들어가야함
            }
            else if (b.isFirstAttack)
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
            }
            else
            {
                b.HP -= aFinalDamage; // 
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; //여기에 속도비례 보정 들어가야함
            }
        }
        else
        {
            if (b.isDoubleAttack)
            {
                a.HP -= bFinalDamage; //여기에 속도비례 보정 들어가야함
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
            }
            else if (b.isFirstAttack)
            {
                a.HP -= bFinalDamage; // 여기에 속도비례 보정 들어가야함
                if (a.HP <= 0 || b.HP <= 0)
                {
                    return false;
                }
                b.HP -= aFinalDamage; // 
            }
            else
            {
                a.HP -= bFinalDamage; // 
                b.HP -= aFinalDamage; //여기에 속도비례 보정 들어가야함
            }
        }
        if (a.HP <= 0 || b.HP <= 0)
        {
            return false;
        }
        return true;
    }
}
