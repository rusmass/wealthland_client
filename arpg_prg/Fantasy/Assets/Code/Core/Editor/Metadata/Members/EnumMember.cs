using UnityEngine;
using System;
using System.IO;
using Core.AutoCode;

namespace Metadata.Raw
{
    class EnumMember: MemberBase
    {
        public override void WriteLoad(CodeWriter writer)
        {
			writer.WriteLine("{0} = ({1}) reader.ReadUInt16();", _name, EditorMetaCommon.GetNestedClassName(_type));
        }

        public override void WriteSave(CodeWriter writer)
        {
            writer.WriteLine("writer.Write((ushort) {0});", _name);
        }

        public override void WriteNotEqualsReturn (CodeWriter writer)
		{
			writer.WriteLine("if ({0} != that.{0})", _name);
			using (CodeScope.CreateCSharpScope(writer))
			{
				writer.WriteLine("return false;");
			}
		}
    }
}