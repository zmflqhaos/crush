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

        angle = transform.position - Mouse.Instance.mousePosition;
        angle /= angle.magnitude;

        Debug.Log("Angle : " + angle + " Power : " + power);
        rigid.velocity = (angle * (power * pushPower));
    }

    private void FixedUpdate()
    {
        lastVelocity = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigid.velocity.x == 0 || rigid.velocity.y == 0) return;
        if (collision.transform.CompareTag("Bullet"))
        {
            var target = collision.transform.GetComponent<Bullet>();
            var reflect = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            var reflect2 = (Vector2)collision.transform.position - (Vector2)transform.position;
            reflect2 /= reflect2.magnitude;
            rigid.velocity = (reflect * (lastVelocity.magnitude / pushPower)/2);

            if (rigid.velocity.x == 0 || rigid.velocity.y == 0) return;
            target.Hit((reflect2 * lastVelocity.magnitude + lastVelocity)/2);
        }
    }

    public void Hit(Vector2 velo)
    {
        rigid.velocity = velo;
        velo = rigid.velocity;
        Debug.Log(name + velo);
    }
}
