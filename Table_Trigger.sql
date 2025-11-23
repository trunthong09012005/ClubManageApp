

USE master;
GO

-- Đặt database về chế độ SINGLE_USER để ngắt kết nối
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QL_CLB_LSC')
BEGIN
    ALTER DATABASE QL_CLB_LSC SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QL_CLB_LSC;
END
GO

CREATE DATABASE QL_CLB_LSC;
GO

USE QL_CLB_LSC;
GO

-- =============================================
-- PHẦN 1: CÁC BẢNG TRA CỨU (LOOKUP TABLES)
-- =============================================

-- Bảng Chức vụ
CREATE TABLE ChucVu (
    MaCV INT IDENTITY(1,1) PRIMARY KEY,
    TenCV NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng Ban chuyên môn
CREATE TABLE BanChuyenMon (
    MaBan INT IDENTITY(1,1) PRIMARY KEY,
    TenBan NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    TruongBan INT NULL,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng Loại hoạt động
CREATE TABLE LoaiHoatDong (
    MaLoaiHD INT IDENTITY(1,1) PRIMARY KEY,
    TenLoaiHD NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    MauSac NVARCHAR(20) NULL, -- Để phân loại màu trên UI
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng Nguồn thu
CREATE TABLE NguonThu (
    MaNguon INT IDENTITY(1,1) PRIMARY KEY,
    TenNguon NVARCHAR(150) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    LoaiNguon NVARCHAR(50) NULL, -- 'Cố định', 'Không thường xuyên'
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng Kỹ năng
CREATE TABLE KyNang (
    MaKN INT IDENTITY(1,1) PRIMARY KEY,
    TenKN NVARCHAR(150) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    CapDo NVARCHAR(50) CHECK (CapDo IN (N'Cơ bản', N'Trung bình', N'Nâng cao', N'Chuyên gia')),
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- PHẦN 2: BẢNG THÀNH VIÊN
-- =============================================

CREATE TABLE ThanhVien (
    MaTV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(150) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ', N'Khác')),
    Lop NVARCHAR(50) NULL,
    Khoa NVARCHAR(150) NULL,
    SDT VARCHAR(15) NULL,
    Email NVARCHAR(150) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    VaiTro NVARCHAR(100) NULL,
    NgayThamGia DATE DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) DEFAULT N'Hoạt động' CHECK (TrangThai IN (N'Hoạt động', N'Tạm ngưng', N'Nghỉ')),
    MaCV INT NULL,
    MaBan INT NULL,
    AnhDaiDien NVARCHAR(500) NULL,
    
    CONSTRAINT UQ_ThanhVien_Email UNIQUE (Email),
    CONSTRAINT CK_ThanhVien_SDT CHECK (LEN(SDT) BETWEEN 9 AND 15),
    CONSTRAINT CK_ThanhVien_Email_Format CHECK (Email LIKE '%_@__%.__%'),
    CONSTRAINT CK_ThanhVien_NgaySinh CHECK (NgaySinh <= GETDATE()),
    CONSTRAINT FK_ThanhVien_ChucVu FOREIGN KEY (MaCV) REFERENCES ChucVu(MaCV),
    CONSTRAINT FK_ThanhVien_Ban FOREIGN KEY (MaBan) REFERENCES BanChuyenMon(MaBan)
);
GO

-- Thêm FK cho TruongBan sau khi ThanhVien đã tồn tại
ALTER TABLE BanChuyenMon
ADD CONSTRAINT FK_BanTruong_ThanhVien FOREIGN KEY (TruongBan)
REFERENCES ThanhVien(MaTV);
GO

-- Index cho tìm kiếm nhanh
CREATE NONCLUSTERED INDEX IX_ThanhVien_Email ON ThanhVien(Email);
CREATE NONCLUSTERED INDEX IX_ThanhVien_HoTen ON ThanhVien(HoTen);
CREATE NONCLUSTERED INDEX IX_ThanhVien_TrangThai ON ThanhVien(TrangThai);
GO

-- =============================================
-- PHẦN 3: TÀI KHOẢN & PHÂN QUYỀN
-- =============================================

CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDN NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    MaTV INT NOT NULL UNIQUE,
    QuyenHan NVARCHAR(50) NOT NULL DEFAULT N'Thành viên' CHECK (QuyenHan IN (N'Admin', N'Quản trị viên', N'Thành viên')),
    NgayTao DATETIME DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Hoạt động' CHECK (TrangThai IN (N'Hoạt động', N'Khóa', N'Chờ kích hoạt')),
    
    CONSTRAINT FK_TaiKhoan_ThanhVien FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE
);
GO
SELECT 
    MatKhau,
    LEN(MatKhau) AS DoDai,
    DATALENGTH(MatKhau) AS ByteLength
FROM TaiKhoan
WHERE TenDN = 'thong';



CREATE NONCLUSTERED INDEX IX_TaiKhoan_TenDN ON TaiKhoan(TenDN);
GO

-- =============================================
-- PHẦN 4: QUAN HỆ NHIỀU-NHIỀU
-- =============================================

-- Bảng trung gian: Ban - Thành viên
CREATE TABLE BanChuyenMon_ThanhVien (
    MaBan INT NOT NULL,
    MaTV INT NOT NULL,
    VaiTro NVARCHAR(100) NULL,
    NgayThamGiaBan DATE DEFAULT GETDATE(),
    NgayRaBan DATE NULL,
    
    PRIMARY KEY (MaBan, MaTV),
    CONSTRAINT FK_BCTV_Ban FOREIGN KEY (MaBan) REFERENCES BanChuyenMon(MaBan) ON DELETE CASCADE,
    CONSTRAINT FK_BCTV_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE,
    CONSTRAINT CK_BCTV_NgayRaBan CHECK (NgayRaBan IS NULL OR NgayRaBan >= NgayThamGiaBan)
);
GO
SELECT * FROM TaiKhoan
WHERE TenDN = 'thong' 
  AND MatKhau = 'ec278a38901287b2771a13739520384d43e4b078f78affe702def108774cce24';

-- Bảng trung gian: Thành viên - Kỹ năng
CREATE TABLE ThanhVien_KyNang (
    MaTV INT NOT NULL,
    MaKN INT NOT NULL,
    Diem FLOAT DEFAULT 0 CHECK (Diem BETWEEN 0 AND 10),
    CapDoHienTai NVARCHAR(50) CHECK (CapDoHienTai IN (N'Cơ bản', N'Trung bình', N'Nâng cao', N'Chuyên gia')),
    NgayCapNhat DATE DEFAULT GETDATE(),
    GhiChu NVARCHAR(255) NULL,
    
    PRIMARY KEY (MaTV, MaKN),
    CONSTRAINT FK_TVKN_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE,
    CONSTRAINT FK_TVKN_KN FOREIGN KEY (MaKN) REFERENCES KyNang(MaKN) ON DELETE CASCADE
);
GO

-- =============================================
-- PHẦN 5: HOẠT ĐỘNG
-- =============================================

CREATE TABLE HoatDong (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    TenHD NVARCHAR(200) NOT NULL,
    NgayToChuc DATE NULL,
    GioBatDau TIME NULL,
    GioKetThuc TIME NULL,
    DiaDiem NVARCHAR(255) NULL,
    MoTa NVARCHAR(MAX) NULL,
    MaLoaiHD INT NULL,
    NguoiPhuTrach INT NULL,
    KinhPhiDuKien MONEY NULL CHECK (KinhPhiDuKien >= 0),
    KinhPhiThucTe MONEY NULL CHECK (KinhPhiThucTe >= 0),
    SoLuongToiDa INT NULL CHECK (SoLuongToiDa > 0),
    TrangThai NVARCHAR(50) DEFAULT N'Đang chuẩn bị' CHECK (TrangThai IN (N'Đang chuẩn bị', N'Đang diễn ra', N'Hoàn thành', N'Hủy bỏ')),
    NgayTao DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_HoatDong_Loai FOREIGN KEY (MaLoaiHD) REFERENCES LoaiHoatDong(MaLoaiHD),
    CONSTRAINT FK_HoatDong_TV FOREIGN KEY (NguoiPhuTrach) REFERENCES ThanhVien(MaTV),
    CONSTRAINT CK_HoatDong_NgayToChuc CHECK (NgayToChuc >= CAST(GETDATE() AS DATE) OR TrangThai IN (N'Hoàn thành', N'Hủy bỏ'))
);
GO

CREATE NONCLUSTERED INDEX IX_HoatDong_NgayToChuc ON HoatDong(NgayToChuc);
CREATE NONCLUSTERED INDEX IX_HoatDong_TrangThai ON HoatDong(TrangThai);
GO

-- Đăng ký hoạt động
CREATE TABLE DangKyHoatDong (
    MaDK INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    MaHD INT NOT NULL,
    ThoiGianDangKy DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) DEFAULT N'Chờ duyệt' CHECK (TrangThai IN (N'Chờ duyệt', N'Đã duyệt', N'Từ chối', N'Hủy')),
    LyDoTuChoi NVARCHAR(255) NULL,
    
    CONSTRAINT UQ_DangKy UNIQUE (MaTV, MaHD),
    CONSTRAINT FK_DK_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE,
    CONSTRAINT FK_DK_HD FOREIGN KEY (MaHD) REFERENCES HoatDong(MaHD) ON DELETE CASCADE
);
GO

-- Tham gia hoạt động
CREATE TABLE ThamGia (
    MaHD INT NOT NULL,
    MaTV INT NOT NULL,
    VaiTroTrongHD NVARCHAR(100) NULL,
    DiemDanh BIT DEFAULT 0,
    GioCheckIn DATETIME NULL,
    DiemThuong FLOAT DEFAULT 0 CHECK (DiemThuong >= 0),
    DanhGia NVARCHAR(MAX) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    
    PRIMARY KEY (MaHD, MaTV),
    CONSTRAINT FK_ThamGia_HD FOREIGN KEY (MaHD) REFERENCES HoatDong(MaHD) ON DELETE CASCADE,
    CONSTRAINT FK_ThamGia_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE
);
GO

-- =============================================
-- PHẦN 6: DỰ ÁN
-- =============================================

CREATE TABLE DuAn (
    MaDA INT IDENTITY(1,1) PRIMARY KEY,
    TenDuAn NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NULL,
    NgayBatDau DATE NULL,
    NgayKetThuc DATE NULL,
    MucDoUuTien NVARCHAR(20) CHECK (MucDoUuTien IN (N'Thấp', N'Trung bình', N'Cao', N'Khẩn cấp')),
    TienDo INT DEFAULT 0 CHECK (TienDo BETWEEN 0 AND 100),
    TrangThai NVARCHAR(50) DEFAULT N'Đang thực hiện' CHECK (TrangThai IN (N'Mới tạo', N'Đang thực hiện', N'Tạm dừng', N'Hoàn thành', N'Hủy bỏ')),
    NgayTao DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT CK_DuAn_NgayKetThuc CHECK (NgayKetThuc IS NULL OR NgayKetThuc >= NgayBatDau)
);
GO

-- Phân công dự án
CREATE TABLE PhanCong (
    MaPC INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    MaDA INT NOT NULL,
    NhiemVu NVARCHAR(MAX) NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Chưa hoàn thành' CHECK (TrangThai IN (N'Chưa hoàn thành', N'Đang thực hiện', N'Hoàn thành', N'Quá hạn')),
    NgayPhanCong DATETIME DEFAULT GETDATE(),
    NgayHetHan DATE NULL,
    GhiChu NVARCHAR(MAX) NULL,
    
    CONSTRAINT UQ_PhanCong UNIQUE (MaTV, MaDA),
    CONSTRAINT FK_PhanCong_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_DA FOREIGN KEY (MaDA) REFERENCES DuAn(MaDA) ON DELETE CASCADE
);
GO

-- =============================================
-- PHẦN 7: TÀI CHÍNH
-- =============================================

CREATE TABLE ThuChi (
    MaGD INT IDENTITY(1,1) PRIMARY KEY,
    LoaiGD NVARCHAR(10) NOT NULL CHECK (LoaiGD IN (N'Thu', N'Chi')),
    SoTien MONEY NOT NULL CHECK (SoTien > 0),
    NgayGD DATE DEFAULT GETDATE(),
    NoiDung NVARCHAR(MAX) NULL,
    NguoiThucHien INT NULL,
    MaHD INT NULL,
    MaNguon INT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đã xác nhận' CHECK (TrangThai IN (N'Chờ duyệt', N'Đã xác nhận', N'Từ chối')),
    NgayTao DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_ThuChi_TV FOREIGN KEY (NguoiThucHien) REFERENCES ThanhVien(MaTV),
    CONSTRAINT FK_ThuChi_HD FOREIGN KEY (MaHD) REFERENCES HoatDong(MaHD),
    CONSTRAINT FK_ThuChi_Nguon FOREIGN KEY (MaNguon) REFERENCES NguonThu(MaNguon)
);
GO

CREATE NONCLUSTERED INDEX IX_ThuChi_NgayGD ON ThuChi(NgayGD);
CREATE NONCLUSTERED INDEX IX_ThuChi_LoaiGD ON ThuChi(LoaiGD);
GO

-- Chi tiết thu chi
CREATE TABLE ThuChi_ChiTiet (
    MaCT INT IDENTITY(1,1) PRIMARY KEY,
    MaGD INT NOT NULL,
    NoiDung NVARCHAR(MAX) NULL,
    SoTien DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    DonViTinh NVARCHAR(50) NULL,
    SoLuong INT NULL CHECK (SoLuong >= 0),
    
    CONSTRAINT FK_ThuChiCT_ThuChi FOREIGN KEY (MaGD) REFERENCES ThuChi(MaGD) ON DELETE CASCADE
);
GO

-- =============================================
-- PHẦN 8: TÀI SẢN
-- =============================================

CREATE TABLE TaiSan (
    MaTS INT IDENTITY(1,1) PRIMARY KEY,
    TenTS NVARCHAR(255) NOT NULL,
    MaTS_Code NVARCHAR(50) UNIQUE NULL, -- Mã tài sản để dán lên thiết bị
    SoLuong INT NOT NULL DEFAULT 1 CHECK (SoLuong >= 0),
    DonViTinh NVARCHAR(50) NULL,
    TinhTrang NVARCHAR(100) DEFAULT N'Tốt' CHECK (TinhTrang IN (N'Tốt', N'Bình thường', N'Cần sửa chữa', N'Hỏng')),
    GiaTriMua MONEY NULL CHECK (GiaTriMua >= 0),
    NgayNhap DATE DEFAULT GETDATE(),
    NguoiQuanLy INT NULL,
    ViTri NVARCHAR(255) NULL,
    GhiChu NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_TaiSan_TV FOREIGN KEY (NguoiQuanLy) REFERENCES ThanhVien(MaTV)
);
GO

-- Lịch sử mượn trả tài sản
CREATE TABLE MuonTraTaiSan (
    MaMuon INT IDENTITY(1,1) PRIMARY KEY,
    MaTS INT NOT NULL,
    MaTV INT NOT NULL,
    NgayMuon DATETIME DEFAULT GETDATE(),
    NgayTraDuKien DATE NULL,
    NgayTraThucTe DATETIME NULL,
    MucDich NVARCHAR(255) NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đang mượn' CHECK (TrangThai IN (N'Đang mượn', N'Đã trả', N'Quá hạn')),
    GhiChu NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_MuonTra_TS FOREIGN KEY (MaTS) REFERENCES TaiSan(MaTS),
    CONSTRAINT FK_MuonTra_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV),
    CONSTRAINT CK_MuonTra_NgayTra CHECK (NgayTraDuKien IS NULL OR NgayTraDuKien >= CAST(NgayMuon AS DATE))
);
GO

-- =============================================
-- PHẦN 9: LỊCH HỌP
-- =============================================

CREATE TABLE LichHop (
    MaLH INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    NgayHop DATETIME NOT NULL,
    DiaDiem NVARCHAR(255) NULL,
    NoiDung NVARCHAR(MAX) NULL,
    NguoiChuTri INT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Sắp diễn ra' CHECK (TrangThai IN (N'Sắp diễn ra', N'Đang diễn ra', N'Hoàn thành', N'Hủy bỏ')),
    GhiChu NVARCHAR(MAX) NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_LichHop_TV FOREIGN KEY (NguoiChuTri) REFERENCES ThanhVien(MaTV)
);
GO

-- Điểm danh lịch họp
CREATE TABLE DiemDanhLichHop (
    MaLH INT NOT NULL,
    MaTV INT NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa điểm danh' CHECK (TrangThai IN (N'Có mặt', N'Vắng', N'Trễ', N'Có phép', N'Chưa điểm danh')),
    GioCheckIn DATETIME NULL,
    GhiChu NVARCHAR(255) NULL,
    
    PRIMARY KEY (MaLH, MaTV),
    CONSTRAINT FK_DDLH_LH FOREIGN KEY (MaLH) REFERENCES LichHop(MaLH) ON DELETE CASCADE,
    CONSTRAINT FK_DDLH_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV) ON DELETE CASCADE
);
GO

