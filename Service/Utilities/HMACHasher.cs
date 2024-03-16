using System.Security.Cryptography;
using System.Text;

namespace Service.Utilities;
public static class HMACHasher
{
    const string secretKey = "542669ae-f575-452d-8f30-f1ab658b89be";
    public static async Task<byte[]> HashValue(string valueToHash, string valueSalt)
    {
        var strStreamOne = new MemoryStream(Encoding.UTF8.GetBytes(valueSalt + valueToHash));
        byte[] hashOne;
        byte[] key = Encoding.UTF8.GetBytes(secretKey);
        using (var hmac = new HMACSHA256(key))
        {
            hashOne = await hmac.ComputeHashAsync(strStreamOne);
        }

        return hashOne;
    }
}