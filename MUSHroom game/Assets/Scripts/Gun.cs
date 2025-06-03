using System.Collections;
using UnityEngine;

// Класс Gun отвечает за поведение оружия (стрельбу)
public class Gun : MonoBehaviour {

    public Transform muzzle;          // Точка, откуда будут вылетать снаряды
    public Shooting projectile;        // Префаб снаряда для стрельбы
    public float msBeetweenShots = 100;// Время между выстрелами в миллисекундах
    public float muzzleVelocity = 35;  // Начальная скорость снаряда

    float nextShotTime;                // Время, когда можно будет сделать следующий выстрел

    // Метод Shoot вызывается для совершения выстрела
    public void Shoot()
    {
        // Проверяем, можно ли стрелять (прошло ли достаточно времени с последнего выстрела)
        if (Time.time > nextShotTime)
        {
            // Обновляем время следующего возможного выстрела
            nextShotTime = Time.time + msBeetweenShots / 1000; // делим на 1000 для перевода мс в секунды
            
            // Создаем новый экземпляр снаряда в позиции дула оружия
            Shooting newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Shooting;
            
            // Устанавливаем скорость созданного снаряда
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}
