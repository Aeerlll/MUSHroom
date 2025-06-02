using UnityEngine;
using System.Collections;
using UnityEditor;

// Этот скрипт делает кастомную панель в инспекторе для MapGenerator
[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    // Переопределение метода отрисовки инспектора
    public override void OnInspectorGUI()
    {
        // Получаем ссылку на целевой объект (MapGenerator), к которому прикреплён этот редактор
        MapGenerator map = target as MapGenerator;

        // Отрисовываем стандартный инспектор и проверяем, были ли изменены какие-либо значения
        if (DrawDefaultInspector())
        {
            // Если значения были изменены, автоматически генерируем карту
            map.GenerateMap();
        }

        // Добавляем кнопку "Generate Map" в инспектор
        if (GUILayout.Button("Generate Map"))
        {
            // При нажатии кнопки вызываем метод генерации карты
            map.GenerateMap();
        }
    }
}