using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace UniBT.Editor
{
    public class RectIntResolver : FieldResolver<RectIntField,RectInt>
    {
        public RectIntResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override RectIntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new RectIntField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(RectInt);
    }
}