using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


var dict = new Dictionary<int, int>();
var multiDict = new MultiKeyDict1();
var multiDict2 = new MultiKeyDict2();
var list = new List<int>();

const int dataSize = 1000;
const int recoveryTimes = 100000;

var rng = new Random();

Bench(x => multiDict.Add(x, rng.Next(int.MaxValue), x), dataSize, "MultDict Add");
Bench(x => multiDict2.Add(x, rng.Next(int.MaxValue), x), dataSize, "MultDict2 Add");
Bench(x => dict.Add(x, x), dataSize, "Dict Add");
Bench(x => list.Add(x), dataSize, "List Add");



Bench(_ =>
{
    for (int i = 0; i < dataSize; i++)
    {
        var ok = dict.TryGetValue(i, out int r);
    }
}, recoveryTimes, "Dict Try recovery");


Bench(_ =>
{
    for (int i = 0; i < dataSize; i++)
    {
        var ok = multiDict.Retrive(i);
    }
}, recoveryTimes, "Mult Dict1 Try recovery");


Bench(_ =>
{
    for (int i = 0; i < dataSize; i++)
    {
        var ok = multiDict2.Retrive(i);
    }
}, recoveryTimes, "Mult Dict2 Try recovery");



Bench(_ =>
{
    for (int i = 0; i < dataSize; i++)
        _ = dict[i];
}, recoveryTimes, "Dict recovery");

Bench(_ =>
{
    for (int i = 0; i < dataSize; i++)
        _ = list.Where(f => f == i).First();
}, recoveryTimes, "List recovery");

static void Bench(Action<int> action, int n, string msg)
{
    var sw = new Stopwatch();
    sw.Start();
    for (int i = 0; i < n; i++)
        action(i);
    Console.WriteLine(msg);
    Console.WriteLine($"Take {sw.Elapsed.TotalMilliseconds}ms for {n} times");
}

//return;
//var pbkey = "MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAsRV1+1uWz1pSpmbvs4SbtPOjfAcMpndzz0or+Ryp4qiA5oSi6ogtfAPTqcA43BKJFjO68AIQtQHdUqmA0k0bQhNc+o8q/jqHdMYlRQmuJKO2wppg9o71D6i4YYSazOu2yd3mLUREz6ZrRXaKU1dKDySMNwbrxinOiGFkXp3G8dWazuSd5dMQiH6UsVjNrRELecDU1XLqWkcLxkqmCWc6uLTyVCqp+9V8pl1tIcQAnN//WkY8QoQXYcHHvOt8sSGeTpvUj5BeXu92YIHDZtwK7xexx9Bo8JaGpxcez8W3j+90exh55sOuIj4aNOj8gfH/FBTQhuCrwWo9t6XtB+rQJ1deQM+cc9DD3LXfpIL8qatExRdejTDk7P/lM9aU+Ks7V1pq4JGP8taWYA/Vm2xEN/AB1JnwipuYXftBN19DwfgvF1zLNM+cDTxMedSJR1EIjNimLqkZt2PQ1Fh0dnT13fna+WZBs0RowdxikFjbKeO45xWU6Jj0u7HpaDnzPkojXS3fgYfCBKk1NbbhMuoMAviJ6TchFgumERy822i+MKHsLriVMYFv3+qjq/dgyq8j2f140rMlvo89Fa/ZYi1d8dd5tye8N7wpcflEB8BiVx6gKdRQDeGeAVxLMS2g44o4wXYVyBX3a2eudUHS9VkFS6dYuBDmhAVgHdPACiRPaK8CAwEAAQ==";
//var pvkey = @"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";

//return;


//return;
//var rsaPublicKey = new RSACryptoServiceProvider();
//rsaPublicKey.ImportSubjectPublicKeyInfo(Convert.FromBase64String(pbkey), out var tot);

