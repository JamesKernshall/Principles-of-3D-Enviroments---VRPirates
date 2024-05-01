using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class HeightModifier : MonoBehaviour
{
    [SerializeField] private float controllerDeadZone = 0.2f;
    [SerializeField] private float heightSpeed = 0.5f;
    [SerializeField] private Transform modifyTransform;
    [SerializeField] private float minCameraY = 0;
    [SerializeField] private float maxCameraY = 4;

    [SerializeField]
    [Tooltip("The Input System Action that will be used to read Turn data from the left hand controller. Must be a Value Vector2 Control.")]
    InputActionProperty m_LeftHandTurnAction = new InputActionProperty(new InputAction("Left Hand Turn", expectedControlType: "Vector2"));
    /// <summary>
    /// The Input System Action that Unity uses to read Turn data from the left hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
    /// </summary>
    public InputActionProperty leftHandTurnAction
    {
        get => m_LeftHandTurnAction;
        set => SetInputActionProperty(ref m_LeftHandTurnAction, value);
    }

    [SerializeField]
    [Tooltip("The Input System Action that will be used to read Turn data from the right hand controller. Must be a Value Vector2 Control.")]
    InputActionProperty m_RightHandTurnAction = new InputActionProperty(new InputAction("Right Hand Turn", expectedControlType: "Vector2"));
    /// <summary>
    /// The Input System Action that Unity uses to read Turn data from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
    /// </summary>
    public InputActionProperty rightHandTurnAction
    {
        get => m_RightHandTurnAction;
        set => SetInputActionProperty(ref m_RightHandTurnAction, value);
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnEnable()
    {
        m_LeftHandTurnAction.EnableDirectAction();
        m_RightHandTurnAction.EnableDirectAction();
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnDisable()
    {
        m_LeftHandTurnAction.DisableDirectAction();
        m_RightHandTurnAction.DisableDirectAction();
    }

    /// <inheritdoc />
    protected Vector2 ReadInput()
    {
        var leftHandValue = m_LeftHandTurnAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        var rightHandValue = m_RightHandTurnAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

        return leftHandValue + rightHandValue;
    }

    void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying)
            property.DisableDirectAction();

        property = value;

        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }




    private void Update()
    { 
        if (Mathf.Abs(ReadInput().y) > 0.1f)
        {
            AdjustHeight(ReadInput().y);
        }
    }


    private void AdjustHeight(float yInput) 
    {
        float heightAdjustmentAmount = yInput * Time.deltaTime * heightSpeed;

        if (Camera.main.transform.position.y + heightAdjustmentAmount > minCameraY &&
            Camera.main.transform.position.y + heightAdjustmentAmount < maxCameraY) 
        {
            modifyTransform.localPosition = new Vector3(modifyTransform.localPosition.x, modifyTransform.localPosition.y + heightAdjustmentAmount, modifyTransform.localPosition.z);
        }
    }


}