-- =============================================
-- PHẦN 10: KHEN THƯỞNG & KỶ LUẬT
-- =============================================

CREATE TABLE KhenThuong (
    MaKT INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    LyDo NVARCHAR(MAX) NOT NULL,
    HinhThuc NVARCHAR(255) NULL, -- 'Bằng khen', 'Giấy khen', 'Quà tặng', 'Tiền thưởng'
    GiaTriThuong MONEY NULL CHECK (GiaTriThuong >= 0),
    NgayKT DATE DEFAULT GETDATE(),
    NguoiLap INT NULL,
    GhiChu NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_KhenThuong_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV),
    CONSTRAINT FK_KhenThuong_NguoiLap FOREIGN KEY (NguoiLap) REFERENCES ThanhVien(MaTV)
);
GO

CREATE TABLE KyLuat (
    MaKL INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    LyDo NVARCHAR(MAX) NOT NULL,
    HinhThuc NVARCHAR(255) NULL, -- 'Cảnh cáo', 'Khiển trách', 'Đình chỉ tạm thời'
    ThoiGianKyLuat INT NULL, -- Số ngày đình chỉ (nếu có)
    NgayKL DATE DEFAULT GETDATE(),
    NguoiLap INT NULL,
    GhiChu NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_KyLuat_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV),
    CONSTRAINT FK_KyLuat_NguoiLap FOREIGN KEY (NguoiLap) REFERENCES ThanhVien(MaTV)
);
GO

