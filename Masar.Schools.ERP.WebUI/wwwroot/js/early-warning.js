let currentStudentId = null;

// تحميل البيانات عند بدء الصفحة
document.addEventListener('DOMContentLoaded', function() {
    loadRiskMatrix();
    loadClasses();
});

// تحميل مصفوفة المخاطر
async function loadRiskMatrix() {
    const filter = getFilterValues();
    
    try {
        const response = await fetch('/EarlyWarning/GetRiskMatrix', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(filter)
        });
        
        const result = await response.json();
        
        if (result.success) {
            renderRiskMatrix(result.data);
        } else {
            showNotification('error', 'فشل تحميل البيانات: ' + result.error);
        }
    } catch (error) {
        console.error('Error loading risk matrix:', error);
        showNotification('error', 'حدث خطأ أثناء تحميل البيانات');
    }
}

// تطبيق الفلاتر
function applyFilters() {
    loadRiskMatrix();
}

// الحصول على قيم الفلاتر
function getFilterValues() {
    return {
        classRoomId: document.getElementById('classFilter').value || null,
        riskLevel: document.getElementById('riskLevelFilter').value || null,
        minRiskScore: document.getElementById('minRiskScore').value || null,
        maxRiskScore: document.getElementById('maxRiskScore').value || null
    };
}

