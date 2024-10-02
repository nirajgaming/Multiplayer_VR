
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandController : MonoBehaviour
{
    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;

    private Animator handAnimator;
    private float gripValue;
    private float triggerValue;
    
    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        SendGripInputValueToHandAnimator();
        SendTriggerInputValueToHandAnimator();
    }

    void SendGripInputValueToHandAnimator()
    {
        gripValue = gripInputActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Grip_Value", gripValue);
    }

    void SendTriggerInputValueToHandAnimator()
    {
        triggerValue = triggerInputActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger_Value", triggerValue);
    }
}
