using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace BMI
{
    public partial class frmBMI : Form
    {
        public frmBMI()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)


        {
            // 基本輸入檢查
            if (string.IsNullOrWhiteSpace(txtHeight.Text) || string.IsNullOrWhiteSpace(txtWeight.Text))
            {
                MessageBox.Show("請填寫身高與體重。", "輸入不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double height;
            double weight;

            // 嘗試用目前文化或 invariantCulture 解析，增加容錯（小數點/逗號）
            if (!double.TryParse(txtHeight.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out height) &&
                !double.TryParse(txtHeight.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out height))
            {
                MessageBox.Show("身高格式錯誤，請輸入數字（例如：170 或 1.70）。", "格式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHeight.Focus();
                return;
            }

            if (!double.TryParse(txtWeight.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out weight) &&
                !double.TryParse(txtWeight.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out weight))
            {
                MessageBox.Show("體重格式錯誤，請輸入數字（例如：65.5）。", "格式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWeight.Focus();
                return;
            }

            if (height <= 0 || weight <= 0)
            {
                MessageBox.Show("身高與體重必須大於 0。", "數值錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 若使用者輸入看起來像公分（例如 170 或 175），自動轉換為公尺
            if (height > 3) // 大於 3m 明顯為 cm（或誤輸），常見情況: 170
            {
                height = height / 100.0;
                // 可選：更新輸入框顯示成公尺格式
                txtHeight.Text = height.ToString("0.00", CultureInfo.CurrentCulture);
            }

            // 計算 BMI（kg / m^2）
            double bmi = weight / (height * height);

            // 取得分類與顏色
            var result = GetBmiCategoryAndColor(bmi);

            // 顯示結果並用顏色提示（寫入 lblResult）
            lblResult.Text = bmi.ToString("0.00", CultureInfo.CurrentCulture) + " (" + result.category + ")";
            lblResult.ForeColor = result.color;
        }

        // 回傳 BMI 分類與對應顏色;
        private (string category, Color color) GetBmiCategoryAndColor(double bmi)
        {
            if (bmi < 18.5)
                return ("體重過輕", Color.DodgerBlue);
            if (bmi < 24)
                return ("理想體重", Color.Green);
            if (bmi < 27)
                return ("過重", Color.Orange);
            if (bmi < 30)
                return ("輕度肥胖", Color.OrangeRed);
            if (bmi < 35)
                return ("中度肥胖", Color.Red);
            return ("重度肥胖", Color.DarkRed);
        }
    }
}
