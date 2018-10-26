using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.IO;

public class Sentiment_Api
{
    public OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=path to your database");
    public string status;
    public int exe;
    public List<string> Cdata = new List<string>();
    public List<string> Cdata2 = new List<string>();
    public List<string> Cdata3 = new List<string>();
    public string all_Comments;
	public Sentiment_Api()
	{
		
	}

    // Check if database connectionis open then close it to avoid error

    private void checkconn()
    {
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
    }

    // Optional for returning success or error messages 
    public string msg()
    {
        return status;
    } 

    // load the sentiment table or dataset with data from file e.g .txt

       public void load_sentiments(string filename, string xstatus)
    {
        var lines = File.ReadAllLines(filename);
        foreach (var line in lines)
        {
            new_sentiment(line.ToString(), xstatus);
        }
    }


    // get total number of sentiments words in table
    public int get_total_record_count()
    {
        string check = "select * from tbl_sentimentwords";
        OleDbCommand cmd = new OleDbCommand(check, conn);
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        DataTable dt = new DataTable();
        dt.Clear();
        da.Fill(dt);
        return dt.Rows.Count;

    }

    // get total number of sentiments based on status e.g positive, negative or neutral

    public double get_Sentiments_count( string xstatus)
    {
        checkconn();
        conn.Open();
        string check = "select sentiments_word from tbl_sentimentwords where [status]=@status";
        OleDbCommand cmd = new OleDbCommand(check, conn);
        cmd.Parameters.AddWithValue("@status", xstatus);
        DataTable dt = new DataTable();
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        da.Fill(dt);
        return dt.Rows.Count;
    }


    //get all positive words from sentiments table and add to list

    public List<string> get_Sentiments_Data_P()
    {
        checkconn();
        conn.Open();
        string check = "select sentiments_word from tbl_sentimentwords where [status]=@status";
        OleDbCommand cmd = new OleDbCommand(check, conn);
        cmd.Parameters.AddWithValue("@status", "positive");
        DataTable dt = new DataTable();
        dt.Clear();
        Cdata.Clear();
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        da.Fill(dt);
        OleDbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Cdata.Add(dt.Rows[i][0].ToString());
            }
        }
        dr.Close();
        dr.Dispose();

        return Cdata;
    }

    //get all negative words from sentiments table and add to list
    public List<string> get_Sentiments_Data_N()
    {
        checkconn();
        conn.Open();
        string check = "select sentiments_word from tbl_sentimentwords where [status]=@status";
        OleDbCommand cmd = new OleDbCommand(check, conn);
        cmd.Parameters.AddWithValue("@status", "negative");
        DataTable dt = new DataTable();
        dt.Clear();
        Cdata2.Clear();
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        da.Fill(dt);
        OleDbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Cdata2.Add(dt.Rows[i][0].ToString());
            }
        }
        dr.Close();
        dr.Dispose();

        return Cdata2;
    }

    //get all neutra words from sentiments table and add to list (OPTIONAL)

    public List<string> get_Sentiments_Data_NP()
    {
        checkconn();
        conn.Open();
        string check = "select sentiments_word from tbl_sentimentwords where [status]=@status";
        OleDbCommand cmd = new OleDbCommand(check, conn);
        cmd.Parameters.AddWithValue("@status", "neutral");
        DataTable dt = new DataTable();
        dt.Clear();
        Cdata3.Clear();
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        da.Fill(dt);
        OleDbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Cdata3.Add(dt.Rows[i][0].ToString());
            }
        }
        dr.Close();
        dr.Dispose();

        return Cdata3;
    }



}