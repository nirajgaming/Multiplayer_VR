using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class SpawnManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject networkedXRRigPrefab;
    public static SpawnManager _instance;

    // Dictionary of spawned user prefabs, to store them on the server for host topology,
    // and destroy them on disconnection
    private Dictionary<PlayerRef, NetworkObject> _spawnedUsers = new Dictionary<PlayerRef, NetworkObject>();

    private void Awake()
    {
        Debug.LogError("Inside SpawnManager");
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnNetworkedRig(NetworkRunner runner, PlayerRef player)
    {
        // The user's prefab has to be spawned by the host
        if (runner.IsServer && networkedXRRigPrefab != null)
        {
            // We make sure to give the input authority to the connecting player for their user's object
            NetworkObject networkPlayerObject = runner.Spawn(networkedXRRigPrefab, position: transform.position, rotation: transform.rotation, inputAuthority: player, (runner, obj) => {
            });

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedUsers.Add(player, networkPlayerObject);
        }
    }

    public void DeSpawnNetworkedRig(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar (only the host would have stored the spawned game object)
        if (_spawnedUsers.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedUsers.Remove(player);
        }
    }

    #region Callback implementations

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
