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
                //�E�Ɉړ�
                direction = DIRECTION_TYPE.RIGHT;
            } else if (x < 0 || joystick.Horizontal < 0) {
                //���Ɉړ�
                direction = DIRECTION_TYPE.LEFT;
            } else if (x == 0 || joystick.Horizontal == 0) {
                //�~�܂��Ă���
                direction = DIRECTION_TYPE.STOP;
            }
                //�X�y�[�X��������W�����v����
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
        //�W�����v����Ƃ��Ƀ��W�b�h�{�f�B�̑��x��0��
        rb.velocity = Vector2.zero;
        //�d�͂��t�ɂ���
        rb.gravityScale = -rb.gravityScale;
        //�L�������㉺���]
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
        //���]���鎲�������ɂȂ��Ă���A���̔��Α��ɂ����Ă��܂��Ă����̂ŁA�X�v���C�g�G�f�B�^��
        //�s�{�b�g�̈ʒu��bottomcenter����center�ɕύX�B
        gravityButton = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ground") {
            isGround = true;
        }

        if (collision.gameObject.tag == "Trap" && !isDead) {
            Debug.Log("�g���b�v��");
            isDead = true;
            gm.RandomizeSfx(trapSE);
            gm.RandomizeSfx(trapVoices);
            StartCoroutine(GameOver());
            anim.SetBool("down",true);
            gm.RandomizeSfx(morunmorunSE);
        }

        if (collision.gameObject.tag == "Hole" && !isDead) {
            Debug.Log("������");
            isDead = true;
            gm.RandomizeSfx(fallSE);
            gm.RandomizeSfx(fallVoices);
            StartCoroutine(GameOver());
            anim.SetBool("down", true);
        }

        if (collision.gameObject.tag == "Poison" && !isDead) {
            Debug.Log("�ł�");
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
        //�������~�߂�
        direction = DIRECTION_TYPE.STOP;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        //�_�ł�����
        int count = 0;
        while(count < 20) {
            //������
            spriteRenderer.color = new Color32(255, 255, 255, 50);
            yield return new WaitForSeconds(0.1f);
            //����
            spriteRenderer.color = new Color32(255, 255, 255, 255);

            count++;
        }
        //���X�^�[�g������
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
        if (isClearMotion) {//�����͓���A�u���O�ƈႤ�B�N���A���[�V�������I���ɂȂ�Ɠ����Ȃ��悤�ɂ���B
            rb.velocity = new Vector2(0, -grovity);
        } else if (!isDown && !GameManager.instance.isGameOver) {//�_�E�����ł͂Ȃ��B���b�X��44�Œǉ��B�����͎��ȂȂ�����L��
            //�ݒu����𓾂�
            isGround = ground.IsGround();
            isHead = head.IsGround();


            if (isHead == true && isGround == true) {//�����Œǉ��B���̏Փ˔���Ɛڒn���肪�����ɗL���ɂȂ�ꍇ�̓W�����v�ł��Ȃ��Ȃ�B
                head.isGround = false;//���̂��߁A�����ɗL���ɂȂ�����A���̏Փ˔�����O���B�C���X�y�N�^�[��Ń`�F�b�N�����Ă��A�����ɊO���B
            }


            //�e����W���̑��x�����߂�
            float ySpeed = GetYSpeed();
            float xSpeed = GetXSpeed();

            SetAnimation();

            //54
            Vector2 addVelocity = Vector2.zero;
            if (moveObj != null) {
                addVelocity = moveObj.GetVelocity();
            }
            //velocity���X�N���v�g�ŏ㏑�����A�����@���𖳎�������
            //�Q�l�Fhttps://www.youtube.com/watch?v=klTg9hl_clU
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;//���b�X��40�ő�������ySpeed�ɕύX
        } else {
            rb.velocity = new Vector2(0, -grovity);//���b�X��44�Œǉ��B�_�E�����͏d�͂̂݉e��
        }
        if (!isClearMotion && GameManager.instance.isStageClear) {//�����̓u���O�Ƃ͈Ⴄ�B��Ɏ��ȂȂ�����L���ȕ���������̂ŁA�������番�򂳂��Ă��܂��ƃN���A���Ă��L���ɂȂ�Ȃ��B
            isClearMotion = true;
            anim.Play("StageClear");
            //rb.velocity = new Vector2(0, -grovity);�������ɏ����Ă��A���ȂȂ�����L���̕����őł�������邽�߁AisClearMotion���L���Ȃ�Γ����Ȃ�if�֐�����ԏ�ɂ����B
        }
    }

    public float GetXSpeed() {

        float horizontalKey = Input.GetAxis("Horizontal");//���E�����̃C���v�b�g���擾
        float xSpeed = 0.0f; //Speed�ϐ�������ϐ�
        if (canControl) {//�����Œǉ��BcanControl��false�ɂȂ�ƁA���͂��󂯕t���Ȃ��B
            if (horizontalKey > 0 || joystick.Horizontal > 0)//�E�����̓��͂��������ꍇ�@//�W���C�X�e�B�b�N�̔�����ǉ�
            {
                isRight = true;
                isLeft = false;
                dashTime += Time.deltaTime;//41
                                           //�Q�l����ł͉摜�𔽓]�����č��E�ւ̈ړ�������
                                           //transform.localScale = new Vector3(1,1,1);
                xSpeed = speed;//�E�Ȃ琳�̕�����Speed�ϐ�
            } else if (horizontalKey < 0 || joystick.Horizontal < 0)//�������̓��͂��������ꍇ�@//�W���C�X�e�B�b�N�̔�����ǉ�
              {
                isRight = false;
                isLeft = true;
                dashTime += Time.deltaTime;//41
                                           //transform.localScale = new Vector3(-1,1,1);
                xSpeed = -speed;//�E�Ȃ畉�̕�����Speed�ϐ�
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

            //���b�X��41�Œǉ��B�O��̃L�[���͂ƕ������Ⴄ�Ɖ������O�ɂ���B
            if ((horizontalKey > 0 && beforeKey < 0) || (horizontalKey < 0 && beforeKey > 0 || (horizontalKey > 0 && beforeJoy < 0) || (horizontalKey < 0 && beforeJoy > 0)
                || (joystick.Horizontal > 0 && beforeJoy < 0) || (joystick.Horizontal < 0 && beforeJoy > 0))) {
                dashTime = 0.0f;
            }

            beforeKey = horizontalKey;
            beforeJoy = joystick.Horizontal;
        }

        //�A�j���[�V�����J�[�u�𑬓x�ɓK�p
        xSpeed *= dashCurve.Evaluate(dashTime);

        if (stop) {
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        return xSpeed;
    }

    [Header("�ړ����x")] public float speed;//���x
    [Header("�W�����v��������")] public float jumpLimitTime;//�W�����v�������ԁB40�Œǉ��B
    [Header("���݂�����̍����̊���")] public float stepOnRate;//���b�X��45�Œǉ��B
    [Header("�d��")] public float grovity;//�d��
    [Header("�ݒu����")] public GroundCheck ground; //���b�X��38�@�ݒu����Œǉ�
    [Header("�����Ԃ������̔���")] public GroundCheck head;//�����Ԃ������̔���B40�Œǉ�
    [Header("�_�b�V���A�j���[�V�����J�[�u")] public AnimationCurve dashCurve;//���b�X��41�Œǉ��B�A�j���[�V�����J�[�u
    [Header("�W�����v�A�j���[�V�����J�[�u")] public AnimationCurve jumpCurve;//����
    [Header("JumpVoices")] public AudioClip[] jumpVoices;
    [Header("trapDownVoices")] public AudioClip[] trapDownVoices;
    [Header("FallVoices")] public AudioClip[] fallVoices;
    [Header("pauseButton")] public GameObject pauseButton;
    public bool canControl = true;

    public FixedJoystick joystick;//�W���C�X�e�B�b�N�����@https://note.com/npaka/n/neafdd3059b0c

    #region
    public bool stop;
    public bool jumpbuton;*/
}
