using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject _cedar = default;
    [SerializeField] private Vector3 _mousePosition = default;
    [SerializeField] private GameObject _iceSpace = default;
    [SerializeField] private GameObject _fireSpace = default;
    [SerializeField] private GameObject _pieMenu = default;
    private IceBeam _iceBeam = default;
    private FireBullet _fireBullet = default;
    private bool _menuIsActive = default;
    private bool _isIce = default;
    private PlayerController _playerController = default;
    private InputAction _inputSwitch = default;
    private float _halfWidth = default;

    private PowerType _lastSelected = default;

    void Start()
    {
        _playerController = new PlayerController();
        _playerController.Enable();
        _inputSwitch = _playerController.Land.SwitchMagic;
        _inputSwitch.Enable();
        _inputSwitch.started += ActivateMenu;
        _inputSwitch.canceled += DeactivateMenu;
        _iceBeam = GetComponent<IceBeam>();
        _fireBullet = GetComponent<FireBullet>();
    }

    private void Awake()
    {
        _cedar = GameObject.Find("Cedar");
    }

    private void OnDestroy()
    {
        _inputSwitch.Disable();
        _playerController.Disable();
    }

    void Update()
    {
        _mousePosition = Mouse.current.position.ReadValue();
         _halfWidth = Screen.width / 2f;
        //Debug.Log(_mousePosition.x);
        if (_menuIsActive)
        {
            CheckMenuSelection();
        }

    }
    
    private void CheckMenuSelection()
    {
        if (_mousePosition.x < _halfWidth)
        {
            _iceSpace.transform.localScale = Vector3.one * 1.1f;
            _fireSpace.transform.localScale = Vector3.one;
            _lastSelected = PowerType.Ice;

            _isIce = true;

        }
        else
        {
            _fireSpace.transform.localScale = Vector3.one * 1.1f;
            _iceSpace.transform.localScale = Vector3.one;
            _lastSelected = PowerType.Fire;

            _isIce = false;
        }
    }

    private void ActivateMenu(InputAction.CallbackContext context)
    {
        if (gameObject.activeInHierarchy)
        {
            _menuIsActive = true;
            _pieMenu.SetActive(true);
        }
    }

    private void DeactivateMenu(InputAction.CallbackContext context)
    {
        _menuIsActive = false;
        _pieMenu.SetActive(false);

        if (_isIce)
        {
            _iceBeam.Activate();
            _fireBullet.Deactivate();
        }
        else
        {
            _iceBeam.Deactivate();
            _fireBullet.Activate();
        }
    }

}

public enum PowerType
{
    Fire,
    Ice
}

