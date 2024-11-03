using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkTransform))]
public class NetworkXRRig : NetworkBehaviour
{

    private LocalXRRig localXRRig;
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    public List<GameObject> meshVisuals = new List<GameObject>();

    public bool IsbelongToLocalPlayer => Object.HasInputAuthority;

    public override void Spawned()
    {
        base.Spawned();

        if (IsbelongToLocalPlayer)
        {
            localXRRig = FindObjectOfType<LocalXRRig>();
            if (localXRRig == null) Debug.LogError("Missing LocalXRRig in the scene");

            DisableAllMeshes();
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // Receive input from server for simulation
        if (GetInput<RigInput>(out var input))
        {
            transform.position = input.playAreaPosition;
            transform.rotation = input.playAreaRotation;
            leftHand.transform.position = input.leftHandPosition;
            leftHand.transform.rotation = input.leftHandRotation;
            rightHand.transform.position = input.rightHandPosition;
            rightHand.transform.rotation = input.rightHandRotation;
            head.transform.position = input.headsetPosition;
            head.transform.rotation = input.headsetRotation;


            leftHandAnimator.SetFloat("Grip_Value", input.leftGripValue);
            leftHandAnimator.SetFloat("Trigger_Value", input.leftTriggerValue);
            rightHandAnimator.SetFloat("Grip_Value", input.rightGripValue);
            rightHandAnimator.SetFloat("Trigger_Value", input.rightTriggerValue);
        }
    }

    public override void Render()
    {
        base.Render();

        // Local simulation of Networked Rig using inputs captured by Local XR Rig
        if (IsbelongToLocalPlayer)
        {
            if(localXRRig != null)
            {
                transform.position = localXRRig.transform.position;
                transform.rotation = localXRRig.transform.rotation;
                leftHand.transform.position = localXRRig.leftHandTransform.position;
                leftHand.transform.rotation = localXRRig.leftHandTransform.rotation;
                rightHand.transform.position = localXRRig.rightHandTransform.position;
                rightHand.transform.rotation = localXRRig.rightHandTransform.rotation;
                head.transform.position = localXRRig.headTransform.position;
                head.transform.rotation = localXRRig.headTransform.rotation;
            }
            
        }
    }

    void DisableAllMeshes()
    {
        for (int i = 0; i < meshVisuals.Count; i++)
        {
            meshVisuals[i].SetActive(false);
        }
    }
}
