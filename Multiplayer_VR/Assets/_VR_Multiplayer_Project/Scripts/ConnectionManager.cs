using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;

public class ConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static ConnectionManager _instance;
    private NetworkRunner _runner;
    public string roomName;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void StartNewSession()
    {
        StartMultiplayerExperience(GameMode.Host);
    }
    public void JoinExistingSession()
    {
        StartMultiplayerExperience(GameMode.Client);
    }

    public void LeaveSession()
    {
        if(_runner.IsRunning)
            _runner.Shutdown();
       // SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    async void StartMultiplayerExperience(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo for the Multiplayer_Room_Scene
        var scene = SceneRef.FromIndex(1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    #region Callback implementations

    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){
        Debug.LogError("Inside COnnectionManager on PlayerJoined called");
        SpawnManager._instance.SpawnNetworkedRig(runner, player);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){
        Debug.LogError("Leaving session by :" + player.ToString());
        SpawnManager._instance.DeSpawnNetworkedRig(runner, player);
    }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}
    public void OnSceneLoadDone(NetworkRunner runner){}
    public void OnSceneLoadStart(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){
        // Callback when Runner shuts down
        SceneManager.LoadScene("Intro_Scene");

    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}

    #endregion
}
