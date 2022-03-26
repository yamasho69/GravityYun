using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//モニターをずっとちらつかせる
//モニターの文字(参考　https://nn-hokuson.hatenablog.com/entry/2016/12/20/201832)
public class Monitor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    IEnumerator Flash() {
        //点滅させる
        //消える
        spriteRenderer.color = new Color32(255, 255, 255, 200);
        yield return new WaitForSeconds(0.1f);
        //着く
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Flash());
    }
}
