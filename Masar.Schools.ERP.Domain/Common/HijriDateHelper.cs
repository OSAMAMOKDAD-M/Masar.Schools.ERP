using System.Globalization;

namespace Masar.Schools.ERP.Domain.Common;

/// <summary>
/// مساعد للتقويم الهجري
/// </summary>
public static class HijriDateHelper
{
    private static readonly UmAlQuraCalendar _hijriCalendar = new UmAlQuraCalendar();

    /// <summary>
    /// تحويل التاريخ الميلادي إلى هجري
    /// </summary>
    public static string ToHijriString(DateTime date)
    {
        return $"{_hijriCalendar.GetYear(date)}-{_hijriCalendar.GetMonth(date):D2}-{_hijriCalendar.GetDayOfMonth(date):D2}";
    }

    /// <summary>
    /// تحويل التاريخ الميلادي إلى هجري بالعربية
    /// </summary>
    public static string ToHijriStringArabic(DateTime date)
    {
        var year = _hijriCalendar.GetYear(date);
        var month = _hijriCalendar.GetMonth(date);
        var day = _hijriCalendar.GetDayOfMonth(date);

        var arabicMonths = new[]
        {
            "محرم", "صفر", "ربيع الأول", "ربيع الآخر", "جمادى الأولى", "جمادى الآخرة",
            "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة"
        };

        return $"{day} {arabicMonths[month - 1]} {year} هـ";
    }

    /// <summary>
    /// تحويل التاريخ الهجري إلى ميلادي
    /// </summary>
    public static DateTime FromHijriString(string hijriDate)
    {
        var parts = hijriDate.Split('-');
        if (parts.Length != 3)
            throw new ArgumentException("تنسيق التاريخ الهجري غير صحيح");

        var year = int.Parse(parts[0]);
        var month = int.Parse(parts[1]);
        var day = int.Parse(parts[2]);

        return _hijriCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
    }

    /// <summary>
    /// الحصول على التاريخ الهجري الحالي
    /// </summary>
    public static DateTime GetCurrentHijriDate()
    {
        var today = DateTime.Now;
        return _hijriCalendar.ToDateTime(
            _hijriCalendar.GetYear(today),
            _hijriCalendar.GetMonth(today),
            _hijriCalendar.GetDayOfMonth(today),
            0, 0, 0, 0
        );
    }

    /// <summary>
    /// حساب عدد الأيام بين تاريخين بالتقويم الهجري
    /// </summary>
    public static int GetHijriDaysBetween(DateTime start, DateTime end)
    {
        var startDate = FromHijriString(ToHijriString(start));
        var endDate = FromHijriString(ToHijriString(end));
        return (endDate - startDate).Days;
    }

    /// <summary>
    /// الحصول على السنة الهجرية
    /// </summary>
    public static int GetHijriYear(DateTime date)
    {
        return _hijriCalendar.GetYear(date);
    }

    /// <summary>
    /// الحصول على الشهر الهجري
    /// </summary>
    public static int GetHijriMonth(DateTime date)
    {
        return _hijriCalendar.GetMonth(date);
    }

    /// <summary>
    /// الحصول على اسم الشهر الهجري بالعربية
    /// </summary>
    public static string GetHijriMonthNameArabic(DateTime date)
    {
        var month = _hijriCalendar.GetMonth(date);
        var arabicMonths = new[]
        {
            "محرم", "صفر", "ربيع الأول", "ربيع الآخر", "جمادى الأولى", "جمادى الآخرة",
            "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة"
        };
        return arabicMonths[month - 1];
    }

    /// <summary>
    /// الحصول على اسم الشهر الهجري بالإنجليزي
    /// </summary>
    public static string GetHijriMonthNameEnglish(DateTime date)
    {
        var month = _hijriCalendar.GetMonth(date);
        var englishMonths = new[]
        {
            "Muharram", "Safar", "Rabi al-Awwal", "Rabi al-Thani", "Jumada al-Awwal", "Jumada al-Thani",
            "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
        };
        return englishMonths[month - 1];
    }

    /// <summary>
    /// التحقق من أن التاريخ هو بداية السنة الهجرية
    /// </summary>
    public static bool IsHijriNewYear(DateTime date)
    {
        return _hijriCalendar.GetMonth(date) == 1 && _hijriCalendar.GetDayOfMonth(date) == 1;
    }

    /// <summary>
    /// الحصول على عدد الأيام في الشهر الهجري
    /// </summary>
    public static int GetDaysInHijriMonth(int year, int month)
    {
        if (month < 1 || month > 12)
            throw new ArgumentException("الشهر يجب أن يكون بين 1 و 12");

        if (month == 12)
        {
            // شهر ذو الحجة يمكن أن يكون 29 أو 30 يوم
            // للتبسيط، سنفترض 30 يوم
            return 30;
        }

        // الشهور الفردية في التقويم الهجري هي 30 يوم
        // الشهور الزوجية هي 29 يوم
        return month % 2 == 1 ? 30 : 29;
    }
}