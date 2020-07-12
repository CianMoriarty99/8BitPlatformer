using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform flag1, flag2, flag3, checkpoint, angelStop;
    Rigidbody2D rb;
    public float speed, jumpForce, dashCD, dashTimer, dashSpeed, xRaw, yRaw, x, y, rotateSpeed, angelRiseSpeed;

    public bool startSequence, walk, onGround, onLeftWall, onRightWall, onEnd, dashing, canDash, canDash2, dashAquired, heart, angelRise, settingAudio;

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    public LayerMask groundLayer, endLayer;

    Droplet dropletRef;
    SpriteRenderer spriteRef;


    public GameObject angelMap, angel, overlay, colliderTile, endTile, title, guide, audioObj1, audioObj2, audioObj3;

    public Animator animator, musicAnim1, musicAnim2, musicAnim3;

    CameraController ppCameraRef;

    public ParticleSystem dust, sparkle;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashAquired = false;
        dropletRef = GetComponent<Droplet>(); 
        ppCameraRef = GameObject.Find("Main Camera").GetComponent<CameraController>();
        spriteRef = GetComponent<SpriteRenderer>();
        startSequence = true;
        endTile.SetActive(false);
        settingAudio = true;
              
    }

    // Update is called once per frame
    void Update(){

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Vector2 rawDir = new Vector2(xRaw, yRaw);

        animator.SetFloat("Speed", Mathf.Abs(xRaw));
        
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onEnd = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, endLayer);
       
        if (rb.velocity.y > 0.01){
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        if (rb.velocity.y < 0.01){
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        if (rb.velocity.x < -0.01){
            spriteRef.flipX = true;
        }
        if (rb.velocity.x > 0.01) {
            spriteRef.flipX = false;
        }




        if(startSequence){
            
            dashing = true;
            rb.gravityScale = 1f; 
            this.transform.Rotate(new Vector3(0,0,1) * 103f * Time.deltaTime);
            rb.freezeRotation = false;
        }
        if (onGround) {
            if(settingAudio){
                StartCoroutine("SetAudioDelayed", audioObj3);
                settingAudio = false;
            }
            rb.gravityScale = 6f;
            startSequence = false;
            rb.freezeRotation = true;
            dashing = false;
            this.transform.rotation = Quaternion.identity;
            canDash = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            canDash2 = false;
        }
        else {
            canDash2 = true;
        }


        if(!dashing)
            Walk(dir);

        if (Input.GetButtonDown("Jump"))
            Jump();

        if(canDash && canDash2){
            if(Input.GetButtonDown("Fire1")){
                canDash = false;
                if(dashAquired){
                    dashing = true;
                    StartCoroutine("Dash", rawDir);
                }
            }
        }


        

        if (this.transform.position.x > flag2.position.x && this.transform.position.y < flag2.position.y){
            dashAquired = true;
            musicAnim3.SetTrigger("fadeOut");
            StartCoroutine("SetAudioDelayed", audioObj1);

            checkpoint = flag2;
        }

        if (heart){
            checkpoint = flag3;
            dropletRef.start = true;
        }

        if(angelRise){
            musicAnim1.SetTrigger("fadeOut");
            StartCoroutine("SetAudioDelayed", audioObj2);

            animator.SetBool("isFalling", false);
            angelMap.transform.position += Vector3.up * angelRiseSpeed;
            angel.transform.position += Vector3.up * angelRiseSpeed;
            this.transform.position = angel.transform.position;

            Color tmp = overlay.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.25f;
            overlay.GetComponent<SpriteRenderer>().color = tmp;
            

            if(angel.transform.position.y >= angelStop.position.y){
                musicAnim2.SetTrigger("fadeOut");
                EndSequence();
                }  

        }

        if(onEnd){
            startSequence = false;
            this.transform.rotation = Quaternion.identity;
            rb.freezeRotation = true;
            CreateDust();
            animator.SetBool("isFalling", true);
            dashing = false;
            canDash2 = false;
            dropletRef.start = false;
            onGround = false;
            ppCameraRef.ppCamera.assetsPPU = 15;
            title.SetActive(true);
            guide.SetActive(false);
            Destroy(this);
        }
        


    }

    IEnumerator SetAudioDelayed(GameObject aud){
        yield return new WaitForSeconds(1f);
        aud.SetActive(true);

    }

    private void Walk(Vector2 dir){
        if (walk)
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(){

        if (onGround){
            CreateDust();
            rb.velocity += jumpForce * Vector2.up;
        }
        
        else if (onRightWall){
            CreateDust();
            dashing = true;
            dashSpeed = 60f;
            StartCoroutine("Dash", new Vector2(-1f, 1f));
        }

        else if (onLeftWall){
            CreateDust();
            dashing = true;
            dashSpeed = 60f;
            StartCoroutine("Dash", new Vector2(1f, 1f));
        }

    }

    void CreateDust(){
        dust.Play();
    }

    void CreateSparkle(){
        sparkle.Play();
    }

    private void EndSequence(){

        angelRise = false;
        rb.velocity = new Vector2(0,0);
        overlay.SetActive(false);
        endTile.SetActive(true);
        startSequence = true;
    }


    IEnumerator Dash(Vector2 rawDir){
        CreateDust();
        rb.gravityScale = 0;
        rb.drag = 10f;
        rb.velocity = rawDir.normalized * new Vector2(dashSpeed, dashSpeed/1.3f);
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = 6f;
        rb.drag = 0;
        dashSpeed = 100f;
        dashing = false;
        
    }

    IEnumerator Death(){
        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.position = checkpoint.position; 
        rb.velocity = new Vector2(0,0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Lava"){
            StartCoroutine("Death");

        }
        if(col.gameObject.tag == "Heart"){
            Destroy(col.gameObject);
            heart = true;
        }
        if(col.gameObject.tag == "Angel"){
            if(heart){
                CreateSparkle();
                angelRise = true;
                heart = false;

            }

        }

            
    }





}
