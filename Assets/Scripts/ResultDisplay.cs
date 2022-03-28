// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-21-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-21-2022
// ***********************************************************************
// <copyright file="ResultDisplay.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Class ResultDisplay.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class ResultDisplay : MonoBehaviour
{
    /// <summary>
    /// The race result
    /// </summary>
    public TMP_Text raceResult;

    /// <summary>
    /// Displays the result.
    /// </summary>
    /// <param name="result">The result.</param>
    public void DisplayResult(RacingResult result)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowResult(result));
    }


    /// <summary>
    /// Shows the result.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>IEnumerator.</returns>
    private IEnumerator ShowResult(RacingResult result)
    {
        var resultString = $@"{result.HorseNumber1} - {result.HorseNumber2} {result.Coins}";
        for (var i = 0; i < 40; i++)
        {
            raceResult.text = resultString;
            yield return new WaitForSeconds(0.5f);
            raceResult.text = "";
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}