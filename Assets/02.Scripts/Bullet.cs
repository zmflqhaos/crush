using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public float HP { get; set; }
    public float ATK { get; set; }

    [SerializeField] private float minDragPower;
    [SerializeField] private float maxDragPower;
    [SerializeField] private float pushPower;

    public UnityEvent MouseUp;
    public UnityEvent BeforeCrash;
    public UnityEvent AfterCrash;
    public UnityEvent BeforeAttack;
    public UnityEvent AfterAttack;
    public UnityEvent OnDie;

    private Vector2 defaultScale = new Vector2(0.5f, 0.5f);
    private GameObject line;
    public Rigidbody2D rigid;

    private float dragAngle;
    private Vector2 dis;

    private float power;
    private Vector2 angle;

    public Vector2 lastVelocity;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        line = transform.GetChild(0).gameObject;
        HP = 100;
        ATK = 35;
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag()
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

    private void OnMouseUp()
    {
        line.transform.localScale = defaultScale;
        if (power <= minDragPower) return;

        MouseUp?.Invoke(); // 발사 직후 발동하는 트리거

        angle = transform.position - Mouse.Instance.mousePosition;
        angle /= angle.magnitude;

        rigid.velocity = (angle * (power * pushPower));
    }

    private void FixedUpdate()
    {
        lastVelocity = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BeforeCrash?.Invoke(); //충돌 직전 발동하는 트리거
            AM.Instance.CrashSet(this, collision.contacts[0].normal);
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

    private bool DeadCheck()
    {
        if(HP<=0)
        {
            OnDie.Invoke();
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
