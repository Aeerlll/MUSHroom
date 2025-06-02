using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Класс Menu управляет основными функциями главного меню игры
public class Menu : MonoBehaviour
{
    // Ссылка на GameObject, содержащий элементы главного меню
    public GameObject mainMenuHolder;

    // Метод для загрузки игровой сцены
    public void Play()
    {
        // Загружает сцену с именем "Game"
        SceneManager.LoadScene("Game");
    }

    // Метод для выхода из игры
    public void Quit()
    {
        Application.Quit();
    }
}