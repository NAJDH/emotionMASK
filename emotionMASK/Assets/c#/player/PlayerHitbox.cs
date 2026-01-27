using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("收到攻击 " + collision.name);
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage();
        }
    }
}