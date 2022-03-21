using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] bool goRight;
    [SerializeField] bool goLeft;
    float cameraHorizontalMove = 18.0f;
    //プレイヤーが左から右に移動：カメラを右に移動
    //プレイヤーが右から左に移動：カメラを左に移動
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(collision.transform.position.x < transform.position.x) {
                //左側から触った　プレイヤーx<オブジェクトx　＝　右に行こうとしている
                goRight = true;
            } else {
                //右側から触った　プレイヤーx>オブジェクトx　＝　左に行こうとしている
                goLeft = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            // 左側から触って(goRight)、右側に出た(プレイヤーx>オブジェクトx)
            if (collision.transform.position.x > transform.position.x&&goRight) {
                //カメラを右に移動
                cam.transform.position = new Vector3(cam.transform.position.x + cameraHorizontalMove, 0, -10);
                goRight = false;
            // 右側から触って(goLeft)、左側に出た(プレイヤーx<オブジェクトx)
            } else if(collision.transform.position.x < transform.position.x &&goLeft) {
                //カメラを左に移動
                cam.transform.position = new Vector3(cam.transform.position.x - cameraHorizontalMove, 0, -10);
                goLeft = false;
            //触った方向から出た
            } else {
                goLeft = false;
                goRight = false;
            }
        }

    }

}
