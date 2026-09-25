/*
 * ╔══════════════════════════════════════════════════════════════════════════════╗
 * ║                                                                              ║
 * ║                     نظام مَسَار للمدارس - Masar Schools ERP                   ║
 * ║                           DbInitializer - تهيئة قاعدة البيانات                   ║
 * ║                           نسخة شاملة - جميع البيانات التجريبية                  ║
 * ║                                                                              ║
 * ║                      جميع الحقوق محفوظة © 2026                               ║
 * ║                  شركة مسار للبرمجيات والتقنية - Masar Software               ║
 * ║                                                                              ║
 * ╚══════════════════════════════════════════════════════════════════════════════╝
 */

using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.Entities.Canteen;
using Masar.Schools.ERP.Domain.Entities.Admissions;
using Masar.Schools.ERP.Domain.Entities.Clinic;
using Masar.Schools.ERP.Domain.Entities.Alumni;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Masar.Schools.ERP.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MasarDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MasarUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<MasarRole>>();

        // التأكد من إنشاء قاعدة البيانات
        await context.Database.EnsureCreatedAsync();

        // التحقق من وجود البيانات قبل الإضافة
        if (await context.Tenants.AnyAsync())
        {
            // البيانات موجودة بالفعل، فقط نضمن وجود المستخدم admin@masar.com مع الصلاحيات
            await SeedMasarAdminUser(context, userManager, roleManager);
            return;
        }

        // إنشاء بيانات التجربة
        await SeedTenants(context);
        await SeedUsersAndRoles(context, userManager, roleManager);
        await SeedSchoolsAndBranches(context);
        await SeedGradeLevelsAndSections(context);
        await SeedClassRooms(context);
        await SeedStudentsAndGuardians(context);
        await SeedEmployees(context);
        await SeedHRData(context);
        await SeedAcademicTerms(context);
        await SeedFinancialPeriods(context);
        await SeedTransportData(context);
        await SeedClinicData(context);
        await SeedCanteenData(context);
        await SeedAdmissionData(context);
        await SeedAlumniData(context);
        await SeedFinancialData(context);
        await SeedInventoryData(context);
        await SeedProcurementData(context);
        await SeedChatData(context);
    }

    private static async Task SeedTenants(MasarDbContext context)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = "مدارس مَسَار الخاصة",
            NameArabic = "مدارس مَسَار الخاصة",
            LicenseNumber = "MASAR-2024-001",
            Email = "mokdadvip@hotmail.com",
            Phone = "01006765664",
            Address = "الرياض، المملكة العربية السعودية",
            City = "الرياض",
            IsActive = true,
            SubscriptionStartDate = DateTime.UtcNow,
            SubscriptionEndDate = DateTime.UtcNow.AddYears(1)
        };

        await context.Tenants.AddAsync(tenant);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAndRoles(MasarDbContext context, UserManager<MasarUser> userManager, RoleManager<MasarRole> roleManager)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        // إنشاء الأدوار
        var roles = new[]
        {
            new MasarRole { Id = Guid.NewGuid(), Name = "SuperAdmin", Description = "مدير النظام", DescriptionArabic = "مدير النظام", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "SchoolAdmin", Description = "مدير المدرسة", DescriptionArabic = "مدير المدرسة", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "Teacher", Description = "معلم", DescriptionArabic = "معلم", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "Accountant", Description = "محاسب", DescriptionArabic = "محاسب", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "Parent", Description = "ولي أمر", DescriptionArabic = "ولي أمر", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "CanteenStaff", Description = "موظف الكانتين", DescriptionArabic = "موظف الكانتين", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "TransportStaff", Description = "موظف النقل", DescriptionArabic = "موظف النقل", TenantId = tenant.Id, IsActive = true },
            new MasarRole { Id = Guid.NewGuid(), Name = "ClinicStaff", Description = "موظف العيادة", DescriptionArabic = "موظف العيادة", TenantId = tenant.Id, IsActive = true }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        // إنشاء المستخدمين
        var adminUser = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = "admin@masarschools.com",
            Email = "admin@masarschools.com",
            FullName = "أحمد محمد",
            FullNameArabic = "أحمد محمد",
            PhoneNumber = "01006765664",
            TenantId = tenant.Id,
            IsActive = true,
            IsSuperAdmin = true
        };

        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "SuperAdmin");

        // إنشاء مستخدمين إضافيين
        var teacherUser = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = "teacher@masarschools.com",
            Email = "teacher@masarschools.com",
            FullName = "محمد علي",
            FullNameArabic = "محمد علي",
            PhoneNumber = "01006765664",
            TenantId = tenant.Id,
            IsActive = true
        };

        await userManager.CreateAsync(teacherUser, "Teacher@123");
        await userManager.AddToRoleAsync(teacherUser, "Teacher");

        var accountantUser = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = "accountant@masarschools.com",
            Email = "accountant@masarschools.com",
            FullName = "سعيد أحمد",
            FullNameArabic = "سعيد أحمد",
            PhoneNumber = "0503456789",
            TenantId = tenant.Id,
            IsActive = true
        };

        await userManager.CreateAsync(accountantUser, "Accountant@123");
        await userManager.AddToRoleAsync(accountantUser, "Accountant");

        // إنشاء مستخدم admin@masar.com بكل الصلاحيات
        var masarAdminUser = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = "admin@masar.com",
            Email = "admin@masar.com",
            FullName = "مسار أدمن",
            FullNameArabic = "مسار أدمن",
            PhoneNumber = "01006765664",
            TenantId = tenant.Id,
            IsActive = true,
            IsSuperAdmin = true
        };

        await userManager.CreateAsync(masarAdminUser, "Masar@Admin123");
        await userManager.AddToRoleAsync(masarAdminUser, "SuperAdmin");
    }

    private static async Task SeedSchoolsAndBranches(MasarDbContext context)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        var school = new School
        {
            Id = Guid.NewGuid(),
            Name = "مدرسة مَسَار الخاصة - الرياض",
            NameArabic = "مدرسة مَسَار الخاصة - الرياض",
            Code = "MS-RYD-001",
            NoorSchoolId = "1234567890",
            Email = "riyadh@masarschools.com",
            Phone = "0112345678",
            Address = "حي الملقا، الرياض",
            City = "الرياض",
            IsActive = true,
            TenantId = tenant.Id
        };

        await context.Schools.AddAsync(school);
        await context.SaveChangesAsync();

        // إنشاء فروع
        var branches = new[]
        {
            new Branch
            {
                Id = Guid.NewGuid(),
                Name = "فرع الملقا",
                NameArabic = "فرع الملقا",
                Code = "BR-MLQ-001",
                Address = "حي الملقا، الرياض",
                Phone = "0112345679",
                SchoolId = school.Id,
                IsActive = true
            },
            new Branch
            {
                Id = Guid.NewGuid(),
                Name = "فرع النخيل",
                NameArabic = "فرع النخيل",
                Code = "BR-NKL-001",
                Address = "حي النخيل، الرياض",
                Phone = "0112345680",
                SchoolId = school.Id,
                IsActive = true
            }
        };

        await context.Branches.AddRangeAsync(branches);
        await context.SaveChangesAsync();
    }

    private static async Task SeedGradeLevelsAndSections(MasarDbContext context)
    {
        var gradeLevels = new[]
        {
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الأول الثانوي", NameArabic = "الصف الأول الثانوي", Code = "G1-SEC", DisplayOrder = 1, IsActive = true },
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الثاني الثانوي", NameArabic = "الصف الثاني الثانوي", Code = "G2-SEC", DisplayOrder = 2, IsActive = true },
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الثالث الثانوي", NameArabic = "الصف الثالث الثانوي", Code = "G3-SEC", DisplayOrder = 3, IsActive = true },
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الأول متوسط", NameArabic = "الصف الأول متوسط", Code = "G1-MID", DisplayOrder = 4, IsActive = true },
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الثاني متوسط", NameArabic = "الصف الثاني متوسط", Code = "G2-MID", DisplayOrder = 5, IsActive = true },
            new GradeLevel { Id = Guid.NewGuid(), Name = "الصف الثالث متوسط", NameArabic = "الصف الثالث متوسط", Code = "G3-MID", DisplayOrder = 6, IsActive = true }
        };

        await context.GradeLevels.AddRangeAsync(gradeLevels);
        await context.SaveChangesAsync();

        var sections = new[]
        {
            new Section { Id = Guid.NewGuid(), Name = "القسم العلمي", NameArabic = "القسم العلمي", Code = "SCI", DisplayOrder = 1, IsActive = true },
            new Section { Id = Guid.NewGuid(), Name = "القسم الأدبي", NameArabic = "القسم الأدبي", Code = "ART", DisplayOrder = 2, IsActive = true }
        };

        await context.Sections.AddRangeAsync(sections);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClassRooms(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var branch = await context.Branches.FirstOrDefaultAsync();
        if (branch == null) return;

        var gradeLevel = await context.GradeLevels.FirstOrDefaultAsync();
        if (gradeLevel == null) return;

        var section = await context.Sections.FirstOrDefaultAsync();
        if (section == null) return;

        var classRooms = new[]
        {
            new ClassRoom
            {
                Id = Guid.NewGuid(),
                Name = "فصل 101",
                NameArabic = "فصل 101",
                Code = "CR-101",
                GradeLevelId = gradeLevel.Id,
                SectionId = section.Id,
                Capacity = 30,
                CurrentCount = 0,
                SchoolId = school.Id,
                BranchId = branch.Id,
                IsActive = true
            },
            new ClassRoom
            {
                Id = Guid.NewGuid(),
                Name = "فصل 102",
                NameArabic = "فصل 102",
                Code = "CR-102",
                GradeLevelId = gradeLevel.Id,
                SectionId = section.Id,
                Capacity = 30,
                CurrentCount = 0,
                SchoolId = school.Id,
                BranchId = branch.Id,
                IsActive = true
            },
            new ClassRoom
            {
                Id = Guid.NewGuid(),
                Name = "فصل 201",
                NameArabic = "فصل 201",
                Code = "CR-201",
                GradeLevelId = gradeLevel.Id,
                SectionId = section.Id,
                Capacity = 30,
                CurrentCount = 0,
                SchoolId = school.Id,
                BranchId = branch.Id,
                IsActive = true
            }
        };

        await context.ClassRooms.AddRangeAsync(classRooms);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStudentsAndGuardians(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var branch = await context.Branches.FirstOrDefaultAsync();
        if (branch == null) return;

        var classRoom = await context.ClassRooms.FirstOrDefaultAsync();
        if (classRoom == null) return;

        // إنشاء أولياء الأمور
        var guardians = new[]
        {
            new Guardian
            {
                Id = Guid.NewGuid(),
                FirstName = "محمد",
                FirstNameArabic = "محمد",
                LastName = "عبدالله",
                LastNameArabic = "عبدالله",
                FullName = "محمد عبدالله",
                FullNameArabic = "محمد عبدالله",
                NationalId = "1234567890",
                PhoneNumber = "0509876543",
                WhatsAppNumber = "0509876543",
                Email = "guardian1@example.com",
                Address = "حي الملقا، الرياض",
                Relationship = "Father",
                RelationshipArabic = "الأب",
                Occupation = "مهندس",
                OccupationArabic = "مهندس",
                IsActive = true
            },
            new Guardian
            {
                Id = Guid.NewGuid(),
                FirstName = "أحمد",
                FirstNameArabic = "أحمد",
                LastName = "السعيد",
                LastNameArabic = "السعيد",
                FullName = "أحمد السعيد",
                FullNameArabic = "أحمد السعيد",
                NationalId = "2345678901",
                PhoneNumber = "0508765432",
                WhatsAppNumber = "0508765432",
                Email = "guardian2@example.com",
                Address = "حي النخيل، الرياض",
                Relationship = "Father",
                RelationshipArabic = "الأب",
                Occupation = "طبيب",
                OccupationArabic = "طبيب",
                IsActive = true
            }
        };

        await context.Guardians.AddRangeAsync(guardians);
        await context.SaveChangesAsync();

        // إنشاء طلاب
        var students = new[]
        {
            new Student
            {
                Id = Guid.NewGuid(),
                FirstName = "أحمد",
                FirstNameArabic = "أحمد",
                LastName = "محمد عبدالله",
                LastNameArabic = "محمد عبدالله",
                FullName = "أحمد محمد عبدالله",
                FullNameArabic = "أحمد محمد عبدالله",
                NationalId = "2234567890",
                BirthDate = new DateTime(2007, 5, 15),
                Gender = "Male",
                BloodType = "O+",
                PhoneNumber = "0508765432",
                Email = "student1@example.com",
                Address = "حي الملقا، الرياض",
                StudentNumber = "STU-2024-001",
                NoorStudentId = "9988776655",
                EnrollmentDate = DateTime.Now,
                IsActive = true,
                IsSaudiCitizen = true,
                SchoolId = school.Id,
                BranchId = branch.Id,
                ClassRoomId = classRoom.Id,
                GuardianId = guardians[0].Id
            },
            new Student
            {
                Id = Guid.NewGuid(),
                FirstName = "يوسف",
                FirstNameArabic = "يوسف",
                LastName = "أحمد السعيد",
                LastNameArabic = "أحمد السعيد",
                FullName = "يوسف أحمد السعيد",
                FullNameArabic = "يوسف أحمد السعيد",
                NationalId = "3345678901",
                BirthDate = new DateTime(2007, 8, 20),
                Gender = "Male",
                BloodType = "A+",
                PhoneNumber = "0507654321",
                Email = "student2@example.com",
                Address = "حي النخيل، الرياض",
                StudentNumber = "STU-2024-002",
                NoorStudentId = "9988776644",
                EnrollmentDate = DateTime.Now,
                IsActive = true,
                IsSaudiCitizen = true,
                SchoolId = school.Id,
                BranchId = branch.Id,
                ClassRoomId = classRoom.Id,
                GuardianId = guardians[1].Id
            },
            new Student
            {
                Id = Guid.NewGuid(),
                FirstName = "فاطمة",
                FirstNameArabic = "فاطمة",
                LastName = "محمد عبدالله",
                LastNameArabic = "محمد عبدالله",
                FullName = "فاطمة محمد عبدالله",
                FullNameArabic = "فاطمة محمد عبدالله",
                NationalId = "4456789012",
                BirthDate = new DateTime(2007, 3, 10),
                Gender = "Female",
                BloodType = "B+",
                PhoneNumber = "0506543210",
                Email = "student3@example.com",
                Address = "حي الملقا، الرياض",
                StudentNumber = "STU-2024-003",
                NoorStudentId = "9988776633",
                EnrollmentDate = DateTime.Now,
                IsActive = true,
                IsSaudiCitizen = true,
                SchoolId = school.Id,
                BranchId = branch.Id,
                ClassRoomId = classRoom.Id,
                GuardianId = guardians[0].Id
            }
        };

        await context.Students.AddRangeAsync(students);
        await context.SaveChangesAsync();
    }

    private static async Task SeedEmployees(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var branch = await context.Branches.FirstOrDefaultAsync();
        if (branch == null) return;

        var employees = new[]
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "خالد",
                FirstNameArabic = "خالد",
                LastName = "العتيبي",
                LastNameArabic = "العتيبي",
                FullName = "د. خالد العتيبي",
                FullNameArabic = "د. خالد العتيبي",
                NationalId = "1012345678",
                EmployeeNumber = "EMP-2024-001",
                JobTitle = "مدير المدرسة",
                JobTitleArabic = "مدير المدرسة",
                Department = "الإدارة",
                DepartmentArabic = "الإدارة",
                PhoneNumber = "0501234567",
                Email = "khaled@masarschools.com",
                HireDate = new DateTime(2015, 9, 1),
                Salary = 15000,
                Gender = "Male",
                GenderArabic = "ذكر",
                Nationality = "سعودي",
                NationalityArabic = "سعودي",
                ContractStart = new DateTime(2023, 9, 1),
                ContractEnd = new DateTime(2026, 8, 31),
                MaritalStatus = "Married",
                MaritalStatusArabic = "متزوج",
                BirthDate = new DateTime(1980, 3, 20),
                Education = "دكتوراه في الإدارة التعليمية",
                EducationArabic = "دكتوراه في الإدارة التعليمية",
                IsActive = true,
                SchoolId = school.Id,
                BranchId = branch.Id
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "فاطمة",
                FirstNameArabic = "فاطمة",
                LastName = "الشمري",
                LastNameArabic = "الشمري",
                FullName = "أ. فاطمة الشمري",
                FullNameArabic = "أ. فاطمة الشمري",
                NationalId = "1023456789",
                EmployeeNumber = "EMP-2024-002",
                JobTitle = "معلمة رياضيات",
                JobTitleArabic = "معلمة رياضيات",
                Department = "الأكاديمية",
                DepartmentArabic = "الأكاديمية",
                PhoneNumber = "0502345678",
                Email = "fatima@masarschools.com",
                HireDate = new DateTime(2018, 9, 1),
                Salary = 8000,
                Gender = "Female",
                GenderArabic = "أنثى",
                Nationality = "سعودية",
                NationalityArabic = "سعودية",
                ContractStart = new DateTime(2023, 9, 1),
                ContractEnd = new DateTime(2026, 8, 31),
                MaritalStatus = "Single",
                MaritalStatusArabic = "عزباء",
                BirthDate = new DateTime(1985, 7, 10),
                Education = "بكالوريوس في الرياضيات",
                EducationArabic = "بكالوريوس في الرياضيات",
                Specialty = "رياضيات",
                SpecialtyArabic = "رياضيات",
                IsActive = true,
                SchoolId = school.Id,
                BranchId = branch.Id
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "سعيد",
                FirstNameArabic = "سعيد",
                LastName = "الغامدي",
                LastNameArabic = "الغامدي",
                FullName = "سعيد الغامدي",
                FullNameArabic = "سعيد الغامدي",
                NationalId = "1034567890",
                EmployeeNumber = "EMP-2024-003",
                JobTitle = "محاسب",
                JobTitleArabic = "محاسب",
                Department = "المالية",
                DepartmentArabic = "المالية",
                PhoneNumber = "0503456789",
                Email = "saeed@masarschools.com",
                HireDate = new DateTime(2019, 9, 1),
                Salary = 7000,
                Gender = "Male",
                GenderArabic = "ذكر",
                Nationality = "سعودي",
                NationalityArabic = "سعودي",
                ContractStart = new DateTime(2023, 9, 1),
                ContractEnd = new DateTime(2026, 8, 31),
                MaritalStatus = "Married",
                MaritalStatusArabic = "متزوج",
                BirthDate = new DateTime(1982, 11, 5),
                Education = "بكالوريوس في المحاسبة",
                EducationArabic = "بكالوريوس في المحاسبة",
                IsActive = true,
                SchoolId = school.Id,
                BranchId = branch.Id
            }
        };

        await context.Employees.AddRangeAsync(employees);
        await context.SaveChangesAsync();
    }

    private static async Task SeedHRData(MasarDbContext context)
    {
        var employee = await context.Employees.FirstOrDefaultAsync();
        if (employee == null) return;

        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        // أنواع الإجازات
        var leaveTypes = new[]
        {
            new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "إجازة سنوية",
                NameArabic = "إجازة سنوية",
                DaysPerYear = 30,
                Note = "إجازة سنوية مدفوعة",
                IsAutomatic = true,
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 1
            },
            new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "إجازة مرضية",
                NameArabic = "إجازة مرضية",
                DaysPerYear = 120,
                Note = "إجازة مرضية مدفوعة",
                IsAutomatic = true,
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 2
            },
            new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "إجازة طارئة",
                NameArabic = "إجازة طارئة",
                DaysPerYear = 7,
                Note = "إجازة طارئة",
                IsAutomatic = false,
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 3
            }
        };

        await context.LeaveTypes.AddRangeAsync(leaveTypes);
        await context.SaveChangesAsync();

        // الجنسيات
        var nationalities = new[]
        {
            new Nationality
            {
                Id = Guid.NewGuid(),
                Name = "سعودي",
                NameArabic = "سعودي",
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 1
            },
            new Nationality
            {
                Id = Guid.NewGuid(),
                Name = "مصري",
                NameArabic = "مصري",
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 2
            },
            new Nationality
            {
                Id = Guid.NewGuid(),
                Name = "سوري",
                NameArabic = "سوري",
                SchoolId = school.Id,
                IsActive = true,
                SortOrder = 3
            }
        };

        await context.Nationalities.AddRangeAsync(nationalities);
        await context.SaveChangesAsync();

        // عقود الموظفين
        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            EmployeeId = employee.Id,
            ContractNumber = 1,
            StartDate = new DateTime(2023, 9, 1),
            EndDate = new DateTime(2026, 8, 31),
            Salary = 15000,
            JobTitle = "مدير المدرسة",
            JobTitleArabic = "مدير المدرسة",
            Department = "الإدارة",
            DepartmentArabic = "الإدارة",
            DurationYears = 3,
            DurationMonths = 36,
            DurationDays = 1095,
            Status = "ساري",
            StatusArabic = "ساري",
            IsCurrent = true,
            SchoolId = school.Id
        };

        await context.Contracts.AddRangeAsync(contract);
        await context.SaveChangesAsync();

        // ملف الرواتب
        var payrollProfile = new PayrollProfile
        {
            Id = Guid.NewGuid(),
            EmployeeId = employee.Id,
            EffectiveFrom = new DateTime(2024, 1, 1),
            PeriodStart = new DateTime(2024, 1, 1),
            PeriodEnd = new DateTime(2024, 1, 31),
            Basic = 15000,
            Housing = 3000,
            Transport = 1000,
            OtherEarnings = 500,
            VariableEarnings = 0,
            Deductions = 950,
            AbsenceDays = 0,
            LateDays = 0,
            LateDeduction = 0,
            GosiEnabled = true,
            GosiScheme = "auto",
            GosiCommission = 950,
            GosiInKindHousing = 0,
            GosiWage = 15000,
            GosiServiceDays = 30,
            GosiEmployeePensionRate = 0.09m,
            GosiEmployerPensionRate = 0.09m,
            GosiEmployeeSanedRate = 0.01m,
            GosiEmployerSanedRate = 0.01m,
            GosiOccupationalHazardRate = 0.02m,
            GosiEmployeeContribution = 950,
            GosiEmployerContribution = 1500,
            Approved = true,
            SchoolId = school.Id
        };

        await context.PayrollProfiles.AddRangeAsync(payrollProfile);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAcademicTerms(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var terms = new[]
        {
            new AcademicTerm
            {
                Id = Guid.NewGuid(),
                TermName = "الفصل الأول 1446",
                TermNameArabic = "الفصل الأول 1446",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 12, 31),
                SchoolId = school.Id,
                Status = "Active",
                StatusArabic = "نشط",
                IsActive = true
            },
            new AcademicTerm
            {
                Id = Guid.NewGuid(),
                TermName = "الفصل الثاني 1446",
                TermNameArabic = "الفصل الثاني 1446",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 5, 31),
                SchoolId = school.Id,
                Status = "Pending",
                StatusArabic = "قيد الانتظار",
                IsActive = false
            }
        };

        await context.AcademicTerms.AddRangeAsync(terms);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFinancialPeriods(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        var financialPeriod = new FinancialPeriod
        {
            Id = Guid.NewGuid(),
            PeriodName = "2026",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 12, 31),
            IsClosed = false,
            IsActive = true,
            TenantId = tenant.Id,
            SchoolId = school.Id
        };

        await context.FinancialPeriods.AddAsync(financialPeriod);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMasarAdminUser(MasarDbContext context, UserManager<MasarUser> userManager, RoleManager<MasarRole> roleManager)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        // Check if user already exists
        var existingUser = await userManager.FindByEmailAsync("admin@masar.com");
        if (existingUser != null)
        {
            // Add all permissions to the user
            var userPermissions = await context.Permissions.Where(p => p.IsActive).ToListAsync();
            var existingUserPermissions = await context.UserPermissions
                .Where(up => up.UserId == existingUser.Id)
                .ToListAsync();

            // Remove existing permissions
            context.UserPermissions.RemoveRange(existingUserPermissions);

            // Add all permissions
            foreach (var permission in userPermissions)
            {
                context.UserPermissions.Add(new UserPermission
                {
                    UserId = existingUser.Id,
                    PermissionId = permission.Id,
                    IsGranted = true
                });
            }

            await context.SaveChangesAsync();
            return;
        }

        // Create new user
        var adminUser = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = "admin@masar.com",
            Email = "admin@masar.com",
            FullName = "مسار أدمن",
            FullNameArabic = "مسار أدمن",
            PhoneNumber = "0509999999",
            TenantId = tenant.Id,
            IsActive = true,
            IsSuperAdmin = true
        };

        await userManager.CreateAsync(adminUser, "Masar@Admin123");
        await userManager.AddToRoleAsync(adminUser, "SuperAdmin");

        // Add all permissions to the user
        var allPermissions = await context.Permissions.Where(p => p.IsActive).ToListAsync();
        foreach (var permission in allPermissions)
        {
            context.UserPermissions.Add(new UserPermission
            {
                UserId = adminUser.Id,
                PermissionId = permission.Id,
                IsGranted = true
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedTransportData(MasarDbContext context)
    {
        var bus = new Bus
        {
            Id = Guid.NewGuid(),
            BusNumber = "BUS-001",
            PlateNumber = "أ ب ج 1234",
            Capacity = 45,
            Type = "كبيرة",
            Status = "Active",
            IsActive = true
        };

        await context.Buses.AddAsync(bus);
        await context.SaveChangesAsync();

        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            LicenseNumber = "123456789",
            LicenseExpiryDate = new DateTime(2025, 12, 31),
            LicenseType = "خفيفة",
            YearsOfExperience = 5,
            IsActive = true,
            AssignedBusId = bus.Id
        };

        await context.Drivers.AddAsync(driver);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClinicData(MasarDbContext context)
    {
        var student = await context.Students.FirstOrDefaultAsync();
        if (student == null) return;

        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var clinicVisit = new ClinicVisit
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            SchoolId = school.Id,
            VisitDate = DateTime.Now,
            VisitReason = "صداع",
            VisitReasonArabic = "صداع",
            PreliminaryDiagnosis = "صداع توتري",
            PreliminaryDiagnosisArabic = "صداع توتري",
            Treatment = "مسكن",
            TreatmentArabic = "مسكن",
            PatientCondition = "مستقر",
            PatientConditionArabic = "مستقر",
            ReturnedToClass = true
        };

        await context.ClinicVisits.AddAsync(clinicVisit);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCanteenData(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        // إنشاء فئة المنتجات أولاً
        var productCategory = new ProductCategory
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            NameAr = "طعام",
            NameEn = "Food",
            IsActive = true
        };

        await context.ProductCategories.AddAsync(productCategory);
        await context.SaveChangesAsync();

        var products = new[]
        {
            new CanteenProduct
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                ProductCategoryId = productCategory.Id,
                NameAr = "ساندوتش جبن",
                NameEn = "Cheese Sandwich",
                DescriptionAr = "ساندوتش جبن بالطماطم",
                BasePrice = 15.00m,
                Cost = 8.00m,
                StockQuantity = 50,
                IsAvailable = true
            },
            new CanteenProduct
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                ProductCategoryId = productCategory.Id,
                NameAr = "عصير برتقال",
                NameEn = "Orange Juice",
                DescriptionAr = "عصير برتقال طبيعي",
                BasePrice = 8.00m,
                Cost = 4.00m,
                StockQuantity = 100,
                IsAvailable = true
            }
        };

        await context.CanteenProducts.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdmissionData(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var gradeLevel = await context.GradeLevels.FirstOrDefaultAsync();
        if (gradeLevel == null) return;

        var application = new AdmissionApplication
        {
            Id = Guid.NewGuid(),
            ApplicationNumber = "ADM-2024-001",
            ApplicationType = ApplicationType.NewStudent,
            Status = AdmissionStatus.New,
            AcademicYear = "1446",
            GradeLevelId = gradeLevel.Id,
            FirstName = "يوسف",
            MiddleName = "أحمد",
            LastName = "محمد",
            FullNameArabic = "يوسف أحمد محمد",
            FullNameEnglish = "Yousef Ahmed Mohammed",
            BirthDate = new DateTime(2012, 8, 15),
            Gender = "Male",
            Nationality = "سعودي",
            NationalId = "1234567890",
            IsSaudiCitizen = true,
            City = "الرياض",
            PhoneNumber = "0505678901",
            Email = "parent@example.com",
            GuardianFirstName = "أحمد",
            GuardianLastName = "محمد",
            GuardianFullNameArabic = "أحمد محمد",
            Relationship = "Father",
            RelationshipArabic = "الأب",
            GuardianNationalId = "2345678901",
            GuardianPhoneNumber = "0505678901",
            GuardianWhatsAppNumber = "0505678901",
            GuardianEmail = "parent@example.com",
            SchoolId = school.Id
        };

        await context.AdmissionApplications.AddAsync(application);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAlumniData(MasarDbContext context)
    {
        var school = await context.Schools.FirstOrDefaultAsync();
        if (school == null) return;

        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        var alumni = new AlumniRecord
        {
            Id = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            GraduationDate = new DateTime(2023, 6, 30),
            GraduationYear = 2023,
            FinalGPA = 4.5m,
            GradeLevel = "الصف الثالث الثانوي",
            GradeLevelArabic = "الصف الثالث الثانوي",
            Section = "القسم العلمي",
            SectionArabic = "القسم العلمي",
            University = "جامعة الملك سعود",
            UniversityArabic = "جامعة الملك سعود",
            Major = "هندسة",
            MajorArabic = "هندسة",
            EmploymentStatus = "Employed",
            EmploymentStatusArabic = "يعمل",
            CompanyName = "شركة أرامكو",
            CompanyNameArabic = "شركة أرامكو",
            JobTitle = "مهندس برمجيات",
            JobTitleArabic = "مهندس برمجيات",
            Industry = "النفط والغاز",
            IndustryArabic = "النفط والغاز",
            PersonalEmail = "alumni@example.com",
            PersonalPhone = "0501234567",
            IsActive = true,
            TenantId = tenant.Id,
            SchoolId = school.Id
        };

        await context.AlumniRecords.AddAsync(alumni);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFinancialData(MasarDbContext context)
    {
        var student = await context.Students.FirstOrDefaultAsync();
        if (student == null) return;

        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = "INV-2024-001",
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now.AddMonths(1),
            Status = "Sent",
            Subtotal = 25000,
            TaxAmount = 0,
            DiscountAmount = 0,
            TotalAmount = 25000,
            Currency = "SAR",
            TenantId = tenant.Id,
            StudentId = student.Id
        };

        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();
    }

    private static async Task SeedInventoryData(MasarDbContext context)
    {
        var items = new[]
        {
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "أقلام رصاص",
                NameArabic = "أقلام رصاص",
                SKU = "INV-001",
                Category = "مكتبية",
                CategoryArabic = "مكتبية",
                UnitOfMeasure = "قطعة",
                UnitOfMeasureArabic = "قطعة",
                ReorderLevel = 50,
                MaxStockLevel = 500,
                ReorderQuantity = 100,
                AverageCost = 2.50m,
                SellingPrice = 5.00m,
                Currency = "SAR",
                IsActive = true
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "دفاتر",
                NameArabic = "دفاتر",
                SKU = "INV-002",
                Category = "مكتبية",
                CategoryArabic = "مكتبية",
                UnitOfMeasure = "قطعة",
                UnitOfMeasureArabic = "قطعة",
                ReorderLevel = 30,
                MaxStockLevel = 300,
                ReorderQuantity = 50,
                AverageCost = 5.00m,
                SellingPrice = 10.00m,
                Currency = "SAR",
                IsActive = true
            }
        };

        await context.InventoryItems.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProcurementData(MasarDbContext context)
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = "شركة الأفق للمستلزمات المدرسية",
            NameArabic = "شركة الأفق للمستلزمات المدرسية",
            Code = "SUP-001",
            ContactPerson = "محمد علي",
            Phone = "0506789012",
            Email = "horizon@example.com",
            SupplierCategory = "Preferred",
            SupplierCategoryArabic = "مفضل",
            PaymentTerms = "Net 30",
            Currency = "SAR",
            IsActive = true
        };

        await context.Suppliers.AddAsync(supplier);
        await context.SaveChangesAsync();

        var request = new PurchaseRequest
        {
            Id = Guid.NewGuid(),
            RequestNumber = "PR-2024-001",
            SchoolId = Guid.NewGuid(),
            BudgetAmount = 5000,
            RequestDate = DateTime.Now,
            RequiredBy = DateTime.Now.AddDays(30),
            Status = "Pending",
            StatusArabic = "قيد المراجعة",
            Currency = "SAR"
        };

        await context.PurchaseRequests.AddAsync(request);
        await context.SaveChangesAsync();
    }

    private static async Task SeedChatData(MasarDbContext context)
    {
        var users = await context.Users.Take(2).ToListAsync();
        if (users.Count < 2) return;

        var tenant = await context.Tenants.FirstOrDefaultAsync();
        if (tenant == null) return;

        var chatRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = "قسم الرياضيات",
            NameArabic = "قسم الرياضيات",
            Type = "Department",
            TenantId = tenant.Id,
            IsActive = true
        };

        await context.ChatRooms.AddAsync(chatRoom);
        await context.SaveChangesAsync();

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            SenderId = users[0].Id,
            ReceiverId = users[1].Id,
            Content = "مرحباً بالجميع في نظام مَسَار",
            MessageType = "Text",
            IsRead = true
        };

        await context.ChatMessages.AddAsync(message);
        await context.SaveChangesAsync();
    }
}
