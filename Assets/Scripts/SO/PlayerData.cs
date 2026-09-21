using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game Settings/Player Data")]
public class PlayerData : ScriptableObject
{
    public Key jumpKey = Key.Space;
    [Min(0.1f)] public float jumpSpeed = 9f;
    [Min(0f)] public float gravityScale = 3f;
}
