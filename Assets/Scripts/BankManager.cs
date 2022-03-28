// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-21-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-21-2022
// ***********************************************************************
// <copyright file="BankManager.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

/// <summary>
/// Class BankManager.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class BankManager : MonoBehaviour
{
    /// <summary>
    /// The bet board manager
    /// </summary>
    public BetBoardManager betBoardManager;

    // Start is called before the first frame update
    /// <summary>
    /// The last checked
    /// </summary>
    private float _lastChecked = 0;

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        if (GameData.RemainingCoins < 100)
        {
            if (_lastChecked > 10f)
            {
                GameData.RemainingCoins += 2;
                _lastChecked = 0;
                betBoardManager.UpdateRemainingCoins(GameData.RemainingCoins);
                PlayerPrefs.SetInt("Score",GameData.RemainingCoins);
            }
            else
            {
                _lastChecked += Time.deltaTime;
            }
        }
    }
}