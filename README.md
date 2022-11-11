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

Admins can upload `.zip` files which contain the artifacts of an application while specifying the 
version of the uploaded file. All instances of the **Updater** application that is configured to 
install the new version of the given application will download & extract the latest version of 
that application to it's installation folder.