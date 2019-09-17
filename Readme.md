Clone the repository from git
Using Sql Server Management Studio connect to your database instance
Create a new database 
Update appsettings.json. Change ProductDBConnection to your database connection string
Create a product table in your database using script below:

CREATE TABLE dbo.Product

(

       Id                  int           not null identity

       ,Name                nvarchar(512) not null

       ,Code                nvarchar(5)   not null

       ,Description        nvarchar(max) null

       ,Price               decimal(10,2) not null

       ,CreatedOn           datetime      not null default GETUTCDATE()

       ,CONSTRAINT PK_Product PRIMARY KEY(Id)

)

GO

Add unique indexes for Product Name and Code

CREATE UNIQUE NONCLUSTERED INDEX [UCI_ProductNameIndex] ON [dbo].[Product]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE UNIQUE NONCLUSTERED INDEX [UCI_ProductCodeIndex] ON [dbo].[Product]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

It is a best practise to handle uniqueness of column using unique index as it avoids race condition and other concurrency issue

There is a docker file to and one can deploy ProductAPI using docker too.

Running sql server on docker:

SQL Server on Docker:
docker pull mcr.microsoft.com/mssql/server:2017-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<YourStrong@Passw0rd>" \
   -p 1433:1433 --name sql1 \
   -d mcr.microsoft.com/mssql/server:2017-latest
   
Example:

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Test.123" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest

Use below command to get container ip:

docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' container_name_or_id

Use that ip in connection string:

"ProductDBConnection": "Server=172.17.0.2,1433;Database=Product;User Id=sa;Password=Test.123;"

Then simply run application on docker using visual studio


