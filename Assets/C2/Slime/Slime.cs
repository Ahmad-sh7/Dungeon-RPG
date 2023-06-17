using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] Sprite[] idleSprites;
    [SerializeField] float timeBetweenFrames = 0.1f;

    SpriteRenderer sr;
    float animationTimer = 0f;
    int animationIndex = 0;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void Update()
    {
        // Animate the slime sprite
        if (idleSprites.Length > 0)
        {
            if (animationTimer >= timeBetweenFrames)
            {
                sr.sprite = idleSprites[++animationIndex % idleSprites.Length];
                animationTimer = 0f;
            }
          
            animationTimer += Time.deltaTime;
        }
    }
}
