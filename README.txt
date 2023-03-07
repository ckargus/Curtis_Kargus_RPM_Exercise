This document goes over some of the variables of the application and the instructions to get it started.

Variables:
"DatabaseConnection" is the connection the database that Entity Framework will use.
"MaximumNumberOfDaysToGoBack" is the variable says do not save anything to the database older than this.
"FrequencyOfLoadingWeeklyAverageDieselPricesInDays" is the delay between background job executions in days.

Instructions:
1. Ensure the "DatabaseConnection" variable is pointing to the correct server and database.
2. In the "DataAccess" folder open a console and enter this command "dotnet ef database update".
3. Run the application.
