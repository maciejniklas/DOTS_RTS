using UnityEngine;

namespace DOTS_RTS.Modules.UnitsSelection.Model.FormationPositionsProcessor
{
    public abstract class FormationPositionsProcessor : ScriptableObject
    {
        [SerializeField] protected float unitRadius;
        [SerializeField] protected float unitSpacing;
        
        public abstract Vector3[] Process(Vector3 center, int size);
    }
}