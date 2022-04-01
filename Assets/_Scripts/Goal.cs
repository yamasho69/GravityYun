using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    bool isGoal;
    [SerializeField] GameManager gm;
    public GameObject player;
    [Header("ClearVoices")] public AudioClip[] clearVoices;
    public int nextScene;
    public GameObject goToNextScene;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !isGoal) {
            isGoal = true;
            Debug.Log("�S�[��");
            goToNextScene.SetActive(true);
            //�v���C���[������
            Destroy(player);
            //�N���A�����𗬂��B
            gm.RandomizeSfx(clearVoices);
            //���̃V�[���ɍs��
            Invoke("GoToNextScene", 2.5f);
        }
    }

    public void GoToNextScene() {
        SceneManager.LoadScene(nextScene);
    }
}
