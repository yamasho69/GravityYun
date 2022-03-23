using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject clearText;

    public void GameOver() {
        //リスタートする。
        SceneManager.LoadScene("Stage01");
    }

    public void GameClear() {
        clearText.SetActive(true);
    }

    public static GameManager instance = null;

    private AudioSource audioSource = null;
    public AudioSource sfxSource;

    private void Awake() {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

    }

    //ランダムでボイスを鳴らす、参考http://negi-lab.blog.jp/archives/RandomizeSfx.html
    public void RandomizeSfx(params AudioClip[] clips) {
        var randomIndex = UnityEngine.Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}
