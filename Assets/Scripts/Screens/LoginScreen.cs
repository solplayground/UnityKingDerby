// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="LoginScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Button = UnityEngine.UI.Button;

namespace Screens
{
    /// <summary>
    /// Class LoginScreen.
    /// Implements the <see cref="Screens.SimpleScreen" />
    /// </summary>
    /// <seealso cref="Screens.SimpleScreen" />
    public class LoginScreen : SimpleScreen
    {
        /// <summary>
        /// The login button
        /// </summary>
        public Button loginButton;
        /// <summary>
        /// The create new warning
        /// </summary>
        public GameObject createNewWarning;
        /// <summary>
        /// The need password
        /// </summary>
        public GameObject needPassword;
        /// <summary>
        /// The password text
        /// </summary>
        public InputField passwordText;
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start()
        {
            GameData.Password = UnitySolanaWallet.Instance.LoadPlayerPrefs(UnitySolanaWallet.Instance.PasswordKey);
            GameData.MnemonicText = UnitySolanaWallet.Instance.LoadPlayerPrefs(UnitySolanaWallet.Instance.MnemonicsKey);
            if (GameData.Password.Length == 0 || GameData.MnemonicText.Length == 0)
            {
                loginButton.enabled = false;
                createNewWarning.SetActive(true);
            }
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        public void Login()
        {
            if (GameData.Password != passwordText.text)
            {
                needPassword.SetActive(true);
            }
            else
            {
                Manager.ShowScreen("WalletScreen");
            }
        }
    }
}