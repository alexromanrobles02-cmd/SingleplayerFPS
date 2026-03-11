using UnityEngine;
using UnityEditor;

public class SimpleTerrainGenerator : EditorWindow
{
    [MenuItem("Tools/Environment/Generate Random Terrain (Like Bunker)")]
    public static void GenerateTerrain()
    {
        int resolution = 513; 
        float terrainSize = 400f; // Tamaño total 400x400 metros
        float terrainHeight = 60f; // Altura máxima de las montañas

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = resolution;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);

        float[,] heights = new float[resolution, resolution];

        float offsetX = Random.Range(0f, 9999f);
        float offsetY = Random.Range(0f, 9999f);
        float mountainScale = 2.5f;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float sampleX = (float)x / resolution * mountainScale + offsetX;
                float sampleZ = (float)z / resolution * mountainScale + offsetY;

                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                noise = Mathf.Pow(noise, 3.5f);

                float cx = (x - resolution / 2f) / (resolution / 2f);
                float cz = (z - resolution / 2f) / (resolution / 2f);
                float distFromCenter = Mathf.Sqrt(cx * cx + cz * cz);
                
                float flatteningMask = Mathf.SmoothStep(0f, 1f, distFromCenter * 1.5f);
                noise *= flatteningMask;

                float detailNoise = Mathf.PerlinNoise(sampleX * 15f, sampleZ * 15f) * 0.005f;

                heights[z, x] = Mathf.Clamp01(noise + detailNoise);
            }
        }

        terrainData.SetHeights(0, 0, heights);
        
        // --- ASIGNAR TEXTURA VERDE (CÉSPED) POR DEFECTO ---
        Texture2D grassTexture = new Texture2D(2, 2);
        // Creamos un color verde oscuro (como césped)
        Color[] pixels = new Color[4] { new Color(0.2f, 0.4f, 0.15f), new Color(0.2f, 0.4f, 0.15f), new Color(0.2f, 0.4f, 0.15f), new Color(0.2f, 0.4f, 0.15f) };
        grassTexture.SetPixels(pixels);
        grassTexture.Apply();

        // Para Unity, necesitamos un TerrainLayer.
        TerrainLayer greenLayer = new TerrainLayer();
        greenLayer.diffuseTexture = grassTexture;
        greenLayer.tileSize = new Vector2(10, 10);
        
        // Asignamos la "capa" de pintura al TerrainData
        terrainData.terrainLayers = new TerrainLayer[] { greenLayer };

        GameObject terrainGO = Terrain.CreateTerrainGameObject(terrainData);
        terrainGO.name = "Generated Terrain";
        
        terrainGO.transform.position = new Vector3(-terrainSize / 2f, 0, -terrainSize / 2f);
        
        Debug.Log("¡Terreno Verde Aleatorio Generado! El terreno ya viene pintado de un color verde hierba básico.");
    }
}
