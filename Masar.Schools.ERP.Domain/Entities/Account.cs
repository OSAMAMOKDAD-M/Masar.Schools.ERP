using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Account : BaseEntity
{
    public Guid AccountId { get; set; }
    public string AccountCode { get; set; } = string.Empty; // 1101, 1201, etc.
    public string AccountNameAr { get; set; } = string.Empty; // الصندوق والخصوم النقدية
    public string AccountNameEn { get; set; } = string.Empty; // Cash & Banks
    public AccountType AccountType { get; set; } // Asset, Liability, Revenue, Expense
    public Guid? ParentAccountId { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public int Level { get; set; } // For tree structure depth
    public string? Description { get; set; }

    // Navigation properties
    public Account? ParentAccount { get; set; }
    public ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}

public enum AccountType
{
    Asset = 1,           // الأصول
    Liability = 2,       // الالتزامات
    Equity = 3,          // حقوق الملكية
    Revenue = 4,         // الإيرادات
    Expense = 5          // المصروفات
}
