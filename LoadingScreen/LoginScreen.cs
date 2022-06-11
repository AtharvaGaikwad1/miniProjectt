using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace LoadingScreen
{
    public partial class LoginScreen : Form
    {

        public int id { get; set; }
        
        public LoginScreen()
        {
            InitializeComponent();
        }

        
        SqlConnection conn = new SqlConnection(@"Data Source=den1.mssql7.gear.host;Initial Catalog=manavpandey157;User ID=manavpandey157;Password=Ko2bC40Ov_0-");

        private void button1_Click_1(object sender, EventArgs e)
        {

            
        }


        
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Login";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            


            String username, user_passowrd;

            username = usertxt.Text;
            user_passowrd = passtxt.Text;
           

            try
            {
                String querry = "SELECT * FROM Login WHERE username = '" + usertxt.Text + "' AND password = '" + passtxt.Text + "' ";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);

                DataTable dtable = new DataTable();
                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    username = usertxt.Text;
                    user_passowrd = passtxt.Text;

                    //insert information into database
                    SqlConnection con = new SqlConnection(@"Data Source=den1.mssql7.gear.host;Initial Catalog=manavpandey157;User ID=manavpandey157;Password=Ko2bC40Ov_0-");
                    SqlCommand cmd = new SqlCommand("INSERT INTO EntryLog(Name,PC_Name,EntryTime,MAC_Address) values (@Name,@PC_Name,@EntryTime,@MAC_Address)", con);
                    SqlCommand cmd1 = new SqlCommand("select id from EntryLog where Name=@name AND PC_Name=@pcname AND EntryTime=@entrytime", con);

                    String entryTime = DateTime.Now.ToLongTimeString();
                    String PC_Name = System.Environment.MachineName;
                    var macAddr =
                            (from nic in NetworkInterface.GetAllNetworkInterfaces()
                            where nic.OperationalStatus == OperationalStatus.Up
                            select nic.GetPhysicalAddress().ToString()
                            ).FirstOrDefault();

                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@name", username);
                    cmd1.Parameters.AddWithValue("@pcname", PC_Name);
                    cmd1.Parameters.AddWithValue("@entrytime", entryTime);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", username);
                    cmd.Parameters.AddWithValue("@PC_Name", PC_Name);
                    cmd.Parameters.AddWithValue("@EntryTime", entryTime);
                    cmd.Parameters.AddWithValue("@MAC_Address", macAddr);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlDataReader = cmd1.ExecuteReader();
                    sqlDataReader.Read();
                    id = (int)sqlDataReader["Id"];
                    con.Close();


                    //next screen

                    Entry formn = new Entry(id);
                    formn.Show();
                    this.Hide();
                    button2.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passtxt.Clear();
                    usertxt.Clear();
                }

            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                conn.Close();
            }


        }

        private void usertxt_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            AdminLogin fr5 = new AdminLogin();
            fr5.Show();
            this.Hide();
        }

        private void LoginScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown", "/s /t 0");
        }
    }
}
