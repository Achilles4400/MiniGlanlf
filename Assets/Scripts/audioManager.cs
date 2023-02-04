using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] private int currentAudio;
    [SerializeField] private AudioClip[] clip = new AudioClip[4];

    private int currentSample;
    [SerializeField] private List<AudioSource> speaker = new List<AudioSource>();
    private GameManager gameManagerScript;

    private float fadeDelay;
    private float fadeCoeff;



    // Start is called before the first frame update
    void Start()
    {
        fadeDelay = 0.01f;
        fadeCoeff = 0.002f;

        foreach(AudioSource speak in GetComponents<AudioSource>())
        {
            speaker.Add(speak);
        }

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentAudio = clip.Length - 1;
        speaker[0].clip = clip[currentAudio];
        speaker[0].Play();

    }

    private void switchAudio()
    {
        if (currentAudio != gameManagerScript.closeness)
        {
            currentSample = speaker[0].timeSamples;
            //Switch old audio to speaker 1
            speaker[1].clip = clip[currentAudio];
            speaker[1].timeSamples = currentSample;

            //Switch new audio to speaker 0
            speaker[0].clip = clip[gameManagerScript.closeness];
            speaker[0].timeSamples = currentSample;
            currentAudio = gameManagerScript.closeness;

            //Cross Fade
            StartCoroutine(crossFade(fadeDelay));
            

        }
    }

    private IEnumerator crossFade(float fadeDelay)
    {
        speaker[0].volume = 0f;
        speaker[1].volume = 1f;

        speaker[0].Play();
        speaker[1].Play();

        while(speaker[1].volume >= 0f)
        {
            speaker[0].volume += fadeCoeff;
            speaker[1].volume -= fadeCoeff;
            yield return new WaitForSeconds(fadeDelay);
            
        }

        speaker[1].Stop();

    }


    // Update is called once per frame
    void Update()
    {
        switchAudio();
    }
}
