using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollactableBase : MonoBehaviour
/*{
    public string compareTag = "Player";
    public ParticleSystem particleVFX;
    public float timToHide = 3;
    public GameObject graphicItem;

    [Header("Sounds")]
    public AudioSource audioSource;

  private void Awake()
    {
        if (particleVFX != null) particleVFX.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            Collect();
        }
    }
    protected virtual void Collect()
    {
        if (graphicItem != null) graphicItem.SetActive(false);
        Invoke("HideObject", timToHide);
        gameObject.SetActive(false);
        OnCollect();
    }
    private void HideObject()
    {
        gameObject.SetActive(false);
    }
    protected virtual void OnCollect()
    {
        if (particleVFX != null) particleVFX.Play();
        if (audioSource != null) audioSource.Play();
    }

}
 }*/

{
    public string compareTag = "Player";

    [Header("VFX Settings")]
    public ParticleSystem particleVFX;
    public float timeToHide = 1f;
    public GameObject graphicItem;

    private Collider2D col;

    [Header("Sounds")]
    public AudioSource audioSource;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        if (particleVFX != null)
            particleVFX.transform.SetParent(null); // solta o VFX para não desaparecer junto
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(compareTag))
        {
            if (audioSource != null) audioSource.Play(); // toca SOM só na coleta
            Collect();
        }
    }

    protected virtual void Collect()
    {
        col.enabled = false;

        if (graphicItem != null)
            graphicItem.SetActive(false);

        if (particleVFX != null)
        {
            particleVFX.Play();

            // destrói a particula depois do tempo correto
            var main = particleVFX.main;
            float totalLife = main.duration + main.startLifetime.constantMax;
            Destroy(particleVFX.gameObject, totalLife);
        }

        Invoke(nameof(HideObject), timeToHide);

        OnCollect();
    }

    protected virtual void OnCollect() { }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }
}

   





    

