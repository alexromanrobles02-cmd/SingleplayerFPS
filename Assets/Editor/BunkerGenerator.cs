using UnityEngine;
using UnityEditor;
using UnityEngine.ProBuilder;

public class BunkerGenerator : EditorWindow
{
    [MenuItem("Tools/ProBuilder/Generate Random Bunker")]
    public static void GenerateBunker()
    {
        // 1. Crear el objeto contenedor
        GameObject bunkerRoot = new GameObject("Generated Bunker");
        
        int gridSize = 7; // Más grande para nivel más chulo
        float roomSize = 10f;
        float wallThickness = 0.5f;
        float height = 5f;
        
        bool[,] hasRoom = new bool[gridSize, gridSize];

        // Decidimos dónde hay habitaciones (generamos un camino base para asegurar conexión)
        hasRoom[gridSize/2, gridSize/2] = true;
        for(int x = 0; x < gridSize; x++)
        {
            for(int z = 0; z < gridSize; z++)
            {
                if(Random.value > 0.45f) hasRoom[x, z] = true;
            }
        }
        
        // Construimos paredes individuales
        for(int x = 0; x < gridSize; x++)
        {
            for(int z = 0; z < gridSize; z++)
            {
                if(hasRoom[x, z])
                {
                    Vector3 center = new Vector3(x * roomSize, 0, z * roomSize);
                    
                    // SUELO
                    GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    floor.transform.position = center + new Vector3(0, -wallThickness/2f, 0);
                    floor.transform.localScale = new Vector3(roomSize, wallThickness, roomSize);
                    floor.transform.SetParent(bunkerRoot.transform);
                    
                    // TECHO (comentado por si quieres verlo abierto, quita el /* y */ para ponerlo)
                    /*
                    GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    ceiling.transform.position = center + new Vector3(0, height + wallThickness/2f, 0);
                    ceiling.transform.localScale = new Vector3(roomSize, wallThickness, roomSize);
                    ceiling.transform.SetParent(bunkerRoot.transform);
                    */

                    // PARED NORTE (Z + 1)
                    if (z == gridSize - 1 || !hasRoom[x, z + 1])
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = center + new Vector3(0, height/2f, roomSize/2f);
                        wall.transform.localScale = new Vector3(roomSize + wallThickness, height, wallThickness);
                        wall.transform.SetParent(bunkerRoot.transform);
                    }
                    
                    // PARED SUR (Z - 1)
                    if (z == 0 || !hasRoom[x, z - 1])
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = center + new Vector3(0, height/2f, -roomSize/2f);
                        wall.transform.localScale = new Vector3(roomSize + wallThickness, height, wallThickness);
                        wall.transform.SetParent(bunkerRoot.transform);
                    }
                    
                    // PARED ESTE (X + 1)
                    if (x == gridSize - 1 || !hasRoom[x + 1, z])
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = center + new Vector3(roomSize/2f, height/2f, 0);
                        wall.transform.localScale = new Vector3(wallThickness, height, roomSize + wallThickness);
                        wall.transform.SetParent(bunkerRoot.transform);
                    }
                    
                    // PARED OESTE (X - 1)
                    if (x == 0 || !hasRoom[x - 1, z])
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = center + new Vector3(-roomSize/2f, height/2f, 0);
                        wall.transform.localScale = new Vector3(wallThickness, height, roomSize + wallThickness);
                        wall.transform.SetParent(bunkerRoot.transform);
                    }
                }
            }
        }
        
        Debug.Log("¡Búnker modular abierto generado con paredes perfectas! Aplica el Material de Paredes al objeto 'Generated Bunker' entero.");
    }
}
