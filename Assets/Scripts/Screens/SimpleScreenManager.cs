// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="SimpleScreenManager.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screens
{
    /// <summary>
    /// Class SimpleScreenManager.
    /// Implements the <see cref="UnityEngine.MonoBehaviour" />
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class SimpleScreenManager : MonoBehaviour
    {
        /// <summary>
        /// The bet canvas
        /// </summary>
        public GameObject betCanvas;
        /// <summary>
        /// The screens
        /// </summary>
        public SimpleScreen[] screens;
        /// <summary>
        /// The racing point animator
        /// </summary>
        public Animator racingPointAnimator;
        /// <summary>
        /// The switch board
        /// </summary>
        private static readonly int SwitchBoard = Animator.StringToHash("SwitchBoard");

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            PopulateDictionary();
            ShowScreen("StartScreen");
        }

        /// <summary>
        /// Populates the dictionary.
        /// </summary>
        private void PopulateDictionary()
        {
            if (screens != null && screens.Length > 0)
            {
                foreach (SimpleScreen screen in screens)
                {
                    SetupScreen(screen);
                }

                screens[0].gameObject.SetActive(true);
                screens[0].ShowScreen();
            }
        }

        /// <summary>
        /// Setups the screen.
        /// </summary>
        /// <param name="screen">The screen.</param>
        private void SetupScreen(SimpleScreen screen)
        {
            screen.gameObject.SetActive(false);
            screen.Manager = this;
        }


        /// <summary>
        /// Shows the screen.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        public void ShowScreen(string screenName)
        {
            StopAllCoroutines();
            StartCoroutine(SwapScreenCoroutine(screenName));
        }

        /// <summary>
        /// Begins the game.
        /// </summary>
        public void BeginGame()
        {
            StopAllCoroutines();
            StartCoroutine(BeginGameCoroutine());
        }

        /// <summary>
        /// Begins the game coroutine.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        private IEnumerator BeginGameCoroutine()
        {
            racingPointAnimator.SetTrigger(SwitchBoard);
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);
            betCanvas.SetActive(true);
        }


        /// <summary>
        /// Swaps the screen coroutine.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <returns>IEnumerator.</returns>
        private IEnumerator SwapScreenCoroutine(string screenName)
        {
            racingPointAnimator.SetTrigger(SwitchBoard);
            yield return new WaitForSeconds(1);
            foreach (var simpleScreen in screens)
            {
                if (simpleScreen.gameObject.name != screenName)
                {
                    simpleScreen.HideScreen();
                }
                else
                {
                    simpleScreen.ShowScreen();
                }
            }
        }
    }
}