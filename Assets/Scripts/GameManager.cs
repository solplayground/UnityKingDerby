// ***********************************************************************
// Assembly         : Assembly-CSharp
// Author           : James Shen
// Created          : 03-21-2022
//
// Last Modified By : James Shen
// Last Modified On : 03-28-2022
// ***********************************************************************
// <copyright file="GameManager.cs" company="Solana Playground">
//     Copyright (c) Solana Playground. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Enum GameState
/// </summary>
public enum GameState
{
    /// <summary>
    /// The betting
    /// </summary>
    Betting,
    /// <summary>
    /// The racing
    /// </summary>
    Racing,
    /// <summary>
    /// The show result
    /// </summary>
    ShowResult,
}

/// <summary>
/// Enum CheckPointPos
/// </summary>
public enum CheckPointPos
{
    /// <summary>
    /// The start point
    /// </summary>
    StartPoint,
    /// <summary>
    /// The check point1
    /// </summary>
    CheckPoint1,
    /// <summary>
    /// The check point2
    /// </summary>
    CheckPoint2,
    /// <summary>
    /// The check point3
    /// </summary>
    CheckPoint3,
    /// <summary>
    /// The check point4
    /// </summary>
    CheckPoint4,
    /// <summary>
    /// The check point5
    /// </summary>
    CheckPoint5,
    /// <summary>
    /// The finish point
    /// </summary>
    FinishPoint,
}

