using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Structure : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private new string name;
    [SerializeField] private float unlockPrice;
    [SerializeField] private float buildPrice;
    [SerializeField] private float currencyUnitsPerMinute;
    [SerializeField] private float researchUnitsPerMinute;
    [SerializeField] private GameObject model;
    [SerializeField] private VisualEffect buildVFX;

    public string ID { get { return id; } }
    public string Name { get { return name; } }
    public float UnlockPrice { get { return unlockPrice; } }
    public float BuildPrice { get { return buildPrice; } }
    public float CurrencyUnitsPerMinute { get { return currencyUnitsPerMinute; } }
    public float ResearchUnitsPerMinute { get { return researchUnitsPerMinute; } }

    public void Initialize()
    {
        StartCoroutine(InitializeCoroutine());
    }

    private IEnumerator InitializeCoroutine()
    {
        model.SetActive(false);
        buildVFX.Play();

        AudioManager.Instance.PlaySound("Build");

        yield return new WaitForSeconds(buildVFX.GetFloat("ParticleLifetime") * 0.5f);

        model.SetActive(true);
    }

    public void Terminate()
    {
        StartCoroutine(TerminateCoroutine());
    }

    private IEnumerator TerminateCoroutine()
    {
        model.SetActive(true);
        buildVFX.Play();

        AudioManager.Instance.PlaySound("Demolish");

        Destroy(GetComponent<Collider>());

        yield return new WaitForSeconds(buildVFX.GetFloat("ParticleLifetime") * 0.5f);

        model.SetActive(false);

        yield return new WaitForSeconds(buildVFX.GetFloat("ParticleLifetime") * 0.5f);

        Destroy(gameObject);
    }
}