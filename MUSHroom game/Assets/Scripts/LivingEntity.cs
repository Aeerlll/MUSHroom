using UnityEngine;
using System.Collections;

// Этот скрипт является базовым классом для всех живых существ в игре
// Реализует интерфейс IDamageable для получения урона
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth; // Начальное количество здоровья
    protected float health;      // Текущее количество здоровья
    protected bool dead;        // Флаг смерти существа

    public event System.Action OnDeath; // Событие, вызываемое при смерти

    // Метод Start вызывается при инициализации объекта
    protected virtual void Start()
    {
        health = startingHealth; // Устанавливаем начальное здоровье
    }

    // Метод для обработки попадания с дополнительной информацией
    public void TakeHit(float damage, RaycastHit hit)
    {
        // Здесь можно добавить логику обработки попадания (например, эффекты)
        TakeDamage(damage); // Применяем урон
    }

    // Основной метод для получения урона
    public void TakeDamage(float damage)
    {
        health -= damage; // Уменьшаем здоровье

        // Проверяем, умерло ли существо
        if (health <= 0 && !dead)
        {
            Die(); // Вызываем метод смерти
        }
    }

    // Метод, обрабатывающий смерть существа
    protected void Die()
    {
        dead = true; // Устанавливаем флаг смерти

        // Вызываем событие смерти, если на него есть подписчики
        if (OnDeath != null)
        {
            OnDeath();
        }

        GameObject.Destroy(gameObject); // Уничтожаем объект
    }
}