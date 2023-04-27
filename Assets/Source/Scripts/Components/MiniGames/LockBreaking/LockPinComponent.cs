using UnityEngine;

public class MasterKeyPinComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PinUp(collision.transform);
    }

    public void PinUp(Transform transform)
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
    }
}
