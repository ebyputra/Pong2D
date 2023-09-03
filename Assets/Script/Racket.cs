using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Animator anim;
    public string axis = "Vertical";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    private void Update()
    {

        // Buat Disable Player 2 input kalo lagi Single Player
        if (axis == "Vertical2" && GameData.instance.isSinglePlayer)
        {
            return;
        }
        
        // Ngambil Variable Dari Axing yang udah di seting di unity Input dengan output  (-1,1)
        float v = Input.GetAxis(axis);
        rb.velocity = new Vector2(0, v) * speed;


        // Agar tidak keluar batas 
        if (transform.position.y > 1.5f)
        {
            transform.position = new  Vector2(transform.position.x, 1.5f);
        }

        // agar tidak keluar batas bawah
        if (transform.position.y < -1.5f)
        {
            transform.position = new Vector2(transform.position.x, -1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            anim.SetTrigger("Shoot");
        }
    }
}
