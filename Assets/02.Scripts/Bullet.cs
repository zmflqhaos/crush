using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float minDragPower;
    [SerializeField] private float maxDragPower;

    private bool isClicked;
    private Rigidbody2D rigid;
    private float power;
    private Vector2 angle;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        isClicked = true;
    }
    private void OnMouseDrag()
    {
        if (!isClicked) return;

    }

    private void OnMouseUp()
    {
        if (!isClicked) return;
        isClicked = false;

        power = Mathf.Clamp(Vector2.Distance(transform.position, Mouse.Instance.mousePosition), minDragPower, maxDragPower);
        angle = transform.position - Mouse.Instance.mousePosition;
        Debug.Log("Angle : " + angle + " Power : " + power);
        rigid.velocity = (angle * power);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet hit = collision.gameObject.GetComponent<Bullet>();
        rigid.velocity.Set(0,0);
    }

    public void GetPower()
    {

    }
}
