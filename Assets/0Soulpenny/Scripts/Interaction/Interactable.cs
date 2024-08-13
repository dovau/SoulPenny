using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class Interactable : MonoBehaviour, IInteractable, IDamagable
    {
        public Renderer[] renderers;

        public string ItemName { get; set; }

        [SerializeField] private string description;

        public ItemType ItemType { get; set; }
        public string GetDescription() => description;
        public string GetItemName() => ItemName;

        private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();



        [SerializeField] float hitPoints;
        [SerializeField] bool isDamagable; //Don't conflate with IDamagable interface, all interactable objects have this
        [SerializeField] bool isBreakable;
        [SerializeField] bool vanishWhenBroken;
        [SerializeField] bool replaceWhenBroken;
        [SerializeField] GameObject brokenPrefab;
        [SerializeField] GameObject[] brokenPrefabs;
        [SerializeField] GameObject unbrokenMesh;
        [SerializeField] GameObject[] brokenMesh;
        private List<GameObject> remainingParts = new List<GameObject>();

        private bool isTakingDamage = false;
        [SerializeField] private float damageCooldown = .5f;
        private float damageTimer = 0.0f;

        Color originalOutlineColor;



        public virtual void Awake()
        {
            SetUpRenderers();

        }

        public virtual void Start()
        {

        }

        private void SetUpRenderers()
        {
            renderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                originalMaterials[renderer] = renderer.materials;
            }

            // Check if _OutlineColor property exists in materials
            foreach (var renderer in renderers)
            {
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty("_OutlineColor"))
                    {
                        originalOutlineColor = material.GetColor("_OutlineColor");
                        // Exit the loop if found
                        return;
                    }
                }
            }
            // If _OutlineColor property is not found, use a default color
            originalOutlineColor = Color.white; // You can change this to any default color
        }


        //public virtual void Interact()
        //{
        //    Debug.Log($"Interacting with {gameObject.name}");
        //}
        public virtual void Interact()
        {
            Debug.Log($"Interacting with {gameObject.transform.GetType().Name}");
        }
        public virtual void Highlight(bool highlight)
        {
            foreach (var renderer in renderers)
            {
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty("_OutlineColor"))
                    {
                        Color outlineColor = material.GetColor("_OutlineColor");
                        outlineColor.a = highlight ? 1.0f : 0.0f;
                        material.SetColor("_OutlineColor", outlineColor);
                    }
                }
            }
        }
        public virtual void Highlight(bool highlight, Color? customColor = null)
        {
            foreach (var renderer in renderers)
            {
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty("_OutlineColor"))
                    {
                        Color outlineColor = customColor ?? material.GetColor("_OutlineColor");
                        outlineColor.a = highlight ? 1.0f : 0.0f;
                        material.SetColor("_OutlineColor", outlineColor);
                    }
                }
            }
        }
        public virtual void TakeDamage(float damageAmount)
        {
            if (isDamagable)
            {
                Debug.Log("Equippable damage output: "+ damageAmount);
                hitPoints -= damageAmount;
                Debug.Log($"Damage taken: {damageAmount}"); // expand this like armor or other factors
                Debug.Log($"Current hit points left: { hitPoints}");

                if ( hitPoints <= 0 )
                {
            
                }

                else
                {
                    isTakingDamage = true;
                    damageTimer = 0.0f;
                    UpdateDamageHighlight();
                }
            }

        }

        public virtual void TakeDamage(float damageAmount, Vector3 impactDirection)
        {

        }

        private void UpdateDamageHighlight()
        {

        }

        public void BreakItem()
        {
            if(isBreakable)
            {
                Debug.Log(this.name + " broken.");
                if(brokenPrefab != null)
                {
                    GameObject brokenObject = Instantiate(brokenPrefab, transform.position, transform.rotation);
                    foreach(Transform child in brokenObject.transform)
                    {
                        //do the remainafterbroken logic and the rest of the breaking logic
                    }
                }
            }
        }


    }

}