using System.Collections;

public static class Utility
{
    // Метод для перемешивания элементов массива в случайном порядке
    // <T> означает, что метод работает с массивами любых типов 
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        // Создаём генератор случайных чисел с "сидом"
        // Сид (seed) — это число, от которого зависит последовательность случайных чисел
        // Если указать один и тот же сид, порядок будет каждый раз одинаковым
        System.Random prng = new System.Random(seed);

        // Перебираем элементы массива (кроме последнего)
        for (int i = 0; i < array.Length - 1; i++)
        {
            // Получаем случайный индекс от i до конца массива
            int randomIndex = prng.Next(i, array.Length);
            // Меняем местами текущий элемент и случайный
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        // Возвращаем перемешанный массив
        return array;
    }

}