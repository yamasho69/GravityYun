using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageNum : MonoBehaviour
{
    private Text stageText = null;

    // Start is called before the first frame update
    void Start() {
        stageText = GetComponent<Text>();
        stageText.text = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update() {
    }
}
