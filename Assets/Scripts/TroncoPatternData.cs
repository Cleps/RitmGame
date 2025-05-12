using UnityEngine;

[CreateAssetMenu(fileName = "NewTroncoPattern", menuName = "Rhythm Game/Tronco Pattern")]
public class TroncoPatternData : ScriptableObject
{
    // delay entre as batidas
    public float[] beatTimings;
}