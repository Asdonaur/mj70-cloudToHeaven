using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehaviour : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    public GameObject objSpirit, objDie;
    public float flSpeed;
    public int health, healthM;

    float size = 1;

    public AudioClip seDemonHit;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        objSpirit = GameObject.FindGameObjectWithTag("Spirit");
        StartCoroutine(ienStart());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vc3Spirit = objSpirit.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, vc3Spirit, flSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.001f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Rain":
                StartCoroutine(other.gameObject.GetComponent<RainBehaviour>().ienMojar());
                LoseLife();
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Spirit":
                DamageSpirit();
                break;
        }
    }

    void OnDestroy()
    {
        if (GameManager.instance.blResarting == false)
        {
            GameObject defeatanim = Instantiate(objDie, transform.position, new Quaternion());
            defeatanim.transform.localScale = new Vector3(size, size, size);
            defeatanim.GetComponentInChildren<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }
    }

    void LoseLife()
    {
        health -= 1;
        animator.Play("animDemonHit");
        GameManager.instance.PlaySE(seDemonHit);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void DamageSpirit()
    {
        if (GameManager.instance.blLost == false)
        {
            GameManager.instance.ChangeValue(0, -0.05f * size);
            GameManager.instance.spirit.GetComponent<SpritBehaviour>().anim("animSpiritHit");
            //GameManager.instance.PlaySE(seDemonHit);
        }
    }

    IEnumerator ienStart()
    {
        float tim = 0, timm = 0.3f;
        while (tim < timm)
        {
            tim += Time.deltaTime;
            yield return null;
        }

        size = transform.localScale.x;
        flSpeed = flSpeed - (0.6f * (size - 1));
        healthM = Mathf.RoundToInt(healthM * transform.localScale.x);
        health = healthM;
        spriteRenderer.flipX = (transform.position.x < objSpirit.transform.position.x) ? false : true;
        StopCoroutine(ienStart());
    }
}
