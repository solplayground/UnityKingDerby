// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-18-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="BetBoardManager.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using TMPro;
using UnityEngine;

// End location -11,-6.3,0, Start Location: -6,6.3
/// <summary>
/// Class BetBoardManager.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class BetBoardManager : MonoBehaviour
{
    /// <summary>
    /// The bet value coins
    /// </summary>
    public BetValueCoin[] betValueCoins;

    /// <summary>
    /// The last results
    /// </summary>
    public LastResult[] lastResults;
    /// <summary>
    /// The won coin
    /// </summary>
    public TMP_Text wonCoin;
    /// <summary>
    /// The remaining coin
    /// </summary>
    public TMP_Text remainingCoin;

    /// <summary>
    /// The re bet count
    /// </summary>
    public TMP_Text reBetCount;

    /// <summary>
    /// The title
    /// </summary>
    public TMP_Text title;


    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        DisplayGameData();
    }

    /// <summary>
    /// Updates the coins.
    /// </summary>
    /// <param name="index">The index.</param>
    public void UpdateCoins(int index)
    {
        betValueCoins[index].coinCount.text = GameData.Bets[index].ToString();
    }

    /// <summary>
    /// Updates the remaining coins.
    /// </summary>
    /// <param name="remainingCoins">The remaining coins.</param>
    public void UpdateRemainingCoins(int remainingCoins)
    {
        remainingCoin.text = remainingCoins.ToString();
    }

    /// <summary>
    /// Updates the won coins.
    /// </summary>
    /// <param name="wonCoins">The won coins.</param>
    public void UpdateWonCoins(int wonCoins)
    {
        wonCoin.text = wonCoins.ToString();
    }


    /// <summary>
    /// Shows the result.
    /// </summary>
    public void ShowResult()
    {
        DisplayGameData();
        title.text = "Last Odds";
        StopAllCoroutines();
        StartCoroutine(FlashResult());
    }

    /// <summary>
    /// Stops the flush.
    /// </summary>
    public void StopFlush()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Updates the re bet.
    /// </summary>
    public void UpdateReBet()
    {
        reBetCount.text = $@"x {GameData.ReBetOptions[GameData.ReBetOptionIndex]}";
    }

    /// <summary>
    /// Flashes the result.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    private IEnumerator FlashResult()
    {
        var oldText = betValueCoins[GameData.ResultIndex].coinValue.text;
        var currentIndex = GameData.ResultCount;
        if (GameData.ResultCount < 5)
        {
            GameData.ResultCount += 1;
        }
        else
        {
            GameData.ShiftResults();
            currentIndex = 4;
        }

        GameData.RacingResults[currentIndex] = new RacingResult
        {
            Coins = GameData.RacingResult.Coins, HorseNumber1 = GameData.RacingResult.HorseNumber1,
            HorseNumber2 = GameData.RacingResult.HorseNumber2
        };
        for (var i = 0; i < 5; i++)
        {
            if (GameData.RacingResults[i] != null)
            {
                lastResults[i].coinValue.text = GameData.RacingResults[i].Coins.ToString();
                lastResults[i].winHorses.text =
                    $@"{GameData.RacingResults[i].HorseNumber1} - {GameData.RacingResults[i].HorseNumber2}";
            }
        }

        for (int i = 0; i < 400; i++)
        {
            yield return new WaitForSeconds(0.5f);
            betValueCoins[GameData.ResultIndex].coinValue.text = "";
            lastResults[currentIndex].coinValue.text = "";
            lastResults[currentIndex].winHorses.text = "";
            yield return new WaitForSeconds(0.5f);
            betValueCoins[GameData.ResultIndex].coinValue.text = oldText;
            lastResults[currentIndex].coinValue.text = GameData.RacingResults[currentIndex].Coins.ToString();
            lastResults[currentIndex].winHorses.text =
                $@"{GameData.RacingResults[currentIndex].HorseNumber1} - {GameData.RacingResults[currentIndex].HorseNumber2}";
        }
    }

    /// <summary>
    /// Displays the game data.
    /// </summary>
    private void DisplayGameData()
    {
        remainingCoin.text = GameData.RemainingCoins.ToString();

        for (var i = 0; i < betValueCoins.Length; i++)
        {
            betValueCoins[i].coinValue.text = GameData.Odds[GameData.CoinValueIndexes[i]].ToString();
            betValueCoins[i].coinCount.text = GameData.Bets[i].ToString();
        }
        
        for (var i = 0; i < 5; i++)
        {
            if (GameData.RacingResults[i] != null)
            {
                lastResults[i].coinValue.text = GameData.RacingResults[i].Coins.ToString();
                lastResults[i].winHorses.text =
                    $@"{GameData.RacingResults[i].HorseNumber1} - {GameData.RacingResults[i].HorseNumber2}";
            }
        }
    }


    /// <summary>
    /// Shows the new bet.
    /// </summary>
    public void ShowNewBet()
    {
        DisplayGameData();
        title.text = "New Bet";
    }
}


/// <summary>
/// Class BetValueCoin.
/// </summary>
[Serializable]
public class BetValueCoin
{
    /// <summary>
    /// The coin value
    /// </summary>
    public TMP_Text coinValue;
    /// <summary>
    /// The coin count
    /// </summary>
    public TMP_Text coinCount;
}

/// <summary>
/// Class LastResult.
/// </summary>
[Serializable]
public class LastResult
{
    /// <summary>
    /// The coin value
    /// </summary>
    public TMP_Text coinValue;
    /// <summary>
    /// The win horses
    /// </summary>
    public TMP_Text winHorses;
}