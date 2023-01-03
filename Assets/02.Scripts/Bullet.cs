using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float minDragPower;
    [SerializeField] private float maxDragPower;
    [SerializeField] private float pushPower;

    private Vector2 defaultScale = new Vector2(0.5f, 0.5f);
    private GameObject line;
    private Rigidbody2D rigid;

    private float dragAngle;
    private Vector2 dis;

    private float power;
    private Vector2 angle;

    private Vector2 lastVelocity;

    private bool isAttack = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        line = transform.GetChild(0).gameObject;
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseDrag()
    {
        power = Mathf.Clamp(Vector2.Distance(transform.position, Mouse.Instance.mousePosition) * 2, minDragPower, maxDragPower);
        if (power < 1.5f)
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
        if (power < 1.5f) return;

        isAttack = true;
        angle = transform.position - Mouse.Instance.mousePosition;
        angle /= angle.magnitude;

        Debug.Log("Angle : " + angle + " Power : " + power);
        rigid.velocity = (angle * (power * pushPower));
    }

    private void Update()
    {
        lastVelocity = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Bullet")&&isAttack)
        {
            var target = collision.transform.GetComponent<Bullet>();
            var reflect = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rigid.velocity = (reflect * power);
            target.Hit(lastVelocity);
            isAttack = false;
        }
    }

    public void Hit(Vector2 velo)
    {
        rigid.velocity = velo;
        isAttack = true;
    }
}
