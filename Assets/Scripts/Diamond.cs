using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int diamondCount = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();
            player.AddGem();
            Destroy(gameObject);
        }
    }
}
