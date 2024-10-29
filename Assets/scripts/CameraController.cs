using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;                // Obiekt, na który kamera jest skierowana (np. centrum mapy)
    public float moveSpeed = 50.5f;         // Prędkość, z jaką kamera może się poruszać w pionie i poziomie
    public float minX, maxX, minY, maxY;    // Granice ruchu kamery, ograniczające pole widzenia do mapy

    void Start()
    {
        // Ustawienia kamery głównej jako ortograficznej z wybraną wielkością
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 25.25f;
        
        // Pozycjonowanie kamery, aby była skierowana na target i ustawiona na określonej głębokości
        Camera.main.transform.position = new Vector3(target.position.x, target.position.y - 2.4f, -15.8f);

        // Docelowy współczynnik proporcji kamery
        float targetAspect = 1.285f;                                        // Określa docelowy stosunek szerokości do wysokości
        float windowAspect = (float)Screen.width / (float)Screen.height;    // Oblicza rzeczywisty stosunek na podstawie rozdzielczości ekranu
        float scaleHeight = windowAspect / targetAspect;                    // Skala wysokości, aby dopasować okno kamery do docelowej proporcji

        // Jeśli rzeczywisty stosunek szerokości do wysokości jest mniejszy od docelowego, dostosuj wysokość
        if (scaleHeight < 1.0f)
        {
            // Obliczenie i ustawienie nowego prostokąta widoku kamery, aby dopasować wysokość
            Rect rect = Camera.main.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;   // Centruje obraz w pionie
            Camera.main.rect = rect;
        }
        else
        {
            // Jeśli rzeczywisty stosunek szerokości do wysokości jest większy niż docelowy, dostosuj szerokość
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = Camera.main.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;    // Centruje obraz w poziomie
            rect.y = 0;
            Camera.main.rect = rect;
        }

        // Ustawienia granic dla ruchu kamery w zakresie mapy
        minX = -65.135f;
        maxX = 68.44f;
        minY = -46.35f;
        maxY = 51.25f;
    }

    void Update()
    {
        // Przechwycenie wejść poziomych i pionowych (strzałki lub klawisze WSAD)
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        
        // Skaluje ruch na podstawie prędkości i czasu
        move *= moveSpeed * Time.deltaTime;

        // Aktualizacja pozycji kamery na podstawie ruchu użytkownika
        Vector3 newPos = Camera.main.transform.position + move;

        // Ograniczenie ruchu kamery do granic mapy zdefiniowanych przez minX, maxX, minY, maxY
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        // Przypisanie nowej pozycji do kamery głównej
        Camera.main.transform.position = newPos;
    }
}

