//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Input System.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input System"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""da7e4852-0ac5-4a87-abab-1d21e71e9e70"",
            ""actions"": [
                {
                    ""name"": ""rightStick"",
                    ""type"": ""Value"",
                    ""id"": ""c6d0e6c1-9d23-407c-988c-2c82051b0c9f"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""toggleArm"",
                    ""type"": ""Button"",
                    ""id"": ""bdd25a1f-e931-470b-94ac-5a8381014289"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""leftTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""c04dea43-2455-42d7-b992-7763ace584f0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""rightTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""27ae91cc-c12e-4018-994e-af9d2ebe90a2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4396eae3-7eb6-4df7-9dd9-590c49ffd259"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""rightStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8020ebc5-d38a-4457-a6c1-704188631792"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""toggleArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09ca4a29-d344-4201-8dfe-adcc2a5368ad"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""leftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac78d080-6c9e-4977-bcd5-26f4a07cd6bb"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""rightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_rightStick = m_Gameplay.FindAction("rightStick", throwIfNotFound: true);
        m_Gameplay_toggleArm = m_Gameplay.FindAction("toggleArm", throwIfNotFound: true);
        m_Gameplay_leftTrigger = m_Gameplay.FindAction("leftTrigger", throwIfNotFound: true);
        m_Gameplay_rightTrigger = m_Gameplay.FindAction("rightTrigger", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_rightStick;
    private readonly InputAction m_Gameplay_toggleArm;
    private readonly InputAction m_Gameplay_leftTrigger;
    private readonly InputAction m_Gameplay_rightTrigger;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @rightStick => m_Wrapper.m_Gameplay_rightStick;
        public InputAction @toggleArm => m_Wrapper.m_Gameplay_toggleArm;
        public InputAction @leftTrigger => m_Wrapper.m_Gameplay_leftTrigger;
        public InputAction @rightTrigger => m_Wrapper.m_Gameplay_rightTrigger;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @rightStick.started += instance.OnRightStick;
            @rightStick.performed += instance.OnRightStick;
            @rightStick.canceled += instance.OnRightStick;
            @toggleArm.started += instance.OnToggleArm;
            @toggleArm.performed += instance.OnToggleArm;
            @toggleArm.canceled += instance.OnToggleArm;
            @leftTrigger.started += instance.OnLeftTrigger;
            @leftTrigger.performed += instance.OnLeftTrigger;
            @leftTrigger.canceled += instance.OnLeftTrigger;
            @rightTrigger.started += instance.OnRightTrigger;
            @rightTrigger.performed += instance.OnRightTrigger;
            @rightTrigger.canceled += instance.OnRightTrigger;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @rightStick.started -= instance.OnRightStick;
            @rightStick.performed -= instance.OnRightStick;
            @rightStick.canceled -= instance.OnRightStick;
            @toggleArm.started -= instance.OnToggleArm;
            @toggleArm.performed -= instance.OnToggleArm;
            @toggleArm.canceled -= instance.OnToggleArm;
            @leftTrigger.started -= instance.OnLeftTrigger;
            @leftTrigger.performed -= instance.OnLeftTrigger;
            @leftTrigger.canceled -= instance.OnLeftTrigger;
            @rightTrigger.started -= instance.OnRightTrigger;
            @rightTrigger.performed -= instance.OnRightTrigger;
            @rightTrigger.canceled -= instance.OnRightTrigger;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnRightStick(InputAction.CallbackContext context);
        void OnToggleArm(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
    }
}
