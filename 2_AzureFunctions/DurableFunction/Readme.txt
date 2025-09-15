Publishing DurableFunction project breaks the Function App in Azure. Every function invocation through HTTP trigger causes
HTTP 504 "Function host is not running." error.

Take host_key on Azure Portal in the Function App - Functions - App keys.

Microsoft default Durable function demo can be triggered by the folloging url: http://defaultDomain/api/Function2_HttpStart?code=<host_key>

"Long running task" durable function's monitoring demo can be triggered by the following URL:
http://defaultDomain/api/Function3_HttpStart?code=<host_key>

Make sure your App service plan is on Y1 pricing. That is a free plan for Azure Function hosting.