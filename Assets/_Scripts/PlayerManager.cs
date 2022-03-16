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
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        if(x == 0) {
            //Ž~‚Ü‚Á‚Ä‚¢‚é
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0) {
            //‰E‚ÉˆÚ“®
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0) {
            //¶‚ÉˆÚ“®
            direction = DIRECTION_TYPE.LEFT;
        }
        
    }

    private void FixedUpdate() {
        switch (direction) {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;

            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                break;

            case DIRECTION_TYPE.LEFT:
                speed = -3;
                break;
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
}
