using System.Collections.Generic;
using UnityEngine;

public class GenericNarrationSystem : MonoBehaviour, IGenericObserver<PlayerActions>
{
    private AudioSource _audioSource;
    [SerializeField] GenericSubject<PlayerActions> _playerSubject;
    [SerializeField] AudioClip _repatedJumpingAudioClip;

    private int _jumpCount = 0;
    private int _jumpThreshold = 8;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {

            _playerSubject.AddObserver(this); 
    }

    void OnDisable()
    {
            _playerSubject.RemoveObserver(this);
    }

    public void OnNotify(PlayerActions action)
    {
        switch (action)
        {
            case PlayerActions.Jump:
                if (_jumpCount == _jumpThreshold) {
                    Debug.Log("HIT");
                    // _audioSource.clip = _repatedJumpingAudioClip;
                    // _audioSource.Play();
                } else {
                    _jumpCount += 1;
                }
                return;
            default:
                return;
        }
    }

    // public void OnNotify(TestActions action)
    // {
    //     switch(action)
    //     {
    //         case TestActions.Die:
    //             Debug.Log("DIED");
    //             return;
    //         default:
    //             return;
    //     }
    // }
}
