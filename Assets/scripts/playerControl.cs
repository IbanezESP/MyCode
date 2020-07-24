using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource jumpAudio, hurtAudio, cherryAudio;
    public Collider2D coll;
    public Collider2D disColl;
    public Transform cellingCheck, groundCheck;
    public LayerMask ground;
    [Space] // 可使 Unity 的編輯器裡有間格 方便辨識
    public float speed;
    public float jumpForce;
    [Space]
    //[SerializeField]
    public int cherry;
    public int gam;
    private bool ishurt;  // 默認是false
    [Space]
    public Text cherryNum;
    public Text GamNum;
    [Space]
    public bool isGround, isJump;
    public bool jumpPressed;
    int jumpCound;



    // Start is called before the first frame update  開始執行時觸發
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame  取決於當時電腦渲染一幀的時間快慢
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCound > 0)
        {
            jumpPressed = true;
        }

        Crouch();
        cherryNum.text = cherry.ToString();
        GamNum.text = gam.ToString();
    }

    // FixedUpdate is called once per frame  每秒(固定50)幀
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!ishurt)
        {
            Movement();
        }
        Jump();
        SwitchAnim();
    }

    // 設置人物動作
    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");   // 取的 nuity 的內建按鍵 獲得 -1 ~ 1 的數(包含小數點)
        float faceDirection = Input.GetAxisRaw("Horizontal"); // 取的 nuity 的內建按鍵 可直接獲得 - 1,0,1 獲得三個整數
        // 設置人物移動
        //if (horizontalMove != 0)
        //{
        //    rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
        //    anim.SetFloat("running", Mathf.Abs(faceDirection));
        //}
        rb.velocity = new Vector2(faceDirection * speed, rb.velocity.y);

        // 設置人物擺頭方向
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
    }

    // 設置人物跳躍
    void Jump()
    {
        //if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        //{
        //    rb.velocity = new Vector2(0, jumpForce * Time.deltaTime);
        //    jumpAudio.Play();
        //    anim.SetBool("jumping", true);
        //}

        if (isGround)
        {
            jumpCound = 2;  // 多段跳設定
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            jumpAudio.Play();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCound--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCound > 0 && isJump)
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCound--;
            jumpPressed = false;
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
    }

    // 設置切換動畫效果
    void SwitchAnim()
    {

        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        //anim.SetBool("idle", false);

        // 離開地面時觸發下落動畫
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        //if (anim.GetBool("jumping"))
        //{
        //    if (rb.velocity.y < 0)
        //    {
        //        anim.SetBool("jumping", false);
        //        anim.SetBool("falling", true);
        //    }
        //}

        // 怪物傷害的觸發條件
        else if (ishurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                ishurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            //anim.SetBool("idle", true);
        }
    }

    // 碰撞觸發器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 收集物品
        if (collision.tag == "collection")
        {
            cherryAudio.Play();
            //Destroy(collision.gameObject);
            //cherry += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //cherryNum.text = cherry.ToString();
        }
        if (collision.tag == "Gam")
        {
            cherryAudio.Play();
            //Destroy(collision.gameObject);
            //gam += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //GamNum.text = gam.ToString();
        }
        if (collision.tag == "deadLine")
        {
            GetComponent<AudioSource>().enabled = false; // 關閉音源
            Invoke("Restart", 2f); // 延遲執行
        }
    }


    // 消滅敵人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemies")
        {
            enemies enemy = collision.gameObject.GetComponent<enemies>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
            }
            // 受傷
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                hurtAudio.Play();
                ishurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                hurtAudio.Play();
                ishurt = true;
            }
        }
    }

    void Restart()
    {
        // 重新加載場景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        cherry++;
    }
    public void GamCount()
    {
        gam++;
    }
}
