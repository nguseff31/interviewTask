using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KasperskyTest.Queue
{
	class Node<T>
	{
		public T Value { get; private set; }

		public Node<T> Next { get; set; }

		public Node(T value)
		{
			Value = value;
		}
	}

	class OptimisticConcurrentStack<T>
	{
		Node<T> head = null;

		public int SleepTimeout = 10;

		public OptimisticConcurrentStack()
		{
		}

		public T pop()
		{
			while (true)
			{
				if (head != null)
				{
					Node<T> currentHead;
					Node<T> newHead;
					do
					{
						currentHead = head;
						newHead = currentHead.Next;
					}
					while (Interlocked.CompareExchange(ref head, newHead, currentHead) != currentHead);
					return currentHead.Value;
				}
				Thread.Sleep(SleepTimeout);
			}
		}

		public void push(T item)
		{
			Node<T> currentHead;
			Node<T> newHead;
			do
			{
				currentHead = head;
				newHead = new Node<T>(item)
				{
					Next = currentHead
				};
			} while (Interlocked.CompareExchange(ref head, newHead, currentHead) != currentHead);
		}

		public bool Empty()
		{
			return head == null;
		}
	}
}
