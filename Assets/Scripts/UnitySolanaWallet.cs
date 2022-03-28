// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="UnitySolanaWallet.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using AllArt.Solana;
using dotnetstandard_bip39;
using Solnet.Wallet;
using UnityEngine;
using KnownTokens = Data.KnownTokens;


/// <summary>
/// Class UnitySolanaWallet.
/// Implements the <see cref="AllArt.Solana.WalletBaseComponent" />
/// </summary>
/// <seealso cref="AllArt.Solana.WalletBaseComponent" />
public class UnitySolanaWallet : WalletBaseComponent
{
    /// <summary>
    /// The instance
    /// </summary>
    public static UnitySolanaWallet Instance;

    /// <summary>
    /// The tokens
    /// </summary>
    public KnownTokens tokens;

    /// <summary>
    /// The mint token account
    /// </summary>
    public string mintTokenAccount = "";

    //This is treasure wallet, for prove of concept,the key is just stored in the source code.
    //mint address B8CkpKeRJU2aUn5vb9o6FTayWi6fnbTsfZbQBveB4st3 , this mint is game coin mint
    ////2rNfFqWRckipeB1WRLmRp2YcrsUCHQh9CZWpCY694SAB
    /// <summary>
    /// The treasure mnemonic
    /// </summary>
    private static string _treasureMnemonic =
        "other exile original december december mimic figure deal service humble frost candy faint mass ancient aware diamond time table creek flag seek calm run";
    // private static byte[] _unityTreasureKeyPair =
    // {
    //     28, 102, 188, 189, 89, 159, 127, 192, 127, 82, 22,
    //     56, 208, 230, 82, 219, 209, 45, 150, 162, 229, 4,
    //     4, 11, 126, 163, 212, 46, 15, 68, 44, 9, 27,
    //     129, 163, 27, 143, 208, 75, 63, 223, 166, 167, 130,
    //     161, 109, 255, 36, 69, 189, 211, 46, 3, 58, 69,
    //     124, 136, 106, 26, 255, 238, 224, 123, 240
    // };

    /// <summary>
    /// The game token account
    /// </summary>
    public static string GameTokenAccount = "6AnTyyVjDvvfNXggAaQEaPnEHPWCiSNNo1qyBVnZQnAB";
    /// <summary>
    /// The treasure wallet
    /// </summary>
    public static Wallet TreasureWallet;


    /// <summary>
    /// Awakes this instance.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            TreasureWallet = new Wallet(_treasureMnemonic, BIP39Wordlist.English);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Settles the reward.
    /// </summary>
    /// <param name="reward">The reward.</param>
    public async void SettleReward(int reward)
    {
        if (!GameData.UseLocal && mintTokenAccount != ""  && reward!=0)
        {
            if (reward > 0)
            {
                var result = await UnitySolanaWallet.Instance.TransferToken(
                    UnitySolanaWallet.GameTokenAccount,
                    UnitySolanaWallet.Instance.wallet.Account.GetPublicKey,
                    UnitySolanaWallet.TreasureWallet.Account,
                    UnitySolanaWallet.Instance.tokens.knownTokens[0].mint, reward);
                Debug.Log(result.Result != null ? "Airdrop Success" : "Airdrop Failed");
            }
            else
            {
                var result = await UnitySolanaWallet.Instance.TransferToken(
                    UnitySolanaWallet.Instance.mintTokenAccount,
                    UnitySolanaWallet.TreasureWallet.Account.GetPublicKey,
                    UnitySolanaWallet.Instance.wallet.Account,
                    UnitySolanaWallet.Instance.tokens.knownTokens[0].mint, -reward);
                Debug.Log(result.Result != null ? "Airdrop Success" : "Airdrop Failed");
            }
        }
    }
}