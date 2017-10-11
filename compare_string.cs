using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Globalization;
using System.Threading;

//	文字列比較関数ごとの比較結果
//		
//		Ordinal は unicode コードポイント数値の比較						'a' > 'A'
//		ja-JP, en-US 等ではカルチャーの特性として大文字の方が大きい。	'a' < 'A'
//	
//	2013-07-08
//	
namespace CompareStringNS{
	
	class CompareStringCC{
		
		public static void Main()
		{
//			//	文字列比較テスト
//			CompareStrings();
			
			//	文字列ソートテスト
			SortStrings();
			
		}
		
		//	文字列比較テスト
		public static void CompareStrings()
		{
			//	カルチャー
			var culture_names = new List<string>{
				"ja-JP",
				"en-US",
				"",
				
				//	http://msdn.microsoft.com/ja-jp/library/xk2wykcz(v=vs.80).aspx
				//		たとえば、デンマーク語("da-DK")では、2 文字のペア aA と AA を大文字と小文字を区別せずに比較した結果は同じであるとは見なされません。
				"da-DK",
				
				//	http://msdn.microsoft.com/ja-jp/library/xk2wykcz(v=vs.80).aspx
				//		文字列 "FILE" と "file" に対し、大文字小文字を区別しない String.Compare 操作を実行したとき、カルチャによって結果がどのように異なるかを次のコード例で示します。Thread.CurrentCulture プロパティが "en-US" (米国の英語) に設定されている場合は、この比較操作によって true が返されます。CurrentCulture が "tr-TR" (トルコのトルコ語) に設定されている場合は、この比較によって false が返されます。
				"tr-TR",
				
			};
			
			
			//	比較対象文字列
			var string_pairs = new List< KeyValuePair< string, string > >{
				new KeyValuePair<string, string>( "abcdef", "ABCDEF" ),
				new KeyValuePair<string, string>( "coop", "co-op" ),
				new KeyValuePair<string, string>( "aA", "AA" ),					//	"da-DK"
				new KeyValuePair<string, string>( "file", "FILE" ),				//	"tr-TR"
			};
			
			//	比較関数配列 名前と比較結果をタプルで返す
			var function_lists_each_culture = new List< Func< string, string, Tuple< string, int > > >{
//				{ ( a, b ) => Tuple.Create( "String.Compare( a, b )", String.Compare( a, b ) ) },
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.CurrentCulture )", String.Compare( a, b, StringComparison.CurrentCulture ) ) },
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.CurrentCultureIgnoreCase )", String.Compare( a, b, StringComparison.CurrentCultureIgnoreCase ) ) },
			};
			
