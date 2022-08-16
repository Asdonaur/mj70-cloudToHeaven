using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBehaviour : MonoBehaviour
{
    float flTimer = 2;
    SpriteRenderer sprRen;
    // Start is called before the first frame update
    void Start()
    {
        sprRen = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(ienDisappear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ienMojar()
    {
        while ((sprRen != null) && (sprRen.color.a > 0.1f))
        {
            sprRen.color = new Color(1, 1, 1, sprRen.color.a - 0.1f);
            yield return null;
        }
        flTimer = flTimer * 0.75f;
    }

    IEnumerator ienDisappear()
    {
        float tim = 0;
        while (tim < flTimer)
        {
            tim += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
