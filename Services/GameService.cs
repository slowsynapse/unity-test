using System.Collections.Generic;
using Commands;
using Firesplash.UnityAssets.SocketIO;
using Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.injection;

public enum PlayerState
{
    Connected,
    Loading,
    Idle,
    Waiting
}

public enum ActionType
{
    AttackBest = 0,
    Defend = 1,
    Card = 2,
    None = 3,

    AttackNoWeapon = 4,
    AttackMelee = 5,
    AttackRanged = 6,
    AttackAll = 7,
}

[Singleton]
public class GameService
{
    [Inject] private PlayerStateModel _model;
    [Inject] private PlayerHandModel _hand;

    [Inject] private DrawCard _drawCard;
    [Inject] private PlayerAction _playerAction;
    [Inject] private OpponentAction _opponentAction;
    [Inject] private TurnReset _turnReset;
    [Inject] private Resolve _resolve;

    private SocketIOInstance _socket;

    public void Init(SocketIOInstance value)
    {
        Injector.Instance.Resolve(this);

        _socket = value;
        _socket.On("prepare", OnPrepare);
        _socket.On("start", OnStart);
        _socket.On("draw", OnDraw);

        _socket.On("player_action", OnPlayerAction);
        _socket.On("opponent_action", OnOpponentAction);

        _socket.On("resolve", OnResolve);

        _socket.On("new_turn", OnResetTurn);
        _socket.On("update", OnUpdateStats);
        _socket.On("end_game", OnEndGame);
    }

    private void OnPrepare(string data)
    {
        Debug.Log("OnPrepare::\n" + data);
        SceneManager.LoadScene("GameScene");

        UpdateStatus(PlayerState.Waiting);
    }

    private void OnStart(string data)
    {
        Debug.Log("OnStart::\n" + data);

        _model.PlayerId = _socket.SocketID;
        _model.Set(JsonConvert.DeserializeObject<Dictionary<string, PlayerGameState>>(data));
    }

    private void OnDraw(string data)
    {
        Debug.Log("OnDraw::\n" + data);
        _drawCard.Dispatch(JsonConvert.DeserializeObject<CardInPlay>(data));
    }

    private void OnPlayerAction(string data)
    {
        Debug.Log("OnPlayerAction::\n" + data);
        _playerAction.Dispatch(JsonConvert.DeserializeObject<PlayerActionPayload>(data));
    }

    private void OnOpponentAction(string data)
    {
        Debug.Log("OnOpponentAction::\n" + data);
        _opponentAction.Dispatch();
    }

    private void OnResolve(string data)
    {

        data = data.Replace("\\", "");
        Debug.Log("OnResolve::\n" + data);

        var playerActions = JsonConvert.DeserializeObject<Dictionary<string, PlayerActionPayload[]>>(data);
        _resolve.Dispatch(playerActions[_model.PlayerId][0], playerActions[_model.OpponentId][0]);
    }

    private void OnResetTurn(string data)
    {
        Debug.Log("OnResetTurn::\n" + data);
        if (int.TryParse(data, out var seconds))
            _turnReset.Dispatch(seconds);
    }

    private void OnUpdateStats(string data)
    {
        var parsed = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, PlayerGameState>>>(data);
        _model.Set(parsed["players"]);
        // _hand.Trim(_model.Player.CardsDrawn);
    }

    private void OnEndGame(string data)
    {
        // throw new System.NotImplementedException();
    }

    public void UpdateStatus(PlayerState state, string payload = null)
    {
        _socket.Emit("player_status", JsonConvert.SerializeObject(new
        {
            status = state.ToString().ToLower(), payload
        }), false);
    }

    public void SendChatMessage(string message)
    {
        _socket.Emit("chat", JsonConvert.SerializeObject(new
        {
            message
        }), false);
    }

    public void SendUserInput(ActionType action, int? payload = null, int? target = null)
    {
        _socket.Emit("userInput", JsonConvert.SerializeObject(new
        {
            type = (int)action,
            payload,
            target
        }), false);
    }
}