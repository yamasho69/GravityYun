using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    bool isGoal;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !isGoal) {
            isGoal = true;
            Debug.Log("ÉSÅ[Éã");
        }
    }
}
