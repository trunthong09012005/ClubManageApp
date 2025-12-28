# ?? TÀI LI?U NÂNG C?P VALIDATION CHO USERTEST.CS

## ?? M?c Tiêu
Nâng c?p h? th?ng validation cho module qu?n lý thành viên, ??c bi?t là **quy t?c ch? ???c có 1 Admin duy nh?t** trong h? th?ng.

---

## ? CÁC C?I TI?N CHO `FormAddEditMember.cs`

### 1. **QUY T?C ADMIN DUY NH?T** ??

#### a) Ki?m tra khi T?O Admin m?i
```csharp
// Không cho phép t?o Admin th? 2
if (isChangingToAdmin && !wasOriginallyAdmin)
{
    if (HasExistingAdmin())
    {
        MessageBox.Show("KHÔNG TH? T?O ADMIN M?I! H? th?ng ch? cho phép 1 Admin duy nh?t");
        return;
    }
}
```

#### b) C?nh báo khi XÓA vai trò Admin duy nh?t
```csharp
if (wasOriginallyAdmin && !isChangingToAdmin)
{
    int adminCount = GetAdminCount();
    if (adminCount <= 1)
    {
        // Hi?n th? c?nh báo nghiêm tr?ng
        DialogResult confirmResult = MessageBox.Show("C?NH BÁO C?C K? NGHIÊM TR?NG! H? th?ng s? không còn Admin...");
    }
}
```

#### c) Ki?m tra real-time khi thay ??i Vai trò
```csharp
private void CboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
{
    // Ng?n ch?n ngay khi ng??i dùng ch?n Admin (n?u ?ã có Admin khác)
    // C?nh báo khi ??i t? Admin sang vai trò khác
}
```

### 2. **PHÂN QUY?N NGHIÊM NG?T** ??

#### a) Ch? Admin m?i ???c t?o/s?a Admin khác
```csharp
if (string.Equals(selectedRole, "Admin", StringComparison.OrdinalIgnoreCase) &&
    !string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
{
    MessageBox.Show("? Ch? có Admin m?i ???c phép ??t vai trò Admin!");
    return;
}
```

#### b) ?n option "Admin" v?i ng??i dùng không ph?i Admin
```csharp
if (!string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
{
    // Xóa "Admin" kh?i dropdown
    for (int i = cboVaiTro.Items.Count - 1; i >= 0; i--)
    {
        if (string.Equals(cboVaiTro.Items[i]?.ToString(), "Admin", ...))
            cboVaiTro.Items.RemoveAt(i);
    }
}
```

### 3. **VALIDATION NÂNG CAO** ??

#### a) Ki?m tra Email trùng l?p
```csharp
private bool IsEmailExists(string email)
{
    // Ki?m tra trong database, lo?i tr? b?n thân khi edit
    string query = maTV.HasValue 
        ? "SELECT COUNT(*) FROM ThanhVien WHERE Email = @Email AND MaTV != @MaTV"
        : "SELECT COUNT(*) FROM ThanhVien WHERE Email = @Email";
}
```

#### b) Ki?m tra S?T trùng l?p (M?I)
```csharp
private bool IsPhoneExists(string phone)
{
    // ??m b?o không có 2 thành viên cùng s? ?i?n tho?i
}
```

#### c) Validate các tr??ng b?t bu?c
- ? H? tên: 2-150 ký t?, ch? ch? cái và kho?ng tr?ng
- ? Email: ??nh d?ng chu?n, không trùng
- ? S?T: ??nh d?ng Vi?t Nam (0xxxxxxxxx ho?c +84xxxxxxxxx), không trùng
- ? Ngày sinh: 15-100 tu?i
- ? Gi?i tính: B?t bu?c ch?n
- ? Vai trò: B?t bu?c ch?n
- ? Tr?ng thái: B?t bu?c ch?n

### 4. **DOUBLE-CHECK TR??C KHI L?U** ??

```csharp
// Ki?m tra l?i l?n cu?i trong transaction
if (isChangingToAdmin && !wasOriginallyAdmin)
{
    string checkQuery = "SELECT COUNT(*) FROM ThanhVien WHERE VaiTro = N'Admin'";
    int existingAdminCount = (int)checkCmd.ExecuteScalar();
    if (existingAdminCount > 0)
    {
        MessageBox.Show("Phát hi?n Admin khác trong h? th?ng!");
        return;
    }
}
```

---

