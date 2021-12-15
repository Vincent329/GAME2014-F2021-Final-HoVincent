//----------- ShrinkingPlatform.cs---------------
// Date Created: Dec. 14, 2021
//
// Description: Script that details behaviours for a shrinking platform that floats up and down
//              and shrinks upon player's landing, then expands when a player leaves contact of platform
//
// Revision History: 
// 1) Created Script
// 2) Created Oscillation function via sin wave
// 3) On Collision Enter and Exit functions will trigger appropriate Boolean states
// 4) Created Shrinking logic
// 5) Created Expanding Logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Shrinking platform class
/// </summary>
public class ShrinkingPlatform : MonoBehaviour
{
    [Header("Elapsed Time monitors")]
    [SerializeField]
    private float oscillatingTimeElapsed;
    [SerializeField] private float shrinkTimeElapsed;
    [Range(0.1f, 10.0f)]
    [SerializeField] private float shrinkingTime; // Must alter in the editor, total time it takes to fully shrink the platform, as well as the max value for scaling purposes

    [SerializeField] private Vector3 platformPosition; // keeps track of the platform's original position
    
    // boolean factors
    [Header("Platform States")]
    [SerializeField] private bool isPlayerOn;
    [SerializeField] private bool isExpanding;

    // scaling properties
    private Vector3 tempScale;          // scale that will reference the transform's own local scale, will scale down/up over time given contextual action
    private Vector2 originalBoxScale;   // keep a reference to the box' collider scale to change size as well
    float scale;                        // scaling factor (stays between 0 and 1)
    private BoxCollider2D boxCollider;

    // Sound effects
    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        platformPosition = transform.position;

        originalBoxScale = boxCollider.size;
        isPlayerOn = false;
        isExpanding = false;
        oscillatingTimeElapsed = 0.0f;
        shrinkTimeElapsed = shrinkingTime;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player hasn't touched the platform
        if (!isPlayerOn)
        {
            if (!isExpanding)
                Oscillate();
            else
                Expand();
        } 
        // shrink the platform should the player touch it
        else
        {
            Shrink();
        }
    }

    /// <summary>
    /// The function used to give a floating-style movement on the platform
    /// </summary>
    void Oscillate()
    {

        oscillatingTimeElapsed += Time.deltaTime;

        // limiting factor, as we keep adding you'll reach a float limit
        // since the platform is oscillating via sin wave, it will take 2*pi to complete a full cycle
        if (oscillatingTimeElapsed > 2*Mathf.PI) 
        {
            oscillatingTimeElapsed = 0;
        }

        // change the platform's y position, fluctuates as a sin wave
        transform.position = new Vector3(platformPosition.x, platformPosition.y + (Mathf.Sin(oscillatingTimeElapsed) / 2), 0.0f);
    }

    /// <summary>
    /// Shrinking function, called when player makes contact with the platform
    /// </summary>
    void Shrink()
    {
        shrinkTimeElapsed -= Time.deltaTime;
        scale = shrinkTimeElapsed / shrinkingTime; 

        tempScale = transform.localScale;
        tempScale.x = scale;
        tempScale.y = scale;
        transform.localScale = tempScale;

        Vector2 alteredBoxScale = originalBoxScale * scale;
        boxCollider.size = alteredBoxScale;

    }

    /// <summary>
    /// Expansion function, called when player leaves contact with the platform
    /// </summary>
    void Expand()
    {
        shrinkTimeElapsed += Time.deltaTime;
        scale = shrinkTimeElapsed / shrinkingTime;

        tempScale = transform.localScale;
        tempScale.x = scale;
        tempScale.y = scale;
        transform.localScale = tempScale;

        Vector2 alteredBoxScale = originalBoxScale * scale;
        boxCollider.size = alteredBoxScale;

        // when we return to the original scale, we're done expanding, change the if statement
        if (scale >= 1.0f)
        {
            scale = 1.0f;
            isExpanding = false;
        }
        
    }

    /// <summary>
    /// Collision enter functionality
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            isPlayerOn = true;
            isExpanding = false;
        }
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    /// <summary>
    /// Collision exit functionality
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        isPlayerOn = false;
        isExpanding = true;
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
}
