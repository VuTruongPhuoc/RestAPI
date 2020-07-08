1. Change to .NET framework 4.7.2 because Elastic APM required<br>
2. Update package.config targetFramework net45 to net472<br>
--Update-Package -reinstall

3. Install Elastic.Apm.AspNetFullFramework Version 1.3.0<br>
-- Install-Package Elastic.Apm.AspNetFullFramework -Version 1.3.0