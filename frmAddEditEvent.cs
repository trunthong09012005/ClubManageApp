using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class frmAddEditEvent : Form
    {
        public EventInfo Event { get; set; }
        private bool isEditMode;
        private List<EventInfo> existingEvents; // Danh sách sự kiện hiện có để kiểm tra trùng

        public frmAddEditEvent(DateTime selectedDate, EventInfo existingEvent = null)
        {
            InitializeComponent();
            
            isEditMode = existingEvent != null;
            existingEvents = new List<EventInfo>(); // Khởi tạo danh sách rỗng
            
            if (isEditMode)
            {
                this.Text = "Sửa sự kiện";
                Event = existingEvent;
                LoadEventData();
            }
            else
            {
                this.Text = "Thêm sự kiện mới";
                Event = new EventInfo();
                dtpDate.Value = selectedDate;
            }
        }

        // Constructor mới để nhận danh sách sự kiện hiện có
        public frmAddEditEvent(DateTime selectedDate, List<EventInfo> allEvents, EventInfo existingEvent = null)
        {
            InitializeComponent();
            
            isEditMode = existingEvent != null;
            existingEvents = allEvents ?? new List<EventInfo>();
            
            if (isEditMode)
            {
                this.Text = "Sửa sự kiện";
                Event = existingEvent;
                LoadEventData();
            }
            else
            {
                this.Text = "Thêm sự kiện mới";
                Event = new EventInfo();
                dtpDate.Value = selectedDate;
            }
        }

        private void LoadEventData()
        {
            txtTitle.Text = Event.Title;
            dtpDate.Value = Event.Date;
            mtbStartTime.Text = Event.StartTime.Replace(":", "");
            mtbEndTime.Text = Event.EndTime.Replace(":", "");
            txtLocation.Text = Event.Location;
            txtDescription.Text = Event.Description;
        }

        // Xử lý nút chọn phòng từ sơ đồ
        private void btnSelectRoom_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate thời gian trước khi mở form chọn phòng
                if (!ValidateTime(mtbStartTime.Text))
                {
                    MessageBox.Show("Giờ bắt đầu không hợp lệ! (Định dạng: HH:mm)", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbStartTime.Focus();
                    return;
                }

                if (!ValidateTime(mtbEndTime.Text))
                {
                    MessageBox.Show("Giờ kết thúc không hợp lệ! (Định dạng: HH:mm)", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbEndTime.Focus();
                    return;
                }

                if (!ValidateTimeRange(mtbStartTime.Text, mtbEndTime.Text))
                {
                    MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbEndTime.Focus();
                    return;
                }

                using (var roomForm = new RoomSelectionForm(
                    dtpDate.Value.Date, 
                    existingEvents, 
                    txtLocation.Text,
                    mtbStartTime.Text,
                    mtbEndTime.Text))
                {
                    if (roomForm.ShowDialog() == DialogResult.OK)
                    {
                        txtLocation.Text = roomForm.SelectedRoom;
                        // Cập nhật lại thời gian nếu user đổi trong form chọn phòng
                        dtpDate.Value = roomForm.SelectedDate;
                        mtbStartTime.Text = roomForm.SelectedStartTime.Replace(":", "");
                        mtbEndTime.Text = roomForm.SelectedEndTime.Replace(":", "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở sơ đồ phòng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề sự kiện!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (!ValidateTime(mtbStartTime.Text))
            {
                MessageBox.Show("Giờ bắt đầu không hợp lệ! (Định dạng: HH:mm)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbStartTime.Focus();
                return;
            }

            if (!ValidateTime(mtbEndTime.Text))
            {
                MessageBox.Show("Giờ kết thúc không hợp lệ! (Định dạng: HH:mm)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbEndTime.Focus();
                return;
            }

            // Kiểm tra giờ kết thúc phải sau giờ bắt đầu
            if (!ValidateTimeRange(mtbStartTime.Text, mtbEndTime.Text))
            {
                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbEndTime.Focus();
                return;
            }

            // Kiểm tra trùng phòng và giờ
            string location = txtLocation.Text.Trim();
            DateTime selectedDate = dtpDate.Value.Date;
            string startTime = mtbStartTime.Text;
            string endTime = mtbEndTime.Text;

            if (!string.IsNullOrWhiteSpace(location) && existingEvents != null)
            {
                var conflictingEvent = CheckRoomTimeConflict(location, selectedDate, startTime, endTime);
                if (conflictingEvent != null)
                {
                    MessageBox.Show(
                        $"Phòng '{location}' đã được đặt vào ngày {selectedDate:dd/MM/yyyy} từ {conflictingEvent.StartTime} đến {conflictingEvent.EndTime}\n\n" +
                        $"Sự kiện: {conflictingEvent.Title}\n\n" +
                        $"Vui lòng chọn phòng khác hoặc thời gian khác!",
                        "Trùng lịch phòng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtLocation.Focus();
                    return;
                }
            }

            // Save data
            Event.Title = txtTitle.Text.Trim();
            Event.Date = dtpDate.Value.Date;
            Event.StartTime = mtbStartTime.Text;
            Event.EndTime = mtbEndTime.Text;
            Event.Location = txtLocation.Text.Trim();
            Event.Description = txtDescription.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
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

        // Kiểm tra giờ kết thúc sau giờ bắt đầu
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

        // Kiểm tra trùng phòng và giờ
        private EventInfo CheckRoomTimeConflict(string location, DateTime date, string startTime, string endTime)
        {
            if (existingEvents == null || existingEvents.Count == 0)
                return null;

            // Parse thời gian thành phút để so sánh
            int newStartMinutes = TimeToMinutes(startTime);
            int newEndMinutes = TimeToMinutes(endTime);

            foreach (var evt in existingEvents)
            {
                // Bỏ qua sự kiện hiện tại khi đang edit
                if (isEditMode && evt.Id == Event.Id)
                    continue;

                // Kiểm tra cùng ngày và cùng phòng
                if (evt.Date.Date == date.Date && 
                    !string.IsNullOrWhiteSpace(evt.Location) &&
                    evt.Location.Trim().Equals(location, StringComparison.OrdinalIgnoreCase))
                {
                    int existingStartMinutes = TimeToMinutes(evt.StartTime);
                    int existingEndMinutes = TimeToMinutes(evt.EndTime);

                    // Kiểm tra trùng thời gian:
                    // Trùng nếu:
                    // - Thời gian mới bắt đầu trong khoảng thời gian cũ
                    // - Thời gian mới kết thúc trong khoảng thời gian cũ
                    // - Thời gian mới bao phủ hoàn toàn thời gian cũ
                    bool isOverlap = (newStartMinutes >= existingStartMinutes && newStartMinutes < existingEndMinutes) ||
                                     (newEndMinutes > existingStartMinutes && newEndMinutes <= existingEndMinutes) ||
                                     (newStartMinutes <= existingStartMinutes && newEndMinutes >= existingEndMinutes);

                    if (isOverlap)
                    {
                        return evt;
                    }
                }
            }

            return null;
        }

        // Chuyển đổi thời gian HH:mm thành phút
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
    }
}
