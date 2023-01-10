using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : PoolObject
{
    public float MaxHP { get; set; }
    public float HP { get; set; }
    public float ATK { get; set; }

    protected float minDragPower = 1.25f;
    protected float maxDragPower = 6;
    protected float pushPower = 2;

    protected Vector2 defaultScale = new Vector2(0.5f, 0.5f);
    protected GameObject line;
    protected Transform hpBar;
    protected SpriteRenderer hpImage;

    protected float dragAngle;
    protected Vector2 dis;

    protected float power;
    protected Vector2 angle;

    protected int coolTurn;
    protected int coolTime;

    public Rigidbody2D rigid { get; protected set; }
    public Vector2 lastVelocity { get; protected set; }

    public bool isFirstAttack { get; protected set; }
    public bool isDoubleAttack { get; protected set; }

    public UnityEvent MouseUp;
    public UnityEvent BeforeCrash;
    public UnityEvent AfterCrash;
    public UnityEvent BeforeBattle;
    public UnityEvent AfterBattle;
    public UnityEvent BeforeAttack;
    public UnityEvent AfterAttack;
    public UnityEvent BeforeDefence;
    public UnityEvent AfterDefence;
    public UnityEvent OnOutDie;
    public UnityEvent OnBattleDie;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        line = transform.GetChild(0).gameObject;
        hpBar = transform.GetChild(1).GetChild(0);
        hpImage = hpBar.GetComponentInChildren<SpriteRenderer>();
        MaxHP = 100; //
        HP = MaxHP; //
        ATK = 35; // 나중에 변경
    }

    protected void StatReset() // 수치 초기화
    {
        HP = MaxHP;
        hpBar.localScale = new Vector3(Mathf.Clamp(HP / MaxHP, 0, 1), 1, 1);
    }

    protected void OnMouseDown()
    {
        BulletInfoManager.Instance.ShowBulletInfo(this);
    }

    protected void OnMouseDrag()
    {
        power = Mathf.Clamp(Vector2.Distance(transform.position, Mouse.Instance.mousePosition) * 4, minDragPower, maxDragPower);
        if (power <= minDragPower)
        {
            line.transform.localScale = defaultScale;
            return;
        }
        dis = Mouse.Instance.mousePosition - transform.position;
        dragAngle = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
        line.transform.localScale = new Vector2(power, 0.5f);
        line.transform.rotation = Quaternion.Euler(0, 0, dragAngle);
    }

    protected void OnMouseUp()
    {
        line.transform.localScale = defaultScale;
        if (power <= minDragPower) return;

        TurnManager.Instance.AddTrun();
        Debug.Log(TurnManager.Turn);

        angle = transform.position - Mouse.Instance.mousePosition;
        angle /= angle.magnitude;

        rigid.velocity = (angle * (power * pushPower));
        MouseUp?.Invoke(); // 발사 직후 발동하는 트리거
    }

    protected void FixedUpdate()
    {
        lastVelocity = rigid.velocity;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BeforeCrash?.Invoke(); //충돌 직전 발동하는 트리거
            AM.Instance.CrashSet(this, collision.contacts[0].normal);
        }
        else if (collision.transform.CompareTag("Out"))
        {
            OnOutDie.Invoke();
            Pooling();
        }
        else if (collision.transform.CompareTag("Map"))
        {
            rigid.velocity = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal) * pushPower;
            AfterCrash?.Invoke();
        }
    }

    public void AttackFinish()
    {
        Debug.Log(name + HP);
        if (!DeadCheck())
            AfterCrash?.Invoke(); //충돌 직후 발동하는 트리거
    }

    public void Hit(Vector2 velo)
    {
        rigid.velocity = velo;
    }

    protected bool DeadCheck()
    {
        BulletInfoManager.Instance.ShowBulletInfo(this);
        if (HP<=0)
        {
            OnBattleDie.Invoke();
            if(HP<=0)
            {
                Pooling();
                return true;
            }
        }
        hpBar.localScale = new Vector3(Mathf.Clamp(HP / MaxHP, 0, 1), 1, 1);
        return false;
    }

    protected void SetCool()
    {
        coolTurn = coolTime;
    }

    public void CoolDown()
    {
        if (coolTurn > 0) coolTurn--;
    }

    public override void Pooling()
    {
        StatReset();
        base.Pooling();
    }

}