			var function_lists_once = new List< Func< string, string, Tuple< string, int > > >{
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.InvariantCulture )", String.Compare( a, b, StringComparison.InvariantCulture ) ) },
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.InvariantCultureIgnoreCase )", String.Compare( a, b, StringComparison.InvariantCultureIgnoreCase ) ) },
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.Ordinal )", String.Compare( a, b, StringComparison.Ordinal ) ) },
				{ ( a, b ) => Tuple.Create( "String.Compare( a, b, StringComparison.OrdinalIgnoreCase )", String.Compare( a, b, StringComparison.OrdinalIgnoreCase ) ) },
				
			};
			
			
			
			
			//	カルチャー関係なし比較
			Console.WriteLine( "CultureInfo.CurrentCulture.Name : {0}", CultureInfo.CurrentCulture.Name );
			foreach( var string_pair in string_pairs ){
				
				//	比較関数ごと
				foreach( var function in function_lists_once ){
					
					var result = function( string_pair.Key, string_pair.Value );
					
					//	比較関数文字列名
					string function_name = result.Item1;
					//	比較結果
					int function_result = result.Item2;
					
					//	結果表示
					PrintResult( function_name, string_pair.Key, string_pair.Value, function_result );
				}
			}
			
			
			
			//	カルチャーごと
			foreach( var culture in culture_names ){
				
				Thread.CurrentThread.CurrentCulture = new CultureInfo( culture, false );
				Console.WriteLine( "CultureInfo.CurrentCulture.Name : {0}", CultureInfo.CurrentCulture.Name );
				
				//	文字列ごと
				foreach( var string_pair in string_pairs ){
					
					//	比較関数ごと
					foreach( var function in function_lists_each_culture ){
						
						var result = function( string_pair.Key, string_pair.Value );
						
						//	比較関数文字列名
						string function_name = result.Item1;
						//	比較結果
						int function_result = result.Item2;
						
						//	結果表示
						PrintResult( function_name, string_pair.Key, string_pair.Value, function_result );
					}
				}
			}
			
			
		}
		
		//	比較結果表示
		private static void PrintResult( string title, string a, string b, int value )
		{
			Console.WriteLine( "---- {0}", title );
			if ( value < 0 ){
				Console.WriteLine( "      {0} <  {1} ", a, b );
			} else if ( value > 0 ){
				Console.WriteLine( "      {0} >  {1} ", a, b );
			} else {
				Console.WriteLine( "      {0} == {1} ", a, b );
			}
		}
		
		
		//	文字列ソートテスト
		public static void SortStrings()
		{
			//	カルチャー
			var culture_names = new List<string>{
				"ja-JP",
				"en-US",
				"",
				
				//	http://msdn.microsoft.com/ja-jp/library/xk2wykcz(v=vs.80).aspx
				//		たとえば、デンマーク語("da-DK")では、2 文字のペア aA と AA を大文字と小文字を区別せずに比較した結果は同じであるとは見なされません。
			//	"da-DK",
				
				//	http://msdn.microsoft.com/ja-jp/library/xk2wykcz(v=vs.80).aspx
				//		文字列 "FILE" と "file" に対し、大文字小文字を区別しない String.Compare 操作を実行したとき、カルチャによって結果がどのように異なるかを次のコード例で示します。Thread.CurrentCulture プロパティが "en-US" (米国の英語) に設定されている場合は、この比較操作によって true が返されます。CurrentCulture が "tr-TR" (トルコのトルコ語) に設定されている場合は、この比較によって false が返されます。
			//	"tr-TR",
				
			};
			
			
			//	比較対象文字列
			var string_lists = new List< List< string > >{
				new List< string >{ "a", "A" },
				new List< string >{ "aa", "aA", "Aa", "AA" },
				new List< string >{ "BB", "Bb", "bB", "bb" },
				new List< string >{ "coop", "co-op", "cop", "coo-p", "c-oop" },
			};
			
			//	比較関数配列 名前と比較結果をタプルで返す
			var function_lists_each_culture = new List< Func< List< string >, Tuple< string, IEnumerable< string > > > >{
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.CurrentCulture )", string_list.OrderBy( x => x, StringComparer.CurrentCulture ).Select( x => x ) ) },
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.CurrentCultureIgnoreCase )", string_list.OrderBy( x => x, StringComparer.CurrentCultureIgnoreCase ).Select( x => x ) ) },
			};
			
			var function_lists_once = new List< Func< List< string >, Tuple< string, IEnumerable< string > > > >{
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.InvariantCulture )", string_list.OrderBy( x => x, StringComparer.InvariantCulture ).Select( x => x ) ) },
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.InvariantCultureIgnoreCase )", string_list.OrderBy( x => x, StringComparer.InvariantCultureIgnoreCase ).Select( x => x ) ) },
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.Ordinal )", string_list.OrderBy( x => x, StringComparer.Ordinal ).Select( x => x ) ) },
				{ ( string_list ) => Tuple.Create( "String.Compare( a, b, StringComparer.OrdinalIgnoreCase )", string_list.OrderBy( x => x, StringComparer.OrdinalIgnoreCase ).Select( x => x ) ) },
				
				
				
			};
			
			
			//	カルチャー関係なし比較
			Console.WriteLine( "CultureInfo.CurrentCulture.Name : {0}", CultureInfo.CurrentCulture.Name );
			foreach( var string_list in string_lists ){
				
				Console.WriteLine( "      {0,-70} : {1}", " ", String.Join( ",", string_list ) );
					
				//	比較関数ごと
				foreach( var function in function_lists_once ){
					
					var result = function( string_list );
					
					//	比較関数文字列名
					var function_name = result.Item1;
					//	比較結果
					var function_result = result.Item2;
					
					//	結果表示
					Console.WriteLine( "      {0,-70} : {1}", function_name, String.Join( ",", function_result ) );
				}
			}
			
			
			
			//	カルチャーごと
			foreach( var culture in culture_names ){
				
				Thread.CurrentThread.CurrentCulture = new CultureInfo( culture, false );
				Console.WriteLine( "CultureInfo.CurrentCulture.Name : {0}", CultureInfo.CurrentCulture.Name );
				
				//	文字列ごと
				foreach( var string_list in string_lists ){
					
					Console.WriteLine( "      {0,-70} : {1}", " ", String.Join( ",", string_list ) );
					
					//	比較関数ごと
					foreach( var function in function_lists_each_culture ){
						
						var result = function( string_list );
						
						//	比較関数文字列名
						var function_name = result.Item1;
						//	比較結果
						var function_result = result.Item2;
						
						//	結果表示
						Console.WriteLine( "      {0,-70} : {1}", function_name, String.Join( ",", function_result ) );
						
					}
				}
			}
			
			
		}
		
		
		
		
	}
}



