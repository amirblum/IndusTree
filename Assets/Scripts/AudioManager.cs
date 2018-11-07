using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _sfxSource;
    public AudioSource[] winningMusic;
    public static AudioManager Instance;
    protected void Awake()
    {
        Instance = this;

        _sfxSource = GetComponent<AudioSource>();
    }

    protected void Start()
    {
        BoardData.Instance.OnScoreUpdatedEvent += OnScoreUpdated;
    }

    public void PlaySfx(AudioClip sfx)
    {
        _sfxSource.PlayOneShot(sfx);
    }

    private void OnScoreUpdated()
    {
        var playerScores = BoardData.Instance.PlayerScores;
        if (playerScores[0] > playerScores[1])
        {
            winningMusic[0].volume = 1;
            winningMusic[1].volume = 0;
        }
        else if (playerScores[0] < playerScores[1])
        {
            winningMusic[0].volume = 0;
            winningMusic[1].volume = 1;
        } 
        else
        {
            winningMusic[0].volume = 0;
            winningMusic[1].volume = 0;
        }
    }
}
