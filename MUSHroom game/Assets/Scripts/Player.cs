using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Этот скрипт отвечает за поведение игрока: движение, прицеливание и стрельбу

// Требуемые компоненты: контроллер игрока и контроллер оружия
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{

    public float moveSpeed = 5; // Скорость передвижения игрока

    Camera viewCamera; // Камера, с которой мы смотрим на сцену
    PlayerController controller; // Компонент, управляющий движением
    GunController gunController; // Компонент, управляющий стрельбой

    // Метод Start вызывается при запуске игры
    protected override void Start()
    {
        base.Start();   // Вызов Start() родительского класса LivingEntity
        controller = GetComponent<PlayerController>(); // Получаем компонент управления движением
        gunController = GetComponent<GunController>(); // Получаем компонент управления оружием
        viewCamera = Camera.main; // Получаем главную камеру
    }

    void Update()
    {
        // Получаем направление движения от клавиш (WASD или стрелки)
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed; // Вычисляем вектор скорости
        controller.Move(moveVelocity); // Передаём скорость в контроллер движения

        // Создаём луч от камеры к позиции мыши
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Плоскость "земли" (XZ)
        float rayDistance;


        // Проверяем, пересекает ли луч плоскость земли
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance); // Получаем точку пересечения
                                                       //controller будет поворачиваться к этой точке (куда смотрит мышь)
            controller.LookAt(point);
        }

        // Если нажата левая кнопка мыши — стреляем
        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }
}

