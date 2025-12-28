# ?? TEST SCRIPT - ADMIN VALIDATION

## Checklist ki?m tra tính n?ng "Ch? 1 Admin duy nh?t"

### ? PHASE 1: KH?I T?O

- [ ] **Test 1.1**: T?o Admin ??u tiên
  - M? UserTest ? Click "Thêm m?i"
  - Nh?p thông tin h?p l?
  - Ch?n vai trò "Admin"
  - Click "L?u"
  - **Mong ??i**: ? Thành công, thông báo "?? Vai trò Admin ?ã ???c gán"

- [ ] **Test 1.2**: Ki?m tra Admin v?a t?o
  - Xem l?i danh sách thành viên
  - **Mong ??i**: ? Th?y thành viên v?i vai trò "Admin"

---

### ? PHASE 2: NG?N CH?N ADMIN TH? 2

- [ ] **Test 2.1**: C? t?o Admin th? 2 (v?i quy?n Admin)
  - ??ng nh?p b?ng tài kho?n Admin
  - Click "Thêm m?i"
  - Nh?p thông tin h?p l?
  - Ch?n vai trò "Admin"
  - **Mong ??i**: ?? Ngay khi ch?n "Admin", hi?n th? c?nh báo "H? TH?NG CH? CHO PHÉP M?T ADMIN DUY NH?T"
  - **Mong ??i**: ? ComboBox t? ??ng reset v? vai trò c? ho?c không ch?n

- [ ] **Test 2.2**: C? s?a thành viên thành Admin (khi ?ã có Admin)
  - Ch?n 1 thành viên thông th??ng
  - Click "S?a"
  - Th? ??i vai trò thành "Admin"
  - **Mong ??i**: ?? Hi?n th? c?nh báo t??ng t? Test 2.1

---

### ?? PHASE 3: PHÂN QUY?N

- [ ] **Test 3.1**: Ng??i không ph?i Admin c? t?o Admin
  - ??ng nh?p b?ng tài kho?n "Thành viên" ho?c vai trò khác
  - Click "Thêm m?i"
  - **Mong ??i**: ? Không th?y option "Admin" trong dropdown vai trò

- [ ] **Test 3.2**: Ng??i không ph?i Admin c? s?a Admin
  - Ch?n thành viên có vai trò "Admin"
  - Click "S?a"
  - **Mong ??i**: ? ComboBox vai trò b? disabled
  - **Mong ??i**: ? Hi?n th? label warning "?? Ch? Admin m?i có quy?n thay ??i vai trò Admin"

- [ ] **Test 3.3**: Ng??i không ph?i Admin c? xóa Admin
  - Ch?n thành viên có vai trò "Admin"
  - Click "Xóa"
  - **Mong ??i**: ? Hi?n th? "? Ch? có Admin m?i ???c phép xóa Admin khác"

---

### ??? PHASE 4: B?O V? ADMIN DUY NH?T

- [ ] **Test 4.1**: C? xóa Admin duy nh?t
  - ??m b?o ch? có 1 Admin trong h? th?ng
  - Ch?n Admin ?ó
  - Click "Xóa"
  - **Mong ??i**: ?? Hi?n th? "KHÔNG TH? XÓA ADMIN DUY NH?T!"
  - **Mong ??i**: ? Không cho phép xóa

- [ ] **Test 4.2**: C? ??i vai trò Admin duy nh?t sang vai trò khác
  - Ch?n Admin duy nh?t
  - Click "S?a"
  - ??i vai trò thành "Thành viên" ho?c vai trò khác
  - **Mong ??i**: ?? Hi?n th? c?nh báo nghiêm tr?ng khi ch?n vai trò khác
  - **Mong ??i**: ?? "C?NH BÁO QUAN TR?NG! H? th?ng s? KHÔNG CÒN ADMIN..."
  - Click "No"
  - **Mong ??i**: ? ComboBox reset v? "Admin"
  - Click "S?a" l?i ? Ch?n vai trò khác ? Click "Yes"
  - Click "L?u"
  - **Mong ??i**: ?? Hi?n th? c?nh báo cu?i cùng "C?NH BÁO C?C K? NGHIÊM TR?NG"
  - Click "Yes"
  - **Mong ??i**: ? Cho phép l?u (v?i c?nh báo)

---

### ?? PHASE 5: NHI?U ADMIN (EDGE CASE)

**L?u ý**: Test này ch? có th? th?c hi?n b?ng cách can thi?p tr?c ti?p vào database

- [ ] **Test 5.1**: T?o th? công 2 Admin qua SQL
  ```sql
  -- Trong SQL Server Management Studio
  UPDATE ThanhVien SET VaiTro = N'Admin' WHERE MaTV = 1
  UPDATE ThanhVien SET VaiTro = N'Admin' WHERE MaTV = 2
  ```
  - Reload ?ng d?ng
  - Xem danh sách thành viên
  - **Mong ??i**: ? Th?y 2 Admin

- [ ] **Test 5.2**: Xóa 1 trong 2 Admin
  - Ch?n 1 Admin
  - Click "Xóa"
  - **Mong ??i**: ?? C?nh báo "B?N ?ANG XÓA M?T ADMIN" (không ph?i "Admin duy nh?t")
  - Click "Yes"
  - **Mong ??i**: ? Cho phép xóa

- [ ] **Test 5.3**: Xóa Admin cu?i cùng còn l?i
  - Ch?n Admin còn l?i
  - Click "Xóa"
  - **Mong ??i**: ?? "KHÔNG TH? XÓA ADMIN DUY NH?T"

---

### ?? PHASE 6: VALIDATION KHÁC

