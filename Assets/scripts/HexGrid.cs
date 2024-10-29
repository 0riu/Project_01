using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{
    public Tilemap hexTilemap;          // Odnosi się do Tilemapy, w której są przechowywane kafelki
    public Font font;                   // Czcionka używana do wyświetlania współrzędnych kafelków
    public GameObject textPrefab;       // Prefab do wyświetlania tekstu współrzędnych nad kafelkami
    private GameObject[,] textObjects;  // Tablica do przechowywania obiektów tekstowych dla współrzędnych
    void Start()
    {
        DisplayHexCoordinates(); // Wywołanie metody, która wyświetla współrzędne kafelków na starcie
    }
    void DisplayHexCoordinates()
    {
        // Ustala granice Tilemapy, aby wiedzieć, które kafelki zawierają współrzędne do wyświetlenia
        var cellBounds = hexTilemap.cellBounds;
        
        // Inicjalizacja tablicy tekstowych obiektów do przechowywania współrzędnych każdego kafelka
        textObjects = new GameObject[cellBounds.size.x, cellBounds.size.y];

        // Iteracja po każdym kafelku w granicach tilemapy
        for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);  // Współrzędne pozycji kafelka

                // Sprawdza, czy Tilemapa zawiera kafelek na danej pozycji
                if (hexTilemap.HasTile(tilePosition))
                {
                    Vector3 worldPosition = hexTilemap.CellToWorld(tilePosition);   // Przekształca pozycję kafelka na współrzędne świata

                    // Wyświetla współrzędne nad kafelkiem i zapisuje obiekt tekstowy w tablicy
                    textObjects[x - cellBounds.xMin, y - cellBounds.yMin] = ShowCoordinatesAtPosition(worldPosition, tilePosition);
                }
            }
        }
    }
    GameObject ShowCoordinatesAtPosition(Vector3 worldPosition, Vector3Int tilePosition)
    {
        // Tworzy obiekt tekstowy w pozycji świata z niewielkim przesunięciem w osi Y
        GameObject textObj = Instantiate(textPrefab, worldPosition + new Vector3(0, 0.2f, 0), Quaternion.identity);

        // Pozycjonowanie tekstu dokładnie nad kafelkiem z uwzględnieniem przesunięcia
        textObj.transform.position = worldPosition + new Vector3(-2.55f, 0.1f, 0);

         // Ustawienia właściwości tekstu współrzędnych kafelka
        TextMesh text = textObj.GetComponent<TextMesh>();
        text.text = "(" + tilePosition.x + "," + tilePosition.y + ")";  // Tekst wyświetlający współrzędne kafelka
        text.font = font;                                               // Ustawienie czcionki
        text.fontStyle = FontStyle.Bold;                                // Ustawienie stylu czcionki na pogrubioną
        text.fontSize = 12;                                             // Ustawienie rozmiaru czcionki
        text.color = Color.red;                                         // Ustawienie koloru tekstu na czerwony
        
        // Zwraca obiekt tekstowy, aby można było go przechowywać w tablicy
        return textObj;                                           
    }
}
