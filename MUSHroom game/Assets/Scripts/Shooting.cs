using UnityEngine;
using System.Collections;

// Скрипт пули: летит, проверяет столкновения и наносит урон
public class Shooting : MonoBehaviour
{
    // Настройки пули
    public LayerMask collisionMask; // С какими слоями сталкиваемся
    float speed = 10;              // Скорость полета
    float damage = 1;              // Наносимый урон

    // Технические параметры
    float lifetime = 3;            // Через сколько секунд исчезнет
    float skinWidth = .1f;         // Доп. отступ для проверки столкновений

    void Start()
    {
        // Удаляем пулю через заданное время
        Destroy(gameObject, lifetime);

        // Проверяем столкновение сразу при появлении
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    // Метод для изменения скорости извне
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        // Двигаем пулю вперед
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance); // Проверяем столкновения
        transform.Translate(Vector3.forward * moveDistance);
    }

    // Проверка столкновений с помощью raycast
    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit); // Попали во что-то
        }
    }

    // Обработка попадания (с информацией о точке попадания)
    void OnHitObject(RaycastHit hit)
    {
        // Пробуем получить компонент с интерфейсом IDamageable
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit); // Наносим урон
        }

        // Уничтожаем пулю в любом случае
        GameObject.Destroy(gameObject);
    }

    // Альтернативная версия обработки попадания (просто через коллайдер)
    void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage); // Без информации о точке попадания
        }
        GameObject.Destroy(gameObject);
    }
}