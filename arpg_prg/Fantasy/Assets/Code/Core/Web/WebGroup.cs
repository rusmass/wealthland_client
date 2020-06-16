using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Web
{
	public class WebGroup: IWebNode, IIsYieldable
	{
		internal WebGroup (IList<IWebNode> nodes, Action<WebGroup> handler)
		{
			_nodes = nodes ?? _emptyNodes;
			_loadItemsRoutine = CoroutineManager.StartCoroutine(_CoLoadItems(handler));
		}

		private IEnumerator _CoLoadItems (Action<WebGroup> handler)
		{
			var oldNodesCount = _nodes.Count;
			for (int index= 0; index < _nodes.Count; ++index)
			{
				var node = _nodes[index];
				if (null == node)
				{
					continue;
				}

				// 这个地方切记不能使用yield return node，有两个重要的原因:
				// 1. WebGroup的handler是需要在所有子节点的handler都调用完成之后调用，这是假设前提;
				// 2. 每一个yield return node至少意味着一帧，当_nodes特别大时（1000+），仅仅yield操作就将花费几十秒
				while (!node.isUseless)
				{
					yield return null;
				}
			}

			if (_nodes.Count < oldNodesCount)
			{
				Console.Error.WriteLine("[_CoLoadItems()] _nodes list may be cleared, old={0}, new={1}", oldNodesCount.ToString(), _nodes.Count.ToString());
				yield break;
			}

			_loadItemsRoutine = null;
			_nodeState.isLoaded = true;

			if (null != handler)
			{
				CallbackTools.Handle(ref handler, this, "[WebGroup:_CoLoadItems()]");
			}

			_nodeState.isHandled = true;
		}

		public override string ToString()
		{
			return string.Format("[WebGroup: progress={0}, isDone={1}, nodes.Count={2}]", 
				progress, isDone, nodes.Count);
		}

		public float progress
		{ 
			get
			{
				var nodesCount = _nodes.Count;

				if (nodesCount > 0)
				{
					var totalLoaded = 0.0f;
					var totalGiven  = 0L;

					for (int index= 0; index < nodesCount; ++index)
					{
						var node    = _nodes[index] ?? EmptyWebNode.Instance;
						var nodeSize= node.size;

						totalLoaded += node.progress * nodeSize;
						totalGiven  += nodeSize;
					}

					var currentProgress = totalGiven > 0 ? totalLoaded / totalGiven : 0.0f;
					return currentProgress;
				}

				return 1.0f;
			} 
		}

		public long size
		{
			get 
			{
				var total = 0L;
				var nodesCount = _nodes.Count;

				for (int index= 0; index < nodesCount; ++index)
				{
					var node = _nodes[index] ?? EmptyWebNode.Instance;
					total += node.size;
				}

				return total;
			}
		}

		public void kill ()
		{
			if (!_nodeState.isKilled)
			{
				_nodeState.isKilled = true;

				var nodesCount = _nodes.Count;
				for (int index = 0; index < nodesCount; ++index)
				{
					var node = _nodes[index] ?? EmptyWebNode.Instance;
					node.kill();
				}

				CoroutineManager.KillCoroutine(ref _loadItemsRoutine);
			}
		}

		public IList<IWebNode>	nodes	{ get { return _nodes;} }
		public bool	isDone				{ get { return _nodeState.isLoaded; } }
		public bool isKilled			{ get { return _nodeState.isKilled; } }
		bool IIsYieldable.isYieldable	{ get { return _nodeState.isLoaded || _nodeState.isKilled; } }
		bool IWebNode.isUseless     	{ get { return _nodeState.isHandled || _nodeState.isKilled; } }

		private IList<IWebNode>     _nodes;
		private WebNodeState		_nodeState;
		private IEnumerator			_loadItemsRoutine;

		private static readonly IWebNode[] _emptyNodes = new IWebNode[] { EmptyWebNode.Instance }; 
	}
}
