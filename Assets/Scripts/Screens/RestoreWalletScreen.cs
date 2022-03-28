// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="RestoreWalletScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Solnet.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    /// <summary>
    /// Class RestoreWalletScreen.
    /// Implements the <see cref="Screens.SimpleScreen" />
    /// </summary>
    /// <seealso cref="Screens.SimpleScreen" />
    public class RestoreWalletScreen : SimpleScreen
    {
        /// <summary>
        /// The mnemonic text
        /// </summary>
        public TMP_InputField mnemonicText;
        /// <summary>
        /// The password text
        /// </summary>
        public InputField passwordText;
        /// <summary>
        /// The need password
        /// </summary>
        public GameObject needPassword;
        /// <summary>
        /// The invalid seed phrase
        /// </summary>
        public GameObject invalidSeedPhrase;
        // Start is called before the first frame update


        /// <summary>
        /// Saves the wallet.
        /// </summary>
        public void SaveWallet()
        {
            if (string.IsNullOrEmpty(passwordText.text))
            {
                needPassword.SetActive(true);
            }
            else if (string.IsNullOrEmpty(mnemonicText.text))
            {
                invalidSeedPhrase.SetActive(true);
            }
            else
            {
                Wallet keypair = UnitySolanaWallet.Instance.GenerateWalletWithMenmonic(mnemonicText.text);
                if (keypair != null)
                {
                    UnitySolanaWallet.Instance.SavePlayerPrefs(UnitySolanaWallet.Instance.PasswordKey,
                        passwordText.text);
                    UnitySolanaWallet.Instance.SavePlayerPrefs(UnitySolanaWallet.Instance.MnemonicsKey,
                        mnemonicText.text);
                    GameData.Password = passwordText.text;
                    GameData.MnemonicText = mnemonicText.text;
                    if (UnitySolanaWallet.Instance.LoadSavedWallet())
                    {
                        Debug.Log(UnitySolanaWallet.Instance.wallet.Account.GetPublicKey);
                    }
                    Manager.ShowScreen("WalletScreen");
                }
                else
                {
                    invalidSeedPhrase.SetActive(true);
                }
            }
        }
    }
}