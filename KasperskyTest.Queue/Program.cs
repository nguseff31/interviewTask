using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KasperskyTest.Queue
{
	class Program
	{
		static IConcurrentQueue<int> _queue;

		static void Main(string[] args)
		{
			_queue = new SimpleConcurrentQueue<int>() { SleepTimeout = 10 };
			SimpleTest();

			Console.ReadLine();
		}

		static void SimpleTest()
		{
			_queue = new SimpleConcurrentQueue<int>();
			Parallel.Invoke(() => PushingTask(1, 10), () => PushingTask(2, 10), () => DequeueTask(20));
		}

		static void ParallelWriteTest(int count)
		{
			long elapsed = ParallelPerformanceTest(() => PushingTask(1, count), () => PushingTask(2, count));
			Console.WriteLine("Параллельная запись: {0} мс", elapsed);
		}

		static void ParallelWriteReadTest(int count)
		{
			long elapsed = ParallelPerformanceTest(() => PushingTask(1, count), () => DequeueTask(count));
			Console.WriteLine("Параллельные запись/чтение: {0} мс", elapsed);
		}

		static void TwoWriteOneReadTest()
		{
			long elapsed = ParallelPerformanceTest(() => PushingTask(1, 100000), () => PushingTask(1, 100000), () => DequeueTask(200000));
			Console.WriteLine("Два на запись/один на чтение: {0} мс", elapsed);
		}

		static long ParallelPerformanceTest(params Action[] actions)
		{
			ManualResetEvent[] resetEvents = new ManualResetEvent[actions.Length];
			for (int i = 0; i < resetEvents.Length; i++)
			{
				resetEvents[i] = new ManualResetEvent(false);
			}
			Action[] actionsWithResetEvents = new Action[actions.Length];
			for (int i = 0; i < actionsWithResetEvents.Length; i++)
			{
				int copy_i = i;
				actionsWithResetEvents[i] = () => { actions[copy_i](); resetEvents[copy_i].Set(); };
			}

			var sw = new Stopwatch();
			long totalTime = 0;
			for (int i = 0; i < 10; i++)
			{
				sw.Start();
				Parallel.Invoke(actionsWithResetEvents);
				sw.Stop();
				totalTime += sw.ElapsedMilliseconds;
				sw.Reset();
			}
			return totalTime / 10;
		}

		static void PushingTask(int threadId, int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				_queue.push(threadId);
				Thread.Sleep(10);
			}
		}

		static void DequeueTask(int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				Console.WriteLine(_queue.pop());
			}
		}
	}
}
