using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KasperskyTest.Queue
{
	class SimpleConcurrentQueue<T> : IConcurrentQueue<T>
	{
		Queue<T> _queue;

		object _lockObject = new object();

		public int SleepTimeout = 20;

		public SimpleConcurrentQueue(int n)
		{
			_queue = new Queue<T>(n);
		}

		public SimpleConcurrentQueue()
		{
			_queue = new Queue<T>();
		}

		public void push(T item)
		{
			lock (_lockObject)
			{
				_queue.Enqueue(item);
			}
		}

		public T pop()
		{
			while (true)
			{
				lock (_lockObject)
				{
					if (_queue.Any())
					{
						return _queue.Dequeue();
					}
				}
				Thread.Sleep(SleepTimeout);
			}
		}
	}
}
