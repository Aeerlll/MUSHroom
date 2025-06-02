using System.Collections;
using UnityEngine;

// Этот скрипт управляет оружием игрока: экипировкой и стрельбой
public class GunController : MonoBehaviour
{
    public Transform weaponHold;       // Точка крепления оружия на персонаже
    public Gun startingGun;            // Стартовое оружие
    Gun equippedGun;                    // Текущее экипированное оружие

    void Start()
    {
        // Экипируем стартовое оружие, если оно задано
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    // Метод для экипировки нового оружия
    public void EquipGun(Gun gunToEquip)
    {
        // Если уже есть экипированное оружие - уничтожаем его
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        // Создаем новое оружие в точке крепления
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        // Делаем оружие дочерним объектом точки крепления
        equippedGun.transform.parent = weaponHold;
    }

    // Метод для совершения выстрела
    public void Shoot()
    {
        // Стреляем только если есть экипированное оружие
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}