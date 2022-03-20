using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum DIRECTION_TYPE {
        STOP,
        RIGHT,
        LEFT
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    float speed;
    bool isGround;
    bool isDead;
    [SerializeField] GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if (!isDead) {
            if (x == 0) {
                //止まっている
                direction = DIRECTION_TYPE.STOP;
            } else if (x > 0) {
                //右に移動
                direction = DIRECTION_TYPE.RIGHT;
            } else if (x < 0) {
                //左に移動
                direction = DIRECTION_TYPE.LEFT;
            }
            //スペース押したらジャンプする
            if (Input.GetKeyDown(KeyCode.Space) && isGround) {
                Jump();
            }
        }

    }

    private void FixedUpdate() {
        switch (direction) {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                anim.SetBool("move", false);
                break;

            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                anim.SetBool("right", true);
                anim.SetBool("move", true);
                break;

            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                anim.SetBool("right", false);
                anim.SetBool("move", true);
                break;
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    void Jump() {
        //ジャンプするときにリジッドボディの速度を0に
        rb.velocity = Vector2.zero;
        //重力を逆にする
        rb.gravityScale = -rb.gravityScale;
        //キャラを上下反転
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
        //反転する軸が足元になっており、床の反対側にいってしまっていたので、スプライトエディタで
        //ピボットの位置をbottomcenterからcenterに変更。
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ground") {
            isGround = true;
        }

        if (collision.gameObject.tag == "Trap") {
            Debug.Log("トラップだ");
            isDead = true;
            StartCoroutine(GameOver());
        }

        if (collision.gameObject.tag == "BounceBar") {
            Jump();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGround = false;
        }
    }

    IEnumerator GameOver() {
        //動きを止める
        direction = DIRECTION_TYPE.STOP;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        //点滅させる
        int count = 0;
        while(count < 10) {
            //消える
            spriteRenderer.color = new Color32(255, 120, 120, 50);
            yield return new WaitForSeconds(0.1f);
            //着く
            spriteRenderer.color = new Color32(255, 120, 120, 255);

            count++;
        }
        //リスタートさせる
        gm.GameOver();
    }


    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "BounceBar") {
            Jump();
        }
    }
}
