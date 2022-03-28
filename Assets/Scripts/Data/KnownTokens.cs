// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="KnownTokens.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Class KnownTokens.
    /// Implements the <see cref="UnityEngine.ScriptableObject" />
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(fileName = "Tokens", menuName = "Solana/KnownTokensData", order = 1)]
    public class KnownTokens : ScriptableObject
    {
        /// <summary>
        /// The known tokens
        /// </summary>
        public KnownToken[] knownTokens;

        /// <summary>
        /// Gets the known token.
        /// </summary>
        /// <param name="pubKey">The pub key.</param>
        /// <returns>KnownToken.</returns>
        public KnownToken GetKnownToken(string pubKey) {
            KnownToken token = knownTokens.FirstOrDefault((e) => e.mint == pubKey);

            if (token != null)
                return token;

            return null;
        }
    }

    /// <summary>
    /// Class KnownToken.
    /// </summary>
    [System.Serializable]
    public class KnownToken {
        /// <summary>
        /// The name
        /// </summary>
        public string name;
        /// <summary>
        /// The logo
        /// </summary>
        public Sprite logo;
        /// <summary>
        /// The mint
        /// </summary>
        public string mint;
    }
}
