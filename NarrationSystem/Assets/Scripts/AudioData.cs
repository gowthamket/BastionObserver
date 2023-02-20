using UnityEngine;

[CreateAssetMenu(menuName = "Audio Data")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public bool repeatable;
    public int priority;
}