- [ ] **Test 6.1**: Email trùng l?p
  - T?o thành viên m?i v?i email ?ã t?n t?i
  - **Mong ??i**: ? "Email này ?ã ???c s? d?ng"

- [ ] **Test 6.2**: S?T trùng l?p
  - T?o thành viên m?i v?i S?T ?ã t?n t?i
  - **Mong ??i**: ? "S? ?i?n tho?i này ?ã ???c s? d?ng"

- [ ] **Test 6.3**: H? tên không h?p l?
  - Nh?p h? tên có ký t? ??c bi?t: "Nguyen@123"
  - **Mong ??i**: ? "H? tên ch? ???c ch?a ch? cái"

- [ ] **Test 6.4**: Email không h?p l?
  - Nh?p email: "test@"
  - **Mong ??i**: ? "Email không h?p l?"

- [ ] **Test 6.5**: S?T không h?p l?
  - Nh?p S?T: "123"
  - **Mong ??i**: ? "S? ?i?n tho?i không h?p l?"

- [ ] **Test 6.6**: Ngày sinh < 15 tu?i
  - Ch?n ngày sinh: 2020
  - **Mong ??i**: ? "Thành viên ph?i ít nh?t 15 tu?i"

- [ ] **Test 6.7**: Thi?u tr??ng b?t bu?c
  - B? tr?ng "H? tên"
  - Click "L?u"
  - **Mong ??i**: ? "Vui lòng nh?p h? tên"

---

### ?? PHASE 7: TRANSACTION & DATABASE

- [ ] **Test 7.1**: Xóa thành viên có d? li?u liên quan
  - T?o thành viên tham gia ho?t ??ng
  - Th? xóa thành viên ?ó
  - **Mong ??i**: ? "Không th? xóa! Thành viên ?ang có d? li?u liên quan"

- [ ] **Test 7.2**: Race condition (ki?m tra Transaction)
  - M? 2 form "Thêm m?i" cùng lúc
  - C? 2 ??u ch?n vai trò "Admin"
  - Click "L?u" g?n nh? ??ng th?i
  - **Mong ??i**: ? Ch? 1 trong 2 thành công, 1 b? reject

---

### ?? PHASE 8: UI/UX

- [ ] **Test 8.1**: TextBox ??i màu khi l?i
  - Nh?p email sai
  - Click ra ngoài (Leave event)
  - **Mong ??i**: ? TextBox chuy?n sang màu ?? nh?t

- [ ] **Test 8.2**: TextBox v? bình th??ng khi ?úng
  - S?a l?i email ?úng
  - Click ra ngoài
  - **Mong ??i**: ? TextBox chuy?n v? màu tr?ng

- [ ] **Test 8.3**: Icon MessageBox phù h?p
  - Ki?m tra các MessageBox hi?n th? ?úng icon:
    - Error: ?
    - Warning: ??
    - Information: ?
    - Question: ?

---

### ?? PHASE 9: TÍCH H?P

- [ ] **Test 9.1**: Sau khi thêm Admin, reload l?i UserTest
  - Thêm Admin thành công
  - Click "Làm m?i"
  - **Mong ??i**: ? Th?y Admin trong danh sách

- [ ] **Test 9.2**: Statistics c?p nh?t ?úng
  - Sau khi thêm/s?a/xóa
  - Ki?m tra tab "Th?ng kê"
  - **Mong ??i**: ? Bi?u ?? "Thành viên theo Vai trò" hi?n th? ?úng s? Admin

- [ ] **Test 9.3**: Export Excel có Admin
  - Click "Xu?t Excel"
  - M? file CSV
  - **Mong ??i**: ? Th?y c?t "Vai trò" có giá tr? "Admin"

---

## ?? TEST REPORT TEMPLATE

```
Ngày test: ___________
Ng??i test: ___________
Phiên b?n: 2.0

| Test ID | Mô t? | K?t qu? | Ghi chú |
|---------|-------|---------|---------|
| 1.1 | T?o Admin ??u tiên | ? Pass | |
| 1.2 | Ki?m tra Admin v?a t?o | ? Pass | |
| 2.1 | C? t?o Admin th? 2 | ? Pass | C?nh báo hi?n th? ?úng |
| ... | ... | ... | ... |

T?ng s? test: ___
S? test Pass: ___
S? test Fail: ___
Pass rate: ____%
```

---

## ?? BUG REPORT TEMPLATE

N?u phát hi?n l?i, s? d?ng template:

```
Bug ID: ___
Ngày phát hi?n: ___________
Ng??i phát hi?n: ___________

Mô t?:
___________

Các b??c tái hi?n:
1. ___________
2. ___________
3. ___________

K?t qu? th?c t?:
___________

K?t qu? mong ??i:
___________

Screenshot:
[?ính kèm ?nh màn hình n?u có]

?? ?u tiên: [ ] Critical [ ] High [ ] Medium [ ] Low
```

---

## ? ACCEPTANCE CRITERIA

D? án ???c coi là hoàn thành khi:

- [ ] ? T?t c? test trong PHASE 1-7 ??u PASS
- [ ] ? Không có bug Critical ho?c High
- [ ] ? UI/UX thân thi?n, rõ ràng
- [ ] ? Thông báo l?i h?u ích, d? hi?u
- [ ] ? Performance t?t (<2s cho m?i thao tác)
- [ ] ? Tài li?u ??y ??

---

**L?u ý quan tr?ng:**
- ?? Backup database tr??c khi test
- ?? Test trên môi tr??ng test, không test trên production
- ?? Ghi chép k? l??ng k?t qu? test

**Chúc may m?n!** ??
