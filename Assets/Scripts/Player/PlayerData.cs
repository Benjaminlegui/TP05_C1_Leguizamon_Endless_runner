using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Runner/Player Data")]
public class PlayerData : ScriptableObject
{
    [Min(0.1f)] public float jumpSpeed = 9f;
    [Min(0f)] public float gravityScale = 3f;
}
