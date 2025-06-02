using UnityEngine;

// Этот интерфейс определяет поведение объектов, которые могут получать урон
public interface IDamageable
{
    // Метод для обработки попадания с информацией о точке попадания
    void TakeHit(float damage, RaycastHit hit);

    // Метод для обработки урона без дополнительной информации
    void TakeDamage(float damage);
}
