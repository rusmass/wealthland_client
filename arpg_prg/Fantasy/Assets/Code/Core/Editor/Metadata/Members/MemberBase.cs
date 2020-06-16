using System;
using UnityEngine;
using Core.AutoCode;
using System.Collections.Generic;

namespace Metadata.Raw
{
    abstract class MemberBase
    {
        public static MemberBase Create(Type type, string name)
        {

            MemberBase member = null;

            if (null != type)
            {
                if (type.IsPrimitive)
                {
                    member = new PrimitiveMember();
                }
                else if (type == typeof(string))
                {
                    member = new StringMember();
                }
                else if (type == typeof(Vector2))
                {
                    member = new Vector2Member();
                }
                else if (type == typeof(Vector3))
                {
                    member = new Vector3Member();
                }
                else if (type == typeof(Vector4))
                {
                    member = new Vector4Member();
                }
                else if (type == typeof(Color))
                {
                    member = new ColorMember();
                }
                else if (type.IsArray)
                {
                    member = new ArrayMember();
                }
                else if (type.IsEnum)
                {
                    member = new EnumMember();
                }
                else if (type.IsGenericType)
                {
                    var genericTypeDefinition = type.GetGenericTypeDefinition();

                    if (typeof(List<>) == genericTypeDefinition)
                    {
                        member = new ListMember();
                    }
                }
                else if (EditorMetaCommon.IsMetadata(type))
                {
                    member = new MetadataMember();
                }
            }

            if (null != member)
            {
                member._type = type;
                member._name = name;
            }
            else
            {
                var text = string.Format("Invalid type found: typeof({0}) = {1}", name, type);
                Console.Error.WriteLine(text);
            }

            return member;
        }

        protected void _WriteLoadType(CodeWriter writer, Type type, string name)
        {
            var member = Create(type, name);
            member.WriteLoad(writer);
        }

        protected void _WriteSaveType(CodeWriter writer, Type type, string name)
        {
            var member = Create(type, name);
            member.WriteSave(writer);
        }

        protected void _WriteNotEqualsReturn(CodeWriter writer, Type type, string name)
        {
            var member = Create(type, name);
            member.WriteNotEqualsReturn(writer);
        }

        public abstract void WriteLoad(CodeWriter writer);
        public abstract void WriteSave(CodeWriter writer);

        public abstract void WriteNotEqualsReturn(CodeWriter writer);

        public Type GetMemberType()
        {
            return _type;
        }

        public string GetMemberName()
        {
            return _name;
        }

        protected Type _type;
        protected string _name;
    }
}