using System.Security.Cryptography;

namespace Chatrum {
    public class SecretKey {
        public const string _secretKey = "eflnYB/pTy85vgdnu2einytr4Aq9XqvKRKXkb23ETrE=";

        //public static string SecretKeyProp { get => _secretKey; set => _secretKey = value; }
        // is null?
        // mozna pro jednoduchost uz predem dany klic?
        //public SecretKey() {
        //    if (null == _secretKey) {
        //        _secretKey = GenerateSecretKey();
        //    }
        //}
        public string GenerateSecretKey() {
            byte[] key = new byte[32]; // 256 bits
            using (var rng = new RNGCryptoServiceProvider()) {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
    }
}