/// <summary>
/// Class GameManager.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The result display
    /// </summary>
    public ResultDisplay resultDisplay;
    /// <summary>
    /// The bet board manager
    /// </summary>
    public BetBoardManager betBoardManager;
    /// <summary>
    /// The audio manager
    /// </summary>
    public AudioManager audioManager;
    /// <summary>
    /// The horse animators
    /// </summary>
    public Animator[] horseAnimators;
    /// <summary>
    /// The horse positions
    /// </summary>
    public Transform[] horsePositions;

    /// <summary>
    /// The racing point animator
    /// </summary>
    public Animator racingPointAnimator;

    /// <summary>
    /// The finish point
    /// </summary>
    public Transform finishPoint;
    /// <summary>
    /// The door open
    /// </summary>
    public GameObject doorOpen;
    /// <summary>
    /// The check point position
    /// </summary>
    private readonly CheckPointPos[] _checkPointPos = new CheckPointPos[6];
    /// <summary>
    /// The game state
    /// </summary>
    private GameState _gameState;
    /// <summary>
    /// The begin racing
    /// </summary>
    private static readonly int BeginRacing = Animator.StringToHash("StartRacing");
    /// <summary>
    /// The play racing
    /// </summary>
    private static readonly int PlayRacing = Animator.StringToHash("Racing");
    /// <summary>
    /// The end racing
    /// </summary>
    private static readonly int EndRacing = Animator.StringToHash("EndRacing");
    /// <summary>
    /// The switch board
    /// </summary>
    private static readonly int SwitchBoard = Animator.StringToHash("SwitchBoard");
    /// <summary>
    /// The initialize horse positions
    /// </summary>
    private readonly Vector2[] _initHorsePositions = new Vector2[6];
    /// <summary>
    /// The turn right down
    /// </summary>
    private static readonly int TurnRightDown = Animator.StringToHash("TurnRightDown");
    /// <summary>
    /// The turn down
    /// </summary>
    private static readonly int TurnDown = Animator.StringToHash("TurnDown");
    /// <summary>
    /// The turn left down
    /// </summary>
    private static readonly int TurnLeftDown = Animator.StringToHash("TurnLeftDown");
    /// <summary>
    /// The turn left
    /// </summary>
    private static readonly int TurnLeft = Animator.StringToHash("TurnLeft");
    /// <summary>
    /// The turn right
    /// </summary>
    private static readonly int TurnRight = Animator.StringToHash("Reset");


    // Start is called before the first frame update
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        _gameState = GameState.Betting;
        for (var i = 0; i < _initHorsePositions.Length; i++)
        {
            _initHorsePositions[i] = new Vector2 { x = horsePositions[i].position.x, y = horsePositions[i].position.y };
        }

        GameData.RemainingCoins = PlayerPrefs.GetInt("Score", 500);
        InitializedNewGame();
    }

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        if (_gameState == GameState.Racing)
        {
            for (var i = 0; i < _initHorsePositions.Length; i++)
            {
                MoveHorse(i);
            }
        }
    }

    /// <summary>
    /// Plays the racing background sound.
    /// </summary>
    private void PlayRacingBackgroundSound()
    {
        audioManager.PlaySound(AudioTypes.RacingBeginBackground);
    }

    /// <summary>
    /// Stops the racing background sound.
    /// </summary>
    public void StopRacingBackgroundSound()
    {
        audioManager.StopSound();
    }

    /// <summary>
    /// Plays the background sound.
    /// </summary>
    public void PlayBackgroundSound()
    {
        audioManager.PlaySound(AudioTypes.Background);
    }

    /// <summary>
    /// Starts the or new.
    /// </summary>
    public void StartOrNew()
    {
        switch (_gameState)
        {
            case GameState.Betting:
                if (GameData.HasBets())
                {
                    audioManager.PlaySound(AudioTypes.Button);
                    StopAllCoroutines();
                    StartCoroutine(BeginRacingCoroutine());
                }

                break;
            case GameState.Racing:
                break;
            case GameState.ShowResult:
                audioManager.PlaySound(AudioTypes.Button);
                StopAllCoroutines();
                StartCoroutine(StartNewBetCoroutine());


                break;
        }
    }

    /// <summary>
    /// Bets the horses.
    /// </summary>
    /// <param name="index">The index.</param>
    public void BetHorses(int index)
    {
        if (_gameState == GameState.Betting)
        {
            var reBetOption = GameData.ReBetOptions[GameData.ReBetOptionIndex];
            var totalBet = CalculateTotalBets();
            if ((GameData.Bets[index] + 1) * reBetOption <= 80 && (totalBet + reBetOption <= GameData.RemainingCoins))
            {
                GameData.Bets[index] += 1;
                audioManager.PlaySound(AudioTypes.Bet);
                betBoardManager.UpdateCoins(index);
                betBoardManager.UpdateRemainingCoins(GameData.RemainingCoins - totalBet - reBetOption);
            }
        }
    }

    /// <summary>
    /// Res the bet.
    /// </summary>
    public void ReBet()
    {
        if (_gameState == GameState.Betting)
        {
            var reBetOption = GameData.ReBetOptions[(GameData.ReBetOptionIndex + 1) % GameData.ReBetOptions.Length];
            var canReBet = true;
            foreach (var t in GameData.Bets)
            {
                if (t * reBetOption > 80)
                {
                    canReBet = false;
                    break;
                }
            }

            if (canReBet)
            {
                GameData.ReBetOptionIndex += 1;
                GameData.ReBetOptionIndex %= GameData.ReBetOptions.Length;
                var totalBet = CalculateTotalBets();
                if (totalBet <= GameData.RemainingCoins)
                {
                    audioManager.PlaySound(AudioTypes.Button);
                    betBoardManager.UpdateReBet();
                    betBoardManager.UpdateRemainingCoins(GameData.RemainingCoins - totalBet);
                }
                else
                {
                    GameData.ReBetOptionIndex -= 1;
                    if (GameData.ReBetOptionIndex < 0)
                    {
                        GameData.ReBetOptionIndex = GameData.ReBetOptions.Length - 1;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the check point.
    /// </summary>
    /// <param name="horseName">Name of the horse.</param>
    /// <param name="checkPoint">The check point.</param>
    public void UpdateCheckPoint(string horseName, string checkPoint)
    {
        var horseIndex = GetHorseIndex(horseName);
        if (horseIndex >= 0)
        {
            var nextCheckPoint = GetNextCheckPoint(checkPoint);
            _checkPointPos[horseIndex] = nextCheckPoint;
            switch (nextCheckPoint)
            {
                case CheckPointPos.CheckPoint2:
                    horseAnimators[horseIndex].SetTrigger(TurnRightDown);
                    break;
                case CheckPointPos.CheckPoint3:
                    horseAnimators[horseIndex].SetTrigger(TurnDown);
                    break;
                case CheckPointPos.CheckPoint4:
                    horseAnimators[horseIndex].SetTrigger(TurnLeftDown);
                    break;
                case CheckPointPos.CheckPoint5:
                    horseAnimators[horseIndex].SetTrigger(TurnLeft);
                    break;
                case CheckPointPos.StartPoint:
                    if (horseIndex == GameData.RacingResult.HorseNumber1 - 1)
                    {
                        StopAllCoroutines();
                        resultDisplay.DisplayResult(GameData.RacingResult);
                        StartCoroutine(MoveToResultBoard());
                        //Display result
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Calculates the total bets.
    /// </summary>
    /// <returns>System.Int32.</returns>
    private int CalculateTotalBets()
    {
        var reBetOption = GameData.ReBetOptions[GameData.ReBetOptionIndex];
        var total = 0;
        foreach (var t in GameData.Bets)
        {
            total += reBetOption * t;
        }

        return total;
    }

    /// <summary>
    /// Initializeds the new game.
    /// </summary>
    private void InitializedNewGame()
    {
        GameData.RandomOdds();
        GameData.ResetBets();
        Debug.Log(
            $@"{GameData.RacingResult.HorseNumber1}-{GameData.RacingResult.HorseNumber2} {GameData.RacingResult.Coins}");
        betBoardManager.UpdateRemainingCoins(GameData.RemainingCoins);
        betBoardManager.UpdateWonCoins(0);
        betBoardManager.ShowNewBet();
        for (var i = 0; i < _checkPointPos.Length; i++)
        {
            _checkPointPos[i] = CheckPointPos.StartPoint;
            horsePositions[i].position = new Vector3(_initHorsePositions[i].x, _initHorsePositions[i].y, 0);
            horseAnimators[i].SetTrigger(TurnRight);
        }
    }


    /// <summary>
    /// Moves to result board.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    private IEnumerator MoveToResultBoard()
    {
        _gameState = GameState.ShowResult;
        yield return new WaitForSeconds(6);
        doorOpen.SetActive(false);
        betBoardManager.ShowResult();
        racingPointAnimator.SetTrigger(EndRacing);
        for (var i = 0; i < _checkPointPos.Length; i++)
        {
            _checkPointPos[i] = CheckPointPos.StartPoint;
        }

        //Check won or lose
        var reBetOption = GameData.ReBetOptions[GameData.ReBetOptionIndex];
        var totalBet = CalculateTotalBets();
        var won = GameData.Bets[GameData.ResultIndex] * reBetOption *
                  GameData.Odds[GameData.CoinValueIndexes[GameData.ResultIndex]];

        if (won > 0)
        {
            betBoardManager.UpdateWonCoins(won);
            audioManager.PlaySound(AudioTypes.Winner);
            yield return new WaitForSeconds(1);
            var displayRemaining = GameData.RemainingCoins - totalBet;
            var waitSeconds = 4f / won;
            for (var i = 0; i < won; i++)
            {
                betBoardManager.UpdateWonCoins(won - i - 1);
                betBoardManager.UpdateRemainingCoins(displayRemaining + i + 1);
                yield return new WaitForSeconds(waitSeconds);
            }
        }

        GameData.RemainingCoins = GameData.RemainingCoins - totalBet + won;
        UnitySolanaWallet.Instance.SettleReward(won-totalBet);
        PlayerPrefs.SetInt("Score", GameData.RemainingCoins);
        betBoardManager.UpdateRemainingCoins(GameData.RemainingCoins);
    }


    /// <summary>
    /// Gets the index of the horse.
    /// </summary>
    /// <param name="horseName">Name of the horse.</param>
    /// <returns>System.Int32.</returns>
    private int GetHorseIndex(string horseName)
    {
        switch (horseName)
        {
            case "WhiteBlueHorse":
                return 0;
            case "PurpleRedHorse":
                return 1;
            case "RedBlueHorse":
                return 2;
            case "BlueRedHorse":
                return 3;
            case "YellowBlueHorse":
                return 4;
            case "GreenRedHorse":
                return 5;
        }

        return -1;
    }

    /// <summary>
    /// Gets the next check point.
    /// </summary>
    /// <param name="checkPoint">The check point.</param>
    /// <returns>CheckPointPos.</returns>
    private CheckPointPos GetNextCheckPoint(string checkPoint)
    {
        switch (checkPoint)
        {
            case "Checkpoint1":
                return CheckPointPos.CheckPoint2;
            case "Checkpoint2":
                return CheckPointPos.CheckPoint3;
            case "Checkpoint3":
                return CheckPointPos.CheckPoint4;
            case "Checkpoint4":
                return CheckPointPos.CheckPoint5;
            case "Checkpoint5":
                return CheckPointPos.FinishPoint;
            case "EndPoint":
                return CheckPointPos.StartPoint;
        }

        return CheckPointPos.StartPoint;
    }


    //right
    /// <summary>
    /// Calculates the check point1 destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateCheckPoint1Destination(int index)
    {
        var position = transform.position;
        var destinationX = position.x - 3f;
        var destination = new Vector2
        {
            x = destinationX,
            y = horsePositions[index].position.y
        };


        //random speed
        var num = Random.Range(0, 2);
        if (num == 0)
        {
            destination.x = destinationX + 3f;
        }
        else if (num == 1)
        {
            destination.x = destinationX + 6f;
        }


        return destination;
    }


    //right down
    /// <summary>
    /// Calculates the check point2 destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateCheckPoint2Destination(int index)
    {
        var position = transform.position;
        var destinationX = position.x;


        var destination = new Vector2
        {
            x = destinationX + 3f + index * 0.1f,
            y = position.y - 3f - index * 0.5f
        };


        return destination;
    }

    //down//
    /// <summary>
    /// Calculates the check point3 destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateCheckPoint3Destination(int index)
    {
        var position = transform.position;
        var destinationY = position.y;
        var destination = new Vector2
        {
            x = position.x + 0.1f * index,
            y = destinationY - 0.5f * index
        };

        var deltaX = destinationY - horsePositions[index].position.y;
        //random speed
        var num = Random.Range(0, 3);
        if (num == 1)
        {
            destination.y = horsePositions[index].position.y + deltaX * 0.5f - 0.5f * index;
        }
        else if (num == 2)
        {
            destination.y = horsePositions[index].position.y + deltaX * 0.75f - 0.5f * index;
        }
        else
        {
            destination.y = horsePositions[index].position.y + deltaX * 1f - 0.5f * index;
        }


        return destination;
    }

    // left down
    /// <summary>
    /// Calculates the check point4 destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateCheckPoint4Destination(int index)
    {
        var position = transform.position;
        var destinationX = position.x;
        var destination = new Vector2
        {
            x = destinationX - 3f + index * 0.25f,
            y = position.y - 0.5f - index * 0.5f
        };


        return destination;
    }


    /// <summary>
    /// Calculates the check point5 destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateCheckPoint5Destination(int index)
    {
        var position = transform.position;
        var destinationX = position.x;
        var destination = new Vector2
        {
            x = destinationX,
            y = horsePositions[index].position.y
        };

        //random speed
        var num = Random.Range(0, 2);
        if (num == 0)
        {
            destination.x = destinationX;
        }
        else if (num == 1)
        {
            destination.x = destinationX - 3f;
        }


        return destination;
    }

    /// <summary>
    /// Calculates the finish point destination.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>Vector2.</returns>
    private Vector2 CalculateFinishPointDestination(int index)
    {
        var position = transform.position;
        var destinationX = position.x - 1f;
        var destination = new Vector2
        {
            x = destinationX,
            y = horsePositions[index].position.y
        };

        //random speed
        var firstHorseIndex = GameData.RacingResult.HorseNumber1;
        var secondHorseIndex = GameData.RacingResult.HorseNumber2;
        var remainDistance = Mathf.Abs(horsePositions[index].position.x - finishPoint.position.x);
        var num = Random.Range(0, 2);
        if (num == 1)
        {
            destination.x = destinationX;
        }
        else
        {
            destination.x = destinationX - 4f;
        }

        if (remainDistance < 8f)
        {
            if (index == firstHorseIndex - 1)
            {
                destination.x = destinationX - 5.5f;
            }
            else
            {
                if (index == secondHorseIndex - 1)
                {
                    destination.x = destinationX - 5f;
                }
            }
        }

        return destination;
    }


    /// <summary>
    /// Moves the horse.
    /// </summary>
    /// <param name="index">The index.</param>
    private void MoveHorse(int index)
    {
        var nextHorseCheckPoint = _checkPointPos[index];
        switch (nextHorseCheckPoint)
        {
            case CheckPointPos.StartPoint:
                //Do nothing
                break;
            case CheckPointPos.CheckPoint1:
            {
                var destination = CalculateCheckPoint1Destination(index);
                horsePositions[index].position =
                    Vector2.LerpUnclamped(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
            case CheckPointPos.CheckPoint2:
            {
                var destination = CalculateCheckPoint2Destination(index);
                horsePositions[index].position =
                    Vector2.Lerp(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
            case CheckPointPos.CheckPoint3:
            {
                var destination = CalculateCheckPoint3Destination(index);
                horsePositions[index].position =
                    Vector2.Lerp(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
            case CheckPointPos.CheckPoint4:
            {
                var destination = CalculateCheckPoint4Destination(index);
                horsePositions[index].position =
                    Vector2.Lerp(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
            case CheckPointPos.CheckPoint5:
            {
                var destination = CalculateCheckPoint5Destination(index);
                horsePositions[index].position =
                    Vector2.LerpUnclamped(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
            case CheckPointPos.FinishPoint:
            {
                var destination = CalculateFinishPointDestination(index);
                horsePositions[index].position =
                    Vector2.LerpUnclamped(horsePositions[index].position, destination, Time.deltaTime);
            }
                break;
        }
    }

    /// <summary>
    /// Begins the racing coroutine.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    private IEnumerator BeginRacingCoroutine()
    {
        racingPointAnimator.SetTrigger(BeginRacing);
        PlayRacingBackgroundSound();
        yield return new WaitForSeconds(1);
        doorOpen.SetActive(true);
        yield return new WaitForSeconds(1);
        racingPointAnimator.SetTrigger(PlayRacing);
        _gameState = GameState.Racing;
        for (var i = 0; i < _checkPointPos.Length; i++)
        {
            _checkPointPos[i] = CheckPointPos.CheckPoint1;
        }
    }

    /// <summary>
    /// Starts the new bet coroutine.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    private IEnumerator StartNewBetCoroutine()
    {
        racingPointAnimator.SetTrigger(SwitchBoard);
        yield return new WaitForSeconds(1);
        betBoardManager.StopFlush();
        InitializedNewGame();
        yield return new WaitForSeconds(1);
        audioManager.PlaySound(AudioTypes.Waiting);
        _gameState = GameState.Betting;
    }
}