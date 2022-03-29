using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

 public class  GameManager : MonoBehaviour
{
    public void GameOver() {
        //���X�^�[�g����B
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//���̃V�[������ĊJ_https://qiita.com/haifuri/items/0a03270b1b3d4331196b
    }

    public static GameManager instance = null;

    private AudioSource audioSource;
    public AudioSource sfxSource;

    private void Awake() {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //�����_���Ń{�C�X��炷�A�Q�lhttp://negi-lab.blog.jp/archives/RandomizeSfx.html
    public void RandomizeSfx(params AudioClip[] clips) {
        var randomIndex = UnityEngine.Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}
