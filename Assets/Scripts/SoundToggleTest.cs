using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggleTest : MonoBehaviour
{
    float fraction = 0;
    bool startTimer = false;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Hello World");
            audioSource.Play();
            startTimer = true;
        }

        if(startTimer)
        {
            fraction += Time.deltaTime;

            if(fraction >= 1)
            {
                audioSource.Stop();
                fraction = 0;
                startTimer = false;
            }
        }
    }
}
