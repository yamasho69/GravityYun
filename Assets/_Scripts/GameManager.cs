using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

 public class  GameManager : MonoBehaviour
{
    public void GameOver() {
        //リスタートする。
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//今のシーンから再開_https://qiita.com/haifuri/items/0a03270b1b3d4331196b
        Scene loadScene = SceneManager.GetActiveScene();
        // 現在のシーンを再読み込みする
        SceneManager.LoadScene(loadScene.name);//上の方向はダメだったが、こちらで成功。https://chiritsumo-blog.com/unity-scene-reload/
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

    //ランダムでボイスを鳴らす、参考http://negi-lab.blog.jp/archives/RandomizeSfx.html
    public void RandomizeSfx(params AudioClip[] clips) {
        var randomIndex = UnityEngine.Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}
