using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace _PROJECT.Scripts.ModularTiles
{
    public class AdjustableObject : MonoBehaviour
    {
        public Vector2 objectSize = new Vector2(1, 1);
        public List<Material> materials = new List<Material>();
        [Description("The material being used in the 'Materials' list")]
        public int materialIndex = 0;
        public float materialTiling;
        public bool snapToCenter;

        private Material _newMaterial;
        private void OnValidate()
        {
            if (materialIndex >= materials.Count)
                materialIndex = materials.Count - 1;
            if (materialIndex < 0)
                materialIndex = 0;
            
            if (snapToCenter)
            {
                var pos = transform.position;
                var x = MathF.Round(pos.x);
                var y = MathF.Round(pos.y);
                var z = MathF.Round(pos.z);
                transform.position = new Vector3(x, y, z);

                snapToCenter = false;
            }
            
            UpdateTile(objectSize, materialIndex);
        }
        
        // Update the tile's dimensions & material
        public void UpdateTile(Vector2 newDimensions, int _materialIndex)
        {
            // Dimensions
            var dimensions = new Vector3(newDimensions.x, 0.025f, newDimensions.y);
            transform.localScale = dimensions;

            // Material
            var renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = materials[_materialIndex];
            _newMaterial = new Material(renderer.sharedMaterial);
            var materialScale = new Vector2(
                newDimensions.x / materialTiling, 
                newDimensions.y / materialTiling);
            
            materialIndex = _materialIndex;

            // Apply material
            _newMaterial.mainTextureScale = materialScale;
            renderer.sharedMaterial = _newMaterial;
        }
        
        public void UpdateTile(Vector2 newDimensions, int _materialIndex, float _materialTilingMultiplier)
        {
            // Dimensions
            var dimensions = new Vector3(newDimensions.x, 0.025f, newDimensions.y);
            transform.localScale = dimensions;

            // Material
            var renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = materials[_materialIndex];
            _newMaterial = new Material(renderer.sharedMaterial);
            var materialScale = new Vector2(
                newDimensions.x / _materialTilingMultiplier, 
                newDimensions.y / _materialTilingMultiplier);
            
            materialIndex = _materialIndex;

            // Apply material
            _newMaterial.mainTextureScale = materialScale;
            renderer.sharedMaterial = _newMaterial;
        }
    }
}
