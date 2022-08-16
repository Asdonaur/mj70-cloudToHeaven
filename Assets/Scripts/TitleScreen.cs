using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    Animator animator;
    int state = 0;

    bool blCoroutine = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case 0:
                if(Input.GetKeyDown("space"))
                {
                    PlayAnim("Intro");
                    state = 2;
                }

                if (Input.GetKeyDown("c"))
                {
                    PlayAnim("Credits");
                    state = 1;
                }

                if (Input.GetKeyDown("escape"))
                {
                    Application.Quit();
                }
                break;

            case 1:
                if (Input.anyKeyDown)
                {
                    PlayAnim("titleStart");
                    state = 0;
                }
                break;

            case 2:
                if (blCoroutine == false)
                {
                    StartCoroutine(ienIntro());
                }

                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("SampleScene");
                }
                break;
        }
    }

    void PlayAnim(string anim)
    {
        animator.Play(anim);
    }

    IEnumerator ienIntro()
    {
        blCoroutine = true;

        float tim = 0, timm = 35;
        while (tim < timm)
        {
            tim += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");
    }
}
