using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Masar.Schools.ERP.Licensing.Models;

namespace Masar.Schools.ERP.Licensing.Services;

/// <summary>
/// محرك الترخيص المشفر
/// يستخدم AES-256-GCM لتشفير البيانات و RSA-4096 للتوقيع الرقمي
/// </summary>
public class LicenseEngine
{
    private const int AesKeySize = 256; // 32 bytes
    private const int AesNonceSize = 12; // 96 bits for GCM
    private const int RsaKeySize = 4096;

    /// <summary>
    /// توليد مفتاح ترخيص جديد (للسوبر أدمن فقط)
    /// </summary>
    /// <param name="license">بيانات الترخيص</param>
    /// <param name="privateKey">المفتاح الخاص RSA (فقط على سيرفر السوبر أدمن)</param>
    /// <returns>مفتاح الترخيص المشفر Base64</returns>
    public string GenerateLicenseKey(LicenseModel license, string privateKey)
    {
        try
        {
            // 1. تحويل بيانات الترخيص إلى JSON
            string licenseJson = JsonSerializer.Serialize(license);
            byte[] licenseBytes = Encoding.UTF8.GetBytes(licenseJson);

            // 2. توليد مفتاح AES عشوائي لهذا الترخيص
            byte[] aesKey = new byte[AesKeySize / 8];
            byte[] nonce = new byte[AesNonceSize];
            RandomNumberGenerator.Fill(aesKey);
            RandomNumberGenerator.Fill(nonce);

            // 3. تشفير بيانات الترخيص بـ AES-256-GCM
            byte[] encryptedData;
            byte[] tag;
            using (var aesGcm = new AesGcm(aesKey, AesGcm.TagByteSizes.MaxSize))
            {
                encryptedData = new byte[licenseBytes.Length];
                tag = new byte[AesGcm.TagByteSizes.MaxSize];
                aesGcm.Encrypt(nonce, licenseBytes, encryptedData, tag);
            }

            // 4. تجميع حزمة الترخيص: Nonce + Tag + EncryptedData + AES Key
            var licensePackage = new LicensePackage
            {
                Version = 1,
                Nonce = nonce,
                Tag = tag,
                EncryptedData = encryptedData,
                AesKey = aesKey,
                Timestamp = DateTime.UtcNow
            };

            // 5. تحويل الحزمة إلى JSON وتشفير المفتاح AES بـ RSA
            string packageJson = JsonSerializer.Serialize(licensePackage);
            byte[] packageBytes = Encoding.UTF8.GetBytes(packageJson);

            // تشفير مفتاح AES بالمفتاح الخاص RSA
            byte[] encryptedAesKey = EncryptWithRsa(aesKey, privateKey);

            // 6. التوقيع الرقمي للحزمة بالمفتاح الخاص RSA
            byte[] signature = SignWithRsa(packageBytes, privateKey);

            // 7. تجميع المفتاح النهائي: EncryptedAesKey + Signature + Package
            var finalPackage = new LicenseFinalPackage
            {
                EncryptedAesKey = encryptedAesKey,
                Signature = signature,
                PackageData = packageBytes
            };

            // 8. تحويل إلى Base64
            string finalJson = JsonSerializer.Serialize(finalPackage);
            byte[] finalBytes = Encoding.UTF8.GetBytes(finalJson);
            return Convert.ToBase64String(finalBytes);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("فشل توليد مفتاح الترخيص", ex);
        }
    }

