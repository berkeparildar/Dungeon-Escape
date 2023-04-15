using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool _canDamage = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        var enemy = other.gameObject.GetComponent<Enemy>();
        var player = other.gameObject.GetComponent<Player>();
        if (enemy != null && _canDamage)
        {
            enemy.TakeDamage();
            StartCoroutine(DamageCooldown());
        }
        else if (player != null && _canDamage)
        {
            player.TakeDamage();
            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        _canDamage = false;
        yield return new WaitForSeconds(1);
        _canDamage = true;
    }
}
