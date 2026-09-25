using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Services;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ClosedXML.Excel;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class StudentsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<StudentsController> _logger;
    private readonly AttendanceAnalyticsService _attendanceAnalyticsService;
    private readonly IDocumentService _documentService;

    public StudentsController(MasarDbContext context, ILogger<StudentsController> logger, 
        AttendanceAnalyticsService attendanceAnalyticsService, IDocumentService documentService)
    {
        _context = context;
        _logger = logger;
        _attendanceAnalyticsService = attendanceAnalyticsService;
        _documentService = documentService;
    }

    // GET: /Students/Index
    public async Task<IActionResult> Index(StudentFilterDto filter)
    {
        var query = _context.Students
            .Include(s => s.School)
            .Include(s => s.Branch)
            .Include(s => s.ClassRoom)
            .Include(s => s.Guardian)
            .Where(s => !s.IsDeleted);

        // Apply filters
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query = query.Where(s =>
                s.FullName.Contains(filter.SearchTerm) ||
                s.FullNameArabic.Contains(filter.SearchTerm) ||
                s.StudentNumber.Contains(filter.SearchTerm) ||
                s.NationalId.Contains(filter.SearchTerm));
        }

        if (filter.SchoolId.HasValue)
        {
            query = query.Where(s => s.SchoolId == filter.SchoolId.Value);
        }

        if (filter.BranchId.HasValue)
        {
            query = query.Where(s => s.BranchId == filter.BranchId.Value);
        }

        if (filter.ClassRoomId.HasValue)
        {
            query = query.Where(s => s.ClassRoomId == filter.ClassRoomId.Value);
        }

        if (filter.GradeLevel.HasValue)
        {
            query = query.Where(s => s.ClassRoom != null && s.ClassRoom.GradeLevelLegacy == filter.GradeLevel.Value.ToString());
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == filter.IsActive.Value);
        }

        var students = await query
            .OrderBy(s => s.FullNameArabic)
            .ToListAsync();

        // Populate filter dropdowns
        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic");

        return View(new StudentIndexViewModel
        {
            Students = students,
            Filter = filter
        });
    }

    // GET: /Students/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Guardians = new SelectList(await _context.Guardians.Where(g => !g.IsDeleted).ToListAsync(), "Id", "FullNameArabic");

        return View();
    }

    // POST: /Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.Guardians = new SelectList(await _context.Guardians.Where(g => !g.IsDeleted).ToListAsync(), "Id", "FullNameArabic");
            return View(model);
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            FirstNameArabic = model.FirstNameArabic,
            LastName = model.LastName,
            LastNameArabic = model.LastNameArabic,
            FullName = $"{model.FirstName} {model.LastName}",
            FullNameArabic = $"{model.FirstNameArabic} {model.LastNameArabic}",
            NationalId = model.NationalId,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            BloodType = model.BloodType,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Address = model.Address,
            ProfileImagePath = model.ProfileImagePath,
            StudentNumber = await GenerateStudentNumber(),
            NoorStudentId = model.NoorStudentId,
            EnrollmentDate = DateTime.Now,
            IsActive = true,
            EmergencyContactName = model.EmergencyContactName,
            EmergencyContactPhone = model.EmergencyContactPhone,
            MedicalNotes = model.MedicalNotes,
            
            // الحقول الإضافية الجديدة
            PreviousSchool = model.PreviousSchool,
            PreviousSchoolArabic = model.PreviousSchoolArabic,
            TransferDate = model.TransferDate,
            TransferReason = model.TransferReason,
            AcademicStanding = model.AcademicStanding,
            SpecialNeeds = model.SpecialNeeds,
            SpecialNeedsArabic = model.SpecialNeedsArabic,
            IsGifted = model.IsGifted,
            GiftedProgram = model.GiftedProgram,
            LastMedicalCheckup = model.LastMedicalCheckup,
            Allergies = model.Allergies,
            AllergiesArabic = model.AllergiesArabic,
            DietaryRestrictions = model.DietaryRestrictions,
            DietaryRestrictionsArabic = model.DietaryRestrictionsArabic,
            
            SchoolId = model.SchoolId,
            BranchId = model.BranchId,
            ClassRoomId = model.ClassRoomId,
            GuardianId = model.GuardianId,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إضافة الطالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.Branch)
            .Include(s => s.ClassRoom)
            .Include(s => s.Guardian)
            .Include(s => s.AttendanceRecords.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.ClassRoom)
            .Include(s => s.Grades.Where(g => !g.IsDeleted))
                .ThenInclude(g => g.Teacher)
            .FirstOrDefaultAsync(s => s.Id == id.Value && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: /Students/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students.FindAsync(id.Value);
        if (student == null)
        {
            return NotFound();
        }

        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Guardians = new SelectList(await _context.Guardians.Where(g => !g.IsDeleted).ToListAsync(), "Id", "FullNameArabic");

        var model = new StudentEditDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            FirstNameArabic = student.FirstNameArabic,
            LastName = student.LastName,
            LastNameArabic = student.LastNameArabic,
            NationalId = student.NationalId,
            BirthDate = student.BirthDate,
            Gender = student.Gender,
            BloodType = student.BloodType,
            PhoneNumber = student.PhoneNumber,
            Email = student.Email,
            Address = student.Address,
            ProfileImagePath = student.ProfileImagePath,
            NoorStudentId = student.NoorStudentId,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactPhone = student.EmergencyContactPhone,
            MedicalNotes = student.MedicalNotes,
            
            // الحقول الإضافية الجديدة
            PreviousSchool = student.PreviousSchool,
            PreviousSchoolArabic = student.PreviousSchoolArabic,
            TransferDate = student.TransferDate,
            TransferReason = student.TransferReason,
            AcademicStanding = student.AcademicStanding,
            SpecialNeeds = student.SpecialNeeds,
            SpecialNeedsArabic = student.SpecialNeedsArabic,
            IsGifted = student.IsGifted,
            GiftedProgram = student.GiftedProgram,
            LastMedicalCheckup = student.LastMedicalCheckup,
            Allergies = student.Allergies,
            AllergiesArabic = student.AllergiesArabic,
            DietaryRestrictions = student.DietaryRestrictions,
            DietaryRestrictionsArabic = student.DietaryRestrictionsArabic,
            
            SchoolId = student.SchoolId,
            BranchId = student.BranchId,
            ClassRoomId = student.ClassRoomId,
            GuardianId = student.GuardianId,
            IsActive = student.IsActive
        };

        return View(model);
    }

    // POST: /Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid? id, StudentEditDto model)
    {
        if (id == null || id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.Guardians = new SelectList(await _context.Guardians.Where(g => !g.IsDeleted).ToListAsync(), "Id", "FullNameArabic");
            return View(model);
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        student.FirstName = model.FirstName;
        student.FirstNameArabic = model.FirstNameArabic;
        student.LastName = model.LastName;
        student.LastNameArabic = model.LastNameArabic;
        student.FullName = $"{model.FirstName} {model.LastName}";
        student.FullNameArabic = $"{model.FirstNameArabic} {model.LastNameArabic}";
        student.NationalId = model.NationalId;
        student.BirthDate = model.BirthDate;
        student.Gender = model.Gender;
        student.BloodType = model.BloodType;
        student.PhoneNumber = model.PhoneNumber;
        student.Email = model.Email;
        student.Address = model.Address;
        student.ProfileImagePath = model.ProfileImagePath;
        student.NoorStudentId = model.NoorStudentId;
        student.EmergencyContactName = model.EmergencyContactName;
        student.EmergencyContactPhone = model.EmergencyContactPhone;
        student.MedicalNotes = model.MedicalNotes;
        
        // الحقول الإضافية الجديدة
        student.PreviousSchool = model.PreviousSchool;
        student.PreviousSchoolArabic = model.PreviousSchoolArabic;
        student.TransferDate = model.TransferDate;
        student.TransferReason = model.TransferReason;
        student.AcademicStanding = model.AcademicStanding;
        student.SpecialNeeds = model.SpecialNeeds;
        student.SpecialNeedsArabic = model.SpecialNeedsArabic;
        student.IsGifted = model.IsGifted;
        student.GiftedProgram = model.GiftedProgram;
        student.LastMedicalCheckup = model.LastMedicalCheckup;
        student.Allergies = model.Allergies;
        student.AllergiesArabic = model.AllergiesArabic;
        student.DietaryRestrictions = model.DietaryRestrictions;
        student.DietaryRestrictionsArabic = model.DietaryRestrictionsArabic;
        
        student.SchoolId = model.SchoolId;
        student.BranchId = model.BranchId;
        student.ClassRoomId = model.ClassRoomId;
        student.GuardianId = model.GuardianId;
        student.IsActive = model.IsActive;
        student.UpdatedAt = DateTime.Now;
        student.UpdatedBy = User.Identity?.Name;

        _context.Update(student);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم تحديث بيانات الطالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/DigitalCard/5
    public async Task<IActionResult> DigitalCard(Guid? id)
    {
        if (id == null)
        {
            // If no ID provided, get first available student
            var firstStudent = await _context.Students
                .Where(s => !s.IsDeleted && s.IsActive)
                .FirstOrDefaultAsync();
            
            if (firstStudent != null)
            {
                return RedirectToAction(nameof(DigitalCard), new { id = firstStudent.Id });
            }
            
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .FirstOrDefaultAsync(s => s.Id == id.Value && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        // Generate QR Code
        ViewBag.QRCodeUrl = Url.Action("GenerateQRCode", "Students", new { id = student.Id });

        return View(student);
    }

    // GET: /Students/GenerateQRCode/5
    public IActionResult GenerateQRCode(Guid id)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        // Generate QR Code from student number
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(student.StudentNumber, QRCodeGenerator.ECCLevel.Q);

        using (QRCode qrCode = new QRCode(qrCodeData))
        {
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                // Convert to base64
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return File(imageBytes, "image/png");
                }
            }
        }
    }

    // GET: /Students/ExportToExcel
    public async Task<IActionResult> ExportToExcel(StudentFilterDto filter)
    {
        var query = _context.Students
            .Include(s => s.School)
            .Include(s => s.Branch)
            .Include(s => s.ClassRoom)
            .Include(s => s.Guardian)
            .Where(s => !s.IsDeleted);

        // Apply filters
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query = query.Where(s =>
                s.FullName.Contains(filter.SearchTerm) ||
                s.FullNameArabic.Contains(filter.SearchTerm) ||
                s.StudentNumber.Contains(filter.SearchTerm) ||
                s.NationalId.Contains(filter.SearchTerm));
        }

        if (filter.SchoolId.HasValue)
        {
            query = query.Where(s => s.SchoolId == filter.SchoolId.Value);
        }

        if (filter.BranchId.HasValue)
        {
            query = query.Where(s => s.BranchId == filter.BranchId.Value);
        }

        if (filter.ClassRoomId.HasValue)
        {
            query = query.Where(s => s.ClassRoomId == filter.ClassRoomId.Value);
        }

        var students = await query
            .OrderBy(s => s.FullNameArabic)
            .ToListAsync();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("الطلاب");

            // Headers
            worksheet.Cell("A1").Value = "رقم الطالب";
            worksheet.Cell("B1").Value = "الاسم (عربي)";
            worksheet.Cell("C1").Value = "الاسم (إنجليزي)";
            worksheet.Cell("D1").Value = "رقم الهوية";
            worksheet.Cell("E1").Value = "تاريخ الميلاد";
            worksheet.Cell("F1").Value = "الجنس";
            worksheet.Cell("G1").Value = "الفصل";
            worksheet.Cell("H1").Value = "المدرسة";
            worksheet.Cell("I1").Value = "ولي الأمر";
            worksheet.Cell("J1").Value = "رقم الهاتف";
            worksheet.Cell("K1").Value = "العنوان";
            worksheet.Cell("L1").Value = "الحالة";

            // Data
            int row = 2;
            foreach (var student in students)
            {
                worksheet.Cell(row, 1).Value = student.StudentNumber;
                worksheet.Cell(row, 2).Value = student.FullNameArabic;
                worksheet.Cell(row, 3).Value = student.FullName;
                worksheet.Cell(row, 4).Value = student.NationalId;
                worksheet.Cell(row, 5).Value = student.BirthDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 6).Value = student.Gender == "Male" ? "ذكر" : "أنثى";
                worksheet.Cell(row, 7).Value = student.ClassRoom?.NameArabic;
                worksheet.Cell(row, 8).Value = student.School?.NameArabic;
                worksheet.Cell(row, 9).Value = student.Guardian?.FullNameArabic;
                worksheet.Cell(row, 10).Value = student.PhoneNumber;
                worksheet.Cell(row, 11).Value = student.Address;
                worksheet.Cell(row, 12).Value = student.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            // Style headers
            var range = worksheet.Range(1, 1, 1, 12);
            range.Style.Fill.BackgroundColor = XLColor.LightBlue;
            range.Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students_Export.xlsx");
            }
        }
    }

    // GET: /Students/ImportFromExcel
    public IActionResult ImportFromExcel()
    {
        return View();
    }

    // POST: /Students/ImportFromExcel
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportFromExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "يرجى اختيار ملف Excel");
            return View();
        }

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

                    int importedCount = 0;
                    foreach (var row in rows)
                    {
                        try
                        {
                            var studentNumber = row.Cell(1).GetString();
                            var fullNameArabic = row.Cell(2).GetString();
                            var fullName = row.Cell(3).GetString();
                            var nationalId = row.Cell(4).GetString();
                            var birthDate = row.Cell(5).GetDateTime();
                            var gender = row.Cell(6).GetString();
                            var classroomName = row.Cell(7).GetString();
                            var schoolName = row.Cell(8).GetString();
                            var guardianName = row.Cell(9).GetString();
                            var phoneNumber = row.Cell(10).GetString();
                            var address = row.Cell(11).GetString();
                            var isActive = row.Cell(12).GetString() == "نشط";

                            // Find or create school
                            var school = await _context.Schools
                                .FirstOrDefaultAsync(s => s.NameArabic == schoolName);

                            if (school == null)
                            {
                                school = new School
                                {
                                    Id = Guid.NewGuid(),
                                    NameArabic = schoolName,
                                    Name = schoolName,
                                    IsActive = true,
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.Schools.Add(school);
                            }

                            // Find classroom
                            var classroom = await _context.ClassRooms
                                .FirstOrDefaultAsync(c => c.NameArabic == classroomName);

                            // Find guardian
                            Guardian? guardian = null;
                            if (!string.IsNullOrEmpty(guardianName))
                            {
                                guardian = await _context.Guardians
                                    .FirstOrDefaultAsync(g => g.FullNameArabic == guardianName);
                            }

                            var student = new Student
                            {
                                Id = Guid.NewGuid(),
                                StudentNumber = studentNumber,
                                FullNameArabic = fullNameArabic,
                                FullName = fullName,
                                NationalId = nationalId,
                                BirthDate = birthDate,
                                Gender = gender == "ذكر" ? "Male" : "Female",
                                SchoolId = school.Id,
                                ClassRoomId = classroom?.Id,
                                GuardianId = guardian?.Id,
                                PhoneNumber = phoneNumber,
                                Address = address,
                                IsActive = isActive,
                                EnrollmentDate = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = User.Identity?.Name
                            };

                            _context.Students.Add(student);
                            importedCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error importing row {Row}", row.RowNumber());
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"تم استيراد {importedCount} طالب بنجاح";
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing students from Excel");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء استيراد الملف");
            return View();
        }
    }

    // GET: /Students/DownloadTemplate
    public IActionResult DownloadTemplate()
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("قالب الاستيراد");

            // Headers
            worksheet.Cell("A1").Value = "رقم الطالب*";
            worksheet.Cell("B1").Value = "الاسم (عربي)*";
            worksheet.Cell("C1").Value = "الاسم (إنجليزي)*";
            worksheet.Cell("D1").Value = "رقم الهوية*";
            worksheet.Cell("E1").Value = "تاريخ الميلاد (yyyy-MM-dd)*";
            worksheet.Cell("F1").Value = "الجنس (ذكر/أنثى)*";
            worksheet.Cell("G1").Value = "الفصل";
            worksheet.Cell("H1").Value = "المدرسة*";
            worksheet.Cell("I1").Value = "ولي الأمر";
            worksheet.Cell("J1").Value = "رقم الهاتف";
            worksheet.Cell("K1").Value = "العنوان";
            worksheet.Cell("L1").Value = "الحالة (نشط/غير نشط)";

            // Sample data
            worksheet.Cell("A2").Value = "STU001";
            worksheet.Cell("B2").Value = "أحمد محمد";
            worksheet.Cell("C2").Value = "Ahmed Mohamed";
            worksheet.Cell("D2").Value = "1234567890";
            worksheet.Cell("E2").Value = "2010-01-01";
            worksheet.Cell("F2").Value = "ذكر";
            worksheet.Cell("G2").Value = "الصف الأول - أ";
            worksheet.Cell("H2").Value = "المدرسة الابتدائية";
            worksheet.Cell("I2").Value = "محمد علي";
            worksheet.Cell("J2").Value = "0501234567";
            worksheet.Cell("K2").Value = "الرياض";
            worksheet.Cell("L2").Value = "نشط";

            // Style headers
            var range = worksheet.Range(1, 1, 1, 12);
            range.Style.Fill.BackgroundColor = XLColor.LightBlue;
            range.Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students_Template.xlsx");
            }
        }
    }

    // GET: /Students/Promote
    public async Task<IActionResult> Promote()
    {
        var currentAcademicYear = DateTime.Now.Year;
        var classRooms = await _context.ClassRooms
            .Include(c => c.Branch)
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.GradeLevelLegacy)
            .ThenBy(c => c.SectionLegacy)
            .ToListAsync();

        ViewBag.CurrentAcademicYear = currentAcademicYear;
        return View(classRooms);
    }

    // POST: /Students/Promote
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Promote(PromoteStudentsDto model)
    {
        if (!ModelState.IsValid)
        {
            return await Promote();
        }

        var students = await _context.Students
            .Where(s => s.ClassRoomId == model.SourceClassRoomId && !s.IsDeleted)
            .ToListAsync();

        foreach (var student in students)
        {
            student.ClassRoomId = model.TargetClassRoomId;
            student.UpdatedAt = DateTime.Now;
            student.UpdatedBy = User.Identity?.Name;
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"تم ترفيع {students.Count} طالب بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Documents/5
    public async Task<IActionResult> Documents(Guid? id)
    {
        if (id == null)
        {
            // If no ID provided, get first available student
            var firstStudent = await _context.Students
                .Where(s => !s.IsDeleted && s.IsActive)
                .FirstOrDefaultAsync();
            
            if (firstStudent != null)
            {
                return RedirectToAction(nameof(Documents), new { id = firstStudent.Id });
            }
            
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .Include(s => s.Documents.Where(d => !d.IsDeleted))
            .FirstOrDefaultAsync(s => s.Id == id.Value && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: /Students/AcademicRecord/5
    public async Task<IActionResult> AcademicRecord(Guid? id)
    {
        if (id == null)
        {
            // If no ID provided, get first available student
            var firstStudent = await _context.Students
                .Where(s => !s.IsDeleted && s.IsActive)
                .FirstOrDefaultAsync();
            
            if (firstStudent != null)
            {
                return RedirectToAction(nameof(AcademicRecord), new { id = firstStudent.Id });
            }
            
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .Include(s => s.AcademicRecords.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(s => s.Id == id.Value && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: /Students/AttendanceAnalysis/5
    public async Task<IActionResult> AttendanceAnalysis(Guid? id)
    {
        if (id == null)
        {
            // If no ID provided, get first available student
            var firstStudent = await _context.Students
                .Where(s => !s.IsDeleted && s.IsActive)
                .FirstOrDefaultAsync();
            
            if (firstStudent != null)
            {
                return RedirectToAction(nameof(AttendanceAnalysis), new { id = firstStudent.Id });
            }
            
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .FirstOrDefaultAsync(s => s.Id == id.Value && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        var analysis = await _attendanceAnalyticsService.AnalyzeStudentPattern(id.Value, 30);
        var trend = await _attendanceAnalyticsService.GetTrendAnalysis(id.Value, 6);

        var viewModel = new StudentAttendanceAnalysisViewModel
        {
            Student = student,
            Pattern = analysis,
            Trend = trend
        };

        return View(viewModel);
    }

    // POST: /Students/UploadDocument
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadDocument(StudentDocumentUploadDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "بيانات المستند غير صحيحة";
            return RedirectToAction(nameof(Documents), new { id = model.StudentId });
        }

        // هنا يجب معالجة رفع الملف الفعلي
        // للتبسيط، سنفترض أن الملف تم رفعه بنجاح
        var document = new StudentDocument
        {
            Id = Guid.NewGuid(),
            StudentId = model.StudentId,
            DocumentType = model.DocumentType,
            DocumentTypeArabic = model.DocumentTypeArabic,
            OriginalFileName = model.FileName,
            FileName = $"{Guid.NewGuid()}_{model.FileName}",
            FilePath = $"uploads/documents/{model.StudentId}/{Guid.NewGuid()}_{model.FileName}",
            FileSize = model.FileSize,
            MimeType = model.MimeType,
            Category = model.Category,
            CategoryArabic = model.CategoryArabic,
            Description = model.Description,
            DescriptionArabic = model.DescriptionArabic,
            IsVerified = false,
            IsConfidential = model.IsConfidential,
            IsRequired = model.IsRequired,
            Status = "Pending",
            StatusArabic = "قيد المراجعة",
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.StudentDocuments.Add(document);
        await _context.SaveChangesAsync();

        // Log activity
        await _documentService.LogActivityAsync(document.Id, model.StudentId, "Uploaded", "تم الرفع",
            $"Document uploaded: {model.FileName}", $"تم رفع المستند: {model.FileName}",
            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["UserAgent"].ToString());

        TempData["Success"] = "تم رفع المستند بنجاح";
        return RedirectToAction(nameof(Documents), new { id = model.StudentId });
    }

    // POST: /Students/SearchDocuments
    [HttpPost]
    public async Task<IActionResult> SearchDocuments(DocumentSearchRequest request)
    {
        var result = await _documentService.SearchDocumentsAsync(request);
        return Json(new { documents = result.Documents, totalCount = result.TotalCount });
    }

    // POST: /Students/ShareDocument
    [HttpPost]
    public async Task<IActionResult> ShareDocument(DocumentShareRequest request)
    {
        var shareLink = await _documentService.CreateShareLinkAsync(request);
        return Json(new { shareUrl = shareLink.ShareUrl });
    }

    // POST: /Students/BulkUploadDocuments
    [HttpPost]
    public async Task<IActionResult> BulkUploadDocuments(BulkDocumentUploadRequest request)
    {
        var uploadedDocs = await _documentService.BulkUploadDocumentsAsync(request);
        return Json(new { count = uploadedDocs.Count });
    }

    // GET: /Students/GetDocumentActivityLog
    [HttpGet]
    public async Task<IActionResult> GetDocumentActivityLog(Guid studentId)
    {
        var logs = await _context.DocumentActivityLogs
            .Include(l => l.Student)
            .Where(l => l.StudentId == studentId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new DocumentActivityLogDto
            {
                Id = l.Id,
                DocumentId = l.DocumentId,
                StudentId = l.StudentId ?? Guid.Empty,
                StudentName = l.Student != null ? l.Student.FullName : string.Empty,
                ActivityType = l.ActivityType,
                ActivityTypeArabic = l.ActivityTypeArabic,
                Description = l.Description,
                DescriptionArabic = l.DescriptionArabic,
                IpAddress = l.IpAddress,
                UserAgent = l.UserAgent,
                IsSuccess = l.IsSuccess,
                ErrorMessage = l.ErrorMessage,
                CreatedAt = l.CreatedAt,
                CreatedBy = l.CreatedBy
            })
            .ToListAsync();

        return Json(logs);
    }

    // GET: /Students/ViewDocument/5
    [HttpGet]
    public async Task<IActionResult> ViewDocument(Guid id)
    {
        var document = await _context.StudentDocuments.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        // Log activity
        await _documentService.LogActivityAsync(id, document.StudentId, "Viewed", "تم العرض",
            $"Document viewed", $"تم عرض المستند",
            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["UserAgent"].ToString());

        // For now, return a placeholder
        return Content($"Document viewer for: {document.OriginalFileName}");
    }

    // GET: /Students/DownloadDocument/5
    [HttpGet]
    public async Task<IActionResult> DownloadDocument(Guid id)
    {
        var document = await _context.StudentDocuments.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        // Log activity
        await _documentService.LogActivityAsync(id, document.StudentId, "Downloaded", "تم التحميل",
            $"Document downloaded", $"تم تحميل المستند",
            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["UserAgent"].ToString());

        // For now, return a placeholder
        return Content($"Download: {document.OriginalFileName}");
    }

    // POST: /Students/VerifyDocument/5
    [HttpPost]
    public async Task<IActionResult> VerifyDocument(Guid id)
    {
        var document = await _context.StudentDocuments.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        document.IsVerified = true;
        document.VerifiedAt = DateTime.Now;
        document.VerifiedBy = User.Identity?.Name;
        document.UpdatedAt = DateTime.Now;
        document.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        // Log activity
        await _documentService.LogActivityAsync(id, document.StudentId, "Verified", "تم التوثيق",
            $"Document verified", $"تم توثيق المستند",
            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["UserAgent"].ToString());

        return Json(new { success = true });
    }

    // POST: /Students/DeleteDocument/5
    [HttpPost]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var document = await _context.StudentDocuments.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        document.IsDeleted = true;
        document.DeletedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        document.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        // Log activity
        await _documentService.LogActivityAsync(id, document.StudentId, "Deleted", "تم الحذف",
            $"Document deleted", $"تم حذف المستند",
            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["UserAgent"].ToString());

        return Json(new { success = true });
    }

    private async Task<string> GenerateStudentNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.Students
            .Where(s => s.StudentNumber != null && s.StudentNumber.StartsWith(year.ToString()))
            .OrderByDescending(s => s.StudentNumber)
            .Select(s => s.StudentNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(4));
        return $"{year}{(lastNum + 1).ToString("D4")}";
    }
}

// DTOs
public class StudentFilterDto
{
    public string? SearchTerm { get; set; }
    public Guid? SchoolId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? ClassRoomId { get; set; }
    public int? GradeLevel { get; set; }
    public bool? IsActive { get; set; }
}

public class StudentIndexViewModel
{
    public List<Student> Students { get; set; } = new();
    public StudentFilterDto Filter { get; set; } = new();
}

public class StudentCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? NoorStudentId { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; }
    
    // الحقول الإضافية الجديدة
    public string? PreviousSchool { get; set; }
    public string? PreviousSchoolArabic { get; set; }
    public DateTime? TransferDate { get; set; }
    public string? TransferReason { get; set; }
    public string? AcademicStanding { get; set; }
    public string? SpecialNeeds { get; set; }
    public string? SpecialNeedsArabic { get; set; }
    public bool IsGifted { get; set; }
    public string? GiftedProgram { get; set; }
    public DateTime? LastMedicalCheckup { get; set; }
    public string? Allergies { get; set; }
    public string? AllergiesArabic { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string? DietaryRestrictionsArabic { get; set; }
    
    public Guid SchoolId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? ClassRoomId { get; set; }
    public Guid? GuardianId { get; set; }
}

public class StudentEditDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? NoorStudentId { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; }
    
    // الحقول الإضافية الجديدة
    public string? PreviousSchool { get; set; }
    public string? PreviousSchoolArabic { get; set; }
    public DateTime? TransferDate { get; set; }
    public string? TransferReason { get; set; }
    public string? AcademicStanding { get; set; }
    public string? SpecialNeeds { get; set; }
    public string? SpecialNeedsArabic { get; set; }
    public bool IsGifted { get; set; }
    public string? GiftedProgram { get; set; }
    public DateTime? LastMedicalCheckup { get; set; }
    public string? Allergies { get; set; }
    public string? AllergiesArabic { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string? DietaryRestrictionsArabic { get; set; }
    
    public Guid SchoolId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? ClassRoomId { get; set; }
    public Guid? GuardianId { get; set; }
    public bool IsActive { get; set; }
}

public class PromoteStudentsDto
{
    public Guid SourceClassRoomId { get; set; }
    public Guid TargetClassRoomId { get; set; }
}

// DTOs إضافية للميزات الجديدة
public class StudentAttendanceAnalysisViewModel
{
    public Student Student { get; set; } = null!;
    public Masar.Schools.ERP.Infrastructure.Services.AttendancePatternDto Pattern { get; set; } = null!;
    public Masar.Schools.ERP.Infrastructure.Services.AttendanceTrendDto Trend { get; set; } = null!;
}

public class StudentDocumentUploadDto
{
    public Guid StudentId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public bool IsConfidential { get; set; }
    public bool IsRequired { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
