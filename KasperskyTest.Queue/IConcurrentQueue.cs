using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasperskyTest.Queue
{
	interface IConcurrentQueue<T>
	{
		void push(T item);
		T pop();
	}
}
