# سكريبت تنظيف ملفات TEMP و CACHE في محرك C
# Created: 2026-08-30

Write-Host "=== بدء عملية التنظيف ===" -ForegroundColor Cyan
Write-Host ""

# متغيرات
$tempPath = $env:TEMP
$windowsTempPath = $env:SystemRoot + "\Temp"
$prefetchPath = $env:SystemRoot + "\Prefetch"
$thumbnailCachePath = $env:LOCALAPPDATA + "\Microsoft\Windows\Explorer"
$recycleBinPath = "C:\`$Recycle.Bin"

# دالة لتنظيف مجلد
function Clean-Folder {
    param (
        [string]$Path,
        [string]$Description
    )
    
    if (Test-Path $Path) {
        Write-Host "تنظيف: $Description" -ForegroundColor Yellow
        Write-Host "المسار: $Path" -ForegroundColor Gray
        
        try {
            $files = Get-ChildItem -Path $Path -Force -ErrorAction SilentlyContinue
            $totalSize = 0
            
            foreach ($file in $files) {
                try {
                    $totalSize += $file.Length
                    Remove-Item -Path $file.FullName -Force -Recurse -ErrorAction SilentlyContinue
                }
                catch {
                    # تخطي الملفات المستخدمة
                }
            }
            
            $sizeMB = [math]::Round($totalSize / 1MB, 2)
            Write-Host "تم تنظيف: $sizeMB MB" -ForegroundColor Green
        }
        catch {
            Write-Host "خطأ: $_" -ForegroundColor Red
        }
        
        Write-Host ""
    }
    else {
        Write-Host "المجلد غير موجود: $Description" -ForegroundColor Gray
        Write-Host ""
    }
}

# 1. تنظيف ملفات TEMP للمستخدم
Write-Host "1. تنظيف ملفات TEMP للمستخدم" -ForegroundColor Cyan
Clean-Folder -Path $tempPath -Description "User Temp"

# 2. تنظيف ملفات TEMP للنظام
Write-Host "2. تنظيف ملفات TEMP للنظام" -ForegroundColor Cyan
Clean-Folder -Path $windowsTempPath -Description "Windows Temp"

# 3. تنظيف ملفات Prefetch
Write-Host "3. تنظيف ملفات Prefetch" -ForegroundColor Cyan
Clean-Folder -Path $prefetchPath -Description "Prefetch"

# 4. تنظيف Thumbnail Cache
Write-Host "4. تنظيف Thumbnail Cache" -ForegroundColor Cyan
if (Test-Path $thumbnailCachePath) {
    $thumbDb = $thumbnailCachePath + "\thumbcache_*.db"
    if (Test-Path $thumbDb) {
        Write-Host "تنظيف: Thumbnail Cache" -ForegroundColor Yellow
        try {
            Remove-Item -Path $thumbDb -Force -ErrorAction SilentlyContinue
            Write-Host "تم تنظيف: Thumbnail Cache" -ForegroundColor Green
        }
        catch {
            Write-Host "خطأ: $_" -ForegroundColor Red
        }
        Write-Host ""
    }
}

# 5. تنظيف سلة المحذوفات (اختياري)
Write-Host "5. تنظيف سلة المحذوفات" -ForegroundColor Cyan
$clearRecycleBin = Read-Host "هل تريد تنظيف سلة المحذوفات؟ (Y/N)"
if ($clearRecycleBin -eq "Y" -or $clearRecycleBin -eq "y") {
    Write-Host "تنظيف: سلة المحذوفات" -ForegroundColor Yellow
    try {
        Clear-RecycleBin -Force -ErrorAction SilentlyContinue
        Write-Host "تم تنظيف: سلة المحذوفات" -ForegroundColor Green
    }
    catch {
        Write-Host "خطأ: $_" -ForegroundColor Red
    }
    Write-Host ""
}

# 6. حساب المساحة المحررة
Write-Host "=== حساب المساحة المحررة ===" -ForegroundColor Cyan
$driveC = Get-PSDrive -Name C
$freeSpaceGB = [math]::Round($driveC.Free / 1GB, 2)
Write-Host "المساحة الحرة في محرك C: $freeSpaceGB GB" -ForegroundColor Green

Write-Host ""
Write-Host "=== اكتملت عملية التنظيف ===" -ForegroundColor Cyan
Write-Host "اضغط أي مفتاح للخروج..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
