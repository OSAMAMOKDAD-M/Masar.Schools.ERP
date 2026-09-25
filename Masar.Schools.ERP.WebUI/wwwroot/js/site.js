// Site-wide JavaScript functionality

// Initialize Bootstrap tooltips
document.addEventListener('DOMContentLoaded', function() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize Bootstrap popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Auto-hide alerts after 5 seconds
setTimeout(function() {
    var alerts = document.querySelectorAll('.alert');
    alerts.forEach(function(alert) {
        var bsAlert = new bootstrap.Alert(alert);
        bsAlert.close();
    });
}, 5000);

// Format numbers with thousands separator
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('ar-SA', {
        style: 'currency',
        currency: 'SAR'
    }).format(amount);
}

// Format date
function formatDate(date) {
    return new Date(date).toLocaleDateString('ar-SA');
}

// Format date time
function formatDateTime(date) {
    return new Date(date).toLocaleString('ar-SA');
}

// Show loading spinner
function showLoading() {
    var spinner = document.createElement('div');
    spinner.className = 'spinner-overlay';
    spinner.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">جاري التحميل...</span></div>';
    document.body.appendChild(spinner);
    return spinner;
}

// Hide loading spinner
function hideLoading(spinner) {
    if (spinner) {
        spinner.remove();
    }
}

// Confirm action
function confirmAction(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Print function
function printElement(elementId) {
    var element = document.getElementById(elementId);
    if (element) {
        var printWindow = window.open('', '_blank');
        printWindow.document.write('<html><head><title>طباعة</title>');
        printWindow.document.write('<link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.rtl.min.css">');
        printWindow.document.write('</head><body>');
        printWindow.document.write(element.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    }
}

// Export to CSV
function exportToCSV(data, filename) {
    var csv = [];
    var headers = Object.keys(data[0]);
    csv.push(headers.join(','));

    for (var i = 0; i < data.length; i++) {
        var row = [];
        for (var j = 0; j < headers.length; j++) {
            row.push(data[i][headers[j]]);
        }
        csv.push(row.join(','));
    }

    var csvFile = new Blob([csv.join('\n')], { type: 'text/csv' });
    var downloadLink = document.createElement('a');
    downloadLink.download = filename + '.csv';
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
}

// Copy to clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function() {
        alert('تم النسخ للحافظة');
    }).catch(function(err) {
        console.error('فشل النسخ: ', err);
    });
}

// Search functionality
function searchTable(tableId, searchInputId) {
    var input = document.getElementById(searchInputId);
    var filter = input.value.toUpperCase();
    var table = document.getElementById(tableId);
    var rows = table.getElementsByTagName('tr');

    for (var i = 1; i < rows.length; i++) {
        var cells = rows[i].getElementsByTagName('td');
        var found = false;
        
        for (var j = 0; j < cells.length; j++) {
            if (cells[j]) {
                var textValue = cells[j].textContent || cells[j].innerText;
                if (textValue.toUpperCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
        }
        
        rows[i].style.display = found ? '' : 'none';
    }
}

// Validate form
function validateForm(formId) {
    var form = document.getElementById(formId);
    if (form.checkValidity()) {
        return true;
    } else {
        form.reportValidity();
        return false;
    }
}

// API call helper
async function apiCall(url, method = 'GET', data = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
        }
    };

    if (data) {
        options.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(url, options);
        return await response.json();
    } catch (error) {
        console.error('API call error:', error);
        throw error;
    }
}

// Initialize all functions when document is ready
document.addEventListener('DOMContentLoaded', function() {
    // Add any initialization code here
    console.log('MASAR ERP System initialized');
});
