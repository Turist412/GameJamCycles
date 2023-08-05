using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float x;
    [SerializeField] private float y;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Получаем текущую позицию игрока
            Vector3 currentPosition = player.position;

            // Изменяем координаты x и y
            currentPosition.x = x;
            currentPosition.y = y;

            // Устанавливаем новую позицию игрока
            player.position = currentPosition;

            // Выводим имя объекта, с которым столкнулись
            Debug.Log("Столкновение с объектом: " + collision.gameObject.name);
        }
    } 
}
