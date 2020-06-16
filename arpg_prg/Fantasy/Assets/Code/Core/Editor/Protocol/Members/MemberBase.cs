using System;
using UnityEngine;
using Core.AutoCode;
using System.Collections.Generic;

namespace Protocol
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
//				else if (type == typeof(string))
//				{
//					member = new StringMember();
//				}
//				else if (type.IsArray)
//				{
//					member = new ArrayMember();
//				}
//				else if (type.IsGenericType)
//				{
//					var genericTypeDefinition = type.GetGenericTypeDefinition();
//
//					if (typeof(List<>) == genericTypeDefinition)
//					{
//						member = new ListMember();
//					}
//				}
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

		protected void _WriteType(CodeWriter writer, Type type, string name)
		{
			var member = Create(type, name);
			member.WriteType(writer);
		}

		public abstract void WriteType(CodeWriter writer);

		protected Type _type;
		protected string _name;
	}
}

