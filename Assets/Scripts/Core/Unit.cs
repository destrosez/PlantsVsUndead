using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    public float Health;

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0f)
        {
            Destroy(gameObject);
            Die();
        }
    }
    
    private void Die()
    {
        if (transform.parent != null && transform.parent.CompareTag("Tile"))
        {
            var collider = transform.parent.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

        Destroy(gameObject);
    }
}