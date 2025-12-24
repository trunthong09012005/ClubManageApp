# H??ng d?n s?a l?i giao di?n MeetingTest

## V?n ??
Giao di?n MeetingTest.cs b? x?u, các control b? trùng lên nhau khi pull code t? GitHub v? máy khác.

## Nguyên nhân
- S? d?ng `Anchor` k?t h?p v?i `Location` và `Size` c? ??nh
- DPI scaling khác nhau gi?a các máy
- Resolution màn hình khác nhau

## Gi?i pháp ?ã áp d?ng

### 1. S? d?ng TableLayoutPanel
? Thay th? vi?c ??t v? trí c? ??nh b?ng `TableLayoutPanel`
- C?t 1 (340px c? ??nh): ch?a `pnlGhiChu` và `pnlChucNang`
- C?t 2 (100% còn l?i): ch?a `pnlLich`
- Row t? ??ng chia theo t? l? 55%/45%

### 2. S? d?ng Dock thay vì Anchor
? Các panel chính dùng `Dock = Fill`
? Các button ?i?u h??ng dùng `Dock = Left/Right/Bottom`

### 3. C?u hình Git attributes
? ?ã c?u hình `.gitattributes` ?? tránh conflict:
```
*.Designer.cs merge=union
*.resx merge=union
```

## H??ng d?n cho Team

### Ng??i ?ã s?a (trên nhánh khoi)
1. ? ?ã hoàn thành s?a code
2. Commit và push lên GitHub:
```bash
git add .
git commit -m "Fix: S?a layout MeetingTest b?ng TableLayoutPanel - tránh l?i scale"
git push origin khoi
```

### Ng??i pull v?
1. **Backup code hi?n t?i** (n?u có thay ??i ch?a commit)
2. Pull code m?i:
```bash
git pull origin khoi
```
3. N?u có conflict trong Designer.cs:
   - Ch?n "Accept Incoming Changes" (l?y code m?i)
   - KHÔNG merge th? công Designer files
4. Clean và Rebuild solution:
   - Visual Studio ? Build ? Clean Solution
   - Build ? Rebuild Solution
5. Test l?i giao di?n

## L?u ý quan tr?ng

?? **KHÔNG S?A DESIGNER TR?C TI?P B?NG CODE EDITOR**
- Luôn dùng Visual Studio Designer (Design View)
- N?u ph?i s?a code, ph?i hi?u rõ v? TableLayoutPanel

?? **KHI CÓ CONFLICT**
- Designer.cs và .resx: luôn ch?n Accept Incoming (code m?i nh?t)
- N?u m?t thay ??i c?a mình, làm l?i trong Designer View

?? **KI?M TRA TR??C KHI PUSH**
1. Build thành công
2. Ch?y th? trên máy c?a mình
3. Ki?m tra không có hard-coded Size/Location

## Test giao di?n

### Checklist
- [ ] Panel "Thông tin s? ki?n" hi?n th? ?úng bên trái
- [ ] Panel "Ch?c n?ng" hi?n th? ?úng d??i panel thông tin
- [ ] L?ch hi?n th? ??y ?? bên ph?i
- [ ] Không có control nào b? che khu?t
- [ ] Resize form: t?t c? control t? ??ng scale ?úng t? l?
- [ ] Các button c?n ch?nh ??u nhau

## N?u v?n g?p l?i

1. Xóa file `.vs` folder (hidden folder)
2. Clean solution
3. Close Visual Studio
4. M? l?i và Rebuild

## Liên h?
N?u v?n g?p v?n ??, liên h? ng??i s?a code (nhánh khoi) ?? h? tr?.
