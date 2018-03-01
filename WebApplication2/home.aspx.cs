using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication2
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        }

        protected void CheckBoxie(object sender, EventArgs e)
        {
            int quantity;
            //string q;
            int price = 0;
            string name;
            int priceOfitem;
            int total = 0;
            DataTable selected = new DataTable();
            selected.Columns.AddRange(new DataColumn[3] { new DataColumn("Items"), new DataColumn("Quantity"), new DataColumn("Price") });



            foreach (GridViewRow row in GridView1.Rows)
            {
               
                TextBox txt = (TextBox)row.FindControl("quantity");
                if (Convert.ToInt32(txt.Text)>0)
                {


                    
                    name = row.Cells[1].Text.ToString();
                    price = Convert.ToInt32(row.Cells[3].Text);
                    quantity = Convert.ToInt32(txt.Text);
                    
                    priceOfitem = price * quantity;
                    total = total + priceOfitem;


                    DataRow gv = selected.NewRow();
                    gv["Items"] = row.Cells[1].Text;
                    gv["Quantity"] = Convert.ToInt32(txt.Text);
                    gv["Price"] = priceOfitem.ToString();
                    selected.Rows.Add(gv);

                    Button1.Visible = true;








                }
            }
            TextBox1.Text = "Total Price: " + total.ToString();
            GridView2.DataSource = selected;
            GridView2.DataBind();
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString);
            conn.Open();
            int user = 4;
            var date = DateTime.Now.ToString("yyyy-MM-dd");


            string userinsertion = "INSERT INTO [order] (userid,currentDate) values (@user,@date)";
            SqlCommand c = new SqlCommand(userinsertion, conn);
            c.Parameters.AddWithValue("@user",user);
            c.Parameters.AddWithValue("@date",date);
            Response.Write("insertion");

           int k= c.ExecuteNonQuery();
            Response.Write(k);
            Response.Write("lala");

            
            foreach (GridViewRow row in GridView2.Rows)
            {
                string identify = "SELECT orderid from [order] where userid='" + user+ "'";
                SqlCommand com = new SqlCommand(identify, conn);
                int order = (int)com.ExecuteScalar();
                Response.Write(order);
                //int order = ((int)com.ExecuteScalar());
                string insertion = "INSERT INTO [orderEntries](orderid,pname,quantity,price)VALUES( @order,@pname, @quantity,@price)";
                SqlCommand command = new SqlCommand(insertion, conn);
                command.Parameters.AddWithValue("@order", order);

                command.Parameters.AddWithValue("@pname", row.Cells[0].Text);
               
                command.Parameters.AddWithValue("@quantity",Convert.ToInt32(row.Cells[1].Text));
                
                command.Parameters.AddWithValue("@price", Convert.ToInt32(row.Cells[2].Text));
                
                command.ExecuteNonQuery();
                
               
            }
            conn.Close();
            user = user + 1;

            pickup.Visible = true;
            delivery.Visible = true;
            Button1.Visible = false;


        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {



        }

        protected void pickup_Click(object sender, EventArgs e)
        {
            delivery.Visible = false;
            pickup.Visible = false;
            address.Visible = true;
            address.Text = "Pick your order in 15 minutes,Thankyou Customer!";




        }

        protected void delivery_Click(object sender, EventArgs e)
        {
            pickup.Visible = false;
            address.Visible = true;
            address.Text = "Enter Address";
            delivery.Visible = false;
            address2.Visible = true;
            Button2.Visible = true;

            
            
            

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            address.Visible = false;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString);
            conn.Open();

            string query = "UPDATE [order] SET Address= '" + address2.Text + "'";
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();
            string add = address2.Text;
            address2.Text = "Your order will get dilevered in 45- 175 minutes at, " + add ;
            Button2.Visible = false;


        }

    }



}

   
