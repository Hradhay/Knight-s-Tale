using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip useClip;

    public void  ApplyItemEffects(ItemSO itemSO)
    {
        if (audioSource && useClip)
        {
            audioSource.PlayOneShot(useClip);
        }
        if (itemSO.currentHealth>0)
        {
            StatsManager.Instance.UpdateHealth(itemSO.currentHealth);
        }
        if (itemSO.maxHealth>0)
        {
            StatsManager.Instance.UpdateMaxHealth(itemSO.maxHealth);
        }
        if (itemSO.speed>0)
        {
            StatsManager.Instance.UpdateSpeed(itemSO.speed);
        }
        if (itemSO.Duration>0)
        {
            StartCoroutine(EffectTimer(itemSO , itemSO.Duration));
        }
    }
    private IEnumerator EffectTimer(ItemSO itemSO , float Duration)
    {
        yield return new WaitForSeconds(Duration);
        if (itemSO.currentHealth>0)
        {
            StatsManager.Instance.UpdateHealth(-itemSO.currentHealth);
        }
        if (itemSO.maxHealth>0)
        {
            StatsManager.Instance.UpdateMaxHealth(-itemSO.maxHealth);
        }
        if (itemSO.speed>0)
        {
            StatsManager.Instance.UpdateSpeed(-itemSO.speed);
        }
    }
}
