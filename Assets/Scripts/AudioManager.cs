// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-21-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="AudioManager.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using UnityEngine;


/// <summary>
/// Enum AudioTypes
/// </summary>
public enum AudioTypes
{
    /// <summary>
    /// The background
    /// </summary>
    Background,
    /// <summary>
    /// The racing begin background
    /// </summary>
    RacingBeginBackground,
    /// <summary>
    /// The bet
    /// </summary>
    Bet,
    /// <summary>
    /// The button
    /// </summary>
    Button,
    /// <summary>
    /// The waiting
    /// </summary>
    Waiting,
    /// <summary>
    /// The winner
    /// </summary>
    Winner,
}

/// <summary>
/// Class AudioManager.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// The audio clips
    /// </summary>
    public AudioClip[] audioClips;
    /// <summary>
    /// The audio source
    /// </summary>
    public AudioSource audioSource;


    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void StopSound()
    {
        audioSource.Stop();
    }
    /// <summary>
    /// Plays the sound.
    /// </summary>
    /// <param name="audioType">Type of the audio.</param>
    public void PlaySound(AudioTypes audioType)
    {
        audioSource.loop = false;
        switch (audioType)
        {
            case AudioTypes.Background:
                audioSource.clip = audioClips[0];

                break;
            case AudioTypes.Bet:
                audioSource.clip = audioClips[1];

                break;
            case AudioTypes.Button:
                audioSource.clip = audioClips[2];

                break;
            case AudioTypes.RacingBeginBackground:
                audioSource.clip = audioClips[3];

                break;
            case AudioTypes.Waiting:
                audioSource.clip = audioClips[4];
                audioSource.loop = true;

                break;
            case AudioTypes.Winner:
                audioSource.clip = audioClips[5];

                break;
        }

        audioSource.Play();
    }
}