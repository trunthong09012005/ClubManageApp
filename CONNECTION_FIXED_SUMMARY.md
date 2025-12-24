# ? HOÀN T?T! ?Ã KH?C PH?C CONNECTION STRING

## ?? T?NG K?T

?ã t?o **`ConnectionHelper.cs`** ?? qu?n lý connectionString t?p trung.  
?ã c?p nh?t **T?T C?** các file ?? s? d?ng `ConnectionHelper.ConnectionString`.

---

## ?? SAU KHI PULL CODE V? - CH? LÀM 1 B??C:

### **M? file: `ConnectionHelper.cs`**
### **S?a dòng 13:**

```csharp
// ?? THAY TÊN_MÁY và PASSWORD c?a b?n
private static string _connectionString = @"Data Source=TÊN_MÁY_C?A_B?N;Initial Catalog=QL_CLB_LSC;User ID=sa;Password=M?T_KH?U;Encrypt=True;TrustServerCertificate=True";
```

**Ví d?:**
```csharp
// Máy c?a Lan
private static string _connectionString = @"Data Source=DESKTOP-LAN\SQLEXPRESS;Initial Catalog=QL_CLB_LSC;User ID=sa;Password=abcd1234;Encrypt=True;TrustServerCertificate=True";
```

**XONG! Không c?n s?a gì n?a.**

---

## ?? CÁC FILE ?Ã C?P NH?T

? **ConnectionHelper.cs** (M?I T?O)  
? Activity.cs  
? DashBoard.cs  
? Schedule.cs  
? Login.cs  
? Account.cs  
? UserTest.cs  
? Project.cs  
? Notification.cs  
? MeetingTest.cs  
? Finance.cs  

**T?ng c?ng: 11 files**

---

## ?? L?U Ý GIT

### **Ph??ng án 1: KHÔNG commit ConnectionHelper.cs**
Thêm vào `.gitignore`:
```
# Connection config - m?i ng??i t? s?a
ConnectionHelper.cs
```

### **Ph??ng án 2: Commit v?i giá tr? m?u**
Tr??c khi commit, s?a `ConnectionHelper.cs` dòng 13:
```csharp
private static string _connectionString = @"Data Source=YOUR_SERVER_NAME;Initial Catalog=QL_CLB_LSC;User ID=sa;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True";
```

Và thêm comment:
```csharp
// ?? SAU KHI PULL V?, THAY YOUR_SERVER_NAME và YOUR_PASSWORD B?NG THÔNG TIN C?A B?N
```

---

## ?? CÁCH COMMIT

```bash
# 1. Add t?t c? file ?ã s?a
git add .

# 2. Commit
git commit -m "Refactor: T?p trung connectionString vào ConnectionHelper.cs

- T?o ConnectionHelper.cs ?? qu?n lý connectionString t?p trung
- C?p nh?t t?t c? file ?? dùng ConnectionHelper.ConnectionString
- Sau khi pull v? ch? c?n s?a 1 file duy nh?t

Files updated:
- ConnectionHelper.cs (NEW)
- Activity.cs
- DashBoard.cs
- Schedule.cs
- Login.cs
- Account.cs
- UserTest.cs
- Project.cs
- Notification.cs
- MeetingTest.cs
- Finance.cs"

# 3. Push lên GitHub
git push origin khoi
```

---

## ?? L?I ÍCH

? **Ch? s?a 1 file** sau khi pull code  
? **Không b? sót** connectionString ? ?âu  
? **D? maintain** khi c?n ??i database  
? **Team làm vi?c hi?u qu?** h?n  
? **Test connection** d? dàng v?i `ConnectionHelper.TestConnection()`  

---

## ?? TEST CONNECTION

N?u mu?n test xem connection có OK không:

```csharp
if (ConnectionHelper.TestConnection())
{
    MessageBox.Show("K?t n?i thành công!");
}
else
{
    MessageBox.Show("L?i k?t n?i! Ki?m tra l?i ConnectionHelper.cs");
}
```

---

## ?? X? LÝ L?I

### **L?i: "Cannot open database QL_CLB_LSC"**
?? Ki?m tra tên database ?úng ch?a

### **L?i: "Login failed for user 'sa'"**
?? Ki?m tra m?t kh?u SQL Server

### **L?i: "A network-related or instance-specific error"**
?? Ki?m tra:
1. SQL Server ?ang ch?y ch?a
2. Tên máy/instance ?úng ch?a (ví d?: `.\SQLEXPRESS`)
3. TCP/IP ?ã enable ch?a trong SQL Server Configuration Manager

---

**?? Hoàn t?t! Gi? ch? c?n commit và team có th? dùng ngay!**

**?? C?p nh?t:** 24/12/2024  
**????? Ng??i th?c hi?n:** GitHub Copilot
