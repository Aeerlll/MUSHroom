using UnityEngine;
using System.Collections;

// Этот скрипт отвечает за спавн врагов волнами и антикемпинг систему
// Требуемые компоненты: MapGenerator для определения точек спавна
public class Spawner : MonoBehaviour
{
    public Wave[] waves; // Массив волн с настройками
    public Enemy enemy; // Префаб врага для спавна

    LivingEntity playerEntity; // Ссылка на сущность игрока
    Transform playerT; // Трансформ игрока
    MapGenerator map; // Генератор карты для поиска точек спавна

    Wave currentWave; // Текущая волна
    int currentWaveNumber; // Номер текущей волны

    int enemiesRemainingToSpawn; // Оставшиеся для спавна враги
    int enemiesRemainingAlive; // Живые враги текущей волны
    float nextSpawnTime; // Время следующего спавна

    // Настройки антикемпинга
    float timeBetweenCampingChecks = 2; // Интервал проверки кемпинга
    float campThresholdDistance = 1.5f; // Пороговое расстояние для кемпинга
    float nextCampCheckTime; // Время следующей проверки
    Vector3 campPositionOld; // Предыдущая позиция игрока
    bool isCamping; // Флаг кемпинга

    bool isDisabled; // Флаг активности спавнера

    public event System.Action<int> OnNewWave; // Событие новой волны

    // Метод Start вызывается при инициализации
    void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        // Инициализация антикемпинг системы
        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave(); // Запуск первой волны
    }

    // Метод Update вызывается каждый кадр
    void Update()
    {
        if (!isDisabled)
        {
            // Проверка на кемпинг
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;
                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

            // Спавн врагов по таймеру
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
                StartCoroutine(SpawnEnemy()); // Запуск корутины спавна
            }
        }
    }

    // Корутина для спавна врага с эффектом
    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1; // Задержка спавна
        float tileFlashSpeed = 4; // Скорость мигания тайла

        // Выбор точки спавна (у игрока при кемпинге или случайная)
        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }

        // Эффект мигания тайла перед спавном
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = tileMat.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        // Создание врага и подписка на его смерть
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    // Обработчик смерти игрока
    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    // Обработчик смерти врага
    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        // Запуск следующей волны при уничтожении всех врагов
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    // Сброс позиции игрока
    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    // Запуск следующей волны
    void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            // Вызов события новой волны
            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }

    // Класс волны для настройки в инспекторе
    [System.Serializable]
    public class Wave
    {
        public int enemyCount; // Количество врагов в волне
        public float timeBetweenSpawns; // Задержка между спавнами
    }
}