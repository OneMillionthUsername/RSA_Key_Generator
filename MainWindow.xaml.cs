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
using System.Security.Cryptography;

namespace RSA_Key_Generator {
	public partial class MainWindow : Window {

		private static readonly UnicodeEncoding ByteConverter = new();
		private static readonly RSACryptoServiceProvider rSA = new(KeySize);
		private static int KeySize { get; set; }

		private byte[] EncryptedData;
		private byte[] DecryptedData;

		public MainWindow() {
			InitializeComponent();
		}
		public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding) {
			try {
				byte[] decryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new(KeySize)) {
					//Import the RSA Key information. This needs
					//to include the private key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Decrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
				}
				return decryptedData;
			}
			catch (CryptographicException e) {
				ErrorMessageBox(e.Message, "Error");
				throw;
			}
		}
		public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding) {
			try {
				byte[] encryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new(KeySize)) {

					//Import the RSA Key information. This only needs
					//to include the public key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Encrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
				}
				return encryptedData;
			}
			catch (CryptographicException e) {
				ErrorMessageBox(e.Message, "Error");
				throw;
			}
		}
		private void btnEncrypt_Click(object sender, RoutedEventArgs e) {
			byte[] DataToEncrypt = ByteConverter.GetBytes(Message_to_encrypt.Text.ToCharArray());
			EncryptedData = RSAEncrypt(DataToEncrypt, rSA.ExportParameters(false), false);
			encrypted_Message.Text = Convert.ToBase64String(EncryptedData); 
		}
		private void btnDecryptMessage_Click(object sender, RoutedEventArgs e) {
			DecryptedData = RSADecrypt(EncryptedData, rSA.ExportParameters(true), false);
			Decrypted_message.Text = ByteConverter.GetString(DecryptedData);
		}
		private void btnGenerateRSAKey_Click(object sender, RoutedEventArgs e) {
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(KeySize)) {
				RSA_public_key_value.Text = Convert.ToBase64String(rSACryptoServiceProvider.ExportRSAPublicKey());
				RSA_private_key_value.Text = Convert.ToBase64String(rSACryptoServiceProvider.ExportRSAPrivateKey());
			}
		}
		private static void ErrorMessageBox(string errMessage, string errCaption) {
			string messageBoxText = errMessage;
			string caption = errCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Error;
			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}
		#region Keysize Selection
		private void keysize_384_Selected(object sender, RoutedEventArgs e) {
			KeySize = 384;
		}

		private void keysize_512_Selected(object sender, RoutedEventArgs e) {
			KeySize = 512;
		}

		private void keysize_1024_Selected(object sender, RoutedEventArgs e) {
			KeySize = 1024;
		}

		private void keysize_2048_Selected(object sender, RoutedEventArgs e) {
			KeySize = 2048;
		}

		private void keysize_4096_Selected(object sender, RoutedEventArgs e) {
			KeySize = 4096;
		}

		private void keysize_8192_Selected(object sender, RoutedEventArgs e) {
			KeySize = 8192;
		}

		private void keysize_16384_Selected(object sender, RoutedEventArgs e) {
			KeySize = 16384;
		}

		#endregion
	}
}
