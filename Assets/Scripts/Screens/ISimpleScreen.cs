// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-27-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-27-2022
// ***********************************************************************
// <copyright file="ISimpleScreen.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Screens
{
    /// <summary>
    /// Interface ISimpleScreen
    /// </summary>
    public interface ISimpleScreen
    {
        public interface ISimpleScreen
        {
            SimpleScreenManager Manager { get; set; }
            void ShowScreen(object data = null);
            void HideScreen();
        }
    }
}