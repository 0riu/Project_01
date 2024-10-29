using UnityEngine;
using UnityEngine.Tilemaps;

public class HexClickHandler : MonoBehaviour
{
    public Tilemap hexTilemap;                  // Odniesienie do Tilemapy
    public GameObject highlightPrefab;          // Prefab do zaznaczania sąsiadów
    public GameObject selectedHighlightPrefab;  // Prefab do zaznaczania klikniętego kafelka
    private GameObject[,] highlightObjects;     // Tablica do przechowywania zaznaczeń sąsiadów
    private GameObject selectedTileHighlight;   // Przechowuje zaznaczenie klikniętego kafelka
    private int offsetX;                        // Offset X dla tablicy highlightObjects
    private int offsetY;                        // Offset Y dla tablicy highlightObjects

    void Start()
    {
        var cellBounds = hexTilemap.cellBounds; // Określ zakres Tilemapy
        offsetX = -cellBounds.xMin;             // Przesunięcie na X do dodatnich indeksów
        offsetY = -cellBounds.yMin;             // Przesunięcie na Y do dodatnich indeksów

        // Inicjalizacja tablicy z rozmiarem na podstawie rzeczywistych granic mapy
        highlightObjects = new GameObject[cellBounds.size.x, cellBounds.size.y];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sprawdź, czy lewy przycisk myszy został naciśnięty
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = hexTilemap.WorldToCell(mouseWorldPosition);

            // Sprawdź, czy kliknięto na kafelek
            if (hexTilemap.HasTile(clickedCell))
            {
                ClearHighlights();                  // Wyczyść poprzednie zaznaczenia
                HighlightTile(clickedCell);         // Zaznacz kliknięty kafelek
                HighlightNeighbors(clickedCell);    // Zaznacz sąsiadów
            }
        }
    }

    void HighlightTile(Vector3Int tilePosition)
    {
        // Zaznacz kliknięty kafelek
        Vector3 worldPosition = hexTilemap.CellToWorld(tilePosition);
        selectedTileHighlight = Instantiate(selectedHighlightPrefab, worldPosition, Quaternion.identity); // Użycie prefabrykatu dla klikniętego kafelka
    }

    void HighlightNeighbors(Vector3Int tilePosition)
    {
        Vector3Int[] neighbors; // Tablica współrzędnych sąsiednich kafelków

        // Przesunięcia sąsiadów dla kolumny x
        neighbors = new Vector3Int[]            
        {
            new Vector3Int(tilePosition.x + 1, tilePosition.y, 0),      // prawa
            new Vector3Int(tilePosition.x - 1, tilePosition.y, 0),      // lewa
            new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, 0),  // góra-lewo
            new Vector3Int(tilePosition.x, tilePosition.y + 1, 0),      // góra-prawo
            new Vector3Int(tilePosition.x - 1, tilePosition.y - 1, 0),  // dół-lewo
            new Vector3Int(tilePosition.x, tilePosition.y - 1, 0)       // dół-prawo
        };

        // Przesunięcia sąsiadów dla nieparzystego wiersza y (nadpisuje poprzednie przesunięcia, gdy y jest nieparzyste)
        if (tilePosition.y % 2 != 0)
        {
            neighbors = new Vector3Int[]
            {
                new Vector3Int(tilePosition.x + 1, tilePosition.y, 0),      // prawa
                new Vector3Int(tilePosition.x - 1, tilePosition.y, 0),      // lewa
                new Vector3Int(tilePosition.x, tilePosition.y + 1, 0),      // góra-lewo
                new Vector3Int(tilePosition.x + 1, tilePosition.y + 1, 0),  // góra-prawo
                new Vector3Int(tilePosition.x, tilePosition.y - 1, 0),      // dół-lewo
                new Vector3Int(tilePosition.x + 1, tilePosition.y - 1, 0)   // dół-prawo
            };
        }
        
        // Zaznacz sąsiadów
        foreach (Vector3Int neighbor in neighbors)
        {
            int adjustedX = neighbor.x + offsetX;   // Przesuń współrzędną X o offsetX
            int adjustedY = neighbor.y + offsetY;   // Przesuń współrzędną Y o offsetY

            // Sprawdzenie, czy sąsiad mieści się w granicach tablicy i mapy
            if (adjustedX >= 0 && adjustedX < highlightObjects.GetLength(0) &&
                adjustedY >= 0 && adjustedY < highlightObjects.GetLength(1) &&
                hexTilemap.HasTile(neighbor))
            {
                Vector3 worldPosition = hexTilemap.CellToWorld(neighbor);
                
                // Zapisanie zaznaczenia w tablicy highlightObjects
                highlightObjects[adjustedX, adjustedY] = Instantiate(highlightPrefab, worldPosition, Quaternion.identity);
            }
        }
    }

    void ClearHighlights()
    {
        // Wyczyść zaznaczenie klikniętego kafelka
        if (selectedTileHighlight != null)
        {
            Destroy(selectedTileHighlight);
        }

        // Wyczyść zaznaczenia sąsiadów z poprzedniego kliknięcia
        for (int x = 0; x < highlightObjects.GetLength(0); x++)
        {
            for (int y = 0; y < highlightObjects.GetLength(1); y++)
            {
                if (highlightObjects[x, y] != null)
                {
                    Destroy(highlightObjects[x, y]);
                    highlightObjects[x, y] = null;  // Wyzeruj, aby zapobiec ponownemu zaznaczeniu tego samego obiektu przy kolejnym kliknięciu
                }
            }
        }
    }
}


