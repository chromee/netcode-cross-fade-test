using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private string _defaultStateName;
    [SerializeField] private string _crossFadeStateName;
    [SerializeField] private float _waitTime;

    private Animator _animator;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        transform.position = Vector3.right * OwnerClientId;
    }

    private void Update()
    {
        if (!IsLocalPlayer) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        _animator.CrossFade(_crossFadeStateName, 0, 0, 0);
        yield return new WaitForSeconds(_waitTime);
        _animator.CrossFade(_defaultStateName, 0, 0, 0);
    }
}
