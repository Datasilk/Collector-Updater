# Collector Updater (and Server)
### A solution for automatically installing & updating web applications & services used by Collector.

## Updater
This .NET worker service runs on all machines within the **Collector Network** and is responsible for 
initially installing all configured applications and then updating existing applications when the 
**Server** posts a new version of any installed & configured application.

Every X minutes, the worker will check the remote REST API (from the **Server** application) for a 
list of supported applications and their latest version number. If any versions of configured & installed
applications mismatch the version provided by the API, the worker will download & install the `.zip` file
associated with the latest version of the given application.

## Server
This .NET web application is responsible for hosting the latest version of each available application
within the **Collector ecosystem**. 

Admins can upload `.zip` files by navigating to `localhost:7005/Upload`. Each zip file should contain the 
artifacts of an application and should be named `{AppName}-{version}.zip` (e.g. `Charlotte-1.3.zip`). 
All instances of the **Updater** application that is configured to 
install the new version of the given application will download & extract the latest version of 
that application to it's installation folder by accessing the public **API** `localhost:7005/Version`.

All instances of the **Updater** application should point to the `address:port` of your **Update Server** (e.g. `192.168.200.1:7005`).