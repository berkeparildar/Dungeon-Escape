using UnityEngine;

public class Acid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(3 * Time.deltaTime, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            var player = other.GetComponent<Player>();
            player.TakeDamage();
            Destroy(gameObject);
        }
    }
}