    /// <summary>
    /// التحقق من صحة الترخيص (للعميل)
    /// </summary>
    /// <param name="licenseKey">مفتاح الترخيص المشفر</param>
    /// <param name="publicKey">المفتاح العام RSA (مدمج في التطبيق)</param>
    /// <param name="currentMachineId">بصمة الجهاز الحالية</param>
    /// <returns>نتيجة التحقق</returns>
    public LicenseValidationResult ValidateLicense(string licenseKey, string publicKey, string currentMachineId)
    {
        try
        {
            // 1. فك التشفير من Base64
            byte[] finalBytes = Convert.FromBase64String(licenseKey);
            string finalJson = Encoding.UTF8.GetString(finalBytes);
            var finalPackage = JsonSerializer.Deserialize<LicenseFinalPackage>(finalJson);

            if (finalPackage == null)
            {
                return LicenseValidationResult.Failure(LicenseStatus.Corrupted, "ملف الترخيص تالف");
            }

            // 2. التحقق من التوقيع الرقمي
            bool signatureValid = VerifyRsaSignature(finalPackage.PackageData, finalPackage.Signature, publicKey);
            if (!signatureValid)
            {
                return LicenseValidationResult.Failure(LicenseStatus.InvalidSignature, "التوقيع الرقمي غير صحيح - الملف تم التلاعب به");
            }

            // 3. فك تشفير مفتاح AES بالمفتاح العام RSA
            byte[] aesKey = DecryptWithRsa(finalPackage.EncryptedAesKey, publicKey);

            // 4. فك تشفير الحزمة
            string packageJson = Encoding.UTF8.GetString(finalPackage.PackageData);
            var licensePackage = JsonSerializer.Deserialize<LicensePackage>(packageJson);

            if (licensePackage == null)
            {
                return LicenseValidationResult.Failure(LicenseStatus.Corrupted, "حزمة الترخيص تالفة");
            }

            // 5. فك تشفير بيانات الترخيص بـ AES-256-GCM
            byte[] decryptedData;
            using (var aesGcm = new AesGcm(aesKey, AesGcm.TagByteSizes.MaxSize))
            {
                decryptedData = new byte[licensePackage.EncryptedData.Length];
                aesGcm.Decrypt(licensePackage.Nonce, licensePackage.EncryptedData, licensePackage.Tag, decryptedData);
            }

            // 6. تحويل بيانات الترخيص من JSON
            string licenseJson = Encoding.UTF8.GetString(decryptedData);
            var license = JsonSerializer.Deserialize<LicenseModel>(licenseJson);

            if (license == null)
            {
                return LicenseValidationResult.Failure(LicenseStatus.Corrupted, "بيانات الترخيص تالفة");
            }

            // 7. التحقق من تطابق بصمة الجهاز
            if (!string.Equals(license.MachineId, currentMachineId, StringComparison.OrdinalIgnoreCase))
            {
                return LicenseValidationResult.Failure(LicenseStatus.HardwareMismatch, 
                    $"بصمة الجهاز غير مطابقة. المتوقع: {license.MachineId}, الحالي: {currentMachineId}");
            }

            // 8. التحقق من تاريخ الانتهاء
            if (DateTime.UtcNow > license.ValidUntil)
            {
                return LicenseValidationResult.Failure(LicenseStatus.Expired, 
                    $"الترخيص منتهي في {license.ValidUntil:yyyy-MM-dd}");
            }

            // 9. التحقق من تاريخ البدء
            if (DateTime.UtcNow < license.ValidFrom)
            {
                return LicenseValidationResult.Failure(LicenseStatus.NotActivated, 
                    $"الترخيص يبدأ من {license.ValidFrom:yyyy-MM-dd}");
            }

            // 10. الترخيص صحيح
            return LicenseValidationResult.Success(license);
        }
        catch (CryptographicException ex)
        {
            return LicenseValidationResult.Failure(LicenseStatus.InvalidSignature, $"خطأ في فك التشفير: {ex.Message}");
        }
        catch (Exception ex)
        {
            return LicenseValidationResult.Failure(LicenseStatus.Corrupted, $"خطأ في قراءة الترخيص: {ex.Message}");
        }
    }

    /// <summary>
    /// توليد زوج مفاتيح RSA جديدة (للسوبر أدمن)
    /// </summary>
    /// <returns>زوج المفاتيح (Private, Public)</returns>
    public (string PrivateKey, string PublicKey) GenerateRsaKeyPair()
    {
        using var rsa = RSA.Create(RsaKeySize);
        
        // مفتاح خاص
        string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        
        // مفتاح عام
        string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        
        return (privateKey, publicKey);
    }

    /// <summary>
    /// تشفير البيانات بـ RSA
    /// </summary>
    private byte[] EncryptWithRsa(byte[] data, string privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// فك تشفير البيانات بـ RSA
    /// </summary>
    private byte[] DecryptWithRsa(byte[] encryptedData, string publicKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        return rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// التوقيع الرقمي بـ RSA
    /// </summary>
    private byte[] SignWithRsa(byte[] data, string privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// التحقق من التوقيع الرقمي
    /// </summary>
    private bool VerifyRsaSignature(byte[] data, byte[] signature, string publicKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}

/// <summary>
/// حزمة الترخيص الداخلية
/// </summary>
internal class LicensePackage
{
    public int Version { get; set; }
    public byte[] Nonce { get; set; } = Array.Empty<byte>();
    public byte[] Tag { get; set; } = Array.Empty<byte>();
    public byte[] EncryptedData { get; set; } = Array.Empty<byte>();
    public byte[] AesKey { get; set; } = Array.Empty<byte>();
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// الحزمة النهائية للترخيص
/// </summary>
internal class LicenseFinalPackage
{
    public byte[] EncryptedAesKey { get; set; } = Array.Empty<byte>();
    public byte[] Signature { get; set; } = Array.Empty<byte>();
    public byte[] PackageData { get; set; } = Array.Empty<byte>();
}