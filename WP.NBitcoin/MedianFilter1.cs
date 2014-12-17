
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin
{
    public class MedianFilterInt64B
	{
		Queue<Int64> vValues;
		Queue<Int64> vSorted;
		uint nSize;

		public MedianFilterInt64B(uint size, Int64 initialValue)
		{
			nSize = size;
			vValues = new Queue<Int64>((int)size);
			vValues.Enqueue(initialValue);
			vSorted = new Queue<Int64>(vValues);
		}

		public Int64 Median
		{
			get
			{
				int size = vSorted.Count;
				if(size <= 0)
					throw new InvalidOperationException("size <= 0");

				var sortedList = vSorted.ToList();
				if(size % 2 == 1)
				{
					return sortedList[size / 2];
				}
				else // Even number of elements
				{
					return (sortedList[size / 2 - 1] + sortedList[size / 2]) / 2;
				}
			}
		}

		public void Input(Int64 value)
		{
			if(vValues.Count == nSize)
			{
				vValues.Dequeue();
			}
			vValues.Enqueue(value);
			vSorted = new Queue<Int64>(vValues.OrderBy(o => o));
		}
	}
}