## ? CÁC C?I TI?N CHO `UserTest.cs`

### 1. **B?O V? ADMIN KHI XÓA** ???

#### a) Không cho xóa Admin duy nh?t
```csharp
if (string.Equals(vaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
{
    int adminCount = GetAdminCount();
    
    if (adminCount <= 1)
    {
        MessageBox.Show("?? KHÔNG TH? XÓA ADMIN DUY NH?T!");
        return;
    }
}
```

#### b) C?nh báo khi xóa Admin (n?u có nhi?u Admin)
```csharp
DialogResult confirmAdmin = MessageBox.Show(
    "?? C?NH BÁO: B?N ?ANG XÓA M?T ADMIN! Hành ??ng nghiêm tr?ng...");
```

#### c) Ki?m tra quy?n xóa Admin
```csharp
if (string.Equals(vaiTro, "Admin", ...) &&
    !string.Equals(currentUserRole, "Admin", ...))
{
    MessageBox.Show("? Ch? có Admin m?i ???c phép xóa Admin khác!");
    return;
}
```

### 2. **S? D?NG TRANSACTION ?? ??M B?O TÍNH TOÀN V?N** ??

```csharp
using (SqlTransaction transaction = conn.BeginTransaction())
{
    try
    {
        // Ki?m tra l?i vai trò và s? Admin
        // Th?c hi?n xóa
        // Commit transaction
        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw;
    }
}
```

### 3. **X? LÝ FOREIGN KEY CONSTRAINT** ??

```csharp
catch (SqlException sqlEx)
{
    if (sqlEx.Number == 547) // Foreign key constraint
    {
        MessageBox.Show(
            "? Không th? xóa thành viên này!\n\n" +
            "Thành viên ?ang có d? li?u liên quan:\n" +
            "• Tham gia ho?t ??ng\n" +
            "• Có ?i?m rèn luy?n\n" +
            "• Thu?c ban chuyên môn\n" +
            "Vui lòng xóa d? li?u liên quan tr??c ho?c ??i tr?ng thái thành 'Ngh?'.");
    }
}
```

### 4. **HELPER METHOD** ???

```csharp
private int GetAdminCount()
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        string query = "SELECT COUNT(*) FROM ThanhVien WHERE VaiTro = N'Admin'";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            return (int)cmd.ExecuteScalar();
        }
    }
}
```

---

## ?? MA TR?N KI?M TRA

| Tình hu?ng | Validation | K?t qu? mong ??i |
|------------|-----------|------------------|
| T?o Admin ??u tiên | ? Cho phép | Thành công |
| T?o Admin th? 2 | ? Ng?n ch?n | Hi?n th? l?i "Ch? 1 Admin" |
| S?a Admin duy nh?t thành vai trò khác | ?? C?nh báo nghiêm tr?ng | Yêu c?u xác nh?n 2 l?n |
| Xóa Admin duy nh?t | ? Ng?n ch?n | Hi?n th? l?i "Không th? xóa" |
| Xóa Admin (có nhi?u Admin) | ?? C?nh báo | Yêu c?u xác nh?n |
| Ng??i không ph?i Admin t?o/s?a/xóa Admin | ? Ng?n ch?n | Hi?n th? l?i "Không có quy?n" |
| Email trùng | ? Ng?n ch?n | Hi?n th? l?i "Email ?ã t?n t?i" |
| S?T trùng | ? Ng?n ch?n | Hi?n th? l?i "S?T ?ã t?n t?i" |
| Thi?u tr??ng b?t bu?c | ? Ng?n ch?n | Hi?n th? l?i validation c? th? |
| Xóa thành viên có d? li?u liên quan | ? Ng?n ch?n | Hi?n th? l?i Foreign Key |

---

## ?? UX/UI IMPROVEMENTS

### Màu s?c báo l?i
- ?? **?? nh?t** (`Color.FromArgb(255, 230, 230)`): TextBox có l?i
- ?? **Icon Error** (`MessageBoxIcon.Error`): L?i nghiêm tr?ng
- ?? **Icon Warning** (`MessageBoxIcon.Warning`): C?nh báo
- ? **Icon Information** (`MessageBoxIcon.Information`): Thành công

### Thông báo rõ ràng
- ? Emoji ?? t?ng tính tr?c quan (??, ??, ?, ??, ???)
- ? Gi?i thích c? th? lý do l?i
- ? H??ng d?n cách kh?c ph?c
- ? Xác nh?n 2 l?n cho hành ??ng nguy hi?m

