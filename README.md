I write the assigment in .net Core (version 8.0) and using PostgreSQL version 17.4

please make sure you work with this version too , when you try to run the services.

Setup Instructions:

	for Database Setup:

	1.please install PostgreSQL version 17.4
	2.create new server and new schema in it
	3.execute the script db.sql for create new table called "users_revenue" with columns user_id, revenue
	(you can used pgAdmin or CLI command)

	for .net services Setup : 

	1. install 'Npgsql' Package in DataProcessorFinonex and FinonexServer projects
	you can used command 'Install-Package Npgsql' at Package Manager Console
	2.change the connection string at files server.cs , data_processor.cs to you connection string as following :
	 "Host=myserver;Username=mylogin;Password=mypassword;Database=mydatabase".

	 for running the projects : 

	 1. open cmd and nevigate to the server project name "ServerFinonex" (you will have inside file 'server.cs') 
	 2. write the command 'dotnet run' and click enter -> the server will be executed.
	 3. open cmd and nevigate to the client project name "ClientFinonex" (you will have inside file 'client.cs') 
	 4. write the command 'dotnet run' and click enter -> the client will be executed.
	 5. open cmd and nevigate to the data_processor project name "DataProcessorFinonex" (you will have inside file 'data_processor.cs') 
	 6. write the command 'dotnet run' and click enter -> the data_processor will be executed.

	Comments :
	 1. I assume that the input file context is writing in a valid json format.
	 2. I used to execute an UPDATE together with INSERT (Upsert) query for each event 
	    to ensures efficient handling of data and communication with the DB (instead of do 2 queries on each event we do only one query).
	 3. I used Task and asynchronous operations for better performance and scalability.

  Have a nice day :)

