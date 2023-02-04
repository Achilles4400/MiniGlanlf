using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    private int currentAudio;
    [SerializeField] private AudioClip[] clip = new AudioClip[4];

    private int timeSample;
    private AudioSource speaker;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        speaker = GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentAudio = clip.Length;
        speaker.clip = clip[currentAudio];

    }

    private void switchAudio()
    {
        if (currentAudio != gameManagerScript.closeness)
        {
            timeSample = speaker.timeSamples;
            speaker.clip = clip[gameManagerScript.closeness];
            speaker.timeSamples = timeSample;
            speaker.Play();
            currentAudio = gameManagerScript.closeness;
        }
    }


    // Update is called once per frame
    void Update()
    {
        switchAudio();
    }
}
