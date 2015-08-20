/**
 * The program is about scrapping data from websites relating to 
 * 1.Safety
 * 2.Attractions * 
 * 3.Traffic
 * 4.Weather
 * 
 * HTMLAgility dll from Nuget Manager has been used to scrap data
 * 
 * Used SQLBULK copy to efficiently send data to database
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace Data_Scrapping
{
    class Main_code
    {
        static void Main(string[] args)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.OptionFixNestedTags = true;

            List<List<string>> sites = new List<List<string>>();
            List<string> lst_safety = new List<string>();
            List<string> lst_attractions = new List<string>();
            List<string> lst_trafficAlerts = new List<string>();
            List<string> lst_weatherinfo = new List<string>();

            sites.Add(lst_safety);
            sites.Add(lst_attractions);
            sites.Add(lst_trafficAlerts);
            sites.Add(lst_weatherinfo);

            Dictionary<string, Dictionary<int, string>> total_list = new Dictionary<string, Dictionary<int, string>>();

            // Adding websites to corresponding category 
            Dictionary<int, string> sub_list_safety = new Dictionary<int, string>();
            sub_list_safety.Add(1, "http://www.neighborhoodscout.com/oh/akron/crime/");

            Dictionary<int, string> sub_list_attractions = new Dictionary<int, string>();
            sub_list_attractions.Add(1, "http://tourist-attractions.find-near-me.info/in/akron-oh");
            sub_list_attractions.Add(2, "http://www.nwf.org/naturefind.aspx");

            Dictionary<int, string> sub_list_traffic = new Dictionary<int, string>();
            sub_list_traffic.Add(1, "http://www.ohgo.com/Dashboard/akron");

            Dictionary<int, string> sub_list_weather = new Dictionary<int, string>();
            sub_list_weather.Add(1, "http://www.accuweather.com/en/us/akron-oh/44308/hourly-weather-forecast/330119");

            total_list.Add("Safety", sub_list_safety);
            total_list.Add("Attractions", sub_list_attractions);
            total_list.Add("Traffic", sub_list_traffic);
            total_list.Add("Weather", sub_list_weather);

            var web = new HtmlWeb();
            DataSet ds = new DataSet("Sites_Collection");

            // Storing the connection settings 
            string connection_string = ConfigurationManager.ConnectionStrings["Connect_string_key_value_pair"].ConnectionString;
            SqlConnection conn = new SqlConnection(connection_string);

            // iterating through our datastructure of type  Dictionary<string, Dictionary<int, string>>
            foreach (var list in total_list)
            {
                var category = list.Key;
                var category_sites_list = list.Value;
                // iterate through the list of all websites related to SAFETY
                if (category == "Safety")
                {
                    foreach (var sites_pair in category_sites_list)
                    {
                        if (sites_pair.Key == 1)
                        {
                            var doc = web.Load("http://www.neighborhoodscout.com/oh/akron/crime/");
                            HtmlNode pages = doc.DocumentNode;
                            //  The element to be scrapped is present @  div[@class='list']//td[@class='name']/span in html page structure

                            HtmlNodeCollection nodes = pages.SelectNodes("//div[@class='list']//td[@class='name']/span");
                            // Create a new data table with name --Safety_Table_1
                            DataTable dt = new DataTable("Safety_Table_1");
                            // Create columns for the datatable
                            DataColumn place_Coulumn = new DataColumn("Place_Name_Akron", Type.GetType("System.String"));
                            DataColumn rating_Coulumn = new DataColumn("Safety_Rating", Type.GetType("System.Int32"));
                            DataRow dr;
                            dt.Columns.Add(place_Coulumn);
                            dt.Columns.Add(rating_Coulumn);

                            // insert data from html node into data columns
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                dr = dt.NewRow();
                                dr["Place_Name_Akron"] = nodes[i].ChildNodes[0].InnerHtml.ToString();
                                dr["Safety_Rating"] = i;
                                dt.Rows.Add(dr);

                            }
                            ds.Tables.Add(dt);
                        }

                    }
                }
                // iterate through the list of all websites related to ATTRACTIONS
                else if (category == "Attractions")
                {
                    foreach (var sites_pair in category_sites_list)
                    {
                        //("http://tourist-attractions.find-near-me.info/in/akron-oh");
                        if (sites_pair.Key == 1)
                        {
                            var doc = web.Load(sites_pair.Value);
                            HtmlNode pages = doc.DocumentNode;
                            //  The element to be scrapped is present @  div[@id='content']//h2 in html page structure

                            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='content']//h2");
                            // Create a new data table with name --Attractions_Table_1
                            DataTable dt = new DataTable("Attractions_Table_1");
                            // Create columns for the datatable
                            DataColumn place_Coulumn = new DataColumn("Placename", Type.GetType("System.String"));
                            DataColumn rating_Coulumn = new DataColumn("Rating", Type.GetType("System.Int32"));
                            DataRow dr;
                            dt.Columns.Add(place_Coulumn);
                            dt.Columns.Add(rating_Coulumn);
                            // insert data from html node into data columns
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                dr = dt.NewRow();
                                dr["Placename"] = nodes[i].ChildNodes[0].InnerHtml.ToString();
                                dr["Rating"] = i;
                                dt.Rows.Add(dr);

                            }
                            ds.Tables.Add(dt);
                        }
                        //"http://www.nwf.org/naturefind.aspx"
                        if (sites_pair.Key == 2)
                        {
                            var doc = web.Load(sites_pair.Value);
                            HtmlNode pages = doc.DocumentNode;
                            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='content_0_grdSiteList']//tr[@class='rgRow']//u");
                            HtmlNodeCollection nodes_alt_places = pages.SelectNodes("//div[@id='content_0_grdSiteList']//tr[@class='rgAltRow']//u");
                            HtmlNodeCollection nodes_dist = pages.SelectNodes("//div[@id='content_0_grdSiteList']//td[@class='rgSorted']");
                            HtmlNodeCollection nodes_alt_dist = pages.SelectNodes("//div[@id='content_0_grdSiteList']//td[@class='rgSorted']");
                            // Create a new data table with name --Traffic_Table_2
                            DataTable dt = new DataTable("Attractions_Table_2");
                            // Create columns for the datatable
                            DataColumn place_Coulumn = new DataColumn("Placename", Type.GetType("System.String"));
                            DataColumn rating_Coulumn = new DataColumn("DistFromAKRON", Type.GetType("System.Int32"));
                            DataRow dr;
                            dt.Columns.Add(place_Coulumn);
                            dt.Columns.Add(rating_Coulumn);
                            // insert data from html node into data columns
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                dr = dt.NewRow();
                                dr["Placename"] = nodes[i].ChildNodes[0].InnerHtml.ToString();
                                dr["DistFromAKRON"] = nodes_dist[i].ChildNodes[0].InnerHtml.ToString();
                                dt.Rows.Add(dr);

                            }
                            ds.Tables.Add(dt);
                        }

                    }
                }
                // iterate through the list of all websites related to TRAFFIC
                else if (category == "Traffic")
                {
                    foreach (var sites_pair in category_sites_list)
                    {
                        //(" http://www.ohgo.com/Dashboard/akron");
                        if (sites_pair.Key == 1)
                        {
                            var doc = web.Load(sites_pair.Value);
                            HtmlNode pages = doc.DocumentNode;

                            //  The element to be scrapped is present @ div[@id='jspPane']//div[@id='alert']//div[@class='highway']
                            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='jspPane']//div[@id='alert']//div[@class='highway']");
                            // Create a new data table with name --Traffic_Table_1
                            DataTable dt = new DataTable("Traffic_Table_1");
                            // Create columns for the datatable
                            DataColumn highway_Coulumn = new DataColumn("Highway", Type.GetType("System.String"));
                            DataColumn category_Coulumn = new DataColumn("Category", Type.GetType("System.String"));
                            DataColumn street_Coulumn = new DataColumn("Street", Type.GetType("System.String"));
                            DataRow dr;
                            dt.Columns.Add(highway_Coulumn);
                            dt.Columns.Add(category_Coulumn);
                            dt.Columns.Add(street_Coulumn);
                            // insert data from html node into data columns
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                dr = dt.NewRow();
                                dr["Highway"] = nodes[i].ChildNodes[0].InnerHtml.ToString();
                                dr["Category"] = i;
                                dr["Street"] = i;
                                dt.Rows.Add(dr);

                            }
                            ds.Tables.Add(dt);
                        }

                    }

                }
                // iterate through the list of all websites related to WEATHER
                else if (category == "Weather")
                {
                    foreach (var sites_pair in category_sites_list)
                    {
                        //("http://www.accuweather.com/en/us/akron-oh/44308/hourly-weather-forecast/330119");
                        if (sites_pair.Key == 1)
                        {
                            var doc = web.Load(sites_pair.Value);
                            HtmlNode pages = doc.DocumentNode;
                            //  The element to be scrapped is present @  div[@class='detail-tab-panel']//section[@id='left-panel']//h1
                            HtmlNodeCollection nodes = pages.SelectNodes("//div[@class='detail-tab-panel']//section[@id='left-panel']//h1");
                            // Create a new data table with name --Weather_Table_1
                            DataTable dt = new DataTable("Weather_Table_1");
                            // Create columns for the datatable
                            DataColumn place_Coulumn = new DataColumn("Place", Type.GetType("System.String"));
                            DataColumn temp_Coulumn = new DataColumn("Temperature", Type.GetType("System.String"));

                            DataRow dr;
                            dt.Columns.Add(place_Coulumn);
                            dt.Columns.Add(temp_Coulumn);

                            // insert data from html node into data columns
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                dr = dt.NewRow();
                                dr["Place"] = nodes[i].ChildNodes[0].InnerHtml.ToString();
                                dr["Temperature"] = i;
                                dt.Rows.Add(dr);

                            }
                            ds.Tables.Add(dt);
                        }

                    }
                }

            }


            var doc1 = web.Load("http://www.neighborhoodscout.com/oh/akron/crime/");
            var doc2 = web.Load("http://tourist-attractions.find-near-me.info/in/akron-oh");
            var doc3 = web.Load("  http://www.ohgo.com/Dashboard/akron");
            var doc4 = web.Load("http://www.accuweather.com/en/us/akron-oh/44308/hourly-weather-forecast/330119");


         /* HtmlNode pages1 = doc1.DocumentNode;
            HtmlNodeCollection nodes1 = pages1.SelectNodes("//div[@class='list']//td[@class='name']/span");
            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='content']//h2");
            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='detail-hourly']//tr[@class='temp']//td");
            HtmlNodeCollection nodes = pages.SelectNodes("//div[@id='dashboard']//section[@id='left-panel']//h1");
        */



            for (int i = 0; i < ds.Tables.Count; i++)
            {
                Console.WriteLine(ds.Tables[i]);
            }

            /*
             * using SQLBULKCOPY  to send all the data collected from all 
             * websites using HTMLNODE element from the datatable.
             * This is disconnected database connection. 
             * 
             */

            using (SqlBulkCopy bc = new SqlBulkCopy(connection_string))
            {

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    bc.DestinationTableName = ds.Tables[i].TableName;

                    Console.WriteLine(DateTime.Now);
                    try
                    {
                        conn.Open();
                        bc.WriteToServer(ds.Tables[i]);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    Console.WriteLine(DateTime.Now);
                }

            }


            Console.ReadLine();
        }


    }
}

