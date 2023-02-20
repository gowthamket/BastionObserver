using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IObserver
{
    AudioSource _audioPlayer;
    [SerializeField] Slime[] _slimes;
    int _slimeCount;
    [SerializeField] AudioClip _adventureMusic;

    // Start is called before the first frame update
    void Awake()
    {
        _audioPlayer = GetComponent<AudioSource>();
        _slimeCount = _slimes.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (_slimeCount == 0 && _audioPlayer.clip.name == "EnemyBackground") {
            _audioPlayer.clip = _adventureMusic;
            _audioPlayer.Play();
        }
    }

    void OnEnable() {
        if (_slimes.Length > 0) {
            foreach (Slime slime in _slimes)
            {
                slime.AddObserver(this);
            }
        }
    }

    void OnDisable()
    {
        if (_slimes.Length > 0)
        {
            foreach (Slime slime in _slimes)
            {
                slime.RemoveObserver(this);
            }
        }
    }

    public void OnNotify(PlayerActions action)
    {
        if (action == PlayerActions.Die) {
            _slimeCount -= 1;
        }
    }
}
