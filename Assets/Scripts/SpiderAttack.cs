using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GameObject.Find("Spider").GetComponent<Enemy>();
    }

    public void Fire()
    {
        _enemy.FireAcid();
    }
}
