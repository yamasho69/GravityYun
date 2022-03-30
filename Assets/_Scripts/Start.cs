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
    public GameObject text;
    


    public void OnCilck() { 
        //�N���A�����𗬂��B
        gm.RandomizeSfx(startVoices);
        gm.RandomizeSfx(startSE);
        StartCoroutine(Flash());
        //���̃V�[���ɍs��
        Invoke("GoToNextScene", 2.0f);
    }
    

    public void GoToNextScene() {
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator Flash() {
        //�_�ł�����
        //������
        text.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //����
        text.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Flash());
    }
}
