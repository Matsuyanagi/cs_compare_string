using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Globalization;
using System.Threading;

namespace CompareString{
	
	class NumericStringsSort{
		
#if false
		public static void Main()
		{
			NumericStringsSortTest();
		}
#endif
		
		public static void NumericStringsSortTest()
		{
			//	入力ファイル名
			string filename = "sample.txt";
			//	出力ファイル名
			string filename_out_base = "sample.out.";
			
			//	ファイル読み込み
			IEnumerable< string > sample_strings = File.ReadAllLines( filename ).Where( x => ! String.IsNullOrEmpty( x ) ).ToList();
			
			//	ソート
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_CurrentCulture", new NumericStringComparer( StringComparer.CurrentCulture ) );
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_CurrentCultureIgnoreCase", new NumericStringComparer( StringComparer.CurrentCultureIgnoreCase ) );
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_Ordinal", new NumericStringComparer( StringComparer.Ordinal ) );
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_OrdinalIgnoreCase", new NumericStringComparer( StringComparer.OrdinalIgnoreCase ) );
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_CultureInfo_ja_JP", new NumericStringComparer( StringComparer.Create( CultureInfo.CreateSpecificCulture( "ja-JP" ), true ) ) );
			SampleStringsResult( sample_strings, filename_out_base, "StringComparer_CultureInfo_en_US", new NumericStringComparer( StringComparer.Create( CultureInfo.CreateSpecificCulture( "en-US" ), true ) ) );
			
		//	//	改行連結して表示
		//	Console.WriteLine( String.Join( "\n", sample_strings_result ) );

		//	Console.WriteLine( new NumericStringComparer().Comparer.GetType().FullName );

		}

		private static void SampleStringsResult( IEnumerable< string > sample_strings,
		                                                string filename_out_base,
		                                                string numeric_string_comparer_name,
		                                                NumericStringComparer numeric_string_comparer )
		{
			IEnumerable< string > sample_strings_result = sample_strings.OrderBy( x => x, numeric_string_comparer );
			
			//	ファイル出力
			string filename_out = filename_out_base + numeric_string_comparer_name + ".txt";
			File.WriteAllLines( filename_out, sample_strings_result );
		}
	}
}
