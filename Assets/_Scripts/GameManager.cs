using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject clearText;

    public void GameOver() {
        //���X�^�[�g����B
        SceneManager.LoadScene("Stage01");
    }

    public void GameClear() {
        clearText.SetActive(true);
    }
}
