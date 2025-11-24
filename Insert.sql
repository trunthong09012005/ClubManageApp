	USE QL_APP_LSC;
	GO

-- ================================
--  RESET TOÀN BỘ DỮ LIỆU (nếu có)
-- ================================
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
EXEC sp_MSForEachTable 'DELETE FROM ?';
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

-- Reset IDENTITY về 1
EXEC sp_MSForEachTable 'DBCC CHECKIDENT (''?'', RESEED, 0)';
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
		(N'Nguyễn Vương Khang', '2004-02-15', N'Nam', N'DHKTPM17A', N'Công nghệ thông tin', '0912345678', N'huytm@student.hcmute.edu.vn', N'Quận 7, TP.HCM', N'Chủ nhiệm', 1, NULL),
		(N'Nguyễn Thị Lan', '2005-05-10', N'Nữ', N'DHKTPM17A', N'Công nghệ thông tin', '0987654321', N'lannt@student.hcmute.edu.vn', N'Quận 5, TP.HCM', N'Phó chủ nhiệm', 2, NULL),
    
		-- Trưởng các ban
		(N'Lê Quốc Bảo', '2005-09-21', N'Nam', N'DHKTPM17B', N'Công nghệ thông tin', '0977112233', N'baolq@student.hcmute.edu.vn', N'Quận 10, TP.HCM', N'Trưởng ban', 4, 1),
		(N'Phạm Thị Mai', '2005-03-18', N'Nữ', N'DHKTPM17B', N'Công nghệ thông tin', '0965432109', N'maipt@student.hcmute.edu.vn', N'Quận 3, TP.HCM', N'Trưởng ban', 4, 2),
		(N'Hoàng Văn Nam', '2004-11-25', N'Nam', N'DHKTPM17C', N'Công nghệ thông tin', '0923456789', N'namhv@student.hcmute.edu.vn', N'Quận 1, TP.HCM', N'Trưởng ban', 4, 3),
    
		-- Thành viên các ban
		(N'Võ Thị Hoa', '2005-07-08', N'Nữ', N'DHKTPM17A', N'Công nghệ thông tin', '0934567890', N'hoavt@student.hcmute.edu.vn', N'Quận 6, TP.HCM', N'Thành viên', 6, 1),
		(N'Đặng Minh Tuấn', '2005-01-30', N'Nam', N'DHKTPM17B', N'Công nghệ thông tin', '0945678901', N'tuandm@student.hcmute.edu.vn', N'Quận 8, TP.HCM', N'Thành viên', 6, 2),
		(N'Trương Thị Lan Anh', '2004-12-12', N'Nữ', N'DHKTPM17C', N'Công nghệ thông tin', '0956789012', N'anhttl@student.hcmute.edu.vn', N'Quận 12, TP.HCM', N'Thành viên', 6, 3),
		(N'Ngô Văn Đức', '2005-04-20', N'Nam', N'DHKTPM17A', N'Công nghệ thông tin', '0967890123', N'ducnv@student.hcmute.edu.vn', N'Thủ Đức, TP.HCM', N'Thành viên', 6, 1),
		(N'Bùi Thị Ngọc', '2005-08-15', N'Nữ', N'DHKTPM17B', N'Công nghệ thông tin', '0978901234', N'ngocbt@student.hcmute.edu.vn', N'Bình Thạnh, TP.HCM', N'Thành viên', 6, 2),

		--ADMIN
		(N'Trần Trung Thông', '2005-11-13', N'Nam', '12A11', 'CNTT', '0912345688', 'thongtt@example.com', N'Hà Nội', 'Admin', 1, 1);

	SELECT MaCV, TenCV FROM ChucVu;
	SELECT * FROM ThanhVien
	SELECT * FROM BANCHUYENMON
	-- Cập nhật Trưởng ban
	UPDATE BanChuyenMon SET TruongBan = 1 WHERE MaBan = 1; 
	UPDATE BanChuyenMon SET TruongBan = 2 WHERE MaBan = 2; 
	UPDATE BanChuyenMon SET TruongBan = 3 WHERE MaBan = 3; 
	GO

	GO

	-- =============================================
	-- 3️⃣ TÀI KHOẢN
	-- =============================================
	SELECT* FROM TaiKhoan;


	INSERT INTO TaiKhoan (TenDN, MatKhau, MaTV, QuyenHan)
	VALUES 
		(N'khang', N'123456', 1, N'Quản trị viên'),
		(N'lantn', N'123456', 2, N'Thành viên'),
		(N'baolq', N'123456', 3, N'Thành viên'),
		(N'maipt', N'123456', 4, N'Thành viên'),
		(N'namhv', N'123456', 5, N'Thành viên'),
		(N'hoavt', N'123456', 6, N'Thành viên'),
		(N'tuandm', N'123456', 7, N'Thành viên'),
		(N'anhttl', N'123456', 8, N'Thành viên'),
		(N'ducnv', N'123456', 9, N'Thành viên'),
		(N'ngocbt', N'123456', 10, N'Thành viên'),
		(N'thong', N'123456', 11, N'Admin');
	GO

