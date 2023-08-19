using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mohbeli.Models;
using mohbeli.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace mohbeli.Repository
{
    public class Data : IData
    {
        public string ConntionString { get; set; }

        public Data()
        {
            ConntionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }

        public void SaveMohBeliDetails(MohBeliDetail details)
        {
            SqlConnection con = new SqlConnection(ConntionString);
            try
            {
                details.TotalAmount = details.Items.Sum(i => i.Price * i.Quantity);
                details.TaxAmount = details.Items.Sum(i => (i.Price * i.Quantity) * 10/100);
                details.NetAmount = details.Items.Sum(i => (i.Price * i.Quantity) - details.TaxAmount);
                con.Open();
                SqlCommand cmd = new SqlCommand("spt_savemohbeliDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerName", details.CustomerName);
                cmd.Parameters.AddWithValue("@MobileNumber", details.MobileNumber);
                cmd.Parameters.AddWithValue("@Address", details.Address);
                cmd.Parameters.AddWithValue("@TotalAmount", details.TotalAmount);
                cmd.Parameters.AddWithValue("@TaxAmount", details.TaxAmount);
                cmd.Parameters.AddWithValue("@NetAmount", details.NetAmount);

                SqlParameter outputPara = new SqlParameter();
                outputPara.DbType = DbType.Int32;
                outputPara.Direction = ParameterDirection.Output;
                outputPara.ParameterName = "@Id";
                cmd.Parameters.Add(outputPara);
                cmd.ExecuteNonQuery();
                int id = int.Parse(outputPara.Value.ToString());
                if (details.Items.Count > 0)
                {
                    SaveMohBeliItems(details.Items, con,id);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void SaveMohBeliItems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into tbl_mohbeliItems(ProductName, Code, Price, Quantity,mohbeliId) values";
                foreach (var item in items)
                {
                    qry += String.Format("('{0}','{1}',{2},{3},{4}),", item.ProductName, item.Code, item.Price, item.Quantity, id);
                }
                qry = qry.Remove(qry.Length - 1);
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<MohBeliDetail> GetAllDetail()
        {
            List<MohBeliDetail> list = new List<MohBeliDetail>();
            MohBeliDetail detail;
            SqlConnection con = new SqlConnection(ConntionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spt_getAllmohbeliDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    detail = new MohBeliDetail();
                    detail.Id = int.Parse(reader["Id"].ToString());
                    detail.CustomerName = reader["CustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    detail.TaxAmount = int.Parse(reader["TaxAmount"].ToString());
                    detail.NetAmount = int.Parse(reader["NetAmount"].ToString());

                    list.Add(detail);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public MohBeliDetail GetDetail(int Id)
        {
            SqlConnection con = new SqlConnection(ConntionString);
            MohBeliDetail detail = new MohBeliDetail();
            Items item;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spt_getmohbeliDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                }
                while (reader.Read())
                {
                    detail.Id = int.Parse(reader["mohbeliId"].ToString());
                    detail.CustomerName = reader["CustomerName"].ToString();
                    detail.MobileNumber = reader["MobileNumber"].ToString();
                    detail.Address = reader["Address"].ToString();
                    detail.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    detail.TaxAmount = int.Parse(reader["TaxAmount"].ToString());
                    detail.NetAmount = int.Parse(reader["NetAmount"].ToString());
                    item = new Items();
                    item.Id = int.Parse(reader["ItemId"].ToString());
                    item.ProductName = reader["ProductName"].ToString();
                    item.Code = reader["Code"].ToString();
                    item.Price = int.Parse(reader["Price"].ToString());
                    item.Quantity = int.Parse(reader["Quantity"].ToString());
                    detail.Items.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return detail;
        }
    }
}