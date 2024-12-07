using UnityEngine;

public class ObeliskScript : MonoBehaviour
{
    public Material defaultMaterial; // Materiaali, joka n�kyy oletuksena
    public Material triggeredMaterial; // Materiaali, joka n�kyy triggerin osuessa

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
            // Vaihda triggeredMaterialiin, kun trigger osuu t�h�n objektiin
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
            // Palauta defaultMaterial, kun trigger poistuu t�st� objektista
            if (meshRenderer != null && defaultMaterial != null)
            {
                meshRenderer.material = defaultMaterial;
                
            }
        }
            

    }
}

