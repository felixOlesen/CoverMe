using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float health = 100f;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public float rotationSpeed = 2f;

    [Header("References")]
    public Rigidbody2D EntityRB { get; set; }

    [Header("Settings")]
    [Tooltip("How far above the character's exact center should this hover?")]
    public Vector3 worldOffset = new Vector3(0, 1.0f, 0);

}
