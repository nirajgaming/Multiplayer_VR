using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class LocalXRRig : MonoBehaviour, INetworkRunnerCallbacks
{

    // References to the Transforms for tracking
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    HandController leftHandController;
    HandController rightHandController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHandController == null)
        {
            HandController[] leftHandControllerComponent = leftHandTransform.GetComponentsInChildren<HandController>();

            if(leftHandControllerComponent.Length > 0)
            {
                leftHandController = leftHandControllerComponent[0];
            }
        }

        if (rightHandController == null)
        {
            HandController[] rightHandControllerComponent = rightHandTransform.GetComponentsInChildren<HandController>();

            if (rightHandControllerComponent.Length > 0)
            {
                rightHandController = rightHandControllerComponent[0];
            }

        }
    }

    #region INetworkRunnerCallbacks

    // Prepare the input, that will be read by NetworkRig in the FixedUpdateNetwork
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        RigInput rigInput = new RigInput();
        rigInput.playAreaPosition = transform.position;
        rigInput.playAreaRotation = transform.rotation;
        rigInput.leftHandPosition = leftHandTransform.position;
        rigInput.leftHandRotation = leftHandTransform.rotation;
        rigInput.rightHandPosition = rightHandTransform.position;
        rigInput.rightHandRotation = rightHandTransform.rotation;
        rigInput.headsetPosition = headTransform.position;
        rigInput.headsetRotation = headTransform.rotation;

        if(leftHandController && rightHandController)
        {
            rigInput.leftGripValue = leftHandController.GetGripValue();
            rigInput.rightGripValue = rightHandController.GetGripValue();
            rigInput.leftTriggerValue = leftHandController.GetTriggerValue();
            rigInput.rightTriggerValue = rightHandController.GetTriggerValue();
        }
        

        input.Set(rigInput);
    }

    #endregion

    #region INetworkRunnerCallbacks (unused)
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}

// Structure to hold all inputs that needs to synced over the network
public struct RigInput : INetworkInput
{
    public Vector3 playAreaPosition;
    public Quaternion playAreaRotation;
    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;
    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;
    public Vector3 headsetPosition;
    public Quaternion headsetRotation;
    public float leftGripValue;
    public float rightGripValue;
    public float leftTriggerValue;
    public float rightTriggerValue;
}
