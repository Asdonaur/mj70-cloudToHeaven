using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour
{
    Animator animator;

    public float flSpeed = 1;
    public float flMovRange;

    public float flTimerRain = 0.5f;
    public bool blIsRaining = false, blRained = false, blRainCorout = false;
    public GameObject prefRain;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //     -- Ajustar variables para calcular la posición del personaje --

        float flUnidadesCam = Camera.main.orthographicSize;

        float flXMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        flXMouse = (Mathf.Abs(flXMouse) >= flUnidadesCam + flMovRange) ? ((flUnidadesCam + flMovRange) * Mathf.Abs(flXMouse)) / flXMouse : flXMouse;

        Vector3 vc3Mouse = new Vector3(flXMouse, transform.position.y, 0f);

        transform.position = Vector3.Lerp(transform.position, vc3Mouse, flSpeed * Time.deltaTime);

        //     -- Control del disparo --

        if (Input.GetMouseButtonDown(0))
        {
            blIsRaining = true;
            StopAllCoroutines();
            StartCoroutine(ienRain());
            animator.speed = 2;
        }
        if (Input.GetMouseButtonUp(0))
        {
            blIsRaining = false;
            StopAllCoroutines();
            StartCoroutine(ienRecharge());
            animator.speed = 1;
        }
    }

    void Shoot() // Funcion para disparar
    {
        if (GameManager.instance.flWater > 0)
        {
            Instantiate(prefRain, transform.position, new Quaternion());
            GameManager.instance.ChangeValue(1, -1f);
            blRained = true;
        }
        else
        {

        }
        
    }

    IEnumerator ienRain() // Corrutina que controla cuando disparar
    {
        float tim = 0f;
        Shoot();
        while (tim < flTimerRain)
        {
            tim += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ienRain());
    }

    IEnumerator ienRecharge() // Corrutina que controla cuando recargar agua
    {
        float tim = 0f;
        GameManager.instance.ChangeValue(1, 2f);
        while (tim < flTimerRain)
        {
            tim += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ienRecharge());
    }
}
