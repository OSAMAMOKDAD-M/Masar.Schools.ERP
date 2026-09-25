using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Masar.Schools.ERP.Licensing.Services;

/// <summary>
/// خدمة توليد بصمة الجهاز الفريدة (Hardware Fingerprint)
/// تستخدم خصائص العتاد الثابتة لإنشاء معرف فريد للجهاز
/// </summary>
public class HardwareFingerprintService
{
    /// <summary>
    /// توليد بصمة الجهاز الفريدة
    /// </summary>
    public string GenerateMachineId()
    {
        try
        {
            var fingerprintBuilder = new StringBuilder();

            // 1. معرف المعالج (Processor ID)
            string processorId = GetProcessorId();
            fingerprintBuilder.Append(processorId);

            // 2. الرقم التسلسلي للوحة الأم (Motherboard Serial)
            string motherboardSerial = GetMotherboardSerial();
            fingerprintBuilder.Append(motherboardSerial);

            // 3. عنوان MAC الرئيسي (Primary MAC Address)
            string macAddress = GetPrimaryMacAddress();
            fingerprintBuilder.Append(macAddress);

            // 4. معرف القرص الرئيسي (Disk UUID/Serial)
            string diskId = GetDiskId();
            fingerprintBuilder.Append(diskId);

            // 5. معرف نظام التشغيل (OS Product ID)
            string osProductId = GetOsProductId();
            fingerprintBuilder.Append(osProductId);

            // توليد Hash SHA-256 للبصمة النهائية
            string rawFingerprint = fingerprintBuilder.ToString();
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawFingerprint));

            // تحويل Hash إلى سلسلة Hex
            string machineId = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            
            return machineId;
        }
        catch (Exception ex)
        {
            // في حالة الفشل، استخدام بديل يعتمد على خصائص النظام
            return GenerateFallbackMachineId(ex);
        }
    }

    /// <summary>
    /// الحصول على معرف المعالج
    /// </summary>
    private string GetProcessorId()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["ProcessorId"]?.ToString() ?? "UNKNOWN_PROCESSOR";
            }
        }
        catch
        {
            // Fallback للأنظمة غير Windows
            return Environment.ProcessorCount.ToString();
        }
        return "UNKNOWN_PROCESSOR";
    }

    /// <summary>
    /// الحصول على الرقم التسلسلي للوحة الأم
    /// </summary>
    private string GetMotherboardSerial()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"]?.ToString() ?? "UNKNOWN_MOTHERBOARD";
            }
        }
        catch
        {
            return "UNKNOWN_MOTHERBOARD";
        }
        return "UNKNOWN_MOTHERBOARD";
    }

    /// <summary>
    /// الحصول على عنوان MAC الرئيسي
    /// </summary>
    private string GetPrimaryMacAddress()
    {
        try
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // تجاهل الواجهات الافتراضية وغير النشطة
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up &&
                    !nic.Description.Contains("Virtual", StringComparison.OrdinalIgnoreCase))
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            
            // Fallback لأول واجهة نشطة
            var firstActive = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up);
            
            return firstActive?.GetPhysicalAddress().ToString() ?? "UNKNOWN_MAC";
        }
        catch
        {
            return "UNKNOWN_MAC";
        }
    }

    /// <summary>
    /// الحصول على معرف القرص الرئيسي
    /// </summary>
    private string GetDiskId()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT SerialNumber, Model FROM Win32_DiskDrive WHERE Index=0");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"]?.ToString() ?? obj["Model"]?.ToString() ?? "UNKNOWN_DISK";
            }
        }
        catch
        {
            return "UNKNOWN_DISK";
        }
        return "UNKNOWN_DISK";
    }

    /// <summary>
    /// الحصول على معرف نظام التشغيل
    /// </summary>
    private string GetOsProductId()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"]?.ToString() ?? Environment.OSVersion.Version.ToString();
            }
        }
        catch
        {
            return Environment.OSVersion.Version.ToString();
        }
        return Environment.OSVersion.Version.ToString();
    }

    /// <summary>
    /// توليد بصمة بديلة في حالة فشل WMI
    /// </summary>
    private string GenerateFallbackMachineId(Exception originalException)
    {
        try
        {
            var fallbackBuilder = new StringBuilder();
            
            // استخدام خصائص النظام الأساسية
            fallbackBuilder.Append(Environment.MachineName);
            fallbackBuilder.Append(Environment.ProcessorCount);
            fallbackBuilder.Append(Environment.OSVersion.VersionString);
            fallbackBuilder.Append(Environment.UserName);
            
            // إضافة بعض خصائص النظام الأخرى
            fallbackBuilder.Append(Environment.SystemDirectory);
            fallbackBuilder.Append(Environment.Is64BitOperatingSystem);
            
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fallbackBuilder.ToString()));
            
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }
        catch
        {
            // الفشل الكامل - إرجاع معرف عشوائي
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }

    /// <summary>
    /// الحصول على معلومات تفصيلية عن الجهاز
    /// </summary>
    public HardwareInfo GetHardwareInfo()
    {
        return new HardwareInfo
        {
            MachineId = GenerateMachineId(),
            ProcessorId = GetProcessorId(),
            MotherboardSerial = GetMotherboardSerial(),
            MacAddress = GetPrimaryMacAddress(),
            DiskId = GetDiskId(),
            OsProductId = GetOsProductId(),
            MachineName = Environment.MachineName,
            OsVersion = Environment.OSVersion.VersionString,
            Is64Bit = Environment.Is64BitOperatingSystem,
            ProcessorCount = Environment.ProcessorCount
        };
    }
}

/// <summary>
/// معلومات العتاد التفصيلية
/// </summary>
public class HardwareInfo
{
    public string MachineId { get; set; } = string.Empty;
    public string ProcessorId { get; set; } = string.Empty;
    public string MotherboardSerial { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string DiskId { get; set; } = string.Empty;
    public string OsProductId { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public bool Is64Bit { get; set; }
    public int ProcessorCount { get; set; }
}