-- =============================================
-- PHẦN 11: GIAO TIẾP & PHẢN HỒI
-- =============================================

CREATE TABLE Feedback (
    MaFB INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    MaHD INT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    DanhGiaSao INT CHECK (DanhGiaSao BETWEEN 1 AND 5),
    NgayGopY DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) DEFAULT N'Đã nhận' CHECK (TrangThai IN (N'Đã nhận', N'Đang xử lý', N'Đã xử lý')),
    PhanHoi NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_Feedback_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV),
    CONSTRAINT FK_Feedback_HD FOREIGN KEY (MaHD) REFERENCES HoatDong(MaHD)
);
GO

CREATE TABLE ThongBao (
    MaTB INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    LoaiThongBao NVARCHAR(50) CHECK (LoaiThongBao IN (N'Thông báo chung', N'Khẩn cấp', N'Sự kiện', N'Nhắc nhở')),
    NgayDang DATETIME DEFAULT GETDATE(),
    NgayGui DATETIME DEFAULT GETDATE(),
    NguoiDang INT NULL,
    DoiTuong NVARCHAR(100) DEFAULT N'Tất cả',
    TrangThai NVARCHAR(50) DEFAULT N'Đã gửi' CHECK (TrangThai IN (N'Nháp', N'Đã gửi')),
    
    CONSTRAINT FK_ThongBao_TV FOREIGN KEY (NguoiDang) REFERENCES ThanhVien(MaTV)
);
GO

