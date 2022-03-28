// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-28-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="WalletScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading.Tasks;
using AllArt.Solana.Nft;
using Solnet.Rpc.Models;
using TMPro;
using UnityEngine;

namespace Screens
{
    /// <summary>
    /// Class WalletScreen.
    /// Implements the <see cref="Screens.SimpleScreen" />
    /// </summary>
    /// <seealso cref="Screens.SimpleScreen" />
    public class WalletScreen : SimpleScreen
    {
        /// <summary>
        /// The sol amount
        /// </summary>
        public TMP_Text solAmount;
        /// <summary>
        /// The coin amount
        /// </summary>
        public TMP_Text coinAmount;
        /// <summary>
        /// The connected status
        /// </summary>
        public GameObject connectedStatus;

        /// <summary>
        /// The disconnected status
        /// </summary>
        public GameObject disconnectedStatus;

        /// <summary>
        /// The failed airdrop
        /// </summary>
        public GameObject failedAirdrop;
        /// <summary>
        /// The success airdrop
        /// </summary>
        public GameObject successAirdrop;

        /// <summary>
        /// The refresh timer
        /// </summary>
        private double refreshTimer = 0;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            //Fix a bug of the library, to avoid null pointer
            WebSocketActions.WebSocketAccountSubscriptionAction += (bool isTrue) => { };
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update()
        {
            if (refreshTimer > 5)
            {
                Refresh();
                refreshTimer = 0;
            }
            else
            {
                refreshTimer += Time.deltaTime;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        async void Start()
        {
            GameData.UseLocal = false;
            if (UnitySolanaWallet.Instance.LoadSavedWallet())
            {
                connectedStatus.SetActive(true);
                disconnectedStatus.SetActive(false);
                Debug.Log(UnitySolanaWallet.Instance.wallet.Account.GetPublicKey);
                await UpdateWalletBalanceDisplay();
                await GetOwnedTokenAccounts();
            }
            else
            {
                connectedStatus.SetActive(false);
                disconnectedStatus.SetActive(true);
            }


            MainThreadDispatcher.Instance().Enqueue(() => { UnitySolanaWallet.Instance.StartWebSocketConnection(); });
            WebSocketActions.WebSocketAccountSubscriptionAction += (bool isTrue) =>
            {
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    UpdateWalletBalanceDisplay();
                    GetOwnedTokenAccounts();
                });
            };
            WebSocketActions.CloseWebSocketConnectionAction += DisconnectToWebSocket;
        }

        /// <summary>
        /// Airdrops the sol.
        /// </summary>
        public async void AirdropSol()
        {
            if (UnitySolanaWallet.Instance.wallet != null)
            {
                var result = await UnitySolanaWallet.Instance.RequestAirdrop(UnitySolanaWallet.Instance.wallet.Account);
                if (result != null)
                {
                    failedAirdrop.SetActive(false);
                    successAirdrop.SetActive(true);
                    await UpdateWalletBalanceDisplay();
                    Debug.Log("Airdrop Success");
                }
                else
                {
                    failedAirdrop.SetActive(true);
                    successAirdrop.SetActive(false);
                    Debug.Log("Airdrop Failed");
                }
            }
        }


        /// <summary>
        /// Airdrops the game coins.
        /// </summary>
        public async void AirdropGameCoins()
        {
            if (UnitySolanaWallet.Instance.wallet != null && UnitySolanaWallet.TreasureWallet != null)
            {
                try
                {
                    var result = await UnitySolanaWallet.Instance.TransferToken(
                        UnitySolanaWallet.GameTokenAccount,
                        UnitySolanaWallet.Instance.wallet.Account.GetPublicKey,
                        UnitySolanaWallet.TreasureWallet.Account,
                        UnitySolanaWallet.Instance.tokens.knownTokens[0].mint, 500);
                    if (result.Result != null)
                    {
                        failedAirdrop.SetActive(false);
                        successAirdrop.SetActive(true);
                        await GetOwnedTokenAccounts();
                        Debug.Log("Airdrop Success");
                    }
                    else
                    {
                        failedAirdrop.SetActive(true);
                        successAirdrop.SetActive(false);
                        Debug.Log("Airdrop Failed");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        /// <summary>
        /// Sols the scan.
        /// </summary>
        public void SolScan()
        {
            Application.OpenURL(
                $@"https://solscan.io/account/{UnitySolanaWallet.Instance.wallet.Account.GetPublicKey}?cluster=devnet");
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public async void Refresh()
        {
            failedAirdrop.SetActive(false);
            successAirdrop.SetActive(false);
            await UpdateWalletBalanceDisplay();
            await GetOwnedTokenAccounts();
        }

        /// <summary>
        /// Disconnects to web socket.
        /// </summary>
        private void DisconnectToWebSocket()
        {
            MainThreadDispatcher.Instance().Enqueue(() => { Manager.ShowScreen("LoginScreen"); });
            MainThreadDispatcher.Instance().Enqueue(() => { UnitySolanaWallet.Instance.DeleteWalletAndClearKey(); });
        }

        /// <summary>
        /// Gets the owned token accounts.
        /// </summary>
        private async Task GetOwnedTokenAccounts()
        {
            TokenAccount[] result =
                await UnitySolanaWallet.Instance.GetOwnedTokenAccounts(UnitySolanaWallet.Instance.wallet.GetAccount(0));
            MainThreadDispatcher.Instance().Enqueue(() => { coinAmount.text = "0"; });
            if (result != null && result.Length > 0)
            {
                foreach (TokenAccount item in result)
                {
                    if (item.Account.Data.Parsed.Info.Mint == UnitySolanaWallet.Instance.tokens.knownTokens[0].mint)
                    {
                        UnitySolanaWallet.Instance.mintTokenAccount = item.pubkey;
                        MainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            Debug.Log(item.Account.Data.Parsed.Info.TokenAmount.Amount);
                            GameData.RemainingCoins = Int32.Parse(item.Account.Data.Parsed.Info.TokenAmount.Amount);
                            coinAmount.text = item.Account.Data.Parsed.Info.TokenAmount.Amount;
                        });
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the wallet balance display.
        /// </summary>
        private async Task UpdateWalletBalanceDisplay()
        {
            if (UnitySolanaWallet.Instance.wallet is null) return;

            double sol =
                await UnitySolanaWallet.Instance.GetSolAmmount(UnitySolanaWallet.Instance.wallet.GetAccount(0));
            Debug.Log(sol);
            MainThreadDispatcher.Instance().Enqueue(() => { solAmount.text = $"{sol:0.000000} SOL"; });
        }
    }
}