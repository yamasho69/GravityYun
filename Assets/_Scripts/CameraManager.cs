using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] bool goRight;
    [SerializeField] bool goLeft;
    float cameraHorizontalMove = 18.0f;
    //�v���C���[��������E�Ɉړ��F�J�������E�Ɉړ�
    //�v���C���[���E���獶�Ɉړ��F�J���������Ɉړ�
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(collision.transform.position.x < transform.position.x) {
                //��������G�����@�v���C���[x<�I�u�W�F�N�gx�@���@�E�ɍs�����Ƃ��Ă���
                goRight = true;
            } else {
                //�E������G�����@�v���C���[x>�I�u�W�F�N�gx�@���@���ɍs�����Ƃ��Ă���
                goLeft = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            // ��������G����(goRight)�A�E���ɏo��(�v���C���[x>�I�u�W�F�N�gx)
            if (collision.transform.position.x > transform.position.x&&goRight) {
                //�J�������E�Ɉړ�
                cam.transform.position = new Vector3(cam.transform.position.x + cameraHorizontalMove, 0, -10);
                goRight = false;
            // �E������G����(goLeft)�A�����ɏo��(�v���C���[x<�I�u�W�F�N�gx)
            } else if(collision.transform.position.x < transform.position.x &&goLeft) {
                //�J���������Ɉړ�
                cam.transform.position = new Vector3(cam.transform.position.x - cameraHorizontalMove, 0, -10);
                goLeft = false;
            //�G������������o��
            } else {
                goLeft = false;
                goRight = false;
            }
        }

    }

}
