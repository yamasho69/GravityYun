using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    //�v���C���[��������E�Ɉړ��F�J�������E�Ɉړ�
    //�v���C���[���E���獶�Ɉړ��F�J���������Ɉړ�
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(collision.transform.position.x > transform.position.x) {
                //�J�������E�Ɉړ�
                cam.transform.position = new Vector3(cam.transform.position.x+18, 0, -10);
            } else {
                //�J���������Ɉړ�
                cam.transform.position = new Vector3(cam.transform.position.x-18, 0, -10);
            }
        }

    }

}
