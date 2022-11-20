# Collector Update Server
### A web server that hosts downloads to the latest version of all software required by Collector to run in a network

This .NET web application is responsible for hosting the latest version of each available application
within the **Collector ecosystem**. 

Admins can upload `.zip` files by navigating to `localhost:7005/Upload`. Each zip file should contain the 
artifacts of an application and should be named `{AppName}-{version}.zip` (e.g. `Charlotte-1.3.zip`). 
All instances of the **Updater** application that is configured to 
install the new version of the given application will download & extract the latest version of 
that application to it's installation folder by accessing the public **API** `localhost:7005/Version`.

All instances of the **Updater** application should point to the `address:port` of your **Update Server** (e.g. `192.168.200.1:7005`).
