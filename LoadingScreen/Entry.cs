using System.Data;
using System.Data.SqlClient;
using System.Timers;

namespace LoadingScreen

{
    public partial class Entry : Form
    {

        public int id { get; set; }
        System.Timers.Timer timer;

        int hour, minute, second;
        int count = 0;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Entry(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        /*  */
        private void button1_Click(object sender, EventArgs e)
        {
            
            count++;

            if (count > 1)
            {
                label1.Text = "Attendance already marked";

            }
            else
            {
                Attendence fe2 = new Attendence();
                fe2.Show();
            }

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            Application.DoEvents();
           
            SqlConnection con = new SqlConnection(@"Data Source=den1.mssql7.gear.host;Initial Catalog=manavpandey157;User ID=manavpandey157;Password=Ko2bC40Ov_0-");
            SqlCommand cmd = new SqlCommand("UPDATE EntryLog SET ExitTime=@ExitTime where id = @id", con);
            cmd.CommandType = CommandType.Text;
            String exitTime = DateTime.Now.ToLongTimeString();
            cmd.Parameters.AddWithValue("@ExitTime", exitTime);
            cmd.Parameters.AddWithValue("@id",id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            System.Diagnostics.Process.Start("shutdown", "/s /t 0");

        }

        private void Entry_Load(object sender, EventArgs e)
        {
            this.Text = "Mark your Attendance";
            timer = new System.Timers.Timer();

            timer.Interval = 1000;  //to make it tick every second
            timer.Elapsed += onTimeEvent;
            timer.Start();

            if (count > 1)
            {
                button1.Hide();
            }
        }


        private void onTimeEvent(object? sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                second += 1;

                if (second == 60)
                {
                    second = 0;
                    minute += 1;
                }
                if (minute == 60)
                {
                    minute = 0;
                    hour += 1;
                }

                textBox1.Text = string.Format("{0}:{1}:{2}", hour.ToString(), minute.ToString().PadLeft(2, '0'), second.ToString().PadLeft(2, '0'));
            }));
        }

        
    }
}
