

	USE QL_CLB_LSC;
	GO





	-- =============================================
	-- 1️⃣ DỮ LIỆU BẢNG TRA CỨU (LOOKUP TABLES)
	-- =============================================



	-- Chức vụ
	INSERT INTO ChucVu (TenCV, MoTa)
	VALUES 
		(N'Chủ nhiệm', N'Lãnh đạo và điều hành CLB'),
		(N'Phó chủ nhiệm', N'Hỗ trợ chủ nhiệm điều hành CLB'),
		(N'Thành viên Ban chủ nhiệm', N'Thành viên ban điều hành'),
		(N'Trưởng ban', N'Lãnh đạo ban chuyên môn'),
		(N'Phó ban', N'Hỗ trợ trưởng ban'),
		(N'Thành viên', N'Thành viên chính thức của CLB');
	GO

	-- Ban chuyên môn
	INSERT INTO BanChuyenMon (TenBan, MoTa)
	VALUES 
		(N'Ban Truyền thông', N'Quản lý hình ảnh, fanpage và truyền thông CLB'),
		(N'Ban Sự kiện', N'Tổ chức và điều phối các sự kiện CLB'),
		(N'Ban Hậu cần', N'Chuẩn bị vật tư, tài sản và hậu cần'),
		(N'Ban Học thuật', N'Tổ chức các buổi học, workshop'),
		(N'Ban Đối ngoại', N'Kết nối với các tổ chức, doanh nghiệp');
	GO

	-- Loại hoạt động
	INSERT INTO LoaiHoatDong (TenLoaiHD, MoTa, MauSac)
	VALUES 
		(N'Tình nguyện', N'Hoạt động vì cộng đồng', N'#10b981'),
		(N'Học thuật', N'Chia sẻ kiến thức và kỹ năng', N'#3b82f6'),
		(N'Giải trí', N'Team building và giao lưu', N'#f59e0b'),
		(N'Văn hóa', N'Hoạt động văn hóa, văn nghệ', N'#ec4899'),
		(N'Thể thao', N'Hoạt động thể dục thể thao', N'#ef4444');
	GO

	-- Nguồn thu
	INSERT INTO NguonThu (TenNguon, MoTa, LoaiNguon)
	VALUES 
		(N'Tài trợ doanh nghiệp', N'Các công ty, doanh nghiệp tài trợ', N'Không thường xuyên'),
		(N'Đóng góp thành viên', N'Các khoản đóng góp định kỳ từ thành viên', N'Cố định'),
		(N'Hoạt động gây quỹ', N'Thu từ các sự kiện gây quỹ', N'Không thường xuyên'),
		(N'Tài trợ trường', N'Hỗ trợ từ nhà trường', N'Cố định'),
		(N'Bán hàng', N'Thu từ bán merchandise, vé', N'Không thường xuyên');
	GO

	-- Kỹ năng
	INSERT INTO KyNang (TenKN, MoTa, CapDo)
	VALUES 
		(N'Giao tiếp', N'Kỹ năng thuyết trình và trình bày', N'Cơ bản'),
		(N'Làm việc nhóm', N'Kỹ năng hợp tác và làm việc nhóm hiệu quả', N'Trung bình'),
		(N'Thiết kế đồ họa', N'Sử dụng Canva, Photoshop, Illustrator', N'Nâng cao'),
		(N'Quản lý thời gian', N'Lập kế hoạch và sắp xếp công việc', N'Trung bình'),
		(N'Lãnh đạo', N'Kỹ năng lãnh đạo và quản lý đội nhóm', N'Nâng cao'),
		(N'Video editing', N'Dựng và chỉnh sửa video', N'Nâng cao'),
		(N'Marketing', N'Kỹ năng marketing và PR', N'Trung bình'),
		(N'Quản lý sự kiện', N'Tổ chức và điều phối sự kiện', N'Nâng cao');
	GO


	-- =============================================
	-- 2️⃣ THÀNH VIÊN
	-- =============================================

	INSERT INTO ThanhVien (HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, Email, DiaChi, VaiTro, MaCV, MaBan)
	VALUES
		-- Ban chủ nhiệm
		(N'Nguyễn Vương Khang', '2004-02-15', N'Nam', N'DHKTPM17A', N'Công nghệ thông tin', '0912345678', N'huytm@student.hcmute.edu.vn', N'Quận 7, TP.HCM', N'Chủ nhiệm', 0, NULL),
		(N'Nguyễn Thị Lan', '2005-05-10', N'Nữ', N'DHKTPM17A', N'Công nghệ thông tin', '0987654321', N'lannt@student.hcmute.edu.vn', N'Quận 5, TP.HCM', N'Phó chủ nhiệm', 1, NULL),
    
		-- Trưởng các ban
		(N'Lê Quốc Bảo', '2005-09-21', N'Nam', N'DHKTPM17B', N'Công nghệ thông tin', '0977112233', N'baolq@student.hcmute.edu.vn', N'Quận 10, TP.HCM', N'Trưởng ban', 3, 1),
		(N'Phạm Thị Mai', '2005-03-18', N'Nữ', N'DHKTPM17B', N'Công nghệ thông tin', '0965432109', N'maipt@student.hcmute.edu.vn', N'Quận 3, TP.HCM', N'Trưởng ban', 3, 2),
		(N'Hoàng Văn Nam', '2004-11-25', N'Nam', N'DHKTPM17C', N'Công nghệ thông tin', '0923456789', N'namhv@student.hcmute.edu.vn', N'Quận 1, TP.HCM', N'Trưởng ban', 3, 3),
    
		-- Thành viên các ban
		(N'Võ Thị Hoa', '2005-07-08', N'Nữ', N'DHKTPM17A', N'Công nghệ thông tin', '0934567890', N'hoavt@student.hcmute.edu.vn', N'Quận 6, TP.HCM', N'Thành viên', 5, 1),
		(N'Đặng Minh Tuấn', '2005-01-30', N'Nam', N'DHKTPM17B', N'Công nghệ thông tin', '0945678901', N'tuandm@student.hcmute.edu.vn', N'Quận 8, TP.HCM', N'Thành viên', 5, 2),
		(N'Trương Thị Lan Anh', '2004-12-12', N'Nữ', N'DHKTPM17C', N'Công nghệ thông tin', '0956789012', N'anhttl@student.hcmute.edu.vn', N'Quận 12, TP.HCM', N'Thành viên', 5, 3),
		(N'Ngô Văn Đức', '2005-04-20', N'Nam', N'DHKTPM17A', N'Công nghệ thông tin', '0967890123', N'ducnv@student.hcmute.edu.vn', N'Thủ Đức, TP.HCM', N'Thành viên', 5, 1),
		(N'Bùi Thị Ngọc', '2005-08-15', N'Nữ', N'DHKTPM17B', N'Công nghệ thông tin', '0978901234', N'ngocbt@student.hcmute.edu.vn', N'Bình Thạnh, TP.HCM', N'Thành viên', 5, 2);
	
	INSERT INTO ThanhVien (HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, Email, DiaChi, VaiTro, MaCV, MaBan)
	VALUES
		(N'Trần Trung Thông', '2005-11-13', N'Nam', '12A11', 'CNTT', '0912345688', 'thongtt@example.com', 'Hà Nội', 'Admin', 1, 1);

	-- Cập nhật Trưởng ban
	UPDATE BanChuyenMon SET TruongBan = 3 WHERE MaBan = 1; 
	UPDATE BanChuyenMon SET TruongBan = 4 WHERE MaBan = 2; 
	UPDATE BanChuyenMon SET TruongBan = 5 WHERE MaBan = 3; 
	GO

	GO

	-- =============================================
	-- 3️⃣ TÀI KHOẢN
	-- =============================================
	SELECT* FROM TaiKhoan;


	INSERT INTO TaiKhoan (TenDN, MatKhau, MaTV, QuyenHan)
	VALUES 
		(N'khang', N'123456', 12, N'Quản trị viên'),
		(N'lantn', N'123456', 13, N'Thành viên'),
		(N'baolq', N'123456', 14, N'Thành viên'),
		(N'maipt', N'123456', 15, N'Thành viên'),
		(N'namhv', N'123456', 16, N'Thành viên'),
		(N'hoavt', N'123456', 17, N'Thành viên'),
		(N'tuandm', N'123456', 18, N'Thành viên'),
		(N'anhttl', N'123456', 19, N'Thành viên'),
		(N'ducnv', N'123456', 20, N'Thành viên'),
		(N'ngocbt', N'123456', 21, N'Thành viên'),
		(N'thong', N'123456', 26, N'Admin');
	GO

	-- Hash mật khẩu
	DECLARE @TenDN NVARCHAR(50), @MatKhau NVARCHAR(255), @HashedPW NVARCHAR(64);

	DECLARE cur CURSOR FOR SELECT TenDN, MatKhau FROM TaiKhoan WHERE LEN(MatKhau) < 64;
	OPEN cur;

	FETCH NEXT FROM cur INTO @TenDN, @MatKhau;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @HashedPW = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @MatKhau), 2);
		UPDATE TaiKhoan SET MatKhau = LOWER(@HashedPW) WHERE TenDN = @TenDN;
		FETCH NEXT FROM cur INTO @TenDN, @MatKhau;
	END;

	CLOSE cur;
	DEALLOCATE cur;
	GO




	-- =============================================
	-- 4️⃣ QUAN HỆ BAN - THÀNH VIÊN
	-- =============================================

	INSERT INTO BanChuyenMon_ThanhVien (MaBan, MaTV, VaiTro)
	VALUES
		(1, 12, N'Trưởng ban'),
		(1, 13, N'Thành viên'),
		(1, 14, N'Thành viên'),
		(2, 15, N'Trưởng ban'),
		(2, 16, N'Thành viên'),
		(2, 17, N'Thành viên'),
		(3, 18, N'Trưởng ban'),
		(3, 19, N'Thành viên'),
		(1, 26, N'Admin');

		SELECT MaTV, HoTen FROM ThanhVien;
	-- =============================================
	-- 5️⃣ KỸ NĂNG THÀNH VIÊN
	-- =============================================
	INSERT INTO ThanhVien_KyNang (MaTV, MaKN, Diem, CapDoHienTai)
	VALUES
		(12, 4, 8.5, N'Nâng cao'), -- Nguyễn Vương Khang - Quản lý thời gian
		(12, 5, 9.0, N'Nâng cao'), -- Nguyễn Vương Khang - Lãnh đạo
		(13, 1, 9.5, N'Nâng cao'), -- Nguyễn Thị Lan - Giao tiếp
		(13, 7, 8.0, N'Trung bình'), -- Nguyễn Thị Lan - Quản lý sự kiện
		(14, 3, 9.0, N'Nâng cao'), -- Lê Quốc Bảo - Thiết kế
		(14, 6, 8.0, N'Trung bình'), -- Lê Quốc Bảo - Marketing
		(17, 3, 7.5, N'Trung bình'), -- Võ Thị Hoa - Thiết kế
		(18, 7, 8.5, N'Nâng cao'), -- Đặng Minh Tuấn - Quản lý sự kiện
		(19, 2, 9.0, N'Nâng cao'), -- Trương Thị Lan Anh - Làm việc nhóm
		(20, 5, 8.0, N'Trung bình'), -- Ngô Văn Đức - Video editing
		(21, 6, 7.0, N'Trung bình'); -- Bùi Thị Ngọc - Marketing
	GO

	-- =============================================
	-- 6️⃣ HOẠT ĐỘNG
	-- =============================================



	INSERT INTO HoatDong (TenHD, NgayToChuc, GioBatDau, GioKetThuc, DiaDiem, MoTa, MaLoaiHD, NguoiPhuTrach, KinhPhiDuKien, SoLuongToiDa, TrangThai)
	VALUES 
		(N'Chiến dịch Mùa hè xanh 2025', '2025-07-10', '07:00', '17:00', N'Củ Chi, TP.HCM', 
		 N'Hoạt động tình nguyện giúp đỡ người dân địa phương, trồng cây xanh và dọn dẹp môi trường', 
		0,13, 5000000, 50, N'Hoàn thành'),
    
		(N'Workshop: Kỹ năng thuyết trình chuyên nghiệp', '2025-08-15', '14:00', '17:00', N'Hội trường B - Tầng 3', 
		 N'Buổi workshop nâng cao kỹ năng thuyết trình và giao tiếp hiệu quả với diễn giả chuyên nghiệp', 
		 1,14, 2000000, 100, N'Hoàn thành'),
    
		(N'Dã ngoại CLB - Vũng Tàu', '2025-09-05', '06:00', '20:00', N'Bãi Sau, Vũng Tàu', 
		 N'Chuyến du lịch team building 1 ngày tại Vũng Tàu với các hoạt động gắn kết', 
		 2,15, 8000000, 40, N'Hoàn thành'),
    
		(N'Ngày hội văn hóa sinh viên', '2025-11-20', '08:00', '18:00', N'Sân khấu trung tâm', 
		 N'Tổ chức ngày hội văn hóa với các tiết mục văn nghệ, ẩm thực và trò chơi dân gian', 
		 3,16, 10000000, 200, N'Đang chuẩn bị'),
    
		(N'Giải bóng đá CLB lần thứ 5', '2025-12-10', '07:00', '12:00', N'Sân bóng Thể Công', 
		 N'Giải bóng đá giao hữu giữa các thành viên CLB', 
		 4, 17, 3000000, 30, N'Đang chuẩn bị');
	GO
	select*from ThanhVien;
	-- Đăng ký và tham gia hoạt động
	INSERT INTO DangKyHoatDong (MaTV, MaHD, TrangThai)
	VALUES
		(17, 2, N'Đã duyệt'), (13, 2, N'Đã duyệt'), (21, 2, N'Đã duyệt'),
		(18, 2, N'Đã duyệt'), (14, 2, N'Đã duyệt'),
		(19, 3, N'Đã duyệt'), (15, 3, N'Đã duyệt'), (18, 3, N'Đã duyệt'),
		(20, 3, N'Đã duyệt'), (16, 3, N'Đã duyệt'),
		(21, 4, N'Đã duyệt'), (20, 4, N'Đã duyệt'), (19, 4, N'Đã duyệt'),
		(16, 5, N'Chờ duyệt'), (21, 5, N'Chờ duyệt');


	INSERT INTO ThamGia (MaHD, MaTV, VaiTroTrongHD, DiemDanh, DiemThuong)
	VALUES
		-- Hoạt động 2 (Chiến dịch Mùa hè xanh 2025)
		(2, 17, N'Trưởng đoàn', 1, 10),
		(2, 13, N'Thành viên', 1, 8),
		(2, 14, N'Thành viên', 1, 8),
		(2, 15, N'Thành viên', 1, 7),
		(2, 16, N'Thành viên', 0, 0),

		-- Hoạt động 3 (Workshop: Kỹ năng thuyết trình chuyên nghiệp)
		(3, 17, N'MC', 1, 10),
		(3, 18, N'Tổ chức', 1, 9),
		(3, 19, N'Thành viên', 1, 8),
		(3, 20, N'Thành viên', 1, 8),

		-- Hoạt động 4 (Dã ngoại CLB - Vũng Tàu)
		(4, 13, N'Trưởng nhóm', 1, 9),
		(4, 14, N'Thành viên', 1, 8),
		(4, 15, N'Thành viên', 1, 8);
	GO



	-- =============================================
	-- 7️⃣ DỰ ÁN
	-- =============================================


	INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc, MucDoUuTien, TienDo, TrangThai)
	VALUES
		(N'Hệ thống quản lý CLB', 
		 N'Xây dựng website và ứng dụng quản lý thành viên, hoạt động và tài chính của CLB', 
		 '2025-05-01', '2025-12-31', N'Cao', 65, N'Đang thực hiện'),
    
		(N'Chiến dịch tuyển thành viên K18', 
		 N'Lập kế hoạch và triển khai chiến dịch tuyển thành viên mới khóa 18', 
		 '2025-09-01', '2025-10-15', N'Cao', 80, N'Đang thực hiện'),
    
		(N'Kế hoạch hoạt động 2026', 
		 N'Lập kế hoạch chi tiết các hoạt động cho năm 2026', 
		 '2025-10-01', '2025-11-30', N'Trung bình', 40, N'Đang thực hiện');
	GO

	INSERT INTO PhanCong (MaTV, MaDA, NhiemVu, TrangThai, NgayHetHan)
	VALUES
		(12, 1, N'Quản lý dự án và phối hợp tổng thể', N'Đang thực hiện', '2025-12-31'),
		(14, 1, N'Thiết kế cơ sở dữ liệu và UI/UX', N'Hoàn thành', '2025-08-30'),
		(17, 1, N'Thiết kế giao diện và banner', N'Đang thực hiện', '2025-11-30'),
		(20, 1, N'Phát triển backend', N'Đang thực hiện', '2025-12-15'),
		(13, 2, N'Lập kế hoạch tuyển thành viên', N'Hoàn thành', '2025-09-15'),
		(15, 2, N'Tổ chức sự kiện tuyển thành viên', N'Đang thực hiện', '2025-10-15'),
		(12, 3, N'Tổng hợp ý kiến và lập kế hoạch', N'Đang thực hiện', '2025-11-30');
	GO



	-- =============================================
	-- 8️⃣ THU CHI
	-- =============================================



	INSERT INTO ThuChi (LoaiGD, SoTien, NgayGD, NoiDung, NguoiThucHien, MaNguon, MaHD)
	VALUES 
		(N'Thu', 5000000, '2025-06-15', N'Tài trợ từ công ty ABC Technology', 12, 1, NULL),
		(N'Thu', 3000000, '2025-07-01', N'Đóng góp quỹ CLB học kỳ 1', 13, 2, NULL),
		(N'Thu', 2000000, '2025-08-10', N'Tài trợ workshop kỹ năng mềm', 13, 1, 3),
		(N'Chi', 4500000, '2025-07-08', N'Chi phí hoạt động Mùa hè xanh', 12, NULL, 2),
		(N'Chi', 1800000, '2025-08-14', N'Chi phí tổ chức workshop', 13, NULL, 3),
		(N'Chi', 7500000, '2025-09-04', N'Chi phí dã ngoại Vũng Tàu', 14, NULL, 4),
		(N'Chi', 500000, '2025-09-20', N'Mua vật tư văn phòng CLB', 15, NULL, NULL);
	GO

	INSERT INTO ThuChi_ChiTiet (MaGD, NoiDung, SoTien, DonViTinh, SoLuong)
	VALUES
		(2, N'Tiền mặt nhận tài trợ', 5000000, N'VNĐ', 1),
		(3, N'Đóng góp từ 30 thành viên', 3000000, N'VNĐ/người', 30),
		(4, N'Tài trợ diễn giả và địa điểm', 2000000, N'VNĐ', 1),
		(5, N'Chi phí di chuyển và ăn uống', 2500000, N'VNĐ', 1),
		(5, N'Mua cây giống và dụng cụ', 2000000, N'VNĐ', 1),
		(6, N'Thuê diễn giả', 1000000, N'VNĐ', 1),
		(6, N'In tài liệu và nước uống', 800000, N'VNĐ', 1),
		(7, N'Thuê xe và chi phí di chuyển', 3000000, N'VNĐ', 1),
		(7, N'Chi phí ăn uống', 2500000, N'VNĐ', 1),
		(7, N'Thuê địa điểm và thiết bị', 2000000, N'VNĐ', 1),
		(8, N'Giấy A4, bút viết', 200000, N'Bộ', 5),
		(8, N'File hồ sơ, kẹp tài liệu', 300000, N'Bộ', 3);
	GO

	
	-- =============================================
	-- 9️⃣ TÀI SẢN
	-- =============================================


	INSERT INTO TaiSan (TenTS, MaTS_Code, SoLuong, DonViTinh, TinhTrang, GiaTriMua, NguoiQuanLy, ViTri)
	VALUES 
		(N'Loa di động JBL', N'TS001', 2, N'Bộ', N'Tốt', 3000000, 17, N'Phòng CLB - Tầng 2'),
		(N'Bàn gấp nhựa', N'TS002', 10, N'Cái', N'Tốt', 2000000, 17, N'Kho CLB - Tầng 1'),
		(N'Máy chiếu Epson', N'TS003', 1, N'Cái', N'Tốt', 8000000, 17, N'Phòng CLB - Tầng 2'),
		(N'Laptop Dell Inspiron', N'TS004', 2, N'Cái', N'Bình thường', 15000000, 12, N'Phòng CLB - Tầng 2'),
		(N'Camera Sony', N'TS005', 1, N'Cái', N'Tốt', 12000000, 14, N'Ban Truyền thông'),
		(N'Micro không dây', N'TS006', 3, N'Bộ', N'Tốt', 4500000, 15, N'Phòng CLB - Tầng 2'),
		(N'Banner cố định CLB', N'TS007', 5, N'Cái', N'Bình thường', 2500000, 14, N'Kho CLB - Tầng 1'),
		(N'Bảng flipchart', N'TS008', 2, N'Cái', N'Tốt', 1000000, 15, N'Phòng họp'),
		(N'Máy in Canon', N'TS009', 1, N'Cái', N'Cần sửa chữa', 5000000, 13, N'Phòng CLB - Tầng 2');
	GO

	-- Lịch sử mượn trả
	INSERT INTO MuonTraTaiSan (MaTS, MaTV, NgayMuon, NgayTraDuKien, NgayTraThucTe, MucDich, TrangThai)
	VALUES
		(2, 16, '2025-08-14 08:00', '2025-08-14', '2025-08-14 18:00', N'Workshop kỹ năng mềm', N'Đã trả'),
		(3, 13, '2025-08-14 13:00', '2025-08-14', '2025-08-14 17:30', N'Workshop kỹ năng mềm', N'Đã trả'),
		(4, 17, '2025-09-04 05:00', '2025-09-05', '2025-09-05 21:00', N'Ghi hình dã ngoại', N'Đã trả'),
		(5, 18, '2025-11-01 14:00', '2025-11-15', NULL, N'Chuẩn bị sự kiện tuyển thành viên', N'Đang mượn'),
		(6, 19, '2025-11-10 09:00', '2025-11-20', NULL, N'Ngày hội văn hóa', N'Đang mượn');
	GO


	-- =============================================
	-- 🔟 LỊCH HỌP
	-- =============================================


	INSERT INTO LichHop (TieuDe, NgayHop, DiaDiem, NoiDung, NguoiChuTri, TrangThai)
	VALUES 
		(N'Họp tổng kết quý I/2025', '2025-04-15 14:00', N'Phòng họp A101', 
		 N'Tổng kết hoạt động quý I và lập kế hoạch quý II', 12, N'Hoàn thành'),
    
		(N'Họp chuẩn bị Mùa hè xanh', '2025-06-20 15:00', N'Phòng CLB', 
		 N'Thảo luận kế hoạch chi tiết cho chiến dịch Mùa hè xanh', 12, N'Hoàn thành'),
    
		(N'Họp Ban chủ nhiệm tháng 8', '2025-08-05 16:00', N'Phòng họp B201', 
		 N'Đánh giá hoạt động tháng 7 và kế hoạch tháng 8', 12, N'Hoàn thành'),
    
		(N'Họp toàn thể CLB - Kế hoạch tuyển thành viên', '2025-09-15 14:00', N'Hội trường lớn', 
		 N'Công bố kế hoạch tuyển thành viên mới và phân công nhiệm vụ', 12, N'Hoàn thành'),
    
		(N'Họp Ban chủ nhiệm tháng 11', '2025-11-18 15:00', N'Phòng họp A101', 
		 N'Thảo luận kế hoạch cuối năm và chuẩn bị đại hội', 12, N'Sắp diễn ra');
	GO

	INSERT INTO DiemDanhLichHop (MaLH, MaTV, TrangThai, GioCheckIn)
	VALUES 
		-- Họp tổng kết quý I
		(1, 12, N'Có mặt', '2025-04-15 13:55'),
		(1, 13, N'Có mặt', '2025-04-15 14:00'),
		(1, 14, N'Vắng', NULL),
		(1, 15, N'Có mặt', '2025-04-15 14:02'),
		(1, 16, N'Trễ', '2025-04-15 14:15'),

		-- Họp chuẩn bị Mùa hè xanh
		(2, 12, N'Có mặt', '2025-06-20 14:58'),
		(2, 13, N'Có mặt', '2025-06-20 15:00'),
		(2, 14, N'Có mặt', '2025-06-20 15:05'),
		(2, 15, N'Có mặt', '2025-06-20 14:55'),

		-- Họp BCN tháng 8
		(3, 12, N'Có mặt', '2025-08-05 15:55'),
		(3, 13, N'Có mặt', '2025-08-05 16:00'),

		-- Họp toàn thể
		(4, 12, N'Có mặt', '2025-09-15 13:50'),
		(4, 13, N'Có mặt', '2025-09-15 13:55'),
		(4, 14, N'Có mặt', '2025-09-15 14:00'),
		(4, 15, N'Có mặt', '2025-09-15 14:00'),
		(4, 16, N'Có mặt', '2025-09-15 14:05'),
		(4, 17, N'Trễ', '2025-09-15 14:20'),
		(4, 18, N'Có mặt', '2025-09-15 13:58'),
		(4, 19, N'Vắng', NULL),
		(4, 20, N'Có mặt', '2025-09-15 14:03'),
		(4, 21, N'Có phép', NULL);
	GO


	GO

	-- =============================================
	-- 1️⃣1️⃣ KHEN THƯỞNG & KỶ LUẬT
	-- =============================================



	INSERT INTO KhenThuong (MaTV, LyDo, HinhThuc, GiaTriThuong, NgayKT, NguoiLap)
	VALUES 
		(12, N'Lãnh đạo xuất sắc CLB trong năm học 2024-2025', N'Bằng khen', 1000000, '2025-06-15', 13),
		(13, N'Tổ chức workshop kỹ năng mềm thành công tốt đẹp', N'Giấy khen', 500000, '2025-08-20', 12),
		(14, N'Thiết kế ấn tượng cho chiến dịch tuyển thành viên', N'Giấy khen', 300000, '2025-09-25', 12),
		(15, N'Tích cực và nhiệt tình trong tổ chức các sự kiện', N'Quà tặng', 200000, '2025-10-10', 12),
		(18, N'Hoàn thành xuất sắc nhiệm vụ được giao', N'Giấy khen', 0, '2025-09-30', 13);
	GO


	INSERT INTO KyLuat (MaTV, LyDo, HinhThuc, ThoiGianKyLuat, NgayKL, NguoiLap)
	VALUES 
		(20, N'Vắng mặt không phép 3 buổi họp liên tiếp', N'Cảnh cáo', NULL, '2025-10-05', 12),
		(21, N'Không hoàn thành nhiệm vụ đúng thời hạn', N'Khiển trách', NULL, '2025-08-25', 12);
	GO


	GO

	-- =============================================
	-- 1️⃣2️⃣ FEEDBACK & GIAO TIẾP
	-- =============================================



	INSERT INTO Feedback (MaTV, MaHD, NoiDung, DanhGiaSao, TrangThai)
	VALUES 
		(13, 2, N'Hoạt động rất ý nghĩa! Được giúp đỡ cộng đồng và hiểu hơn về cuộc sống nông thôn.', 5, N'Đã xử lý'),
		(14, 2, N'Tổ chức tốt nhưng nên có thêm thời gian nghỉ giữa các hoạt động.', 4, N'Đã xử lý'),
		(16, 3, N'Workshop rất bổ ích! Diễn giả nhiệt tình và nội dung thiết thực.', 5, N'Đã xử lý'),
		(17, 3, N'Nên tổ chức thêm các buổi thực hành để áp dụng kiến thức vừa học.', 4, N'Đang xử lý'),
		(15, 4, N'Chuyến đi vui vẻ và ý nghĩa, giúp các thành viên gắn kết hơn.', 5, N'Đã xử lý'),
		(19, 3, N'Nội dung hay nhưng thời gian hơi ngắn, muốn học thêm.', 4, N'Đã nhận');
	GO


	INSERT INTO ThongBao (TieuDe, NoiDung, LoaiThongBao, NguoiDang, DoiTuong, TrangThai)
	VALUES
		(N'Lịch họp tháng 11/2025', 
		 N'Kính gửi toàn thể thành viên CLB,

	CLB xin thông báo lịch họp Ban chủ nhiệm như sau:
	- Thời gian: 15h00 ngày 18/11/2025
	- Địa điểm: Phòng họp A101
	- Nội dung: Thảo luận kế hoạch cuối năm

	Đề nghị các thành viên BCN sắp xếp thời gian tham dự đầy đủ.

	Trân trọng!', 
		 N'Thông báo chung', 12, N'Ban chủ nhiệm', N'Đã gửi'),
    
		(N'Thông báo đóng góp quỹ CLB học kỳ 2', 
		 N'Kính gửi toàn thể thành viên,

	CLB thông báo về việc đóng góp quỹ học kỳ 2 năm học 2025-2026:
	- Mức đóng góp: 100.000đ/người
	- Thời hạn: Trước 30/11/2025
	- Hình thức: Chuyển khoản hoặc nộp trực tiếp

	Mọi thắc mắc xin liên hệ Ban Tài chính.

	Trân trọng!', 
		 N'Nhắc nhở', 13, N'Tất cả', N'Đã gửi'),
    
		(N'🎉 Khai mạc chiến dịch tuyển thành viên K18', 
		 N'CLB chính thức mở đơn đăng ký tuyển thành viên khóa 18!

	📅 Thời gian: 01/10 - 15/10/2025
	📝 Đăng ký tại: bit.ly/CLBLSC_K18
	🎁 Ưu đãi cho 50 người đăng ký đầu tiên

	Cùng tham gia và phát triển bản thân nhé!', 
		 N'Sự kiện', 13, N'Tất cả', N'Đã gửi'),
    
		(N'⚠️ KHẨN - Thay đổi địa điểm Ngày hội văn hóa', 
		 N'Kính gửi các thành viên,

	Do thời tiết không thuận lợi, địa điểm tổ chức Ngày hội văn hóa chuyển từ Sân khấu trung tâm sang Hội trường A.

	Thời gian giữ nguyên: 8h00 ngày 20/11/2025.

	Xin lỗi vì sự bất tiện này!', 
		 N'Khẩn cấp', 12, N'Tất cả', N'Đã gửi');
	GO

	INSERT INTO TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung, TrangThai)
	VALUES
		(12, 13, N'Lan ơi, chuẩn bị tài liệu cho workshop nhé!', N'Đã đọc'),
		(13, 12, N'Vâng anh Huy, em đã chuẩn bị xong ạ!', N'Đã đọc'),
		(14, 12, N'Anh ơi, em đã hoàn thành thiết kế banner rồi ạ!', N'Đã đọc'),
		(12, 14, N'Ok em, anh check và gửi feedback chiều nay nhé!', N'Đã đọc'),
		(15, 17, N'Tuấn ơi, ngày mai họp Ban sự kiện nhé!', N'Đã đọc'),
		(17, 15, N'Dạ em nhận được, em sẽ có mặt đúng giờ ạ!', N'Đã đọc'),
		(13, 16, N'Hoa nhớ gửi file thiết kế poster cho chị trước 5h chiều nay nha!', N'Chưa đọc'),
		(16, 18, N'Lan Anh chuẩn bị checklist vật tư cho sự kiện nhé!', N'Đã đọc');
	GO



	-- =============================================
	-- 1️⃣3️⃣ FILE ĐÍNH KÈM
	-- =============================================


	INSERT INTO FileDinhKem (TenFile, DuongDan, KichThuoc, LoaiFile, MaDA, MaHD, NguoiTai)
	VALUES
		(N'Kế hoạch Mùa hè xanh 2025.pdf', N'/files/projects/ke-hoach-mua-he-xanh-2025.pdf', 2048576, N'PDF', NULL, 2, 12),
		(N'Slide Workshop Kỹ năng mềm.pptx', N'/files/activities/slide-workshop-ky-nang-mem.pptx', 5242880, N'PowerPoint', NULL, 3, 13),
		(N'Báo cáo tài chính Q2-2025.xlsx', N'/files/reports/bao-cao-tai-chinh-q2-2025.xlsx', 1048576, N'Excel', NULL, NULL, 13),
		(N'Database Schema v1.0.sql', N'/files/projects/database-schema-v1.sql', 524288, N'SQL', 1, NULL, 14),
		(N'UI Design CLB Management.fig', N'/files/projects/ui-design-clb-management.fig', 15728640, N'Figma', 1, NULL, 16),
		(N'Video recap Dã ngoại Vũng Tàu.mp4', N'/files/activities/video-recap-da-ngoai-vung-tau.mp4', 104857600, N'Video', NULL, 4, 19);
	GO



	-- =============================================
	-- 1️⃣4️⃣ BÁO CÁO
	-- =============================================



	INSERT INTO BaoCao (TieuDe, LoaiBC, NoiDung, TuNgay, DenNgay, NguoiLap, TrangThai)
	VALUES
		(N'Báo cáo hoạt động Quý II/2025', N'Quý', 
		 N'Tổng kết các hoạt động từ tháng 4 đến tháng 6 năm 2025:
     
	1. Các hoạt động đã triển khai: 3 hoạt động lớn
	2. Số thành viên tham gia: 45 lượt
	3. Kinh phí sử dụng: 15.000.000 VNĐ
	4. Đánh giá chung: Các hoạt động đều đạt mục tiêu đề ra', 
		 '2025-04-01', '2025-06-30', 12, N'Hoàn thành'),
    
		(N'Báo cáo tài chính tháng 9/2025', N'Tháng', 
		 N'Tình hình thu chi tháng 9/2025:
     
	- Tổng thu: 5.000.000 VNĐ
	- Tổng chi: 8.000.000 VNĐ
	- Số dư đầu kỳ: 15.000.000 VNĐ
	- Số dư cuối kỳ: 12.000.000 VNĐ
     
	Chi tiết đính kèm file Excel.', 
		 '2025-09-01', '2025-09-30', 13, N'Hoàn thành'),
    
		(N'Báo cáo chiến dịch Mùa hè xanh 2025', N'Hoạt động', 
		 N'Tổng kết chiến dịch Mùa hè xanh 2025:
     
	- Số thành viên tham gia: 35 người
	- Số người dân được hỗ trợ: 150 hộ
	- Số cây xanh trồng: 500 cây
	- Đánh giá: Hoạt động thành công tốt đẹp, nhận được sự ủng hộ từ địa phương', 
		 '2025-07-10', '2025-07-10', 12, N'Hoàn thành'),
    
		(N'Kế hoạch hoạt động năm 2026', N'Năm', 
		 N'Dự thảo kế hoạch hoạt động năm 2026 (đang hoàn thiện)', 
		 '2026-01-01', '2026-12-31', 12, N'Nháp');
	GO


	-- =============================================
	-- 1️⃣5️⃣ ĐIỂM RÈN LUYỆN
	-- =============================================



	INSERT INTO DiemRenLuyen (MaTV, HocKy, NamHoc, Diem, XepLoai, GhiChu)
	VALUES 
		(12, N'HK1', N'2024-2025', 95, N'Xuất sắc', N'Thực hiện tích cực và có nhiều đóng góp'),
		(12, N'HK2', N'2024-2025', 92, N'Xuất sắc', N'Lãnh đạo CLB hiệu quả'),
		(13, N'HK1', N'2024-2025', 88, N'Giỏi', N'Nhiệt tình trong công việc'),
		(13, N'HK2', N'2024-2025', 90, N'Xuất sắc', N'Tổ chức workshop thành công'),
		(14, N'HK1', N'2024-2025', 85, N'Giỏi', N'Kỹ năng thiết kế tốt'),
		(14, N'HK2', N'2024-2025', 87, N'Giỏi', N'Đóng góp nhiều cho Ban Truyền thông'),
		(15, N'HK1', N'2024-2025', 82, N'Giỏi', N'Tích cực tham gia hoạt động'),
		(15, N'HK2', N'2024-2025', 85, N'Giỏi', N'Cải thiện kỹ năng tổ chức sự kiện'),
		(16, N'HK1', N'2024-2025', 78, N'Khá', N'Cần tích cực hơn'),
		(16, N'HK2', N'2024-2025', 80, N'Khá', N'Có sự tiến bộ'),
		(17, N'HK1', N'2024-2025', 86, N'Giỏi', N'Kỹ năng thiết kế phát triển tốt'),
		(18, N'HK1', N'2024-2025', 84, N'Giỏi', N'Tích cực tham gia'),
		(19, N'HK1', N'2024-2025', 70, N'Trung bình', N'Vắng nhiều buổi họp'),
		(20, N'HK1', N'2024-2025', 88, N'Giỏi', N'Hoàn thành tốt nhiệm vụ'),
		(21, N'HK1', N'2024-2025', 82, N'Giỏi', N'Nhiệt tình và năng động');
	GO


	-- =============================================
	-- 1️⃣6️⃣ LỊCH SỬ THAO TÁC
	-- =============================================



	INSERT INTO LichSuThaoTac (MaTV, TenBang, LoaiThaoTac, KhoaChinh, NoiDung)
	VALUES
		(12, N'HoatDong', N'Thêm', N'MaHD=2', N'Tạo hoạt động Chiến dịch Mùa hè xanh 2025'),
		(13, N'ThanhVien', N'Cập nhật', N'MaTV=13', N'Cập nhật số điện thoại thành viên'),
		(12, N'ThuChi', N'Thêm', N'MaGD=3', N'Thêm khoản thu tài trợ 5.000.000đ'),
		(14, N'TaiSan', N'Thêm', N'MaTS=5', N'Nhập camera Sony mới'),
		(13, N'HoatDong', N'Thêm', N'MaHD=3', N'Tạo Workshop kỹ năng mềm'),
		(12, N'DuAn', N'Thêm', N'MaDA=1', N'Tạo dự án Hệ thống quản lý CLB'),
		(12, N'LichHop', N'Thêm', N'MaLH=1', N'Tạo lịch họp tổng kết quý I'),
		(13, N'ThongBao', N'Thêm', N'MaTB=1', N'Đăng thông báo lịch họp tháng 11'),
		(12, N'KhenThuong', N'Thêm', N'MaKT=1', N'Khen thưởng thành viên xuất sắc'),
		(12, N'HoatDong', N'Cập nhật', N'MaHD=2', N'Cập nhật trạng thái hoạt động thành Hoàn thành');
	GO



	-- Thống kê tổng quan
	SELECT 
		N'Chức vụ' AS [Bảng], COUNT(*) AS [Số bản ghi] FROM ChucVu
	UNION ALL SELECT N'Ban chuyên môn', COUNT(*) FROM BanChuyenMon
	UNION ALL SELECT N'Loại hoạt động', COUNT(*) FROM LoaiHoatDong
	UNION ALL SELECT N'Nguồn thu', COUNT(*) FROM NguonThu
	UNION ALL SELECT N'Kỹ năng', COUNT(*) FROM KyNang
	UNION ALL SELECT N'Thành viên', COUNT(*) FROM ThanhVien
	UNION ALL SELECT N'Tài khoản', COUNT(*) FROM TaiKhoan
	UNION ALL SELECT N'Hoạt động', COUNT(*) FROM HoatDong
	UNION ALL SELECT N'Dự án', COUNT(*) FROM DuAn
	UNION ALL SELECT N'Thu chi', COUNT(*) FROM ThuChi
	UNION ALL SELECT N'Tài sản', COUNT(*) FROM TaiSan
	UNION ALL SELECT N'Lịch họp', COUNT(*) FROM LichHop
	UNION ALL SELECT N'Khen thưởng', COUNT(*) FROM KhenThuong
	UNION ALL SELECT N'Kỷ luật', COUNT(*) FROM KyLuat
	UNION ALL SELECT N'Feedback', COUNT(*) FROM Feedback
	UNION ALL SELECT N'Thông báo', COUNT(*) FROM ThongBao
	UNION ALL SELECT N'Tin nhắn', COUNT(*) FROM TinNhan
	UNION ALL SELECT N'Báo cáo', COUNT(*) FROM BaoCao
	UNION ALL SELECT N'Điểm rèn luyện', COUNT(*) FROM DiemRenLuyen
	UNION ALL SELECT N'Lịch sử thao tác', COUNT(*) FROM LichSuThaoTac;


	SELECT TenDN, LEN(MatKhau) AS [Độ dài mật khẩu], QuyenHan, TrangThai 
	FROM TaiKhoan;
	-- 1️⃣ Vérifier la longueur des mots de passe stockés
SELECT 
    TenDN,
    LEN(MatKhau) AS [Longueur],
    DATALENGTH(MatKhau) AS [Bytes],
    LEFT(MatKhau, 10) + '...' AS [Aperçu]
FROM TaiKhoan;

-- 2️⃣ Vérifier le hash pour le compte 'thong'
SELECT 
    TenDN,
    MatKhau,
    LEN(MatKhau) AS [Longueur Hash]
FROM TaiKhoan
WHERE TenDN = 'thong';

-- 3️⃣ Tester le hash SHA-256 de '123456'
-- Le résultat devrait être : 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
DECLARE @TenDN NVARCHAR(50), @MatKhau NVARCHAR(255), @HashedPW NVARCHAR(64);

DECLARE cur CURSOR FOR SELECT TenDN, MatKhau FROM TaiKhoan WHERE LEN(MatKhau) < 64;
-- Ce script a hashé '123456' en SHA-256
UPDATE TaiKhoan
SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92'
WHERE TenDN = 'thong';

UPDATE TaiKhoan
SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92';