CREATE NONCLUSTERED INDEX IX_ThongBao_NgayGui ON ThongBao(NgayGui);
GO

CREATE TABLE TinNhan (
    MaTN INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiGui INT NOT NULL,
    MaNguoiNhan INT NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    NgayGui DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) DEFAULT N'Chưa đọc' CHECK (TrangThai IN (N'Chưa đọc', N'Đã đọc')),
    
    CONSTRAINT FK_TinNhan_Gui FOREIGN KEY (MaNguoiGui) REFERENCES ThanhVien(MaTV),
    CONSTRAINT FK_TinNhan_Nhan FOREIGN KEY (MaNguoiNhan) REFERENCES ThanhVien(MaTV),
    CONSTRAINT CK_TinNhan_KhacNguoi CHECK (MaNguoiGui <> MaNguoiNhan)
);
GO

CREATE NONCLUSTERED INDEX IX_TinNhan_NguoiNhan ON TinNhan(MaNguoiNhan, TrangThai);
GO

-- =============================================
-- PHẦN 12: FILE & BÁO CÁO
-- =============================================

CREATE TABLE FileDinhKem (
    MaFile INT IDENTITY(1,1) PRIMARY KEY,
    TenFile NVARCHAR(255) NOT NULL,
    DuongDan NVARCHAR(500) NOT NULL,
    KichThuoc BIGINT NULL, -- Byte
    LoaiFile NVARCHAR(50) NULL,
    MaDA INT NULL,
    MaHD INT NULL,
    MaLH INT NULL,
    NguoiTai INT NULL,
    NgayTai DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_File_DA FOREIGN KEY (MaDA) REFERENCES DuAn(MaDA),
    CONSTRAINT FK_File_HD FOREIGN KEY (MaHD) REFERENCES HoatDong(MaHD),
    CONSTRAINT FK_File_LH FOREIGN KEY (MaLH) REFERENCES LichHop(MaLH),
    CONSTRAINT FK_File_TV FOREIGN KEY (NguoiTai) REFERENCES ThanhVien(MaTV)
);
GO

