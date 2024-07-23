using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class CubeLight : Interactable
    {
        private Light objLight;
        [SerializeField] private bool lightOn;

        public override void Awake()
        {
            base.Awake();
            objLight = GetComponentInChildren<Light>();
        }

        public override void Interact()
        {
            base.Interact();
            ToggleLight();

        }
    
        private void ToggleLight()
        {

            if (objLight!=null)
            {
                if (lightOn)
                {
                    lightOn = false;
                    objLight.enabled = false;
                }
                else
                {
                    lightOn = true;
                    objLight.enabled = true;
                } 
            }
        }

    
    } 


}
