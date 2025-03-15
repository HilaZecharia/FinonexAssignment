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
	2.change the connection string at files server.cs , data_processor.cs to your connection string as following :
	  "Host=myserver;Username=mylogin;Password=mypassword;Database=mydatabase".

	Running the projects :
 
	 run server :
	 1. open cmd and nevigate to the server project name "ServerFinonex" (you will have inside file 'server.cs') 
	 2. write the command 'dotnet run' and click enter -> the server will be executed.
  
  	 run client :
         1. put the file events.jsonl in the same directory the 'ClientFinonex.exe' exist (if the file doest allready exist)
	 2. open cmd and nevigate to the client project name "ClientFinonex" (you will have inside file 'client.cs') 
         3. write the command 'dotnet run' and click enter -> the client will be executed.
  
         run data processor : 
	 1. change in the file 'data_processor.cs' the path to your 'EventFile.json' file.
	 2. open cmd and nevigate to the data_processor project name "DataProcessorFinonex" (you will have inside file 'data_processor.cs') 
	 3. write the command 'dotnet run' and click enter -> the data_processor will be executed.

  	 * Alternatively : another way to run the projects is clonning this repository in visual studio and execute the project from there 
	   but make shure the file 'events.jsonl' in the same directory the 'ClientFinonex.exe' exist (if the file doest allready exist)

	Comments :
	 1. I assume that the input file context is writing in a valid json format.
  	 2. I assume the user id should not be longer then 256 charecthers 
	 3. I used to execute an UPDATE together with INSERT (Upsert) query for each event 
	    to ensures efficient handling of data and communication with the DB (instead of do 2 queries on each event we do only one query).
	 4. I used Task and asynchronous operations for better performance and scalability.

  Have a nice day :)

