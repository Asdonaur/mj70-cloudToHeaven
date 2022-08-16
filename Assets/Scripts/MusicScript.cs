using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Destruir()
    {
        while (audioSource.volume >= 0.001f)
        {
            audioSource.volume -= 0.05f;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
