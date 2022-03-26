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
    bool gravityButton;
    [Header("TrapSE")] public AudioClip trapSE;
    [Header("GravitySE")] public AudioClip gravitySE;
    [Header("FallSE")] public AudioClip fallSE;
    [Header("MorunMorunSE")] public AudioClip morunmorunSE;
    [Header("StartVoices")] public AudioClip[] startVoices;
    [Header("TrapVoices")] public AudioClip[] trapVoices;
    [Header("PoisonVoices")] public AudioClip[] poisonVoices;
    [Header("FallVoices")] public AudioClip[] fallVoices;
    public Joystick joystick;
    [SerializeField] GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gm.RandomizeSfx(startVoices);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if (!isDead) {
            if (x > 0 || joystick.Horizontal > 0) {
                //右に移動
                direction = DIRECTION_TYPE.RIGHT;
            } else if (x < 0 || joystick.Horizontal < 0) {
                //左に移動
                direction = DIRECTION_TYPE.LEFT;
            } else if (x == 0 || joystick.Horizontal == 0) {
                //止まっている
                direction = DIRECTION_TYPE.STOP;
            }
                //スペース押したらジャンプする
                if ((Input.GetKeyDown(KeyCode.Space)||gravityButton) && isGround) {
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
        gm.RandomizeSfx(gravitySE);
        //ジャンプするときにリジッドボディの速度を0に
        rb.velocity = Vector2.zero;
        //重力を逆にする
        rb.gravityScale = -rb.gravityScale;
        //キャラを上下反転
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
        //反転する軸が足元になっており、床の反対側にいってしまっていたので、スプライトエディタで
        //ピボットの位置をbottomcenterからcenterに変更。
        gravityButton = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ground") {
            isGround = true;
        }

        if (collision.gameObject.tag == "Trap" && !isDead) {
            Debug.Log("トラップだ");
            isDead = true;
            gm.RandomizeSfx(trapSE);
            gm.RandomizeSfx(trapVoices);
            StartCoroutine(GameOver());
            anim.SetBool("down",true);
            gm.RandomizeSfx(morunmorunSE);
        }

        if (collision.gameObject.tag == "Hole" && !isDead) {
            Debug.Log("落ちた");
            isDead = true;
            gm.RandomizeSfx(fallSE);
            gm.RandomizeSfx(fallVoices);
            StartCoroutine(GameOver());
            anim.SetBool("down", true);
        }

        if (collision.gameObject.tag == "Poison" && !isDead) {
            Debug.Log("毒だ");
            isDead = true;
            gm.RandomizeSfx(poisonVoices);
            StartCoroutine(GameOver());
            anim.SetBool("down", true);
            gm.RandomizeSfx(morunmorunSE);
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
        while(count < 20) {
            //消える
            spriteRenderer.color = new Color32(255, 255, 255, 50);
            yield return new WaitForSeconds(0.1f);
            //着く
            spriteRenderer.color = new Color32(255, 255, 255, 255);

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

    public void OnClickGravityButton() {
        gravityButton = true;
    }

    /*
    void FixedUpdate() {
        if (isClearMotion) {//ここは動画、ブログと違う。クリアモーションがオンになると動かないようにする。
            rb.velocity = new Vector2(0, -grovity);
        } else if (!isDown && !GameManager.instance.isGameOver) {//ダウン中ではない。レッスン44で追加。ここは死なない限り有効
            //設置判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();


            if (isHead == true && isGround == true) {//自分で追加。頭の衝突判定と接地判定が同時に有効になる場合はジャンプできなくなる。
                head.isGround = false;//そのため、同時に有効になったら、頭の衝突判定を外す。インスペクター上でチェックを入れても、すぐに外れる。
            }


            //各種座標軸の速度を求める
            float ySpeed = GetYSpeed();
            float xSpeed = GetXSpeed();

            SetAnimation();

            //54
            Vector2 addVelocity = Vector2.zero;
            if (moveObj != null) {
                addVelocity = moveObj.GetVelocity();
            }
            //velocityをスクリプトで上書きし、物理法則を無視させる
            //参考：https://www.youtube.com/watch?v=klTg9hl_clU
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;//レッスン40で第二引数をySpeedに変更
        } else {
            rb.velocity = new Vector2(0, -grovity);//レッスン44で追加。ダウン中は重力のみ影響
        }
        if (!isClearMotion && GameManager.instance.isStageClear) {//ここはブログとは違う。上に死なない限り有効な部分があるので、そこから分岐させてしまうとクリアしても有効にならない。
            isClearMotion = true;
            anim.Play("StageClear");
            //rb.velocity = new Vector2(0, -grovity);をここに書いても、死なない限り有効の部分で打ち消されるため、isClearMotionが有効ならば動かないif関数を一番上にした。
        }
    }

    public float GetXSpeed() {

        float horizontalKey = Input.GetAxis("Horizontal");//左右方向のインプットを取得
        float xSpeed = 0.0f; //Speed変数を入れる変数
        if (canControl) {//自分で追加。canControlがfalseになると、入力を受け付けない。
            if (horizontalKey > 0 || joystick.Horizontal > 0)//右方向の入力があった場合　//ジョイスティックの判定も追加
            {
                isRight = true;
                isLeft = false;
                dashTime += Time.deltaTime;//41
                                           //参考動画では画像を反転させて左右への移動を処理
                                           //transform.localScale = new Vector3(1,1,1);
                xSpeed = speed;//右なら正の方向のSpeed変数
            } else if (horizontalKey < 0 || joystick.Horizontal < 0)//左方向の入力があった場合　//ジョイスティックの判定も追加
              {
                isRight = false;
                isLeft = true;
                dashTime += Time.deltaTime;//41
                                           //transform.localScale = new Vector3(-1,1,1);
                xSpeed = -speed;//右なら負の方向のSpeed変数
            } else {
                isLeft = false;
                isRight = false;
                dashTime = 0.0f;
                xSpeed = 0.0f;
            }

            if (stop) {
                xSpeed = 0.0f;
                dashTime = 0.0f;
            }

            //レッスン41で追加。前回のキー入力と方向が違うと加速を０にする。
            if ((horizontalKey > 0 && beforeKey < 0) || (horizontalKey < 0 && beforeKey > 0 || (horizontalKey > 0 && beforeJoy < 0) || (horizontalKey < 0 && beforeJoy > 0)
                || (joystick.Horizontal > 0 && beforeJoy < 0) || (joystick.Horizontal < 0 && beforeJoy > 0))) {
                dashTime = 0.0f;
            }

            beforeKey = horizontalKey;
            beforeJoy = joystick.Horizontal;
        }

        //アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);

        if (stop) {
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        return xSpeed;
    }

    [Header("移動速度")] public float speed;//速度
    [Header("ジャンプ制限時間")] public float jumpLimitTime;//ジャンプ制限時間。40で追加。
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;//レッスン45で追加。
    [Header("重力")] public float grovity;//重力
    [Header("設置判定")] public GroundCheck ground; //レッスン38　設置判定で追加
    [Header("頭をぶつけた時の判定")] public GroundCheck head;//頭をぶつけた時の判定。40で追加
    [Header("ダッシュアニメーションカーブ")] public AnimationCurve dashCurve;//レッスン41で追加。アニメーションカーブ
    [Header("ジャンプアニメーションカーブ")] public AnimationCurve jumpCurve;//同上
    [Header("JumpVoices")] public AudioClip[] jumpVoices;
    [Header("trapDownVoices")] public AudioClip[] trapDownVoices;
    [Header("FallVoices")] public AudioClip[] fallVoices;
    [Header("pauseButton")] public GameObject pauseButton;
    public bool canControl = true;

    public FixedJoystick joystick;//ジョイスティック導入　https://note.com/npaka/n/neafdd3059b0c

    #region
    public bool stop;
    public bool jumpbuton;*/
}
