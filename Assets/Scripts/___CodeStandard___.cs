/// 
///     Priorities: region > privacy modifiers > static/non-static > attributes (e.g. [SerializeField])
/// 
///     CamelCase for properties/methods/classes/namespaces only
///     
///     "Co" before Coroutines

#region Usings

using System.Collections;
using UnityEngine;

#endregion

namespace CodeStandard
{
    class CodeStandard : MonoBehaviour
    {
        #region Fields & Properties

        static public int staticPublicVar;

        [HideInInspector] public int publicVarWithAttribute;
        public int publicVar;

        protected int ProtectedProperty { get; set; }
        protected bool protectedField;

        static private bool staticPrivateBoolean;
        [SerializeField] private float privateVarWithAttribute;

        #endregion

        #region Unity methods

        protected void Start()
        {
            // Start    
        }

        private void Awake()
        {
            // Awake
        }

        #endregion

        #region Class-specific methods

        static public void DoNothing()
        {
            // Do nothing
        }

        private IEnumerator CoDoNothingAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            DoNothing();
        }

        #endregion
    }
}