using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace UniBT.Editor
{
    public class Vector2IntResolver : FieldResolver<Vector2IntField, Vector2Int>
    {
        public Vector2IntResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override Vector2IntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector2IntField(fieldInfo.Name);
        }
        public static bool IsAcceptable(FieldInfo info) => info.FieldType == typeof(Vector2Int);
    }
}