using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNarrationSystem : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerSubject;
    int _jumpCount = 0;
    int _jumpAudioThreshold = 3;
    int _totalEnemyHits = 0;
    int _enemyHitThreshold = 6;
    int _health = 3;
    Coroutine _currentJumpResetRoutine = null;
    [SerializeField] AudioClip _jumpingAudioClip;
    [SerializeField] AudioClip _dieAudioClip;
    [SerializeField] AudioClip _encounterAudioClip;
    [SerializeField] AudioClip _attackAudioClip;
    [SerializeField] AudioClip _hurtAudioClip;
    AudioSource _audioPlayer;
    
    Dictionary<PlayerActions, System.Action> _playerActionHandlers;

    private void Awake() {
        _audioPlayer = GetComponent<AudioSource>();

        // assign the handler method to its associated action
        _playerActionHandlers = new Dictionary<PlayerActions, System.Action>()
        {
            { PlayerActions.Jump, HandleJump },
            { PlayerActions.Hurt, HandleHurt },
            { PlayerActions.AttackHit, HandleAttackHit },
            { PlayerActions.Die, HandleDie },
            { PlayerActions.Encounter, HandleEncounter }
        };
    }

    public void OnNotify(PlayerActions action) {
        if (_playerActionHandlers.ContainsKey(action)) {
            // invoke the appropriate handler method for each action
            _playerActionHandlers[action]();
        }
    }
    
    void HandleJump() {
        // stop jump coroutine
        if (_currentJumpResetRoutine != null)
        {
            StopCoroutine(_currentJumpResetRoutine);
        }

        // increment jump count
        _jumpCount += 1;

        if (_jumpCount == _jumpAudioThreshold)
        {
            // set the jumping audio clip and play it!
            _audioPlayer.clip = _jumpingAudioClip;
            _audioPlayer.Play();
        }

        // start jump coroutine
        _currentJumpResetRoutine = StartCoroutine(IJumpResetRoutine());
    }

    void HandleHurt() {
        _health -= 1; 
        if (_health == 1) {
            _audioPlayer.clip = _hurtAudioClip;
            _audioPlayer.Play();
        }
    }

    void HandleAttackHit() {
        _totalEnemyHits += 1;
        if (_totalEnemyHits == _enemyHitThreshold) {
            _audioPlayer.clip = _attackAudioClip;
            _audioPlayer.Play();
        }
    }

    void HandleDie() {
        _audioPlayer.clip = _dieAudioClip;
        _audioPlayer.Play();
    }

    void HandleEncounter() {
        _audioPlayer.clip = _encounterAudioClip;
        _audioPlayer.Play();
    }

    // reset jump count coroutine
    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(1f);
        _jumpCount = 0;
    }

    // called when the gameobject is enabled
    private void OnEnable() {
        // add itself to the subject's list of observers
        _playerSubject.AddObserver(this);
    }

    // called when the gameobject is disabled
    private void OnDisable()
    {
        // remove itself to the subject's list of observers
        _playerSubject.RemoveObserver(this);
    }
}
















