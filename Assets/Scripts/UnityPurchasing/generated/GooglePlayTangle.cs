// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("QGcKQaJSYRY0TLFKoq70eNT2r9jKFj1+CxUkVyV4rjc32qpDWVdCUTDLQJFhzCISK+eDq9hdAMOnASz45WZoZ1flZm1l5WZmZ5QMhXe7DY0BPS/s3v+INEBAqGy7i+Bjf9+3uUgbAVykq4CVzfDPizy+UrqadgNLbkDX++ABOR0b4hytS0YSEIhmjxO5x0IQwnndPdHG5G4T7f7sQG/riNTHrImy9ccntH0ZF3MsIsFAg4pmgeSWYUgmDLeEXfIkyfy3tvkpAvtqFoQTa6XKTIZk53JXPfxM/HhOPFflZkVXamFuTeEv4ZBqZmZmYmdkxzcAARbVtWcnnoDsaP7Hi7suy1RdYgTUDUMYUnLcOmxEE9YWykkIMeUCiSybinvCUmVkZmdm");
        private static int[] order = new int[] { 4,5,3,5,9,6,10,10,8,12,13,12,12,13,14 };
        private static int key = 103;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
