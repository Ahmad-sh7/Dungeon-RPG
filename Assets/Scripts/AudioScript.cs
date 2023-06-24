using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Player SFX
    [SerializeField] AudioClip playerAttackSFX;
    [SerializeField] AudioClip playerDamageSFX;
    [SerializeField] AudioClip playerDefendSFX;
    [SerializeField] AudioClip playerHealSFX;

    // Enemy SFX
    [SerializeField] AudioClip enemyAttackSFX;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }


    public void PlayerAttackSFX()
    {
        audioSource.PlayOneShot(playerAttackSFX, 0.5f);
    }

    public void PlayerDamageSFX()
    {
        audioSource.PlayOneShot(playerDamageSFX, 0.5f);
    }

    public void PlayerDefendSFX()
    {
        audioSource.PlayOneShot(playerDefendSFX, 0.5f);
    }

    public void PlayerHealSFX()
    {
        audioSource.PlayOneShot(playerHealSFX, 0.5f);
    }

    public void EnemyAttackSFX()
    {
        audioSource.PlayOneShot(enemyAttackSFX, 0.5f);
    }
}
