# Website-Scrapping:-

The code objective is to scrap all the data from the given list of websites belonging to different categories-(Weather,Safety,Traffic).

All this data is sent to database and stored in <i>SQL Server 2012</i>.To parse the HTML documents ,I have used <i>HTMLAgilitypack.dll</i>.

Using HTMLNodes,the data from the websites has been scrapped and stored in database.For performance efficiency,SQLBulkCopy has been used to send to database via disconnected access.

Later,SQL queries have to written to refine the data extracted.
