using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompareString
{
	class NumericStringComparer : StringComparer
	{
		//	[ 文字列 + 数値 ] のペアの組み合わせに切り出す
		private Regex reg_number = new Regex( @"(?<str>\D*)(?<num>(\d+)?)" );

		//	文字列部分比較 StringComparer
		public StringComparer Comparer { get; set; }

		public NumericStringComparer()
		{
			Comparer = StringComparer.CurrentCulture;
		}

		public NumericStringComparer( StringComparer comparer )
		{
			Comparer = comparer;
		}

		public override bool Equals( String x, String y )
		{
			return Comparer.Equals( x, y );
		}

		public override int GetHashCode( string x )
		{
			return Comparer.GetHashCode( x );
		}


		//	数値混じり文字列比較
		//		[ ( 文字列, 数値 ), ( 文字列, 数値 ) ... ] パターンに分解してそれぞれを比較する
		public override int Compare( String x, String y )
		{
			//	文字列比較の結果同じだと判断されるならそれで終了
			if ( Comparer.Equals( x, y ) ) {
				return 0;
			}

			if ( !reg_number.IsMatch( x ) || !reg_number.IsMatch( y ) ) {
				//	どちらかが数値を含んでいないなら通常の比較のみ
				return Comparer.Compare( x, y );
			}

			//	両方とも数値文字列を1箇所以上含んでいる

			//	両方の文字列を [ ( 文字列, 数値 ), ( 文字列, 数値 ) ... ] ペアの組み合わせに分解する
			MatchCollection matches_x = reg_number.Matches( x );
			MatchCollection matches_y = reg_number.Matches( y );

			//	各ペアごとに文字列、数値を比較する
			int count_x = matches_x.Count;
			int count_y = matches_y.Count;
			for ( var i = 0; i < count_x; i++ ) {
				//	ペアの長さでの比較
				if ( count_y <= i ) {
					return 1;				//	x の方が長い x > y
				}

				//	文字列部分での比較
				int str_compare_result = Comparer.Compare( matches_x[ i ].Groups[ "str" ].Value, matches_y[ i ].Groups[ "str" ].Value );
				if ( str_compare_result != 0 ) {
					return str_compare_result;	//	str の部分で大小が決まった
				}

				//	数値部分での比較
				UInt64 number_x = 0;
				string str_x = matches_x[ i ].Groups[ "num" ].Value;
				if ( ! String.IsNullOrEmpty( str_x ) ) {
					UInt64.TryParse( str_x, out number_x );
				}
				UInt64 number_y = 0;
				string str_y = matches_y[ i ].Groups[ "num" ].Value;
				if ( ! String.IsNullOrEmpty( str_y ) ) {
					UInt64.TryParse( str_y, out number_y );
				}
				if ( number_x != number_y ) {
					return ( number_x > number_y ) ? 1 : -1;	//	num の部分で大小が決まった
				}
				
				//	数字文字列の長さを比較
				if ( str_x.Length != str_y.Length ){
					//	ここの比較も 文字列部分比較 StringComparer() を使うべきか
					//		数値文字列比較用 Comparer() を持つべきか。値が同じ場合の判定方法
					return ( str_x.Length > str_y.Length ) ? 1 : -1;	//	num の部分の長さで決まった
				}
				
			}

			//	文字列部分も、数値も同じなら最後に全体として文字列比較
			//		等価の文字列でも順位が同じようにする
			//		'a','1' と等価の文字列順を保証する
			//		a001
			//		a01
			//		a1
			//		の順を保証する
			//		
			return Comparer.Compare( x, y );
		}


	}

}
