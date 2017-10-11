Sample front end automation of a .NET based project.


##Tools / Platforms / Nuget packages those are being used in the automation process with Webdriver & SpecFlow: 

• Visual Studio 2015 Update 3 (Project IDE) 

• SpecFlow (Acceptance Criteria/Scenario writing tool maintained as .feature files. This tool binds to .NET code, i.e. *.feature files to *.cs files) 

• TFS (Project repository) [Location: $/Blackline/BLR Test Automation] 

• AutoIt V3 (External script tool. Scripts are converted to .exe executable which is ran as process from within C# code)(Optional, work around with JavaScript) 

• Chrome, Firefox Driver Server for Selenium (Selenium browser drivers) 

• Google Developer Tools, Firebug, Firepath (Web element locator finding tools) 

• NUnit v4.0.30319 

• SpecRun for SpecFlow 

• RestSharp 



Design Pattern Used: 

• Page Object Pattern (POP)



Language Used: 

• Visual C# 

• Gherkin (Used in writing Specflow feature) 

• JavaScript (Used to write script parts unreachable with Selenium) 



Script Execution Tool Used: 

• SpecRun v2.0.50727 (Used to execute Scenario steps of *.feature files) 
