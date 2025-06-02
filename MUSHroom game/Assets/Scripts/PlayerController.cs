using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для управления движением и поворотом игрока
[RequireComponent(typeof(Rigidbody))] // Требует, чтобы на объекте был компонент Rigidbody
public class PlayerController : MonoBehaviour
{

    Vector3 velocity; // Вектор скорости (направление и сила движения)
    Rigidbody myRigibody; // Ссылка на компонент Rigidbody

    void Start()
    {
        // Получаем компонент Rigidbody при старте
        myRigibody = GetComponent<Rigidbody>();
    }
    // Метод для задания скорости движения
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity; // Сохраняем переданный вектор скорости
    }
    // Метод для поворота игрока к указанной точке
    public void LookAt(Vector3 lookPoint)
    {
        // Меняем только X и Z, чтобы поворот был по горизонтали (не вверх/вниз)
        Vector3 heihtCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        // Поворачиваемся к точке
        transform.LookAt(heihtCorrectedPoint);
    }
    // Метод вызывается на каждый кадр физики (фикшн тайм)
    void FixedUpdate()
    {
        // Передвигаем Rigidbody в новое место на основе скорости
        myRigibody.MovePosition(myRigibody.position + velocity * Time.fixedDeltaTime);

    }
}

