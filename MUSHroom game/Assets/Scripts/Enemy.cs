using UnityEngine;
using System.Collections;
using UnityEngine.AI;

// Требует компонент для навигации (движения по карте)
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    // Состояния врага: ничего не делает, преследует, атакует
    public enum State { Idle, Chasing, Attacking };
    State currentState; // Текущее состояние врага

    NavMeshAgent pathfinder; // Компонент, который отвечает за путь до цели
    Transform target; // Цель (игрок)
    LivingEntity targetEntity; //Жизнь цели(игрока)
    Material skinMaterial; // Материал врага (для изменения цвета)

    Color originalColour; // Оригинальный цвет врага

    float attackDistanceThreshold = .5f; // Расстояние, на котором враг может атаковать
    float timeBetweenAttacks = 1; // Время между атаками
    float damage = 1; // Урон, который наносит враг

    float nextAttackTime; // Когда можно будет атаковать снова
    float myCollisionRadius; // Радиус столкновения врага
    float targetCollisionRadius; // Радиус столкновения цели (игрока)

    bool hasTarget; // Есть ли цель (игрок найден)

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>(); // Получаем компонент навигации
        skinMaterial = GetComponent<Renderer>().material; // Получаем материал для изменения цвета
        originalColour = skinMaterial.color; // Сохраняем начальный цвет

        // Проверяем, существует ли игрок на сцене
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing; // Начинаем преследование
            hasTarget = true; // Устанавливаем флаг, что цель есть

            target = GameObject.FindGameObjectWithTag("Player").transform; // Получаем игрока
            targetEntity = target.GetComponent<LivingEntity>(); // Получаем скрипт жизни игрока
            targetEntity.OnDeath += OnTargetDeath; // Подписываемся на событие смерти игрока

            // Считаем радиусы столкновения, чтобы знать расстояние между объектами
            myCollisionRadius = GetComponent<BoxCollider>().size.x * 0.5f;
            targetCollisionRadius = target.GetComponent<BoxCollider>().size.x * 0.5f;

            // Запускаем корутину, которая обновляет путь до цели
            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false; // Убираем цель
        currentState = State.Idle; // Переходим в состояние покоя
    }

    void Update()
    {

        if (hasTarget)
        {
            // Если пришло время атаковать
            if (Time.time > nextAttackTime)
            {
                // Считаем квадрат расстояния до игрока
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                // Если игрок рядом — атакуем
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks; // Устанавливаем следующее время атаки
                    StartCoroutine(Attack()); // Запускаем атаку
                }

            }
        }

    }

    IEnumerator Attack()
    {

        currentState = State.Attacking; // Меняем состояние на атаку
        pathfinder.enabled = false; // Отключаем навигацию, чтобы вручную двигать врага

        Vector3 originalPosition = transform.position; // Запоминаем начальную позицию
        Vector3 dirToTarget = (target.position - transform.position).normalized; // Направление до игрока
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius); // Позиция, куда враг "прыгнет"

        float attackSpeed = 3; // Скорость атаки
        float percent = 0; // Процент прохождения анимации

        skinMaterial.color = Color.red; // Меняем цвет врага на красный	 
        bool hasAppliedDamage = false; // Урон пока не нанесён

        while (percent <= 1)
        {
            // Когда атака прошла на 50% — наносим урон
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage); // Наносим урон игроку
            }

            percent += Time.deltaTime * attackSpeed; // Увеличиваем прогресс
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4; // Вычисляем сглаженное движение
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation); // Перемещаем врага

            yield return null; // Ждём следующий кадр
        }

        skinMaterial.color = originalColour; // Возвращаем цвет
        currentState = State.Chasing; // Возвращаемся к преследованию
        pathfinder.enabled = true; // Возвращаемся к преследованию
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f; // Как часто обновлять путь

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                // Вычисляем точку, к которой идти (чуть ближе, но не прямо в игрока)
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                // Если враг жив, ставим новую точку назначения
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate); // Ждём перед следующим обновлением пути
        }
    }
}