//var rsaPrivate = new RSACryptoServiceProvider();
//rsaPrivate.ImportPkcs8PrivateKey(Convert.FromBase64String(pvkey), out var tots);
//Console.WriteLine(rsaPrivate.KeySize);
//Console.WriteLine(
//rsaPrivate.KeyExchangeAlgorithm
//    );
//var de = rsaPrivate.Decrypt(Convert.FromBase64String(@"kwHARPzZNOw0EvhCwDACUaoKNyqny/jWt/v9cy/od3pVU0NxXrm6kd0kxcMcXRp7Cc68JN7iz514dlnZk+OWvKX/D4wtt4M07tb5j25yUCZHjA9Ubsyg9HCX7xK+Z8leGNcKH6rsG1cHJyb5qRoJkEnY5QjCbE4eDyZd0MnWgddx4iriKXoRggAdlMDftbQgxyVBZwOMtl0CxenxULnZIaqnWAjYAl7LGzxgDarMkPECdmAsNWqUwiLMAM7pqbEPxfTY7P4vUhCfnTGhipLN2Ip1r/zhhsMBuM3TQD4fIuD/pCK5Sk5y5eNwiQyFdj+8n4NewnsgiYLIAtwrwfzY/8wX7gfQ7ph1r3sAGTyYVG+s39Xlp/qAtoNiG2QTami4aZMxILD8kaF9/+lfIDAakq1fHRv3ACbdKI+xmLarbU6+a1jrgON94dp/tr0aI6ZAkMqU9DY760e2GwF6mRwPZpiAjnl/QaWO5eCDQxUj4v6dvwvehjEiCRri1eKJFvJtHburW+5ORj45nSUdyu+JuPborHBGercxi5b6Sowqf8TjkV4Yr1Y8HX8mk0aW5sGNpqmdK0E+3Qs/nKBsLNRYI30aJXZddU0ZNRReXuO2V1FhxRWpaFVCdZdcSSYLAL+PY4sG3ySyfOMWIskCYtsPPyemZu/4CPuJTxDGC3xBcyQ="),
//    RSAEncryptionPadding.CreateOaep(HashAlgorithmName.SHA512));
//Console.WriteLine(Encoding.UTF8.GetString(de));
//try
//{
//    var encrypted = rsaPublicKey.Encrypt(Encoding.Unicode.GetBytes("teste"), RSAEncryptionPadding.Pkcs1);
//    Console.WriteLine(Convert.ToBase64String(encrypted));
//}
//finally { }




//try
//{
//    var encrypted = rsaPublicKey.Encrypt(Encoding.Unicode.GetBytes("teste"), true);
//    Console.WriteLine(Convert.ToBase64String(encrypted));
//}
//finally { }

//void Print(byte[] buff)
//{
//    Console.WriteLine(Encoding.ASCII.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.BigEndianUnicode.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.Unicode.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.Latin1.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.UTF32.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.UTF8.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Encoding.UTF7.GetString(buff));
//    Console.WriteLine();
//    Console.WriteLine(Convert.ToBase64String(buff));
//    Console.WriteLine();
//    Console.WriteLine(Convert.ToHexString(buff));
//    Console.WriteLine();
//}



public class MultiKeyDict1
{
    private Dictionary<int, object> dict = new();

    public void Add(object value, params int[] keys)
    {
        var masterKey = keys[0];
        dict.Add(masterKey,value);
        for (int i = 1; i < keys.Length; i++) 
            dict.Add(keys[i], new RedirectyKey(masterKey));
    }
    public object Retrive(int key)
    {
        var objt = dict[key];
        if (objt.GetType().Equals(typeof(RedirectyKey)))
            objt = dict[((RedirectyKey)objt).masterKey];
        return objt;
    }

    private struct RedirectyKey
    {
        public RedirectyKey(int key) => this.masterKey = key;
        public int masterKey { get; set; }
    }

}




public class MultiKeyDict2
{
    private Dictionary<int, Guid> redirectDict = new();
    private Dictionary<Guid, object> dict = new();

    public void Add(object value, params int[] keys)
    {
        var masterKey = Guid.NewGuid();
        dict.Add(masterKey, value);
        for (int i = 0; i < keys.Length; i++) 
            redirectDict.Add(keys[i], masterKey);
    }
    public object Retrive(int key)
    {
        return dict[redirectDict[key]];
    }
}



