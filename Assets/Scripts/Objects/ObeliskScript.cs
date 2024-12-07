using UnityEngine;

public class ObeliskScript : MonoBehaviour
{
    public Material defaultMaterial; // Materiaali, joka näkyy oletuksena
    public Material triggeredMaterial; // Materiaali, joka näkyy triggerin osuessa

    private MeshRenderer meshRenderer; // MeshRenderer-komponentti

    void Start()
    {
        // Hae MeshRenderer-komponentti
        meshRenderer = GetComponent<MeshRenderer>();

        // Aseta oletusmateriaaliksi defaultMaterial
        if (meshRenderer != null && defaultMaterial != null)
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            // Vaihda triggeredMaterialiin, kun trigger osuu tähän objektiin
            if (meshRenderer != null && triggeredMaterial != null)
            {
                meshRenderer.material = triggeredMaterial;
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Palauta defaultMaterial, kun trigger poistuu tästä objektista
            if (meshRenderer != null && defaultMaterial != null)
            {
                meshRenderer.material = defaultMaterial;
                
            }
        }
            

    }
}

