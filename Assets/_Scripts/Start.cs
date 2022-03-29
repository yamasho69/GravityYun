using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start : MonoBehaviour
{
    bool isGoal;
    [SerializeField] GameManager gm;
    [Header("StartVoices")] public AudioClip[] startVoices;
    [Header("StartVoices")] public AudioClip startSE;
    public int nextScene;
    public Text text;
    


    public void OnCilck() { 
        //クリア音声を流す。
        gm.RandomizeSfx(startVoices);
        gm.RandomizeSfx(startSE);
        text = GetComponent<Text>();
       // StartCoroutine(Flash());
        //次のシーンに行く
        Invoke("GoToNextScene", 2.0f);
    }
    

    public void GoToNextScene() {
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator Flash() {
        //点滅させる
        //消える
        text.color = new Color32(255, 255, 255, 200);
        yield return new WaitForSeconds(0.1f);
        //着く
        text.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Flash());
    }
}
