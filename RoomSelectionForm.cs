using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class RoomSelectionForm : Form
    {
        public string SelectedRoom { get; private set; }
        public DateTime SelectedDate { get; private set; }
        public string SelectedStartTime { get; private set; }
        public string SelectedEndTime { get; private set; }

        private Panel pnlBuildings;
        private Panel pnlFloors;
        private Panel pnlRooms;
        private Panel pnlTimeSelection;
        private Label lblSelectedRoom;
        private Label lblSelectedTime;
        private Button btnConfirm;
        private Button btnCancel;
        private DateTimePicker dtpDate;
        private MaskedTextBox mtbStartTime;
        private MaskedTextBox mtbEndTime;

        // Định nghĩa các dãy nhà và cấu trúc
        private Dictionary<string, BuildingInfo> buildings = new Dictionary<string, BuildingInfo>
        {
            { "DN", new BuildingInfo { Name = "Dãy nhà DN", Floors = 5, RoomsPerFloor = new int[] { 5, 5, 5, 5, 5 } } },
            { "E3", new BuildingInfo { Name = "Dãy E3", Floors = 1, RoomsPerFloor = new int[] { 6 } } },
            { "E4", new BuildingInfo { Name = "Dãy E4", Floors = 1, RoomsPerFloor = new int[] { 6 } } },
            { "E5", new BuildingInfo { Name = "Dãy E5", Floors = 1, RoomsPerFloor = new int[] { 6 } } },
            { "E6", new BuildingInfo { Name = "Dãy E6", Floors = 1, RoomsPerFloor = new int[] { 6 } } },
            { "E7", new BuildingInfo { Name = "Dãy E7 (Máy tính)", Floors = 2, RoomsPerFloor = new int[] { 6, 6 } } },
            { "C2", new BuildingInfo { Name = "Dãy C2", Floors = 5, RoomsPerFloor = new int[] { 5, 5, 5, 5, 5 } } }
        };

        private string currentBuilding = "";
        private int currentFloor = 0;
        private List<EventInfo> allEvents; // Danh sách tất cả sự kiện để kiểm tra trùng

        public RoomSelectionForm(DateTime selectedDate, List<EventInfo> events, string currentLocation = "", string startTime = "", string endTime = "")
        {
            allEvents = events ?? new List<EventInfo>();
            InitializeComponent();
            
            SelectedRoom = currentLocation;
            SelectedDate = selectedDate;
            SelectedStartTime = startTime;
            SelectedEndTime = endTime;
            
            // Set thời gian
            dtpDate.Value = selectedDate;
            if (!string.IsNullOrEmpty(startTime))
            {
                mtbStartTime.Text = startTime.Replace(":", "");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                mtbEndTime.Text = endTime.Replace(":", "");
            }
            
            // Parse current location nếu có
            if (!string.IsNullOrEmpty(currentLocation))
            {
                ParseCurrentLocation(currentLocation);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Chọn phòng học";
            this.Size = new Size(920, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Header
            var lblTitle = new Label
            {
                Text = "SƠ ĐỒ PHÒNG HỌC",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                AutoSize = false,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };
            this.Controls.Add(lblTitle);

            // Main container
            var mainPanel = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(890, 630),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);

            // Panel chọn thời gian (BƯỚC 1)
            var lblTime = new Label
            {
                Text = "1. Chọn thời gian:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblTime);

            pnlTimeSelection = new Panel
            {
                Location = new Point(10, 35),
                Size = new Size(870, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            mainPanel.Controls.Add(pnlTimeSelection);

            CreateTimeSelectionControls();

            // Panel chọn dãy nhà (BƯỚC 2)
            var lblBuilding = new Label
            {
                Text = "2. Chọn dãy nhà:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 110),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblBuilding);

            pnlBuildings = new Panel
            {
                Location = new Point(10, 135),
                Size = new Size(870, 90),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245),
                AutoScroll = true
            };
            mainPanel.Controls.Add(pnlBuildings);

            CreateBuildingButtons();

            // Panel chọn tầng (BƯỚC 3)
            var lblFloor = new Label
            {
                Text = "3. Chọn tầng:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 240),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblFloor);

            pnlFloors = new Panel
            {
                Location = new Point(10, 265),
                Size = new Size(870, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            mainPanel.Controls.Add(pnlFloors);

            // Panel chọn phòng (BƯỚC 4)
            var lblRoom = new Label
            {
                Text = "4. Chọn phòng:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 340),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblRoom);

            pnlRooms = new Panel
            {
                Location = new Point(10, 365),
                Size = new Size(870, 220),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245),
                AutoScroll = true
            };
            mainPanel.Controls.Add(pnlRooms);

            // Selected info display
            lblSelectedTime = new Label
            {
                Text = "Thời gian: Chưa chọn",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(10, 600),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblSelectedTime);

            lblSelectedRoom = new Label
            {
                Text = string.IsNullOrEmpty(SelectedRoom) ? "Phòng: Chưa chọn" : $"Phòng: {SelectedRoom}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                Location = new Point(350, 600),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblSelectedRoom);

            // Buttons
            btnConfirm = new Button
            {
                Text = "Xác nhận",
                Size = new Size(120, 40),
                Location = new Point(660, 705),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(120, 40),
                Location = new Point(790, 705),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnConfirm;
            this.CancelButton = btnCancel;
        }

        private void CreateTimeSelectionControls()
        {
            var lblDate = new Label
            {
                Text = "Ngày:",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 18),
                AutoSize = true
            };
            pnlTimeSelection.Controls.Add(lblDate);

            dtpDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(60, 15),
                Size = new Size(150, 25)
            };
            dtpDate.ValueChanged += TimeSelection_Changed;
            pnlTimeSelection.Controls.Add(dtpDate);

            var lblStart = new Label
            {
                Text = "Từ:",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(230, 18),
                AutoSize = true
            };
            pnlTimeSelection.Controls.Add(lblStart);

            mtbStartTime = new MaskedTextBox
            {
                Font = new Font("Segoe UI", 9F),
                Location = new Point(265, 15),
                Size = new Size(70, 25),
                Mask = "00:00",
                Text = "0800"
            };
            mtbStartTime.TextChanged += TimeSelection_Changed;
            pnlTimeSelection.Controls.Add(mtbStartTime);

            var lblEnd = new Label
            {
                Text = "Đến:",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(350, 18),
                AutoSize = true
            };
            pnlTimeSelection.Controls.Add(lblEnd);

            mtbEndTime = new MaskedTextBox
            {
                Font = new Font("Segoe UI", 9F),
                Location = new Point(395, 15),
                Size = new Size(70, 25),
                Mask = "00:00",
                Text = "0900"
            };
            mtbEndTime.TextChanged += TimeSelection_Changed;
            pnlTimeSelection.Controls.Add(mtbEndTime);

            var btnApplyTime = new Button
            {
                Text = "✓ Áp dụng",
                Size = new Size(100, 30),
                Location = new Point(480, 13),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnApplyTime.FlatAppearance.BorderSize = 0;
            btnApplyTime.Click += BtnApplyTime_Click;
            pnlTimeSelection.Controls.Add(btnApplyTime);
        }

        private void TimeSelection_Changed(object sender, EventArgs e)
        {
            // Reset selection khi thay đổi thời gian
            SelectedRoom = "";
            currentBuilding = "";
            currentFloor = 0;
            lblSelectedRoom.Text = "Phòng: Chưa chọn";
            lblSelectedTime.Text = "Thời gian: Chưa chọn (Click 'Áp dụng')";
            btnConfirm.Enabled = false;
            
            CreateBuildingButtons();
            pnlFloors.Controls.Clear();
            pnlRooms.Controls.Clear();
        }

        private void BtnApplyTime_Click(object sender, EventArgs e)
        {
            if (!ValidateTime(mtbStartTime.Text) || !ValidateTime(mtbEndTime.Text))
            {
                MessageBox.Show("Thời gian không hợp lệ! (Định dạng: HH:mm)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateTimeRange(mtbStartTime.Text, mtbEndTime.Text))
            {
                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lưu thời gian đã chọn - sẽ được trả về form cha
            SelectedDate = dtpDate.Value.Date;
            SelectedStartTime = mtbStartTime.Text;
            SelectedEndTime = mtbEndTime.Text;

            lblSelectedTime.Text = $"Thời gian: {SelectedDate:dd/MM/yyyy} {SelectedStartTime}-{SelectedEndTime}";
            lblSelectedTime.ForeColor = Color.FromArgb(46, 204, 113);

            // Reset phòng selection
            SelectedRoom = "";
            currentBuilding = "";
            currentFloor = 0;
            lblSelectedRoom.Text = "Phòng: Chưa chọn";
            btnConfirm.Enabled = false;

            CreateBuildingButtons();
            pnlFloors.Controls.Clear();
            pnlRooms.Controls.Clear();

            // Thông báo ngắn gọn hơn
            MessageBox.Show("✓ Đã áp dụng thời gian!\n\nThời gian này sẽ được cập nhật khi bạn chọn phòng và xác nhận.", 
                "Thành công",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private bool ValidateTime(string time)
        {
            if (string.IsNullOrWhiteSpace(time) || time.Replace(":", "").Replace(" ", "").Length != 4)
                return false;

            string[] parts = time.Split(':');
            if (parts.Length != 2)
                return false;

            int hours, minutes;
            if (!int.TryParse(parts[0], out hours) || !int.TryParse(parts[1], out minutes))
                return false;

            return hours >= 0 && hours < 24 && minutes >= 0 && minutes < 60;
        }

        private bool ValidateTimeRange(string startTime, string endTime)
        {
            try
            {
                var startParts = startTime.Split(':');
                var endParts = endTime.Split(':');

                int startHour = int.Parse(startParts[0]);
                int startMin = int.Parse(startParts[1]);
                int endHour = int.Parse(endParts[0]);
                int endMin = int.Parse(endParts[1]);

                int startMinutes = startHour * 60 + startMin;
                int endMinutes = endHour * 60 + endMin;

                return endMinutes > startMinutes;
            }
            catch
            {
                return false;
            }
        }

        private void CreateBuildingButtons()
        {
            pnlBuildings.Controls.Clear();
            int x = 10;
            int y = 10;
            int cols = 0;

            foreach (var kvp in buildings)
            {
                var btn = new Button
                {
                    Text = kvp.Value.Name,
                    Tag = kvp.Key,
                    Size = new Size(120, 45),
                    Location = new Point(x, y),
                    BackColor = kvp.Key == currentBuilding ? Color.FromArgb(52, 152, 219) : Color.White,
                    ForeColor = kvp.Key == currentBuilding ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
                btn.FlatAppearance.BorderSize = 2;
                btn.Click += BuildingButton_Click;
                pnlBuildings.Controls.Add(btn);

                x += 130;
                cols++;
                
                // Xuống hàng sau 6 button
                if (cols >= 6)
                {
                    x = 10;
                    y += 55;
                    cols = 0;
                }
            }
        }

        private void BuildingButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedStartTime) || string.IsNullOrEmpty(SelectedEndTime))
            {
                MessageBox.Show("Vui lòng chọn thời gian trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var btn = sender as Button;
            if (btn == null) return;

            currentBuilding = btn.Tag.ToString();
            currentFloor = 0;
            SelectedRoom = "";
            lblSelectedRoom.Text = "Phòng: Chưa chọn";
            btnConfirm.Enabled = false;

            // Refresh building buttons
            foreach (Control ctrl in pnlBuildings.Controls)
            {
                if (ctrl is Button b)
                {
                    b.BackColor = b.Tag.ToString() == currentBuilding ? Color.FromArgb(52, 152, 219) : Color.White;
                    b.ForeColor = b.Tag.ToString() == currentBuilding ? Color.White : Color.Black;
                }
            }

            CreateFloorButtons();
            pnlRooms.Controls.Clear();
        }

        private void CreateFloorButtons()
        {
            pnlFloors.Controls.Clear();

            if (string.IsNullOrEmpty(currentBuilding)) return;

            var building = buildings[currentBuilding];
            int x = 10;
            int y = 10;

            for (int floor = 1; floor <= building.Floors; floor++)
            {
                var btn = new Button
                {
                    Text = $"Tầng {floor}",
                    Tag = floor,
                    Size = new Size(100, 40),
                    Location = new Point(x, y),
                    BackColor = floor == currentFloor ? Color.FromArgb(52, 152, 219) : Color.White,
                    ForeColor = floor == currentFloor ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
                btn.FlatAppearance.BorderSize = 2;
                btn.Click += FloorButton_Click;
                pnlFloors.Controls.Add(btn);

                x += 110;
            }
        }

        private void FloorButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            currentFloor = (int)btn.Tag;
            SelectedRoom = "";
            lblSelectedRoom.Text = "Phòng: Chưa chọn";
            btnConfirm.Enabled = false;

            // Refresh floor buttons
            foreach (Control ctrl in pnlFloors.Controls)
            {
                if (ctrl is Button b)
                {
                    b.BackColor = (int)b.Tag == currentFloor ? Color.FromArgb(52, 152, 219) : Color.White;
                    b.ForeColor = (int)b.Tag == currentFloor ? Color.White : Color.Black;
                }
            }

            CreateRoomButtons();
        }

        private void CreateRoomButtons()
        {
            pnlRooms.Controls.Clear();

            if (string.IsNullOrEmpty(currentBuilding) || currentFloor == 0) return;

            var building = buildings[currentBuilding];
            int roomCount = building.RoomsPerFloor[currentFloor - 1];

            int x = 10;
            int y = 10;
            int cols = 6;

            for (int room = 1; room <= roomCount; room++)
            {
                string roomNumber = building.Floors == 1 ? $"{room:D2}" : $"{currentFloor}{room:D2}";
                string roomName = $"{currentBuilding}.{roomNumber}";

                // Kiểm tra phòng có bị trùng lịch không
                bool isBooked = IsRoomBooked(roomName);

                var btn = new Button
                {
                    Text = roomNumber,
                    Tag = roomName,
                    Size = new Size(130, 50),
                    Location = new Point(x, y),
                    BackColor = isBooked ? Color.FromArgb(100, 100, 100) : 
                                (roomName == SelectedRoom ? Color.FromArgb(46, 204, 113) : Color.White),
                    ForeColor = isBooked || roomName == SelectedRoom ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = isBooked ? Cursors.No : Cursors.Hand,
                    Enabled = !isBooked
                };
                btn.FlatAppearance.BorderColor = isBooked ? Color.FromArgb(70, 70, 70) : Color.FromArgb(52, 152, 219);
                btn.FlatAppearance.BorderSize = 2;
                
                if (!isBooked)
                {
                    btn.Click += RoomButton_Click;
                }
                
                // Tooltip hiển thị tên đầy đủ và trạng thái
                var tooltip = new ToolTip();
                if (isBooked)
                {
                    tooltip.SetToolTip(btn, $"Phòng {roomName}\n❌ Đã được đặt trong khung giờ này");
                }
                else
                {
                    tooltip.SetToolTip(btn, $"Phòng {roomName}\n✓ Có thể đặt");
                }

                pnlRooms.Controls.Add(btn);

                x += 140;
                if (room % cols == 0)
                {
                    x = 10;
                    y += 60;
                }
            }
        }

        private bool IsRoomBooked(string roomName)
        {
            if (allEvents == null || allEvents.Count == 0) return false;
            if (string.IsNullOrEmpty(SelectedStartTime) || string.IsNullOrEmpty(SelectedEndTime)) return false;

            int newStartMinutes = TimeToMinutes(SelectedStartTime);
            int newEndMinutes = TimeToMinutes(SelectedEndTime);

            foreach (var evt in allEvents)
            {
                // Kiểm tra cùng ngày và cùng phòng
                if (evt.Date.Date == SelectedDate.Date &&
                    !string.IsNullOrWhiteSpace(evt.Location) &&
                    evt.Location.Trim().Equals(roomName, StringComparison.OrdinalIgnoreCase))
                {
                    int existingStartMinutes = TimeToMinutes(evt.StartTime);
                    int existingEndMinutes = TimeToMinutes(evt.EndTime);

                    // Kiểm tra trùng thời gian
                    bool isOverlap = (newStartMinutes >= existingStartMinutes && newStartMinutes < existingEndMinutes) ||
                                     (newEndMinutes > existingStartMinutes && newEndMinutes <= existingEndMinutes) ||
                                     (newStartMinutes <= existingStartMinutes && newEndMinutes >= existingEndMinutes);

                    if (isOverlap)
                    {
                        return true; // Phòng đã được đặt
                    }
                }
            }

            return false;
        }

        private int TimeToMinutes(string time)
        {
            try
            {
                var parts = time.Split(':');
                int hours = int.Parse(parts[0]);
                int minutes = int.Parse(parts[1]);
                return hours * 60 + minutes;
            }
            catch
            {
                return 0;
            }
        }

        private void RoomButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            SelectedRoom = btn.Tag.ToString();
            lblSelectedRoom.Text = $"Phòng: {SelectedRoom}";
            lblSelectedRoom.ForeColor = Color.FromArgb(46, 204, 113);
            btnConfirm.Enabled = true;

            // Refresh room buttons
            foreach (Control ctrl in pnlRooms.Controls)
            {
                if (ctrl is Button b && b.Enabled)
                {
                    b.BackColor = b.Tag.ToString() == SelectedRoom ? Color.FromArgb(46, 204, 113) : Color.White;
                    b.ForeColor = b.Tag.ToString() == SelectedRoom ? Color.White : Color.Black;
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedRoom))
            {
                MessageBox.Show("Vui lòng chọn phòng học!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(SelectedStartTime) || string.IsNullOrEmpty(SelectedEndTime))
            {
                MessageBox.Show("Vui lòng chọn thời gian!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ParseCurrentLocation(string location)
        {
            try
            {
                // Format: DN.101, E3.05, C2.305, E7.205
                if (string.IsNullOrEmpty(location)) return;

                var parts = location.Split('.');
                if (parts.Length != 2) return;

                string buildingCode = parts[0].Trim();
                string roomNumber = parts[1].Trim();

                if (buildings.ContainsKey(buildingCode))
                {
                    currentBuilding = buildingCode;
                    var building = buildings[buildingCode];
                    
                    if (building.Floors == 1)
                    {
                        // Dãy E chỉ có 1 tầng (E3, E4, E5, E6)
                        currentFloor = 1;
                    }
                    else if (roomNumber.Length >= 3)
                    {
                        currentFloor = int.Parse(roomNumber.Substring(0, 1));
                    }
                    
                    SelectedRoom = location;

                    // Update UI
                    CreateFloorButtons();
                    CreateRoomButtons();

                    lblSelectedRoom.Text = $"Phòng: {SelectedRoom}";
                    lblSelectedRoom.ForeColor = Color.FromArgb(46, 204, 113);
                }
            }
            catch
            {
                // Ignore parse errors
            }
        }

        private class BuildingInfo
        {
            public string Name { get; set; }
            public int Floors { get; set; }
            public int[] RoomsPerFloor { get; set; }
        }
    }
}
