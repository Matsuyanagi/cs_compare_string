using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareString
{
	class Program
	{
		static void Main( string[] args )
		{
#if false
			//	文字列比較テスト
			CompareStringCC.CompareStrings();
#endif
			
			//	数値混じり文字列ソート
			NumericStringsSort.NumericStringsSortTest();
			
		}
	}
}
