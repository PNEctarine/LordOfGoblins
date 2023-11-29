using UnityEngine;

public class WaterComponent : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer Water { get; private set; }

    public void SetColor(Color color)
    {
        Water.color = color;
    }
}
