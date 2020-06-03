using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private GameObject gameObjectPrefab;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;
    [SerializeField] private float myPrefabWidth = .5f;
    [SerializeField] private int dimensionX = 10;
    [SerializeField] private int dimensionY = 10;
    [SerializeField] private float spacing = 1.2f;
    [SerializeField] private bool isCentered = true;

    void Start()
    {
#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif

        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        var conversionSettings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, conversionSettings);

        //InstantiateEntity(new float3(0, 0, 0));

        InstantiateEntityGrid(dimensionX, dimensionY, spacing, isCentered);
        //InstantiateEntity(new float3(4f, 0f, 4f));
        //MakeEntity();     Function for first commit: create Entity
    }

    private void InstantiateEntity(float3 position)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    private void InstantiateEntityGrid(int dimX, int dimY, float spacing, bool isOffset)
    {
        float offsetX = 0;
        float offsetY = 0;
        if (isOffset)
        {
            offsetX = spacing * dimX / 2 - myPrefabWidth;
            offsetY = spacing * dimY / 2 - myPrefabWidth;
        }

        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                InstantiateEntity(new float3(spacing * x - offsetX, spacing * y - offsetY, 0));
            }
        }
    }

    private void MakeEntity()   //First Commit: Create Entity (a cube)
    {
        EntityManager eM = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = eM.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );
        Entity myEntity = eM.CreateEntity(archetype);
        eM.AddComponentData(myEntity, new Translation { Value = new float3(2f, 0f, 4f) }); //Unity.Transforms for Translation, Mathematics for float3

        eM.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });
    }

}
