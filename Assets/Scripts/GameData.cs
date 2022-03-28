// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-18-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="GameData.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Random = System.Random;


/// <summary>
/// Class RacingResult.
/// </summary>
public class RacingResult
{
    /// <summary>
    /// Gets or sets the horse number1.
    /// </summary>
    /// <value>The horse number1.</value>
    public int HorseNumber1 { get; set; } = 0;
    /// <summary>
    /// Gets or sets the horse number2.
    /// </summary>
    /// <value>The horse number2.</value>
    public int HorseNumber2 { get; set; } = 0;
    /// <summary>
    /// Gets or sets the coins.
    /// </summary>
    /// <value>The coins.</value>
    public int Coins { get; set; } = 0;
}


/// <summary>
/// Class GameData.
/// </summary>
public static class GameData
{
    /// <summary>
    /// The use local
    /// </summary>
    public static bool UseLocal = true;
    /// <summary>
    /// The password
    /// </summary>
    public static string Password = "";
    /// <summary>
    /// The mnemonic text
    /// </summary>
    public static string MnemonicText = "";
    /// <summary>
    /// The re bet options
    /// </summary>
    public static readonly int[] ReBetOptions = { 1, 2, 5, 10, 80 };
    /// <summary>
    /// The odds
    /// </summary>
    public static readonly int[] Odds = { 3, 4, 5, 8, 10, 20, 30, 60, 80, 100, 125, 175, 250, 500, 100 };
    /// <summary>
    /// The bets
    /// </summary>
    public static readonly int[] Bets = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    /// <summary>
    /// The coin value indexes
    /// </summary>
    public static readonly int[] CoinValueIndexes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    /// <summary>
    /// The re bet option index
    /// </summary>
    public static int ReBetOptionIndex = 0;
    /// <summary>
    /// The remaining coins
    /// </summary>
    public static int RemainingCoins = 500;


    /// <summary>
    /// The result index
    /// </summary>
    public static int ResultIndex = 0;
    /// <summary>
    /// The racing result
    /// </summary>
    public static readonly RacingResult RacingResult = new RacingResult();
    /// <summary>
    /// The racing results
    /// </summary>
    public static readonly RacingResult[] RacingResults = new RacingResult[5];
    /// <summary>
    /// The result count
    /// </summary>
    public static int ResultCount = 0;
    /// <summary>
    /// The random value
    /// </summary>
    private static readonly Random RandomValue = new Random();


    /// <summary>
    /// Determines whether this instance has bets.
    /// </summary>
    /// <returns><c>true</c> if this instance has bets; otherwise, <c>false</c>.</returns>
    public static bool HasBets()
    {
        foreach (var t in Bets)
        {
            if (t > 0) return true;
        }

        return false;
    }

    /// <summary>
    /// Shifts the results.
    /// </summary>
    public static void ShiftResults()
    {
        if (ResultCount > 4)
        {
            for (var i = 0; i < 4; i++)
            {
                RacingResults[i] = RacingResults[i + 1];
            }
        }
    }

    /// <summary>
    /// Randoms the odds.
    /// </summary>
    public static void RandomOdds()
    {
        var rng = RandomValue;
        var n = CoinValueIndexes.Length;
        while (n > 1)
        {
            var k = rng.Next(n--);
            (CoinValueIndexes[n], CoinValueIndexes[k]) = (CoinValueIndexes[k], CoinValueIndexes[n]);
        }

        var coinValue = RandomWinnerCoinValue();
        for (var i = 0; i < CoinValueIndexes.Length; i++)
        {
            if (coinValue == Odds[CoinValueIndexes[i]])
            {
                ResultIndex = i;
                break;
            }
        }

        RacingResult.Coins = coinValue;
        switch (ResultIndex)
        {
            case 0:
                RacingResult.HorseNumber1 = 1;
                RacingResult.HorseNumber2 = 6;

                break;
            case 1:
                RacingResult.HorseNumber1 = 1;
                RacingResult.HorseNumber2 = 5;

                break;
            case 2:
                RacingResult.HorseNumber1 = 1;
                RacingResult.HorseNumber2 = 4;

                break;
            case 3:
                RacingResult.HorseNumber1 = 1;
                RacingResult.HorseNumber2 = 3;

                break;
            case 4:
                RacingResult.HorseNumber1 = 1;
                RacingResult.HorseNumber2 = 2;

                break;
            case 5:
                RacingResult.HorseNumber1 = 2;
                RacingResult.HorseNumber2 = 6;

                break;
            case 6:
                RacingResult.HorseNumber1 = 2;
                RacingResult.HorseNumber2 = 5;

                break;
            case 7:
                RacingResult.HorseNumber1 = 2;
                RacingResult.HorseNumber2 = 4;

                break;
            case 8:
                RacingResult.HorseNumber1 = 2;
                RacingResult.HorseNumber2 = 3;

                break;
            case 9:
                RacingResult.HorseNumber1 = 3;
                RacingResult.HorseNumber2 = 6;

                break;
            case 10:
                RacingResult.HorseNumber1 = 3;
                RacingResult.HorseNumber2 = 5;

                break;
            case 11:
                RacingResult.HorseNumber1 = 3;
                RacingResult.HorseNumber2 = 4;

                break;

            case 12:
                RacingResult.HorseNumber1 = 4;
                RacingResult.HorseNumber2 = 6;

                break;

            case 13:
                RacingResult.HorseNumber1 = 4;
                RacingResult.HorseNumber2 = 5;

                break;

            case 14:
                RacingResult.HorseNumber1 = 5;
                RacingResult.HorseNumber2 = 6;

                break;
        }
    }

    /// <summary>
    /// Resets the bets.
    /// </summary>
    public static void ResetBets()
    {
        for (var i = 0; i < Bets.Length; i++) Bets[i] = 0;
    }

    /// <summary>
    /// Randoms the winner coin value.
    /// </summary>
    /// <returns>System.Int32.</returns>
    private static int RandomWinnerCoinValue()
    {
        int result;
        var rng = RandomValue;
        var num = rng.Next(100);

        if (num < 60)
        {
            result = rng.Next(3);
            switch (result)
            {
                case 0:
                    return 3;
                case 1:
                    return 4;
                case 2:
                    return 5;
            }
        }
        else if (num < 75)
        {
            result = rng.Next(2);
            switch (result)
            {
                case 0:
                    return 8;
                case 1:
                    return 10;
            }
        }
        else if (num < 90)
        {
            result = rng.Next(4);
            switch (result)
            {
                case 0:
                    return 20;
                case 1:
                    return 30;
                case 2:
                    return 60;
                case 3:
                    return 80;
            }
        }
        else if (num < 99)
        {
            result = rng.Next(4);
            switch (result)
            {
                case 0:
                    return 100;
                case 1:
                    return 125;
                case 2:
                    return 175;
                case 3:
                    return 250;
            }
        }
        else
        {
            result = rng.Next(2);
            switch (result)
            {
                case 0:
                    return 500;
                case 1:
                    return 1000;
            }
        }

        return -1;
    }
}