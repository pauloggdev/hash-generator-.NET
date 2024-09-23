using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string textHash = "2024-06-29;2024-06-29T16:53:59;FR 2024/1;200.00;"; // A mensagem que você deseja assinar
        string privateKeyPem = @"MIICXAIBAAKBgQDWCvQ3GM1IPRC7cQlwdYHTbmqiOBklVzAciKSM+ph7Cg5leg6p
rgogxXlR8HR7LEzGKjNuYV5paamHvoKcpg4I9RHY8tMgBWgr4xKu3mI0rIlqxwqH
Br28+JObzQ8HK+HLytPMJaYVAYF2jZKllXlzPWuICMYXgy/+RhJgwHK7BQIDAQAB
AoGBAMj0u9jGxmUeQAlb1TrqeBtjvWXUOXefZiJEAAoEdQh/poiLkhyotAWUoZTW
puXF78bVdDgb3qIle+9gZAxisyTp2GL6c1jlN3/cUaGfAPAjp2l0NZ4n23Ec2fd7
QoQF0U4I2KAP1PZRfx7/PSftBE4nWs3DCHNrHX5o4m091GkBAkEA88xwDzcOicYX
ybHaPEn0CprWSLTAA4hZnNwVn4L8di76liictXduaSYLFTIMQl3OkH1hpb/0f/PI
cqQ5D3cp5QJBAODBSH3MkQ904+U/ln8eX0B8In7SUYUpWgmQ6aKs6/HyRl3n0VUe
G5JmyNC46AibmdfBVgBGrHaN2/seYTS9uqECQFH5Z6R2CrlglhcHai3jX99A+NQx
km6dpiQMDGk6DdFfMnrS5P5PThyk4g0aauzVxeLnhbHJvVhYjAmgFl+Q3dECQAqz
KPBUPNOvjOntDQ0gNQis4DeJa7gbL94kt/q2oMTz88Wks6KJvGZL3mORafp+7eQH
oECDHNLIDiD2YUpRfgECQHsEUgtBo2hyt4gdwI+yo9RCZ1TLFMKLDlp/puo16eNj
4Iw1WfMck1Pud1M7UaiXJtmWSPTLVXoQ8LQXbFcWNkw=";
        string signature = Execute(textHash, privateKeyPem);
        Console.WriteLine($"Assinatura em Base64: {signature}");
    }

    static string Execute(string textHash, string privateKeyPem)
    {
        // Carregar a chave privada
        using (RSA rsa = LoadPrivateKey(privateKeyPem))
        {
            // Criar um hash SHA-1 da mensagem
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(textHash));

                // Assinar digitalmente o hash com a chave privada
                byte[] signatureBytes = rsa.SignHash(hashBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

                // Converter a assinatura para Base64
                return Convert.ToBase64String(signatureBytes);
            }
        }
    }

    static RSA LoadPrivateKey(string privateKeyPem)
    {
        // Remover cabeçalho e rodapé da chave PEM
        string privateKey = privateKeyPem
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");

        // Converter a chave PEM para bytes
        byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

        // Criar um objeto RSA a partir da chave privada
        RSA rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

        return rsa;
    }
}
