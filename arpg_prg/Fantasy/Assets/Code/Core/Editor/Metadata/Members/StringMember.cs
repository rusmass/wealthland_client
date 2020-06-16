using Core.AutoCode;

namespace Metadata.Raw
{   
    class StringMember: MemberBase
    {
        public override void WriteLoad (CodeWriter writer)
        {
			writer.WriteLine("{0} = reader.ReadString();", _name);
        }

        public override void WriteSave(CodeWriter writer)
        {
            writer.WriteLine("writer.Write({0});", _name);
        }

        public override void WriteNotEqualsReturn (CodeWriter writer)
		{
			writer.WriteLine("if ((!string.IsNullOrEmpty({0}) || !string.IsNullOrEmpty(that.{0})) && ({0} != that.{0}))", _name);
			using (CodeScope.CreateCSharpScope(writer))
			{
				writer.WriteLine("return false;");
			}
		}
    }
}