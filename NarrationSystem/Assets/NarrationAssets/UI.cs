using UnityEngine;

public class UI : MonoBehaviour, IObserver
{
    [SerializeField] int _currentHeartsToDisplay;

    [SerializeField] MeshRenderer[] _hearts;
    [SerializeField] Material _fullHeartMaterial;
    [SerializeField] Material _emptyHeartMaterial;
    [SerializeField] PlayerController _player;

    void Awake()
    {
        _currentHeartsToDisplay = _hearts.Length;
        SetHeartsToDisplay();
    }

    private void SetHeartsToDisplay()
    {
        for (int currentHeartIndex = 0; currentHeartIndex < _hearts.Length; currentHeartIndex++)
        {
            if (currentHeartIndex < _currentHeartsToDisplay)
            {
                _hearts[currentHeartIndex].material = _fullHeartMaterial;
            }
            else
            {
                _hearts[currentHeartIndex].material = _emptyHeartMaterial;
            }
        }
    }

    public void DecreaseHeartCountToDisplay()
    {
        _currentHeartsToDisplay -= 1;
        SetHeartsToDisplay();
    }

    public void OnNotify(PlayerActions action)
    {
        if (action == PlayerActions.Hurt) {
            DecreaseHeartCountToDisplay();
        }
    }

    private void OnEnable() {
        _player.AddObserver(this);
    }

    private void OnDisable() {
        _player.RemoveObserver(this);
    }
}
