-- Xóa tất cả mật khẩu đã hash sai
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'khang'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'lantn'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'baolq'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'maipt'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'namhv'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'hoavt'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'tuandm'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'anhttl'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'ducnv'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'ngocbt'
UPDATE TaiKhoan SET MatKhau = '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92' WHERE TenDN = 'thong'
-- Kiểm tra
SELECT * FROM TaiKhoan


	-- =============================================
	-- 4️⃣ QUAN HỆ BAN - THÀNH VIÊN
	-- =============================================

	INSERT INTO BanChuyenMon_ThanhVien (MaBan, MaTV, VaiTro)
	VALUES
		(1, 3, N'Trưởng ban'),
		(1, 6, N'Thành viên'),
		(1, 9, N'Thành viên'),

		(2, 4, N'Trưởng ban'),
		(2, 10, N'Thành viên'),
		(2, 7, N'Thành viên'),

		(3, 5, N'Trưởng ban'),
		(3, 8, N'Thành viên'),

		(1, 11, N'Admin');

		SELECT MaTV, HoTen FROM ThanhVien;
	-- =============================================
	-- 5️⃣ KỸ NĂNG THÀNH VIÊN
	-- =============================================
	SELECT MaTV, HoTen, TrangThai
	FROM ThanhVien
	ORDER BY MaTV;
	GO
	
	
	INSERT INTO ThanhVien_KyNang (MaTV, MaKN, Diem, CapDoHienTai)
	VALUES
		(2, 4, 8.5, N'Nâng cao'), -- Nguyễn Vương Khang - Quản lý thời gian
		(2, 5, 9.0, N'Nâng cao'), -- Nguyễn Vương Khang - Lãnh đạo
		(3, 1, 9.5, N'Nâng cao'), -- Nguyễn Thị Lan - Giao tiếp
		(3, 8, 8.0, N'Trung bình'), -- Nguyễn Thị Lan - Quản lý sự kiện
		(4, 3, 9.0, N'Nâng cao'), -- Lê Quốc Bảo - Thiết kế
		(4, 7, 8.0, N'Trung bình'), -- Lê Quốc Bảo - Marketing
		(7, 3, 7.5, N'Trung bình'), -- Võ Thị Hoa - Thiết kế
		(8, 8, 8.5, N'Nâng cao'), -- Đặng Minh Tuấn - Quản lý sự kiện
		(9, 2, 9.0, N'Nâng cao'), -- Trương Thị Lan Anh - Làm việc nhóm
		(10, 6, 8.0, N'Trung bình'), -- Ngô Văn Đức - Video editing
		(11, 7, 7.0, N'Trung bình'); -- Bùi Thị Ngọc - Marketing
	GO



	-- =============================================
	-- 6️⃣ HOẠT ĐỘNG
	-- =============================================


	-- lấy dữ liệu dưới đây
	INSERT INTO HoatDong (TenHD, NgayToChuc, GioBatDau, GioKetThuc, DiaDiem, MoTa, MaLoaiHD, NguoiPhuTrach, KinhPhiDuKien, SoLuongToiDa, TrangThai)
	VALUES 
		(N'Chiến dịch Mùa hè xanh 2026', '2026-07-10', '07:00', '17:00', N'Củ Chi, TP.HCM', 
		 N'Hoạt động tình nguyện giúp đỡ người dân địa phương, trồng cây xanh và dọn dẹp môi trường', 
		1, 2, 5000000, 50, N'Hoàn thành'),
    
		(N'Workshop: Kỹ năng thuyết trình chuyên nghiệp', '2026-08-15', '14:00', '17:00', N'Hội trường B - Tầng 3', 
		 N'Buổi workshop nâng cao kỹ năng thuyết trình và giao tiếp hiệu quả với diễn giả chuyên nghiệp', 
		 2, 3, 2000000, 100, N'Hoàn thành'),
    
		(N'Dã ngoại CLB - Vũng Tàu', '2026-09-05', '06:00', '20:00', N'Bãi Sau, Vũng Tàu', 
		 N'Chuyến du lịch team building 1 ngày tại Vũng Tàu với các hoạt động gắn kết', 
		 3, 4, 8000000, 40, N'Hoàn thành'),
    
		(N'Ngày hội văn hóa sinh viên', '2025-11-28', '08:00', '18:00', N'Sân khấu trung tâm', 
		 N'Tổ chức ngày hội văn hóa với các tiết mục văn nghệ, ẩm thực và trò chơi dân gian', 
		 4, 5, 10000000, 200, N'Đang chuẩn bị'), -- Giữ nguyên tháng 11, chỉ lùi ngày
    
		(N'Giải bóng đá CLB lần thứ 5', '2025-12-10', '07:00', '12:00', N'Sân bóng Thể Công', 
		 N'Giải bóng đá giao hữu giữa các thành viên CLB', 
		 5, 6, 3000000, 30, N'Đang chuẩn bị');
	GO

	select*from HoatDong;
	-- Đăng ký và tham gia hoạt động
		-- lấy bảng dưới này
		INSERT INTO DangKyHoatDong (MaTV, MaHD, TrangThai)
		VALUES
			-- MaHD=4 (Workshop)
			(7, 4, N'Đã duyệt'), (2, 4, N'Đã duyệt'), (11, 4, N'Đã duyệt'), -- MaTV cũ 17, 13, 21 -> 7, 2, 11
			(8, 4, N'Đã duyệt'), (4, 4, N'Đã duyệt'), -- MaTV cũ 18, 14 -> 8, 4
	
			-- MaHD=5 (Dã ngoại)
			(9, 5, N'Đã duyệt'), (5, 5, N'Đã duyệt'), (8, 5, N'Đã duyệt'), -- MaTV cũ 19, 15, 18 -> 9, 5, 8
			(10, 5, N'Đã duyệt'), (6, 5, N'Đã duyệt'); -- MaTV cũ 20, 16 -> 10, 6

		GO
		SELECT * FROM DangKyHoatDong;
	INSERT INTO ThamGia (MaHD, MaTV, VaiTroTrongHD, DiemDanh, DiemThuong)
	VALUES
		-- Hoạt động MAHD=4 (Workshop: Kỹ năng thuyết trình chuyên nghiệp)
		(4, 7, N'Trưởng đoàn', 1, 10), -- MaTV cũ 17 -> 7
		(4, 2, N'Thành viên', 1, 8), -- MaTV cũ 13 -> 2
		(4, 4, N'Thành viên', 1, 8), -- MaTV cũ 14 -> 4
		(4, 5, N'Thành viên', 1, 7), -- MaTV cũ 15 -> 5
		(4, 6, N'Thành viên', 0, 0), -- MaTV cũ 16 -> 6

		-- Hoạt động MAHD=5 (Dã ngoại CLB - Vũng Tàu)
		(5, 7, N'MC', 1, 10), -- MaTV cũ 17 -> 7
		(5, 8, N'Tổ chức', 1, 9), -- MaTV cũ 18 -> 8
		(5, 9, N'Thành viên', 1, 8), -- MaTV cũ 19 -> 9
		(5, 10, N'Thành viên', 1, 8); -- MaTV cũ 20 -> 10

	GO
	SELECT * FROM ThamGia;
	-- =============================================
	-- 7️⃣ DỰ ÁN
	-- =============================================


	INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc, MucDoUuTien, TienDo, TrangThai)
	VALUES
		(N'Hệ thống quản lý CLB', 
		 N'Xây dựng website và ứng dụng quản lý thành viên, hoạt động và tài chính của CLB', 
		 '2025-05-01', '2025-12-31', N'Cao', 65, N'Đang thực hiện'), -- MaDA=1
    
		(N'Chiến dịch tuyển thành viên K18', 
		 N'Lập kế hoạch và triển khai chiến dịch tuyển thành viên mới khóa 18', 
		 '2025-09-01', '2025-10-15', N'Cao', 80, N'Đang thực hiện'), -- MaDA=2
    
		(N'Kế hoạch hoạt động 2026', 
		 N'Lập kế hoạch chi tiết các hoạt động cho năm 2026', 
		 '2025-10-01', '2025-11-30', N'Trung bình', 40, N'Đang thực hiện'); -- MaDA=3
	GO

	INSERT INTO PhanCong (MaTV, MaDA, NhiemVu, TrangThai, NgayHetHan)
	VALUES
		(2, 1, N'Quản lý dự án và phối hợp tổng thể', N'Đang thực hiện', '2025-12-31'), -- MaTV cũ 12 -> 2
		(4, 1, N'Thiết kế cơ sở dữ liệu và UI/UX', N'Hoàn thành', '2025-08-30'), -- MaTV cũ 14 -> 4
		(7, 1, N'Thiết kế giao diện và banner', N'Đang thực hiện', '2025-11-30'), -- MaTV cũ 17 -> 7
		(10, 1, N'Phát triển backend', N'Đang thực hiện', '2025-12-15'), -- MaTV cũ 20 -> 10
		(3, 2, N'Lập kế hoạch tuyển thành viên', N'Hoàn thành', '2025-09-15'), -- MaTV cũ 13 -> 3
		(5, 2, N'Tổ chức sự kiện tuyển thành viên', N'Đang thực hiện', '2025-10-15'), -- MaTV cũ 15 -> 5
		(2, 3, N'Tổng hợp ý kiến và lập kế hoạch', N'Đang thực hiện', '2025-11-30'); -- MaTV cũ 12 -> 2
	GO

	-- =============================================
	-- 8️⃣ THU CHI
	-- =============================================

	INSERT INTO ThuChi (LoaiGD, SoTien, NgayGD, NoiDung, NguoiThucHien, MaNguon, MaHD)
	VALUES 
		(N'Thu', 5000000, '2025-06-15', N'Tài trợ từ công ty ABC Technology', 2, 1, NULL), -- NguoiThucHien cũ 12 -> 2
		(N'Thu', 3000000, '2025-07-01', N'Đóng góp quỹ CLB học kỳ 1', 3, 2, NULL), -- NguoiThucHien cũ 13 -> 3
		(N'Thu', 2000000, '2025-08-10', N'Tài trợ workshop kỹ năng mềm', 3, 1, 1), -- NguoiThucHien cũ 13 -> 3, MaHD cũ 3 -> 8
		(N'Chi', 4500000, '2025-07-08', N'Chi phí hoạt động Mùa hè xanh', 2, NULL, 4), -- NguoiThucHien cũ 12 -> 2, MaHD cũ 2 -> 7
		(N'Chi', 1800000, '2025-08-14', N'Chi phí tổ chức workshop', 3, NULL, 5), -- NguoiThucHien cũ 13 -> 3, MaHD cũ 3 -> 8
		(N'Chi', 7500000, '2025-09-04', N'Chi phí dã ngoại Vũng Tàu', 4, NULL, 3), -- NguoiThucHien cũ 14 -> 4, MaHD cũ 4 -> 9
		(N'Chi', 500000, '2025-09-20', N'Mua vật tư văn phòng CLB', 5, NULL, NULL); -- NguoiThucHien cũ 15 -> 5
	GO
	SELECT * FROM THUCHI;
	INSERT INTO ThuChi_ChiTiet (MaGD, NoiDung, SoTien, DonViTinh, SoLuong)
	VALUES
		(1, N'Tiền mặt nhận tài trợ', 5000000, N'VNĐ', 1),
		(2, N'Đóng góp từ 30 thành viên', 3000000, N'VNĐ/người', 30),
		(3, N'Tài trợ diễn giả và địa điểm', 2000000, N'VNĐ', 1),
		(4, N'Chi phí di chuyển và ăn uống', 2500000, N'VNĐ', 1),
		(5, N'Mua cây giống và dụng cụ', 2000000, N'VNĐ', 1),
		(6, N'Thuê diễn giả', 1000000, N'VNĐ', 1),
		(7, N'In tài liệu và nước uống', 800000, N'VNĐ', 1),
		(1, N'Thuê xe và chi phí di chuyển', 3000000, N'VNĐ', 1),
		(2, N'Chi phí ăn uống', 2500000, N'VNĐ', 1),
		(3, N'Thuê địa điểm và thiết bị', 2000000, N'VNĐ', 1),
		(5, N'Giấy A4, bút viết', 200000, N'Bộ', 5),
		(6, N'File hồ sơ, kẹp tài liệu', 300000, N'Bộ', 3);
	GO

	
	-- =============================================
	-- 9️⃣ TÀI SẢN
	-- =============================================


	INSERT INTO TaiSan (TenTS, MaTS_Code, SoLuong, DonViTinh, TinhTrang, GiaTriMua, NguoiQuanLy, ViTri)
	VALUES 
		(N'Loa di động JBL', N'TS001', 2, N'Bộ', N'Tốt', 3000000, 7, N'Phòng CLB - Tầng 2'), -- NguoiQuanLy cũ 17 -> 7
		(N'Bàn gấp nhựa', N'TS002', 10, N'Cái', N'Tốt', 2000000, 7, N'Kho CLB - Tầng 1'), -- NguoiQuanLy cũ 17 -> 7
		(N'Máy chiếu Epson', N'TS003', 1, N'Cái', N'Tốt', 8000000, 7, N'Phòng CLB - Tầng 2'), -- NguoiQuanLy cũ 17 -> 7
		(N'Laptop Dell Inspiron', N'TS004', 2, N'Cái', N'Bình thường', 15000000, 2, N'Phòng CLB - Tầng 2'), -- NguoiQuanLy cũ 12 -> 2
		(N'Camera Sony', N'TS005', 1, N'Cái', N'Tốt', 12000000, 4, N'Ban Truyền thông'), -- NguoiQuanLy cũ 14 -> 4
		(N'Micro không dây', N'TS006', 3, N'Bộ', N'Tốt', 4500000, 5, N'Phòng CLB - Tầng 2'), -- NguoiQuanLy cũ 15 -> 5
		(N'Banner cố định CLB', N'TS007', 5, N'Cái', N'Bình thường', 2500000, 4, N'Kho CLB - Tầng 1'), -- NguoiQuanLy cũ 14 -> 4
		(N'Bảng flipchart', N'TS008', 2, N'Cái', N'Tốt', 1000000, 5, N'Phòng họp'), -- NguoiQuanLy cũ 15 -> 5
		(N'Máy in Canon', N'TS009', 1, N'Cái', N'Cần sửa chữa', 5000000, 3, N'Phòng CLB - Tầng 2'); -- NguoiQuanLy cũ 13 -> 3
	GO

	-- Lịch sử mượn trả
	INSERT INTO MuonTraTaiSan (MaTS, MaTV, NgayMuon, NgayTraDuKien, NgayTraThucTe, MucDich, TrangThai)
	VALUES
		(2, 6, '2025-08-14 08:00', '2025-08-14', '2025-08-14 18:00', N'Workshop kỹ năng mềm', N'Đã trả'), -- MaTV cũ 16 -> 6
		(3, 3, '2025-08-14 13:00', '2025-08-14', '2025-08-14 17:30', N'Workshop kỹ năng mềm', N'Đã trả'), -- MaTV cũ 13 -> 3
		(4, 7, '2025-09-04 05:00', '2025-09-05', '2025-09-05 21:00', N'Ghi hình dã ngoại', N'Đã trả'), -- MaTV cũ 17 -> 7
		(5, 8, '2025-11-01 14:00', '2025-11-15', NULL, N'Chuẩn bị sự kiện tuyển thành viên', N'Đang mượn'), -- MaTV cũ 18 -> 8
		(6, 9, '2025-11-10 09:00', '2025-11-20', NULL, N'Ngày hội văn hóa', N'Đang mượn'); -- MaTV cũ 19 -> 9
	GO
	-- =============================================
	-- 🔟 LỊCH HỌP
	-- =============================================


	INSERT INTO LichHop (TieuDe, NgayHop, DiaDiem, NoiDung, NguoiChuTri, TrangThai)
	VALUES 
		(N'Họp tổng kết quý I/2025', '2025-04-15 14:00', N'Phòng họp A101', 
		 N'Tổng kết hoạt động quý I và lập kế hoạch quý II', 2, N'Hoàn thành'), -- NguoiChuTri cũ 12 -> 2
    
		(N'Họp chuẩn bị Mùa hè xanh', '2025-06-20 15:00', N'Phòng CLB', 
		 N'Thảo luận kế hoạch chi tiết cho chiến dịch Mùa hè xanh', 2, N'Hoàn thành'), -- NguoiChuTri cũ 12 -> 2
    
		(N'Họp Ban chủ nhiệm tháng 8', '2025-08-05 16:00', N'Phòng họp B201', 
		 N'Đánh giá hoạt động tháng 7 và kế hoạch tháng 8', 2, N'Hoàn thành'), -- NguoiChuTri cũ 12 -> 2
    
		(N'Họp toàn thể CLB - Kế hoạch tuyển thành viên', '2025-09-15 14:00', N'Hội trường lớn', 
		 N'Công bố kế hoạch tuyển thành viên mới và phân công nhiệm vụ', 2, N'Hoàn thành'), -- NguoiChuTri cũ 12 -> 2
    
		(N'Họp Ban chủ nhiệm tháng 11', '2025-11-18 15:00', N'Phòng họp A101', 
		 N'Thảo luận kế hoạch cuối năm và chuẩn bị đại hội', 2, N'Sắp diễn ra'); -- NguoiChuTri cũ 12 -> 2
	GO

	INSERT INTO DiemDanhLichHop (MaLH, MaTV, TrangThai, GioCheckIn)
	VALUES 
		-- Họp tổng kết quý I
		(1, 2, N'Có mặt', '2025-04-15 13:55'), -- MaTV cũ 12 -> 2
		(1, 3, N'Có mặt', '2025-04-15 14:00'), -- MaTV cũ 13 -> 3
		(1, 4, N'Vắng', NULL), -- MaTV cũ 14 -> 4
		(1, 5, N'Có mặt', '2025-04-15 14:02'), -- MaTV cũ 15 -> 5
		(1, 6, N'Trễ', '2025-04-15 14:15'), -- MaTV cũ 16 -> 6

		-- Họp chuẩn bị Mùa hè xanh
		(2, 2, N'Có mặt', '2025-06-20 14:58'), -- MaTV cũ 12 -> 2
		(2, 3, N'Có mặt', '2025-06-20 15:00'), -- MaTV cũ 13 -> 3
		(2, 4, N'Có mặt', '2025-06-20 15:05'), -- MaTV cũ 14 -> 4
		(2, 5, N'Có mặt', '2025-06-20 14:55'), -- MaTV cũ 15 -> 5

		-- Họp BCN tháng 8
		(3, 2, N'Có mặt', '2025-08-05 15:55'), -- MaTV cũ 12 -> 2
		(3, 3, N'Có mặt', '2025-08-05 16:00'), -- MaTV cũ 13 -> 3

		-- Họp toàn thể
		(4, 2, N'Có mặt', '2025-09-15 13:50'), -- MaTV cũ 12 -> 2
		(4, 3, N'Có mặt', '2025-09-15 13:55'), -- MaTV cũ 13 -> 3
		(4, 4, N'Có mặt', '2025-09-15 14:00'), -- MaTV cũ 14 -> 4
		(4, 5, N'Có mặt', '2025-09-15 14:00'), -- MaTV cũ 15 -> 5
		(4, 6, N'Có mặt', '2025-09-15 14:05'), -- MaTV cũ 16 -> 6
		(4, 7, N'Trễ', '2025-09-15 14:20'), -- MaTV cũ 17 -> 7
		(4, 8, N'Có mặt', '2025-09-15 13:58'), -- MaTV cũ 18 -> 8
		(4, 9, N'Vắng', NULL), -- MaTV cũ 19 -> 9
		(4, 10, N'Có mặt', '2025-09-15 14:03'), -- MaTV cũ 20 -> 10
		(4, 11, N'Có phép', NULL); -- MaTV cũ 21 -> 11
	GO

	-- =============================================
	-- 1️⃣1️⃣ KHEN THƯỞNG & KỶ LUẬT
	-- =============================================

	INSERT INTO KhenThuong (MaTV, LyDo, HinhThuc, GiaTriThuong, NgayKT, NguoiLap)
	VALUES 
		(2, N'Lãnh đạo xuất sắc CLB trong năm học 2024-2025', N'Bằng khen', 1000000, '2025-06-15', 3), -- MaTV cũ 12, NguoiLap cũ 13 -> 2, 3
		(3, N'Tổ chức workshop kỹ năng mềm thành công tốt đẹp', N'Giấy khen', 500000, '2025-08-20', 2), -- MaTV cũ 13, NguoiLap cũ 12 -> 3, 2
		(4, N'Thiết kế ấn tượng cho chiến dịch tuyển thành viên', N'Giấy khen', 300000, '2025-09-25', 2), -- MaTV cũ 14, NguoiLap cũ 12 -> 4, 2
		(5, N'Tích cực và nhiệt tình trong tổ chức các sự kiện', N'Quà tặng', 200000, '2025-10-10', 2), -- MaTV cũ 15, NguoiLap cũ 12 -> 5, 2
		(8, N'Hoàn thành xuất sắc nhiệm vụ được giao', N'Giấy khen', 0, '2025-09-30', 3); -- MaTV cũ 18, NguoiLap cũ 13 -> 8, 3
	GO


	INSERT INTO KyLuat (MaTV, LyDo, HinhThuc, ThoiGianKyLuat, NgayKL, NguoiLap)
	VALUES 
		(10, N'Vắng mặt không phép 3 buổi họp liên tiếp', N'Cảnh cáo', NULL, '2025-10-05', 2), -- MaTV cũ 20, NguoiLap cũ 12 -> 10, 2
		(11, N'Không hoàn thành nhiệm vụ đúng thời hạn', N'Khiển trách', NULL, '2025-08-25', 2); -- MaTV cũ 21, NguoiLap cũ 12 -> 11, 2
	GO

	-- =============================================
	-- 1️⃣2️⃣ FEEDBACK & GIAO TIẾP
	-- =============================================



	INSERT INTO Feedback (MaTV, MaHD, NoiDung, DanhGiaSao, TrangThai)
	VALUES 
		(3, 3, N'Hoạt động rất ý nghĩa! Được giúp đỡ cộng đồng và hiểu hơn về cuộc sống nông thôn.', 5, N'Đã xử lý'), -- MaTV cũ 13 -> 3, MaHD cũ 2 -> 7
		(4, 4, N'Tổ chức tốt nhưng nên có thêm thời gian nghỉ giữa các hoạt động.', 4, N'Đã xử lý'), -- MaTV cũ 14 -> 4, MaHD cũ 2 -> 7
		(6, 5, N'Workshop rất bổ ích! Diễn giả nhiệt tình và nội dung thiết thực.', 5, N'Đã xử lý'), -- MaTV cũ 16 -> 6, MaHD cũ 3 -> 8
		(7, 1, N'Nên tổ chức thêm các buổi thực hành để áp dụng kiến thức vừa học.', 4, N'Đang xử lý'), -- MaTV cũ 17 -> 7, MaHD cũ 3 -> 8
		(5, 2, N'Chuyến đi vui vẻ và ý nghĩa, giúp các thành viên gắn kết hơn.', 5, N'Đã xử lý'), -- MaTV cũ 15 -> 5, MaHD cũ 4 -> 9
		(9, 3, N'Nội dung hay nhưng thời gian hơi ngắn, muốn học thêm.', 4, N'Đã nhận'); -- MaTV cũ 19 -> 9, MaHD cũ 3 -> 8
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
	 N'Thông báo chung', 2, N'Ban chủ nhiệm', N'Đã gửi'), -- Sửa 12 -> 2
    
	(N'Thông báo đóng góp quỹ CLB học kỳ 2', 
	 N'Kính gửi toàn thể thành viên,

	CLB thông báo về việc đóng góp quỹ học kỳ 2 năm học 2025-2026:
	- Mức đóng góp: 100.000đ/người
	- Thời hạn: Trước 30/11/2025
	- Hình thức: Chuyển khoản hoặc nộp trực tiếp

	Mọi thắc mắc xin liên hệ Ban Tài chính.

	Trân trọng!', 
	 N'Nhắc nhở', 3, N'Tất cả', N'Đã gửi'), -- Sửa 13 -> 3
    
	(N'🎉 Khai mạc chiến dịch tuyển thành viên K18', 
	 N'CLB chính thức mở đơn đăng ký tuyển thành viên khóa 18!

	📅 Thời gian: 01/10 - 15/10/2025
	📝 Đăng ký tại: bit.ly/CLBLSC_K18
	🎁 Ưu đãi cho 50 người đăng ký đầu tiên

	Cùng tham gia và phát triển bản thân nhé!', 
	 N'Sự kiện', 3, N'Tất cả', N'Đã gửi'), -- Sửa 13 -> 3
    
	(N'⚠️ KHẨN - Thay đổi địa điểm Ngày hội văn hóa', 
	 N'Kính gửi các thành viên,

	Do thời tiết không thuận lợi, địa điểm tổ chức Ngày hội văn hóa chuyển từ Sân khấu trung tâm sang Hội trường A.

	Thời gian giữ nguyên: 8h00 ngày 20/11/2025.

	Xin lỗi vì sự bất tiện này!', 
	 N'Khẩn cấp', 2, N'Tất cả', N'Đã gửi'); -- Sửa 12 -> 2
