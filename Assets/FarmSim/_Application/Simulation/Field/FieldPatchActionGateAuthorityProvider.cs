using System;
using System.Reflection;
using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Bootstrap-safe provider that constructs the in-memory FieldPatchActionGateAuthority
    /// from a FieldPatchAssignmentReadOnlyProvider.
    ///
    /// Contract guarantee:
    /// - Gate is NEVER null.
    /// - Before binding, a NullFieldPatchActionGateAuthority is returned.
    ///
    /// In-memory only. No persistence. No UI. No editor tooling.
    /// </summary>
    public sealed class FieldPatchActionGateAuthorityProvider : MonoBehaviour
    {
        [SerializeField]
        private FieldPatchAssignmentReadOnlyProvider assignmentReadOnlyProvider;

        private IFieldPatchActionGateAuthority _gate = NullFieldPatchActionGateAuthority.Instance;
        private bool _bound;

        private int _frames;
        private int _lastWarnFrame;

        public IFieldPatchActionGateAuthority Gate => _gate;

        private void Start()
        {
            TryBind();
        }

        private void Update()
        {
            if (_bound)
                return;

            _frames++;
            TryBind();
        }

        private void TryBind()
        {
            if (_bound)
                return;

            if (assignmentReadOnlyProvider == null)
            {
                Debug.LogError("[FieldPatchActionGateAuthorityProvider] assignmentReadOnlyProvider not assigned.");
                return;
            }

            if (!assignmentReadOnlyProvider.isActiveAndEnabled)
            {
                WarnOccasionally("[FieldPatchActionGateAuthorityProvider] assignmentReadOnlyProvider is not active/enabled. Will retry.");
                return;
            }

            var readOnly = TryGetReadOnlyFromProvider(assignmentReadOnlyProvider);
            if (readOnly == null)
            {
                WarnOccasionally("[FieldPatchActionGateAuthorityProvider] IFieldPatchAssignmentReadOnly not yet available on provider. Will retry.");
                return;
            }

            _gate = new FieldPatchActionGateAuthority(readOnly);
            _bound = true;

            Debug.Log("[FieldPatchActionGateAuthorityProvider] Bound gate authority to assignment read-only.");
        }

        private void WarnOccasionally(string message)
        {
            if (_frames - _lastWarnFrame < 60)
                return;

            _lastWarnFrame = _frames;
            Debug.LogWarning(message);
        }

        private static IFieldPatchAssignmentReadOnly TryGetReadOnlyFromProvider(object provider)
        {
            var t = provider.GetType();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var props = t.GetProperties(flags);
            for (int i = 0; i < props.Length; i++)
            {
                var p = props[i];
                if (!p.CanRead)
                    continue;

                if (typeof(IFieldPatchAssignmentReadOnly).IsAssignableFrom(p.PropertyType))
                {
                    try { return (IFieldPatchAssignmentReadOnly)p.GetValue(provider, null); }
                    catch { }
                }
            }

            var fields = t.GetFields(flags);
            for (int i = 0; i < fields.Length; i++)
            {
                var f = fields[i];
                if (typeof(IFieldPatchAssignmentReadOnly).IsAssignableFrom(f.FieldType))
                {
                    try { return (IFieldPatchAssignmentReadOnly)f.GetValue(provider); }
                    catch { }
                }
            }

            var methods = t.GetMethods(flags);
            for (int i = 0; i < methods.Length; i++)
            {
                var m = methods[i];
                if (m.GetParameters().Length != 0)
                    continue;

                if (typeof(IFieldPatchAssignmentReadOnly).IsAssignableFrom(m.ReturnType))
                {
                    try { return (IFieldPatchAssignmentReadOnly)m.Invoke(provider, null); }
                    catch { }
                }
            }

            return null;
        }
    }
}
