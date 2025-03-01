using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Common;

public class PasswordHelper
{
    public static string GetPasswordHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var subKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01;
        WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA256);
        WriteNetworkByteOrder(outputBytes, 5, 10000);
        WriteNetworkByteOrder(outputBytes, 9, 16);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + 16, subKey.Length);
        return Convert.ToBase64String(outputBytes);
    }
    
    public static bool CheckPasswordHash(string password, string hash)
    {
        var hashBytes = Convert.FromBase64String(hash);

        try
        {
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashBytes, 1);
            var iCount = ReadNetworkByteOrder(hashBytes, 5);
            var saltLength = ReadNetworkByteOrder(hashBytes, 9);

            if (saltLength < 16) return false;
            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashBytes, 13, salt, 0, salt.Length);
            var subKeyLength = hashBytes.Length - 13 - salt.Length;
            if (subKeyLength < 16) return false;

            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashBytes, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);
            var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, (int)iCount, subKeyLength);
            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            return false;
        }
    }
    
    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
    
    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
               | ((uint)(buffer[offset + 1]) << 16)
               | ((uint)(buffer[offset + 2]) << 8)
               | (buffer[offset + 3]);
    }
}