GO
	

	INSERT INTO TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung, TrangThai)
	VALUES
		(2, 3, N'Lan ơi, chuẩn bị tài liệu cho workshop nhé!', N'Đã đọc'), -- 12, 13 -> 2, 3
		(3, 2, N'Vâng anh Huy, em đã chuẩn bị xong ạ!', N'Đã đọc'), -- 13, 12 -> 3, 2
		(4, 2, N'Anh ơi, em đã hoàn thành thiết kế banner rồi ạ!', N'Đã đọc'), -- 14, 12 -> 4, 2
		(2, 4, N'Ok em, anh check và gửi feedback chiều nay nhé!', N'Đã đọc'), -- 12, 14 -> 2, 4
		(5, 7, N'Tuấn ơi, ngày mai họp Ban sự kiện nhé!', N'Đã đọc'), -- 15, 17 -> 5, 7
		(7, 5, N'Dạ em nhận được, em sẽ có mặt đúng giờ ạ!', N'Đã đọc'), -- 17, 15 -> 7, 5
		(3, 6, N'Hoa nhớ gửi file thiết kế poster cho chị trước 5h chiều nay nha!', N'Chưa đọc'), -- 13, 16 -> 3, 6
		(6, 8, N'Lan Anh chuẩn bị checklist vật tư cho sự kiện nhé!', N'Đã đọc'); -- 16, 18 -> 6, 8
	GO



	-- =============================================
	-- 1️⃣3️⃣ FILE ĐÍNH KÈM
	-- =============================================


	INSERT INTO FileDinhKem (TenFile, DuongDan, KichThuoc, LoaiFile, MaDA, MaHD, NguoiTai)
	VALUES
		(N'Kế hoạch Mùa hè xanh 2025.pdf', N'/files/projects/ke-hoach-mua-he-xanh-2025.pdf', 2048576, N'PDF', NULL, 2, 2), -- NguoiTai cũ 12 -> 2, MaHD cũ 2 -> 7
		(N'Slide Workshop Kỹ năng mềm.pptx', N'/files/activities/slide-workshop-ky-nang-mem.pptx', 5242880, N'PowerPoint', NULL, 3, 3), -- NguoiTai cũ 13 -> 3, MaHD cũ 3 -> 8
		(N'Báo cáo tài chính Q2-2025.xlsx', N'/files/reports/bao-cao-tai-chinh-q2-2025.xlsx', 1048576, N'Excel', NULL, NULL, 3), -- NguoiTai cũ 13 -> 3
		(N'Database Schema v1.0.sql', N'/files/projects/database-schema-v1.sql', 524288, N'SQL', 1, NULL, 4), -- NguoiTai cũ 14 -> 4
		(N'UI Design CLB Management.fig', N'/files/projects/ui-design-clb-management.fig', 15728640, N'Figma', 1, NULL, 6), -- NguoiTai cũ 16 -> 6
		(N'Video recap Dã ngoại Vũng Tàu.mp4', N'/files/activities/video-recap-da-ngoai-vung-tau.mp4', 104857600, N'Video', NULL, 4, 9); -- NguoiTai cũ 19 -> 9, MaHD cũ 4 -> 9
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
	 '2025-04-01', '2025-06-30', 2, N'Hoàn thành'), -- Sửa 12 -> 2
    
	(N'Báo cáo tài chính tháng 9/2025', N'Tháng', 
	 N'Tình hình thu chi tháng 9/2025:
      
	- Tổng thu: 5.000.000 VNĐ
	- Tổng chi: 8.000.000 VNĐ
	- Số dư đầu kỳ: 15.000.000 VNĐ
	- Số dư cuối kỳ: 12.000.000 VNĐ
      
	Chi tiết đính kèm file Excel.', 
	 '2025-09-01', '2025-09-30', 3, N'Hoàn thành'), -- Sửa 13 -> 3
    
	(N'Báo cáo chiến dịch Mùa hè xanh 2025', N'Hoạt động', 
	 N'Tổng kết chiến dịch Mùa hè xanh 2025:
      
	- Số thành viên tham gia: 35 người
	- Số người dân được hỗ trợ: 150 hộ
	- Số cây xanh trồng: 500 cây
	- Đánh giá: Hoạt động thành công tốt đẹp, nhận được sự ủng hộ từ địa phương', 
	 '2025-07-10', '2025-07-10', 2, N'Hoàn thành'), -- Sửa 12 -> 2
    
	(N'Kế hoạch hoạt động năm 2026', N'Năm', 
	 N'Dự thảo kế hoạch hoạt động năm 2026 (đang hoàn thiện)', 
	 '2026-01-01', '2026-12-31', 2, N'Nháp'); -- Sửa 12 -> 2
