using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritBehaviour : MonoBehaviour
{
    public float flFHeal = 2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Rain":
                StartCoroutine(other.gameObject.GetComponent<RainBehaviour>().ienMojar());
                Heal();
                break;
        }
    }

    void Heal()
    {
        GameManager.instance.ChangeValue(0, flFHeal);
    }

    public void anim(string an)
    {
        GetComponentInChildren<Animator>().Play(an);
    }
}
