using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

namespace HTMLgeter {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		string NetStatesChecker() {
			bool NetAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
			if (NetAvailable == true) {
				HTMLgeterWindow.Title = "HTMLゲッター";
				return "";
			}	
			else {
				HTMLgeterWindow.Title = "HTMLゲッター (オフライン)";
				return "インターネット接続なし";
			}
		}

		string URLscrutiny(string URL) {
			// ドットが0以上含まれてたら対象
			if (URL.IndexOf(".") > 0) {
				try {
					// 最初が http:// もしくは https:// から始まっているか
					if (URL.Substring(0, 7) == "http://" || URL.Substring(0, 8) == "https://")
						return URL;
					else
						return "http://" + URL;
				}
				// 上のチェックで文字数が足りない場合は http がついていないことになるので、付けて返す
				catch (System.ArgumentOutOfRangeException) {
					return "http://" + URL;
				}
			}
			else {
				result.Text = "入力されたURLは不正です。";
				return URL;
			}	
		}
		
		public MainWindow() {
			InitializeComponent();
			NetStates.Text = NetStatesChecker();
		}

		private void Enter_Event(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				Button_Click_1(sender, e);
				Console.WriteLine("You pushed the enter key.");
			}
				
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			NetStates.Text = NetStatesChecker();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			Console.WriteLine(sender);
			Console.WriteLine(e);
			result.Text = "";

			string url = URLscrutiny(URL.Text);
			if (result.Text != "入力されたURLは不正です。") {
				WebClient wc = new WebClient();
				try {
					// 文字コード指定しておかないと文字化け地獄
					wc.Encoding = Encoding.UTF8;
					HTMLgeterWindow.Title = "HTMLゲッター (読み込み中)";
					// HTML取得
					string text = wc.DownloadString(url);
					result.Text += text;
					HTMLgeterWindow.Title = "HTMLゲッター";
				}
				catch (WebException exc) {
					result.Text += exc.Message;
				}
			}
		}
	}
}
