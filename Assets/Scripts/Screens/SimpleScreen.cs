// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="SimpleScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Screens
{
    /// <summary>
    /// Class SimpleScreen.
    /// Implements the <see cref="UnityEngine.MonoBehaviour" />
    /// Implements the <see cref="Screens.ISimpleScreen" />
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Screens.ISimpleScreen" />
    public class SimpleScreen : MonoBehaviour, ISimpleScreen
    {
        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public SimpleScreenManager Manager { get; set; }

        /// <summary>
        /// Hides the screen.
        /// </summary>
        public virtual void HideScreen()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the screen.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void ShowScreen(object data = null)
        {
            gameObject.SetActive(true);
        }
    }
}