using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Rigidbody2D rb;
    // public Animator anim; // 나중에 애니메이션용으로
    public SpriteRenderer sr;
    
    private bool isGround;
    
    public int speed = 4;
    
    private void Update()
    {
        // 이동
        float axis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * axis, rb.velocity.y);

        if (axis != 0)
        {
            // anim.SetBool("walk", true);
            FlipCharacter(axis);
        }
        // else anim.SetBool("walk", false);
            
        // 점프
        isGround = Physics2D.OverlapCircle(
            (Vector2) transform.position + new Vector2(0, -0.5f), 
            0.07f, 
            1 << LayerMask.NameToLayer("Ground"));
            
        // anim.SetBool("jump", !isGround);
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround) Jump();
        
    }
    
    private void FlipCharacter(float axis) => sr.flipX = axis == -1;
    
    private void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 700);
    }
    
}
