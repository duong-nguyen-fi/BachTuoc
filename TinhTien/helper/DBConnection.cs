using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinhTien.Properties;

namespace TinhTien.helper
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        public static string DatabaseName = "nzuong_btuoc";
        public static string UserNameDb = "nzuong_nzuong";
        public static string PasswordDb = "tyntyn";
        public static string dBServer = "nzuong.heliohost.org";

        //localhost
        //public static string DatabaseName = "btuoc";
        //public static string UserNameDb = "root";
        //public static string PasswordDb = "";
        //public static string dBServer = "localhost";

        //public static string DatabaseName = "ncx89tyznnsqs3lb";
        //public static string UserNameDb = "qctktvqvus2srfjn";
        //public static string PasswordDb = "s1c77u2of7zhhrjv";
        //public static string dBServer = "wvulqmhjj9tbtc1w.cbetxkdyhwsb.us-east-1.rds.amazonaws.com";

        //public static string DatabaseName = "id2789640_btuoc_db";
        //public static string UserNameDb = "id2789640_btuoc_user";
        //public static string PasswordDb = "111111";
        //public static string dBServer =
        //    //"ndng96.000webhostapp.com";
        //    "ns02.000webhost.com"
        //    ;

        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        public enum DBtableNames  {ORDER = 0, ALL_TABLE, CURRENT_TABLE};
        public static string[] DBtablesNames = {"Order","all_tables", "current_table" };
        
        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            
            return _instance;
        }

        internal DataTable TableByDate(string date1, string date2)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            string time_in1, time_in2;

            table.Columns.Add("Bàn", typeof(long));
            table.Columns.Add("Tiền", typeof(string));
            table.Columns.Add("Giờ Vào", typeof(string));
            table.Columns.Add("Giờ Ra", typeof(string));
            table.Columns.Add("ID", typeof(long));
            string sql;
            DateTime d1 = DateTime.Parse(date1);
            DateTime d2 = DateTime.Parse(date2);

            if (d1.Equals(d2) )
            {
                Console.WriteLine("d1 equals d2");
                
                    date1 += " 13:00:00";
                    date2 += " 05:00:00";
                    d1 = DateTime.Parse(date1);
                    d2 = DateTime.Parse(date2);
                    d2 = d2.AddDays(1);
                    time_in1 = d1.ToString("yyyy-MM-dd HH:mm:ss");
                    time_in2 = d2.ToString("yyyy-MM-dd HH:mm:ss");
                
                
            }
            else if (d1 > d2)
            {
                //date1 += " 14:00:00";
                //date2 += " 03:00:00";
                d1 = DateTime.Parse(date1 + " 13:00:00");
                d2 = DateTime.Parse(date1 + " 05:00:00");
                d2 = d2.AddDays(1);
                time_in1 = d1.ToString("yyyy-MM-dd HH:mm:ss");
                time_in2 = d2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                
                date1 += " 13:00:00";
                date2 += " 05:00:00";
                d1 = DateTime.Parse(date1);
                d2 = DateTime.Parse(date2);
                d2 = d2.AddDays(1);
                time_in1 = d1.ToString("yyyy-MM-dd HH:mm:ss");
                time_in2 = d2.ToString("yyyy-MM-dd HH:mm:ss");
            }

            Console.WriteLine("dt1 = {0} dt2= {1}", time_in1, time_in2);

            sql = "SELECT * FROM  `all_tables` WHERE `time_in` between '" + time_in1  +"' and '" + time_in2 +"'" ;
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sum= reader["total"] + "";
                    
                    long num = Int64.Parse(reader["num"] + "");
                    string In = reader["time_in"] + "";
                    string Out = reader["time_out"] + "_";
                    long ID = Int64.Parse(reader["id"] + "");

                    DateTime dateTime = DateTime.Parse(In);
                    In = dateTime.ToString("HH:mm   dd/MM");

                    if (Out.Trim() != "_")
                    {
                        Out = Out.Remove(Out.Length - 1);
                        dateTime = DateTime.Parse(Out);
                        Out = dateTime.ToString("HH:mm   dd/MM ");
                    }
                    else
                        Out = " ";
                    table.Rows.Add(num, Food.formatPrice(Double.Parse(sum)), In, Out, ID);
                }
                reader.Close();
            }
            catch (MySqlException e1)
            {

            }
            

            return table;
        }

        internal void updateTableTimeOutAndSum(long id, double total)
        {
            DateTime dateTime = DateTime.Now;

            string datetime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                string sql = "UPDATE `all_tables` SET  `time_out` = '" + datetime + "', `total` = "+ total  +" WHERE  `id` = " + id;
                Console.WriteLine(sql);
                var cmd = new MySqlCommand(sql, _instance.connection);
                cmd.ExecuteNonQuery();
                //cmd.Connection.Close();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Xảy Ra Lỗi - Hãy Tắt Đi Mở Lại");
            }
            catch (MySqlException e1)
            {
                MessageBox.Show("Xảy Ra Lỗi");
            }
            catch (SocketException e2)
            {
                MessageBox.Show("Xảy Ra Lỗi Kết Nối");
            }
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={3}; database={0}; UID={1}; password={2}; Charset=utf8", DatabaseName,UserNameDb, PasswordDb,dBServer);
                Console.WriteLine(connstring);
                try
                {
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }
                catch (MySqlException e1)
                {
                    Console.WriteLine(e1.ToString());
                    MessageBox.Show("Lỗi khi đọc hệ thông dữ liệu");
                }
                catch (SocketException e2)
                {
                    
                    MessageBox.Show("Không kết nối được với hệ thông dữ liệu");
                    return false;
                }
            }

            return true;
        }

        public  System.Data.DataTable currentOrderTable()
        {
            _instance.connection.Close();
            _instance.connection.Open();
            // New table.
            System.Data.DataTable table = new System.Data.DataTable();

            // Get max columns.

            table.Columns.Add("Bàn", typeof(long));
            table.Columns.Add("Tên", typeof(string));
            table.Columns.Add("Số Lượng", typeof(double));
            
            table.Columns.Add("Ngày Giờ", typeof(string));
            table.Columns.Add("ID", typeof(long));



            // Add rows.
            //foreach (Food food in list)
            //{
            //    //Food food = list[i];
            //    table.Rows.Add(food.Name, food.Price.ToString(), food.Quantity.ToString(), food.Total.ToString());
            //}

            //for heliohost 
            string sql = "SELECT * FROM  `Order hien tai` LIMIT 0,100";
            
            //for heroku jaws
            //string sql = "SELECT * FROM  `CURRENT_NOT_SERVED_ORDERS` LIMIT 0,100";
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["Ten"] + "";
                    double quan = Double.Parse(reader["SLUONG"] + "");
                    long num = Int64.Parse(reader["So_Ban"] + "");
                    string datetime = reader["Ngay_Gio"] + "";
                    long ID = Int64.Parse(reader["ID"] + "");
                    DateTime dateTime = DateTime.Parse(datetime);
                    string dateStr = dateTime.ToString("HH:mm:ss");
                    //dateTime.AddDays(Double.Parse(datetime.Substring()))
                    table.Rows.Add(num, name, quan, dateStr, ID);
                }
                reader.Close();
                //cmd.Connection.Close();
                
            }
            catch (MySqlException e1)
            {
                MessageBox.Show("Loi truy cap du lieu");
            }
            catch (Exception e2)
            { }




            //table.Rows.Add("Tổng Cộng", " ", " ", sum);

            return table;
        }

        public System.Data.DataTable thisTableOrders(long table_id)
        {
            _instance.connection.Close();
            _instance.connection.Open();
            // New table.
            System.Data.DataTable table = new System.Data.DataTable();

            // Get max columns.


            table.Columns.Add("Tên", typeof(string));
            table.Columns.Add("Số Lượng", typeof(double));
            table.Columns.Add("Ngày Giờ", typeof(string));
            table.Columns.Add("Ra Món", typeof(string));
            //table.Columns.Add("Bàn", typeof(int));
            table.Columns.Add("ID", typeof(long));
            //table.Columns.Add("Bàn_ID", typeof(long));




            string sql = "SELECT * FROM  `TAT_CA_ORDER_HIEN_TAI` WHERE `table_id`= "+ table_id +" ORDER BY `NGAY_GIO`  LIMIT 0,30 ";
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["TEN"] + "";
                    double quan = Double.Parse(reader["SL"] + "");
                    long num = Int64.Parse(reader["Ban"] + "");
                    long ID = Int64.Parse(reader["ID"] + "");
                    string datetime = reader["Ngay_Gio"] + "";
                    long table_ID = Int64.Parse(reader["table_id"] + "");

                    DateTime dateTime = DateTime.Parse(datetime);
                    string dateStr = dateTime.ToString("HH:mm:ss");
                    //dateTime.AddDays(Double.Parse(datetime.Substring()))
                    //Console.WriteLine("Reading " + name);
                    bool served = Boolean.Parse(reader["served"] + "");
                    string serve = " ";
                    if (served)
                        serve = "x";
                    
                    table.Rows.Add(name, quan, dateStr, serve,  ID);
                }
                reader.Close();
                //cmd.Connection.Close();
            }
            catch (MySqlException e1)
            {
                Console.WriteLine(e1.StackTrace);
            }
            catch (Exception e2)
            { }




            //table.Rows.Add("Tổng Cộng", " ", " ", sum);

            return table;
        }

        public System.Data.DataTable thisTableReview(long table_id)
        {
            // New table.
            System.Data.DataTable table = new System.Data.DataTable();

            // Get max columns.
            _instance.connection.Close();
            _instance.connection.Open();

            table.Columns.Add("Tên", typeof(string));
            table.Columns.Add("Số Lượng", typeof(double));
            table.Columns.Add("TTiền", typeof(string));
            table.Columns.Add("Ngày Giờ", typeof(string));
            table.Columns.Add("Ra Món", typeof(string));
            




            string sql = "SELECT `name`, `quantity`, `price`, `datetime`, `served` FROM  `Order` WHERE `all_tables_id`= " + table_id + " ORDER BY `datetime`  LIMIT 0,30 ";
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["name"] + "";
                    double quan = Double.Parse(reader["quantity"] + "");
                    string price = reader["price"] + "";
                    
                    string datetime = reader["datetime"] + "";
                    
                    DateTime dateTime = DateTime.Parse(datetime);
                    string dateStr = dateTime.ToString("HH:mm:ss  dd/MM");
                    //dateTime.AddDays(Double.Parse(datetime.Substring()))
                    //Console.WriteLine("Reading " + name);
                    bool served = Boolean.Parse(reader["served"] + "");
                    string serve = " ";
                    if (served)
                        serve = "x";

                    table.Rows.Add(name, quan, Food.formatPrice(Double.Parse(price)), dateStr, serve);
                }
                reader.Close();


                sql = "SELECT  `total` FROM  `all_tables` WHERE id =" + table_id;

                Console.WriteLine(sql);
                cmd = new MySqlCommand(sql, _instance.connection);

                reader = cmd.ExecuteReader();
                reader.Read();

                string total = reader["total"] + "";
                table.Rows.Add("Tổng Cộng", null, Food.formatPrice(Double.Parse(total)), "", "");
                reader.Close();
            }

             
            
            catch (MySqlException e1)
            {

            }
            catch (Exception e2)
            {
                MessageBox.Show("Lỗi kết nối");
            }




            //table.Rows.Add("Tổng Cộng", " ", " ", sum);

            return table;
        }

        public BindingSource bindCurrentOrder()
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            string sql = "SELECT * FROM  `Order hien tai` LIMIT 0,100";
            var cmd = new MySqlCommand(sql, _instance.connection);

            BindingSource bs = new BindingSource();

            adapter.SelectCommand = cmd;
            adapter.Fill(ds);

            bs.DataSource = ds.Tables[0];
            //_instance.Close();
            return bs;
        }

        public void Close()
        {
            connection.Close();
        }

        public void deleteFromCurrentTable(long id)
        {
            _instance.connection.Close();
            _instance.connection.Open();
            string sql = "DELETE FROM `current_table` WHERE all_tables_id =" + id;
            var cmd = new MySqlCommand(sql, _instance.connection);
            cmd.ExecuteNonQuery();
        }

        public void addToCurrentTable(long id)
        {
            string sql = "INSERT INTO `current_table` values (" + id + ")";
            _instance.connection.Close();
            _instance.connection.Open();
            var cmd = new MySqlCommand(sql, _instance.connection);
            cmd.ExecuteNonQuery();
            //cmd.Connection.Close();
        }

        public long addToAllTables(int num, double sum = 0)
        {
            
            DateTime dateTime = DateTime.Now;

            string datetime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string sql = "INSERT INTO `all_tables` values (null," + num + ","+sum+",'"+datetime+"',null)";
            Console.WriteLine(sql);
            _instance.connection.Close();
            _instance.connection.Open();
            var cmd = new MySqlCommand(sql, _instance.connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine("INSERTED ID=" + cmd.LastInsertedId);
            long lastInsertedId = cmd.LastInsertedId;
            //cmd.Connection.Close();
            return lastInsertedId;
        }

        internal void updateOrder( long id, double quan)
        {
            try
            {
                _instance.connection.Close();
                _instance.connection.Open();
                string sql = "UPDATE `Order` SET  `quantity` = "+ quan + " WHERE  `Order`.`id` = " + id;
                Console.WriteLine(sql);
                var cmd = new MySqlCommand(sql, _instance.connection);
                cmd.ExecuteNonQuery();
                //cmd.Connection.Close();
            }
            catch (MySqlException e1)
            {
                MessageBox.Show("Xảy Ra Lỗi");
            }
        }

        

        public string getEntryDateTime(long id)
        {
            string datetime="";
            string sql;
            _instance.connection.Close();
            _instance.connection.Open();
            sql = "SELECT  `time_in` FROM  `all_tables` WHERE id =" + id;
            
            Console.WriteLine(sql);
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {
                var reader = cmd.ExecuteReader();
                reader.Read();
               
                    datetime = reader["time_in"] + "";
                
                Console.WriteLine("Datetime= " + datetime);
                reader.Close();
                //cmd.Connection.Close();
            }
            catch
            { }
            return datetime;
        }

        public string getEntryTimeOut(long id)
        {
            _instance.connection.Close();
            _instance.connection.Open();
            string datetime = "aa";
            string sql;

            sql = "SELECT  `time_out` FROM  `all_tables` WHERE id =" + id;

            Console.WriteLine(sql);
            var cmd = new MySqlCommand(sql, _instance.connection);
            try
            {
                var reader = cmd.ExecuteReader();
                reader.Read();

                datetime += reader["time_out"] + "";

                Console.WriteLine("Datetime= " + datetime);
                reader.Close();
            }
            catch
            { }
            return datetime;
        }

        public void addtoOrder(string name, double quantity, double price, long table_id)
        {
            string datetime;
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("vi-VN");
            DateTime dateTime = DateTime.Now;

            _instance.connection.Close();
            _instance.connection.Open();
            datetime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(table_id);
            //Console.WriteLine("Datetime: " + datetime);
            //for helio
            //string sql =" INSERT INTO `nzuong_btuoc`.`Order` (`id`, `name`, `quantity`, `price`, `datetime`, `all_tables_id`) VALUES(NULL, @name, @quan, @price, @datetime, @id) ";
            //localhost
            string sql =" INSERT INTO `order` (`id`, `name`, `quantity`, `price`, `datetime`, `all_tables_id`) VALUES(NULL, @name, @quan, @price, @datetime, @id) ";

            var cmd = new MySqlCommand(sql, _instance.connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@quan", quantity);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@id", table_id);
            //Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
            //try
            //{
            //    cmd.ExecuteNonQuery();
            //}
            //catch (MySqlException e1)
            //{
            //    MessageBox.Show("Lỗi khi cập nhật dữ liệu");
            //    Console.WriteLine(e1.StackTrace);
            //}
        }

        public void updateOrderServe(long id)
        {
            try
            {
                _instance.connection.Close();
                _instance.connection.Open();
                string sql = "UPDATE `Order` SET  `served` =  true WHERE  `Order`.`id` = " + id;
                Console.WriteLine(sql);
                var cmd = new MySqlCommand(sql, _instance.connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e1)
            {
                MessageBox.Show("Xảy Ra Lỗi");
            }
        }

        public  void createTables()
        {
            //string filePath = "btuoc_create_db.txt";
            string[] fileNames = {"btuoc_create_db.txt","create_all_tables.txt","create_current_table.txt","create_Order.txt", "create_order_hien_tai_view.txt" };
            string directory = "../../Resources/";
            if (!System.IO.Directory.Exists(directory))
                directory = "Resources/";
            string create_db_sql = "";
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    string filePath = directory + fileNames[i];
                    
                    string line;
                    if (!System.IO.File.Exists(filePath))
                        Console.WriteLine("File not exists");
                    else
                    {
                        // Read the file and display it line by line.  
                        System.IO.StreamReader file =
                            new System.IO.StreamReader(filePath);

                        while ((line = file.ReadLine()) != null)
                        {
                            create_db_sql += line;
                        }
                        var cmd = new MySqlCommand(create_db_sql, _instance.Connection);
                        var reader = cmd.ExecuteNonQuery();
                        Console.WriteLine("Create table {0}", reader);
                    }
                }
                catch (MySqlException e2)
                {
                    System.Windows.Forms.MessageBox.Show("Có lỗi DB");
                }
                catch (System.IO.IOException e1)
                {
                    System.Windows.Forms.MessageBox.Show("Có lỗi, kiểm tra file: " + fileNames[i]);


                }
            }
        }

        internal void deleteOrder(long id)
        {
            _instance.connection.Close();
            _instance.connection.Open();
            string sql = "DELETE FROM `Order` WHERE id =" + id;
            var cmd = new MySqlCommand(sql, _instance.connection);
            cmd.ExecuteNonQuery();
        }
    }
}