GO


	-- =============================================
	-- 1️⃣5️⃣ ĐIỂM RÈN LUYỆN
	-- =============================================

	INSERT INTO DiemRenLuyen (MaTV, HocKy, NamHoc, Diem, XepLoai, GhiChu)
	VALUES 
		(2, N'HK1', N'2024-2025', 95, N'Xuất sắc', N'Thực hiện tích cực và có nhiều đóng góp'), -- MaTV cũ 12 -> 2
		(2, N'HK2', N'2024-2025', 92, N'Xuất sắc', N'Lãnh đạo CLB hiệu quả'), -- MaTV cũ 12 -> 2
		(3, N'HK1', N'2024-2025', 88, N'Giỏi', N'Nhiệt tình trong công việc'), -- MaTV cũ 13 -> 3
		(3, N'HK2', N'2024-2025', 90, N'Xuất sắc', N'Tổ chức workshop thành công'), -- MaTV cũ 13 -> 3
		(4, N'HK1', N'2024-2025', 85, N'Giỏi', N'Kỹ năng thiết kế tốt'), -- MaTV cũ 14 -> 4
		(4, N'HK2', N'2024-2025', 87, N'Giỏi', N'Đóng góp nhiều cho Ban Truyền thông'), -- MaTV cũ 14 -> 4
		(5, N'HK1', N'2024-2025', 82, N'Giỏi', N'Tích cực tham gia hoạt động'), -- MaTV cũ 15 -> 5
		(5, N'HK2', N'2024-2025', 85, N'Giỏi', N'Cải thiện kỹ năng tổ chức sự kiện'), -- MaTV cũ 15 -> 5
		(6, N'HK1', N'2024-2025', 78, N'Khá', N'Cần tích cực hơn'), -- MaTV cũ 16 -> 6
		(6, N'HK2', N'2024-2025', 80, N'Khá', N'Có sự tiến bộ'), -- MaTV cũ 16 -> 6
		(7, N'HK1', N'2024-2025', 86, N'Giỏi', N'Kỹ năng thiết kế phát triển tốt'), -- MaTV cũ 17 -> 7
		(8, N'HK1', N'2024-2025', 84, N'Giỏi', N'Tích cực tham gia'), -- MaTV cũ 18 -> 8
		(9, N'HK1', N'2024-2025', 70, N'Trung bình', N'Vắng nhiều buổi họp'), -- MaTV cũ 19 -> 9
		(10, N'HK1', N'2024-2025', 88, N'Giỏi', N'Hoàn thành tốt nhiệm vụ'), -- MaTV cũ 20 -> 10
		(11, N'HK1', N'2024-2025', 82, N'Giỏi', N'Nhiệt tình và năng động'); -- MaTV cũ 21 -> 11
	GO


	-- =============================================
	-- 1️⃣6️⃣ LỊCH SỬ THAO TÁC
	-- =============================================



