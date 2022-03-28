// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="CreateWalletScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using AllArt.Solana;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    /// <summary>
    /// Class CreateWalletScreen.
    /// Implements the <see cref="Screens.SimpleScreen" />
    /// </summary>
    /// <seealso cref="Screens.SimpleScreen" />
    public class CreateWalletScreen : SimpleScreen
    {
        /// <summary>
        /// The mnemonic text
        /// </summary>
        public TMP_Text mnemonicText;
        /// <summary>
        /// The password text
        /// </summary>
        public InputField passwordText;
        /// <summary>
        /// The need password
        /// </summary>
        public GameObject needPassword;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start()
        {
            mnemonicText.text =
                WalletKeyPair
                    .GenerateNewMnemonic(); //"margin toast sheriff air tank liar tuna oyster cake tell trial more rebuild ostrich sick once palace uphold fall faculty clap slam job pitch";
            Debug.Log(mnemonicText.text);
        }

        /// <summary>
        /// Saves the wallet.
        /// </summary>
        public void SaveWallet()
        {
            if (string.IsNullOrEmpty(passwordText.text))
            {
                needPassword.SetActive(true);
            }
            else
            {
                UnitySolanaWallet.Instance.SavePlayerPrefs(UnitySolanaWallet.Instance.PasswordKey, passwordText.text);
                UnitySolanaWallet.Instance.SavePlayerPrefs(UnitySolanaWallet.Instance.MnemonicsKey, mnemonicText.text);
                if (UnitySolanaWallet.Instance.LoadSavedWallet())
                {
                    Debug.Log(UnitySolanaWallet.Instance.wallet.Account.GetPublicKey);
                }
                GameData.Password = passwordText.text;
                GameData.MnemonicText = mnemonicText.text;
                Manager.ShowScreen("WalletScreen");
            }
        }
    }
}