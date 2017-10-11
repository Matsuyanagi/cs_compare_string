using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Globalization;
using System.Threading;

//	2013-07-09
//	数値順ソート、除数ソート

namespace NumericStringsSortNS{
	
	class NumericStringsSort{
		
		public static void Main()
		{
			NumericStringsSortTest();
		}
		
		
		public static void NumericStringsSortTest()
		{
			IEnumerable< string > sample_strings = new List< string >{
				"t1",
				"t1aaa1",
				"t1aaa10",
				"t1aaa2",
				"t1aaa2bbb1",
				"t1aaa2bbb10",
				"t1aaa2bbb2",
				"t1aaa20",
				"t001",
				"t0001aaa1",
				"t0001aaa10",
				"t0001aaa2",
				"t0001aaa2bbb1",
				"t0001aaa2bbb10",
				"t0001aaa2bbb2",
				"t0001aaa20",
				"t2",
				"t3",
				"t4",
				"t5",
				"t6",
				"t7",
				"t8",
				"t9",
				"t10",
				"t11",
				"t11",
				"t100",
				"t101",
				"t20",
				"t21",
				"t22",
				
				"T1",
				"T1AAA1",
				"T1AAA10",
				"T1AAA2",
				"T1AAA2BBB1",
				"T1AAA2BBB10",
				"T1AAA2BBB2",
				"T1AAA20",
				"T001",
				"T0001AAA1",
				"T0001AAA10",
				"T0001AAA2",
				"T0001AAA2BBB1",
				"T0001AAA2BBB10",
				"T0001AAA2BBB2",
				"T0001AAA20",
				"T2",
				"T3",
				"T4",
				"T5",
				"T6",
				"T7",
				"T8",
				"T9",
				"T10",
				"T11",
				"T11",
				"T100",
				"T101",
				"T20",
				"T21",
				"T22",
				
			};
			
			
			
			
			
			
			Console.WriteLine( String.Join( "\n", sample_strings.OrderBy( x => x, new NumericStringComparer() ).Select( x => x ) ) );
			
			
			
			
			
		}
		
		class NumericStringComparer : StringComparer
		{
			private Regex reg_number = new Regex( @"(?<str>.*)(?<num>[0-9]+)?" );
			
			public StringComparer Comparer{ get; set; }
			
			public NumericStringComparer()
			{
				Comparer = StringComparer.CurrentCulture;
			}
			
			public NumericStringComparer( StringComparer cmparer )
			{
				Comparer = cmparer;
			}
			
//			public override bool Equals( Object x, Object y )
//			{
//				return Comparer.Equals( x, y );
//			}
			
			public override bool Equals( String x, String y )
			{
				return Comparer.Equals( x, y );
			}
			
			
//			public override int Compare( Object x, Object y )
//			{
//				return Compare( x as string, y as string )
//			}
			
			public override int GetHashCode( string x )
			{
				return Comparer.GetHashCode( x );
			}
			
			
			public override int Compare( String x, String y )
			{
				//	比較の結果同じだと判断されるならそれで終了
				if ( this.Equals( x, y ) ){
					return 0;
				}
				
				if ( ! reg_number.IsMatch( x ) || ! reg_number.IsMatch( y ) ){
					//	どちらかが数値を含んでいないなら通常の比較のみ
					return Comparer.Compare( x, y );
				}
				
				//	両方とも数値文字列を1箇所以上含んでいる
				
				//	[ 文字列, 数値, 文字列, 数値 ... ] パターンに分解する
				
				MatchCollection mx;
				MatchCollection my;
				
				mx = reg_number.Matches( x );
				my = reg_number.Matches( y );
				
				
				
#if false
				mx = reg_number.Match( x );
				my = reg_number.Match( y );
				
				mx.Groups[ "str" ]
#endif
				
				return Comparer.Compare( x, y );
			}
			
			
		}
		
		
		
		
	}
}
