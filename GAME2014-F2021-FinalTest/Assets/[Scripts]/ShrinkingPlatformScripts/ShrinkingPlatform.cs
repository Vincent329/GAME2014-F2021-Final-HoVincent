using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    [Header("Elapsed Time monitors")]
    [SerializeField]
    private float oscillatingTimeElapsed;
    [SerializeField]
    private float shrinkTimeElapsed;
    [SerializeField]
    private float shrinkingTime;

    [SerializeField]
    private Vector3 platformPosition;
    
    // boolean factors
    [SerializeField] private bool isPlayerOn;
    [SerializeField] private bool isExpanding;

    private Vector3 tempScale;
    private Vector2 originalBoxScale;
    private BoxCollider2D boxCollider;
    float scale;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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
        if (!isPlayerOn)
        {
            if (!isExpanding)
                Oscillate();
            else
                Expand();
        } 
        else
        {
            Shrink();
        }
    }

    void Oscillate()
    {
        oscillatingTimeElapsed += Time.deltaTime * 2;

        // limiting factor, as we keep adding you'll reach a float limit
        // since the platform is oscillating via sin wave, it will take 2*pi to complete a full cycle
        if (oscillatingTimeElapsed > 2*Mathf.PI) 
        {
            oscillatingTimeElapsed = 0;
        }

        transform.position = new Vector3(platformPosition.x, platformPosition.y + (Mathf.Sin(oscillatingTimeElapsed) / 2), 0.0f);
    }

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

        if (scale >= 1.0f)
        {
            scale = 1.0f;
            isExpanding = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            isPlayerOn = true;
            isExpanding = false;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isPlayerOn = false;
        isExpanding = true;
    }
}
