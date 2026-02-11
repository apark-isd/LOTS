
# Introduction 
Lot Operations and Tracking System (LOTS) is a new portal website, the system will allow Parking Services and County department coordinators (users) to view and track parking operations.  LOTS is designed to enhance Parking Services’ operations with both manual and automated parking systems.  LOTS will give department coordinators the ability to view their departments parking permittees, and make request to add new or remove permittees from parking lots or from the LOTS system.  Additionally, the parking app will provide cost and track payments per permittee’s or per department and can also be based on parking lot location.  LOTS will also supply reports/dashboards to easily view Parking Services key performance indicators.  

### Development view
- [Development/Test Build](https://lots_test.azurewebsites.net/Home)

### Deployment views
- [Production Build](https://lots.lacounty.gov/Home)
- [Production Build in Azure](https://lots.azurewebsites.net/Home)

# Configuration
The LOTS system will consists of the following Technologies

### This is an ASP.NET Core application
ASP.NET Core is an open-source and cross-platform framework for building modern cloud based internet connected applications, such as web apps, IoT apps and mobile backends. ASP.NET Core apps run on [.NET Core](https://dot.net), a free, cross-platform and open-source application runtime. It was architected to provide an optimized development framework for apps that are deployed to the cloud or run on-premises. It consists of modular components with minimal overhead, so you retain flexibility while constructing your solutions. You can develop and run your ASP.NET Core apps cross-platform on Windows, Mac and Linux. [Learn more about ASP.NET Core](https://docs.microsoft.com/aspnet/core/).

### using RayGun Crash Reporting 
[RayGun Crash Reporting](https://app.raygun.com/crashreporting/29u6h56?#active) helps detect, diagnose, and resolve errors with ease. RayGun has real-time code-level insights into the health of LOTS and helps in fixing the errors impacting the end-users experience.

### hosted within Azure
Developers will be using Azure DevOps to track and host development. LOTS will also be using Azure Cloud Services to host the URL and SQL database to help reduce cost os system.

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Security issues and bugs

### LOTS is using RayGun Crash Reporting
RayGun Crash Reporting helps detect, diagnose, and resolve errors with ease. RayGun has real-time code-level insights into the health of LOTS and helps in fixing the errors impacting the end-users experience. For more information on [RayGun Crash Reporting](https://raygun.com/platform/crash-reporting?utm_source=crash-reporting&utm_medium=adwords&utm_campaign=brand&utm_term=raygun%20crash%20reporting&utm_matchtype=e&gclid=Cj0KCQjw_4-SBhCgARIsAAlegrUsufOr4ZbFdP2O4qSPkImrcY81yGK-TY9H18z-nHMudE0oQWpDBlwaAsNVEALw_wcB).

### Reporting Microsoft security issues and bugs
Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC)  secure@microsoft.com. You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the MSRC PGP key, can be found in the [Security TechCenter](https://technet.microsoft.com/en-us/security/ff852094.aspx).

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

