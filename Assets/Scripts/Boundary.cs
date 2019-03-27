using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class Boundary : MonoBehaviour
    {
       
        public class Factory:PlaceholderFactory<Boundary>//todo add UsedImplicitlyAttribute 
        {
        }
    }
}
