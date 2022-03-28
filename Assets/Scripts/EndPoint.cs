// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-19-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-21-2022
// ***********************************************************************
// <copyright file="EndPoint.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

/// <summary>
/// Class EndPoint.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class EndPoint : MonoBehaviour
{
    /// <summary>
    /// The game manager
    /// </summary>
    public GameManager gameManager;
    // Start is called before the first frame update
    // called when this GameObject collides with GameObject2.
    /// <summary>
    /// Called when [trigger enter2 d].
    /// </summary>
    /// <param name="col">The col.</param>
    void OnTriggerEnter2D(Collider2D col)
    {
       
        gameManager.UpdateCheckPoint(col.name,gameObject.name);
    }
}