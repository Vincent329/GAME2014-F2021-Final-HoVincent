using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3 platformPosition;
    [SerializeField]
    private bool isPlayerOn;
    [SerializeField]
    private float timeElapsed;
    // Start is called before the first frame update
    
    void Start()
    {
        platformPosition = transform.position;
        isPlayerOn = false;
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerOn)
        {
            Oscillate();
        } 
        else
        {

        }
    }

    void Oscillate()
    {
        timeElapsed += Time.deltaTime;
        transform.position = new Vector3(platformPosition.x, platformPosition.y + Mathf.Sin(timeElapsed), 0.0f);
    }

    void Shrink()
    {

    }

    void Expand()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            isPlayerOn = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isPlayerOn = false;
    }
}