CREATE TABLE BaoCao (
    MaBC INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    LoaiBC NVARCHAR(100) CHECK (LoaiBC IN (N'Tháng', N'Quý', N'Năm', N'Hoạt động', N'Tài chính', N'Khác')),
    NoiDung NVARCHAR(MAX) NULL,
    TuNgay DATE NULL,
    DenNgay DATE NULL,
    NgayLap DATETIME DEFAULT GETDATE(),
    NguoiLap INT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Nháp' CHECK (TrangThai IN (N'Nháp', N'Hoàn thành')),
    
    CONSTRAINT FK_BaoCao_TV FOREIGN KEY (NguoiLap) REFERENCES ThanhVien(MaTV),
    CONSTRAINT CK_BaoCao_NgayLap CHECK (DenNgay IS NULL OR DenNgay >= TuNgay)
);
GO

-- =============================================
-- PHẦN 13: ĐIỂM RÈN LUYỆN
-- =============================================

CREATE TABLE DiemRenLuyen (
    MaDRL INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NOT NULL,
    HocKy NVARCHAR(20) NOT NULL,
    NamHoc NVARCHAR(20) NOT NULL,
    Diem INT CHECK (Diem BETWEEN 0 AND 100),
    XepLoai NVARCHAR(20) CHECK (XepLoai IN (N'Xuất sắc', N'Giỏi', N'Khá', N'Trung bình', N'Yếu')),
    GhiChu NVARCHAR(MAX) NULL,
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT UQ_DiemRL UNIQUE (MaTV, HocKy, NamHoc),
    CONSTRAINT FK_DRL_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV)
);
GO

-- =============================================
-- PHẦN 14: LỊCH SỬ THAO TÁC (AUDIT LOG)
-- =============================================

CREATE TABLE LichSuThaoTac (
    MaLSTT INT IDENTITY(1,1) PRIMARY KEY,
    MaTV INT NULL,
    TenBang NVARCHAR(100) NOT NULL,
    LoaiThaoTac NVARCHAR(50) CHECK (LoaiThaoTac IN (N'Thêm', N'Cập nhật', N'Xóa', N'Đăng nhập', N'Đăng xuất')),
    KhoaChinh NVARCHAR(100) NULL,
    NoiDung NVARCHAR(MAX) NULL,
    NgayThucHien DATETIME DEFAULT GETDATE(),
    DiaChiIP NVARCHAR(50) NULL,
    
    CONSTRAINT FK_LichSu_TV FOREIGN KEY (MaTV) REFERENCES ThanhVien(MaTV)
);
GO

CREATE NONCLUSTERED INDEX IX_LichSu_NgayThucHien ON LichSuThaoTac(NgayThucHien DESC);
CREATE NONCLUSTERED INDEX IX_LichSu_MaTV ON LichSuThaoTac(MaTV);
GO