using System;

namespace Core.Web
{
	public interface IWebNode
	{
		// 1. 死后不可复活.
		// 2. 死后不再调用handler.
		// 3. 死后isDone=true
		void kill();

		bool isDone { get; }

		bool isKilled { get; }

		bool isUseless { get; }

		float progress { get; }

		long size { get; }
	}
}