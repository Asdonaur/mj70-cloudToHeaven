using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCloud : MonoBehaviour
{
    public float coord = 3;
    float size;
    // Start is called before the first frame update
    void Start()
    {
        size = Camera.main.orthographicSize * 1.5f;
        transform.localPosition = new Vector3(Random.Range(-size, size), transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -coord)
        {
            transform.localPosition = new Vector3(Random.Range(-size, size), coord, 0);
        }
    }
}