---

## ?? B?O M?T VÀ TÍNH TOÀN V?N

### Nhi?u l?p ki?m tra
1. **UI Level**: Validation khi nh?p li?u
2. **Code Level**: Validation tr??c khi l?u
3. **Database Level**: Double-check trong transaction
4. **Constraint Level**: Foreign key constraints

### Ng?n ch?n race condition
- ? S? d?ng Transaction ?? ??m b?o atomic operations
- ? Double-check tr??c khi commit
- ? Rollback n?u có l?i

---

## ?? H??NG D?N S? D?NG

### Dành cho Admin
1. ? Có toàn quy?n t?o/s?a/xóa thành viên
2. ? Có th? ??t vai trò Admin cho ng??i khác (n?u ch?a có Admin)
3. ?? C?NH BÁO: Không nên xóa vai trò Admin c?a chính mình n?u là Admin duy nh?t

### Dành cho ng??i dùng khác
1. ? Không th? t?o/s?a/xóa Admin
2. ? Không th?y option "Admin" trong dropdown vai trò
3. ? Có th? qu?n lý thành viên thông th??ng (tùy quy?n)

### Khuy?n ngh?
- ?? Luôn có ít nh?t 1 Admin trong h? th?ng
- ?? Backup database tr??c khi thay ??i vai trò Admin
- ?? S? d?ng email duy nh?t cho m?i thành viên
- ?? S? d?ng S?T duy nh?t cho m?i thành viên

---

## ?? X? LÝ L?I

### SQL Errors
- **2627, 2601**: Duplicate key (Email/S?T trùng)
- **547**: Foreign key constraint (D? li?u liên quan)

### Validation Errors
- H? tên không h?p l?
- Email không ?úng ??nh d?ng
- S?T không ?úng ??nh d?ng
- Ngày sinh không h?p l?
- Thi?u tr??ng b?t bu?c

---

## ?? K?T QU?

### Tr??c khi nâng c?p ?
- Có th? t?o nhi?u Admin
- Có th? xóa Admin duy nh?t
- Không ki?m tra Email/S?T trùng
- Validation c? b?n

### Sau khi nâng c?p ?
- ? Ch? cho phép 1 Admin duy nh?t
- ? B?o v? Admin kh?i b? xóa/s?a b?i ng??i không có quy?n
- ? Ki?m tra Email/S?T trùng l?p
- ? Validation toàn di?n v?i nhi?u l?p
- ? Transaction ??m b?o tính toàn v?n
- ? UX/UI rõ ràng, thân thi?n
- ? X? lý l?i chi ti?t

---

## ?? DEMO SCENARIOS

### Scenario 1: T?o Admin ??u tiên
```
User (Admin): T?o thành viên m?i ? Ch?n vai trò "Admin" 
Result: ? Thành công! "?? Vai trò Admin ?ã ???c gán"
```

### Scenario 2: C? t?o Admin th? 2
```
User (Admin): T?o thành viên m?i ? Ch?n vai trò "Admin"
System: ?? "H? TH?NG CH? CHO PHÉP M?T ADMIN DUY NH?T!"
Result: ? Không cho phép, reset v? vai trò c?
```

### Scenario 3: Ng??i không ph?i Admin c? s?a Admin
```
User (Member): S?a thành viên (Admin) ? C? ??i vai trò
System: ComboBox b? disabled, hi?n th? warning
Result: ? Không th? thay ??i
```

### Scenario 4: Xóa Admin duy nh?t
```
User (Admin): Ch?n Admin ? Click Xóa
System: ?? "KHÔNG TH? XÓA ADMIN DUY NH?T!"
Result: ? Không cho phép
```

### Scenario 5: ??i vai trò Admin duy nh?t
```
User (Admin): S?a Admin ? ??i vai trò sang "Thành viên"
System: ?? "C?NH BÁO C?C K? NGHIÊM TR?NG!" ? Yêu c?u xác nh?n
User: Click "Yes"
Result: ?? Cho phép nh?ng c?nh báo nghiêm tr?ng
```

---

## ?? H? TR?

N?u g?p v?n ??, ki?m tra:
1. ? Connection string ?úng
2. ? Database có b?ng ThanhVien
3. ? Quy?n truy c?p database
4. ? Vai trò ng??i dùng hi?n t?i

---

**Build Status:** ? Build Successful  
**Date:** 2024  
**Version:** 2.0 - Enhanced Validation
