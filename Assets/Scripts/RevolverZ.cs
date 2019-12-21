using UnityEngine;

public class RevolverZ : MonoBehaviour
{
    public float Speed = 1f;
    [SerializeField]
    private float zRotation = 0f;
    private Vector3 rotation;

    void Start() {

    }

    void Update()
    {
        zRotation += Speed * Time.deltaTime;
        if(zRotation >= 180f) zRotation = zRotation - 180f; // Keep the 'change' for smooth rotation
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            zRotation);
    }
}
