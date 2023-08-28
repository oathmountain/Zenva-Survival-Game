using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public Material canPlaceMaterial;
    public Material cannotPlaceMaterial;
    private MeshRenderer[] meshRenderers;
    private List<GameObject> collidingObjects = new List<GameObject>();

    void Awake()
    {
        meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
    }

    public void CanPlace()
    {
        SetMaterial(canPlaceMaterial);
    }

    public void CannotPlace()
    {
        SetMaterial(cannotPlaceMaterial);
    }

    void SetMaterial(Material mat)
    {
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            Material[] mats = new Material[meshRenderers[i].materials.Length];

            for(int y=0; y < mats.Length; y++)
            {
                mats[y] = mat;
            }

            meshRenderers[i].materials = mats;
        }
    }

    public bool CollidingWithObjects()
    {
        collidingObjects.RemoveAll(X => X == null);
        return collidingObjects.Count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 12)
        {
            collidingObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 12)
        {
            collidingObjects.Remove(other.gameObject);
        }
    }
}
