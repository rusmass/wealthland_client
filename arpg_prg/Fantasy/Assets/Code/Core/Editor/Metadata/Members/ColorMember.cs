using UnityEngine;
using System;
using System.IO;
using Core.AutoCode;

namespace Metadata.Raw
{
	class ColorMember : MemberBase
	{
        public override void WriteLoad (CodeWriter writer)
		{
			writer.WriteLine("{0} = reader.ReadColor();", _name);
		}

        public override void WriteSave(CodeWriter writer)
        {
            writer.WriteLine("writer.Write({0});", _name);
        }

        public override void WriteNotEqualsReturn (CodeWriter writer)
		{
			writer.WriteLine("if (!Unique.ColorTools.IsEqual({0}, that.{0}))", _name);
			using (CodeScope.CreateCSharpScope(writer))
			{
				writer.WriteLine("return false;");
			}
		}
	}
}