INSERT INTO LichSuThaoTac (MaTV, TenBang, LoaiThaoTac, KhoaChinh, NoiDung)
	VALUES
		(2, N'HoatDong', N'Thêm', N'MaHD=7', N'Tạo hoạt động Chiến dịch Mùa hè xanh 2025'), -- MaTV cũ 12 -> 2, MaHD cũ 2 -> 7
		(3, N'ThanhVien', N'Cập nhật', N'MaTV=3', N'Cập nhật số điện thoại thành viên'), -- MaTV cũ 13 -> 3
		(2, N'ThuChi', N'Thêm', N'MaGD=3', N'Thêm khoản thu tài trợ 5.000.000đ'), -- MaTV cũ 12 -> 2
		(4, N'TaiSan', N'Thêm', N'MaTS=5', N'Nhập camera Sony mới'), -- MaTV cũ 14 -> 4
		(3, N'HoatDong', N'Thêm', N'MaHD=8', N'Tạo Workshop kỹ năng mềm'), -- MaTV cũ 13 -> 3, MaHD cũ 3 -> 8
		(2, N'DuAn', N'Thêm', N'MaDA=1', N'Tạo dự án Hệ thống quản lý CLB'), -- MaTV cũ 12 -> 2
		(2, N'LichHop', N'Thêm', N'MaLH=1', N'Tạo lịch họp tổng kết quý I'), -- MaTV cũ 12 -> 2
		(3, N'ThongBao', N'Thêm', N'MaTB=1', N'Đăng thông báo lịch họp tháng 11'), -- MaTV cũ 13 -> 3
		(2, N'KhenThuong', N'Thêm', N'MaKT=1', N'Khen thưởng thành viên xuất sắc'), -- MaTV cũ 12 -> 2
		(2, N'HoatDong', N'Cập nhật', N'MaHD=7', N'Cập nhật trạng thái hoạt động thành Hoàn thành'); -- MaTV cũ 12 -> 2, MaHD cũ 2 -> 7
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
