using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : Subject
{
    Animator _animator;
    Coroutine _currentDizzyResetRoutine = null;
    int _currentHealth = 3;
    [SerializeField] UI _userInterface;
    [SerializeField] AudioManager _audioManager;
    [SerializeField] CameraSystem _camSystem;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "rock") {
            _currentHealth -= 1;
            _userInterface.DecreaseHeartCountToDisplay();
            _audioManager.PlayHitEffect();
            _camSystem.ZoomInCamera();
            _animator.SetBool("isDizzy", true);
            _currentDizzyResetRoutine = StartCoroutine(IDizzyResetRoutine());
        }
    }

    IEnumerator IDizzyResetRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        _animator.SetBool("isDizzy", false);
    }
}
