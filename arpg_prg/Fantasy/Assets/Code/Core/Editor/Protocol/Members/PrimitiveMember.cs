using System;
using Core.AutoCode;

namespace Protocol
{
	class PrimitiveMember: MemberBase
	{
		public override void WriteType (CodeWriter writer)
		{
			writer.Write("[ProtoMember({0})]", 1);
			writer.WriteLine("public {0} {1}", _type.Name, _name);
		}
	}
}

