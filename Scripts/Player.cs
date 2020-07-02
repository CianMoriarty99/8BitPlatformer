using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed, jumpForce, dashCD, dashTimer, dashSpeed, xRaw, yRaw, x, y;

    public bool walk, jump, dash;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        

        
        if(!dash)
            Walk(dir);

        if (Input.GetButtonDown("Jump"))
            Jump();

        if(dashTimer <= 0)
            if(Input.GetButtonDown("Fire1")){
                dash = true;
                StartCoroutine("Dash", 0.3f);
                
                
            }

        if(dashTimer >= 0)
            dashTimer -= Time.deltaTime;
                



    }

    private void Walk(Vector2 dir)
    {
        if (walk)
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(){

        if(jump)
            rb.velocity += jumpForce * Vector2.up;

    }

    IEnumerator Dash(float t){
        rb.gravityScale = 0;
        rb.drag = 10f;
        Vector2 rawDir = new Vector2(xRaw, yRaw);
        dash = true;
        rb.velocity = rawDir.normalized * new Vector2(dashSpeed, dashSpeed);
        dashTimer = dashCD;
        yield return new WaitForSeconds(t);
        dash = false;
        rb.gravityScale = 6f;
        rb.drag = 0;
    }



}
