using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed = 15f;
    private float _damage = 5f;
    private float _maxDistance = 6f;

    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.back * (_speed * Time.deltaTime));
        if (Vector3.Distance(_startPos, transform.position) > _maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Unit>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}