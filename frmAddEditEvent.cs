using System;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class frmAddEditEvent : Form
    {
        public EventInfo Event { get; set; }
        private bool isEditMode;

        public frmAddEditEvent(DateTime selectedDate, EventInfo existingEvent = null)
        {
            InitializeComponent();
            
            isEditMode = existingEvent != null;
            
            if (isEditMode)
            {
                this.Text = "S?a S? ki?n";
                Event = existingEvent;
                LoadEventData();
            }
            else
            {
                this.Text = "Thêm S? ki?n m?i";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nh?p tiêu ?? s? ki?n!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (!ValidateTime(mtbStartTime.Text))
            {
                MessageBox.Show("Gi? b?t ??u không h?p l?! (??nh d?ng: HH:mm)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbStartTime.Focus();
                return;
            }

            if (!ValidateTime(mtbEndTime.Text))
            {
                MessageBox.Show("Gi? k?t thúc không h?p l?! (??nh d?ng: HH:mm)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbEndTime.Focus();
                return;
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
    }
}
