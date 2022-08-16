using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    AudioSource audioSource;

    public GameObject enemy;
    public GameObject spirit;
    public GameObject returntitle;
    public SpriteRenderer cielo;
    Animator animator;

    float flHealthM = 100, flWaterM = 100;
    public float flHealth;
    public float flWater;
    public float flSpawnEnemy = 2;

    public Text txtH, txtW, txtT;
    public Image imgH, imgW;
    float flFactorCielo;
    public AudioClip seExplode;

    int timMValue = 0;
    float maxTime = 60, maxTime4, timeRemaining;
    bool blSpawning = false;
    public bool blResarting = false;
    public bool blLost = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        instance = this;
        flHealth = flHealthM;
        flWater = flWaterM;
        audioSource = GetComponent<AudioSource>();
        timMValue = 0;
        Debug.Log(timMValue);
        maxTime4 = maxTime / 4;
        timeRemaining = maxTime;
        spirit = GameObject.FindGameObjectWithTag("Spirit");
        flFactorCielo = 1 / maxTime;
        cielo.color = new Color(cielo.color.r, cielo.color.g, cielo.color.b, 0);

        StartCoroutine(ienAlgorithm());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown("a"))
        {
            SpawnEnemy(Mathf.RoundToInt(Random.Range(1, 4)));
        }
        */

        if (flHealth <= 0)
        {
            Perder();
        }
    }

    public void ChangeValue(int vari, float factor)
    {
        switch(vari)
        {
            case 0:
                flHealth += factor;
                if (flHealth > flHealthM)
                    flHealth = flHealthM;

                txtH.text = Mathf.RoundToInt(flHealth) + "";
                imgH.GetComponent<RectTransform>().sizeDelta = new Vector2(imgH.GetComponent<RectTransform>().sizeDelta.x, flHealth * 2);
                break;

            case 1:
                flWater += factor;
                if (flWater > flWaterM)
                    flWater = flWaterM;

                txtW.text = Mathf.RoundToInt(flWater) + "";
                imgW.GetComponent<RectTransform>().sizeDelta = new Vector2(imgW.GetComponent<RectTransform>().sizeDelta.x, flWater * 2);
                break;
        }
    }

    void SpawnEnemy(float size)
    {
        float flUnidadesCam = Camera.main.orthographicSize;
        int lado = Random.Range(-1, 1);
        if (lado == 0)
            lado = 1;
        float flYPoint = Random.Range(-flUnidadesCam, flUnidadesCam);
        flUnidadesCam = flUnidadesCam * 2f;

        GameObject enmy = Instantiate(enemy, new Vector3(flUnidadesCam * lado, flYPoint, 0f), new Quaternion());
        enmy.transform.localScale = enmy.transform.localScale * size;
    }

    void Resultado()
    {
        StopCoroutine(ienSpawnEnemy(timMValue));
        StopCoroutine(ienAlgorithm());
        txtT.text = " ";
        GameObject.FindGameObjectWithTag("Player").GetComponent<CloudBehaviour>().enabled = false;
        
    }

    void Ganar()
    {
        Resultado();
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Demon");
        foreach(GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }
        GameObject[] lluvias = GameObject.FindGameObjectsWithTag("Rain");
        foreach (GameObject rain in lluvias)
        {
            Destroy(rain);
        }

        StartCoroutine(GameObject.FindGameObjectWithTag("Music").GetComponent<MusicScript>().Destruir());
        StartCoroutine(ienWin());

        animator.enabled = true;
        animator.Play("Won");
    }

    void Perder()
    {
        if (blLost == false)
        {
            Resultado();
            blResarting = true;
            flHealth = 0;
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Demon");
            foreach (GameObject enemigo in enemigos)
            {
                enemigo.GetComponent<DemonBehaviour>().enabled = false;
            }
            spirit.GetComponent<SpritBehaviour>().anim("animSpiritExplode");
            StartCoroutine(ienLose());
            blLost = true;
        }
    }

    public void PlaySE(AudioClip se)
    {
        audioSource.PlayOneShot(se);
    }

    void QuitarTiempo()
    {
        timeRemaining -= 1;
        txtT.text = timeRemaining + "";
        cielo.color = new Color(cielo.color.r, cielo.color.g, cielo.color.b, cielo.color.a + flFactorCielo);

        if (timeRemaining <= 0)
        {
            StopAllCoroutines();
            Ganar();
        }
    }

    IEnumerator ienAlgorithm()
    {

        float tim = 0;
        float second = 0;

        while (tim < maxTime4)
        {
            if (blSpawning == false)
            {
                StartCoroutine(ienSpawnEnemy(timMValue));
            }

            tim += Time.deltaTime;
            second += Time.deltaTime;
            if (second >= 1)
            {
                QuitarTiempo();
                second = 0;
            }
            yield return null;
        }
        timMValue += 1;
        StartCoroutine(ienAlgorithm());
    }

    IEnumerator ienSpawnEnemy(int mxs)
    {
        blSpawning = true;
        float tima = 0, timam = flSpawnEnemy - (0.1f * timMValue);

        while (tima < timam)
        {
            tima += Time.deltaTime;
            yield return null;
        }
        SpawnEnemy(Random.Range(1, (mxs * 0.5f) + 1));
        blSpawning = false;
    }

    IEnumerator ienLose()
    {
        PlaySE(seExplode);
        float tima = 0, timam = 1.5f;

        while (tima < timam)
        {
            tima += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator ienWin()
    {
        float tima = 0, timam = 5f;

        while (tima < timam)
        {
            tima += Time.deltaTime;
            yield return null;
        }
        Instantiate(returntitle);
    }
}