// عرض مصفوفة المخاطر
function renderRiskMatrix(data) {
    const tbody = document.getElementById('riskMatrixBody');
    tbody.innerHTML = '';
    
    if (!data || data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="10" class="text-center text-muted">لا توجد بيانات</td></tr>';
        return;
    }
    
    data.forEach(item => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${item.studentNumber}</td>
            <td>${item.studentNameArabic}</td>
            <td>${item.className || '-'}</td>
            <td>${item.gradeLevel || '-'}</td>
            <td>
                <span class="badge ${getRiskLevelBadgeClass(item.riskLevel)}">
                    ${item.riskScore.toFixed(1)} - ${getRiskLevelText(item.riskLevel)}
                </span>
            </td>
            <td>${item.attendanceRisk.toFixed(1)}%</td>
            <td>${item.academicRisk.toFixed(1)}%</td>
            <td>${item.financialRisk.toFixed(1)}%</td>
            <td>${formatDate(item.calculatedAt)}</td>
            <td>
                <button class="btn btn-sm btn-outline-primary" onclick="showStudentDetails('${item.studentId}')">
                    <i class="bi bi-eye"></i> التفاصيل
                </button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// الحصول على class badge لمستوى الخطر
function getRiskLevelBadgeClass(riskLevel) {
    switch(riskLevel) {
        case 0: return 'bg-success'; // Safe
        case 1: return 'bg-warning text-dark'; // Warning
        case 2: return 'bg-danger'; // Critical
        default: return 'bg-secondary';
    }
}

// الحصول على نص مستوى الخطر
function getRiskLevelText(riskLevel) {
    switch(riskLevel) {
        case 0: return 'آمن';
        case 1: return 'تحذير';
        case 2: return 'خطر حرج';
        default: return 'غير معروف';
    }
}

// تنسيق التاريخ
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('ar-SA');
}

// تحميل الفصول
async function loadClasses() {
    try {
        const response = await fetch('/ClassRooms/GetAll');
        const result = await response.json();
        
        if (result.success) {
            const select = document.getElementById('classFilter');
            result.data.forEach(cls => {
                const option = document.createElement('option');
                option.value = cls.id;
                option.textContent = cls.nameArabic;
                select.appendChild(option);
            });
        }
    } catch (error) {
        console.error('Error loading classes:', error);
    }
}

// عرض تفاصيل الطالب
async function showStudentDetails(studentId) {
    currentStudentId = studentId;
    
    try {
        const response = await fetch(`/EarlyWarning/GetStudentDetails?studentId=${studentId}`);
        const result = await response.json();
        
        if (result.success) {
            renderStudentDetails(result.data);
            const modal = new bootstrap.Modal(document.getElementById('studentDetailsModal'));
            modal.show();
        } else {
            showNotification('error', 'فشل تحميل التفاصيل: ' + result.error);
        }
    } catch (error) {
        console.error('Error loading student details:', error);
        showNotification('error', 'حدث خطأ أثناء تحميل التفاصيل');
    }
}

// عرض تفاصيل الطالب في Modal
function renderStudentDetails(data) {
    const content = document.getElementById('studentDetailsContent');
    content.innerHTML = `
        <div class="row">
            <div class="col-md-6">
                <h6>معلومات الطالب</h6>
                <table class="table table-sm">
                    <tr><td>الاسم:</td><td>${data.studentNameArabic}</td></tr>
                    <tr><td>رقم الطالب:</td><td>${data.studentNumber}</td></tr>
                    <tr><td>الفصل:</td><td>${data.className || '-'}</td></tr>
                </table>
            </div>
            <div class="col-md-6">
                <h6>مستوى الخطر</h6>
                <div class="alert ${getRiskLevelAlertClass(data.riskLevel)}">
                    <strong>${getRiskLevelText(data.riskLevel)}</strong>
                    <br>
                    درجة الخطر: ${data.riskScore.toFixed(1)}
                </div>
            </div>
        </div>
        
        <h6 class="mt-3">العوامل الرئيسية</h6>
        <ul class="list-group mb-3">
            ${data.primaryRiskFactors.map(factor => `
                <li class="list-group-item d-flex justify-content-between align-items-center">
                    ${factor.nameArabic}
                    <span class="badge ${getImpactBadgeClass(factor.impact)}">${factor.score.toFixed(1)}</span>
                </li>
            `).join('')}
        </ul>
        
        <h6>التوصيات</h6>
        <ul class="list-group">
            ${data.recommendations.map(rec => `
                <li class="list-group-item">${rec}</li>
            `).join('')}
        </ul>
    `;
}

// الحصول على class alert لمستوى الخطر
function getRiskLevelAlertClass(riskLevel) {
    switch(riskLevel) {
        case 0: return 'alert-success';
        case 1: return 'alert-warning';
        case 2: return 'alert-danger';
        default: return 'alert-secondary';
    }
}

// الحصول على class badge للأثر
function getImpactBadgeClass(impact) {
    switch(impact.toLowerCase()) {
        case 'high': return 'bg-danger';
        case 'medium': return 'bg-warning text-dark';
        case 'low': return 'bg-success';
        default: return 'bg-secondary';
    }
}

// إرسال تنبيه
async function triggerNotification() {
    if (!currentStudentId) return;
    
    try {
        const response = await fetch('/EarlyWarning/TriggerNotification', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ studentId: currentStudentId })
        });
        
        const result = await response.json();
        
        if (result.success) {
            showNotification('success', 'تم إرسال التنبيه بنجاح');
            bootstrap.Modal.getInstance(document.getElementById('studentDetailsModal')).hide();
        } else {
            showNotification('error', 'فشل إرسال التنبيه: ' + result.error);
        }
    } catch (error) {
        console.error('Error triggering notification:', error);
        showNotification('error', 'حدث خطأ أثناء إرسال التنبيه');
    }
}

// تحديث البيانات
function refreshData() {
    loadRiskMatrix();
    showNotification('info', 'جاري تحديث البيانات...');
}

// عرض إشعار
function showNotification(type, message) {
    // يمكن استبدال هذا بنظام إشعارات أكثر تطوراً
    const alertClass = type === 'success' ? 'alert-success' : 
                      type === 'error' ? 'alert-danger' : 'alert-info';
    
    const alert = document.createElement('div');
    alert.className = `alert ${alertClass} alert-dismissible fade show position-fixed`;
    alert.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(alert);
    
    setTimeout(() => {
        alert.remove();
    }, 5000);
}
