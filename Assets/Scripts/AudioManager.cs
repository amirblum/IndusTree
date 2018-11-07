using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] winningMusic;
    public static AudioManager Instance;
    protected void Start()
    {
        BoardData.Instance.OnScoreUpdatedEvent += OnScoreUpdated;
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
