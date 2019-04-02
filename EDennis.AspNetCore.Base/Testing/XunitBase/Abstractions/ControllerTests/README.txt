To create a Controller Test, instantiate the controller in the test class constructor.

If the controller constructor takes one or more Repo objects as parameters, use 
TestRepoFactory methods to generate the repo arguments.

If the controller constructor takes one or more ApiClient objects as parameters, use 
TestApiClientFactory methods to generate the repo arguments.

Within the test methods, simply invoke the Repo methods (actions).  If the actions
return IActionResult or ActionResult, use extension methods from
EDennis.NetCoreTestingUtilities (EDennis.NetCoreTestingUtilities.Extensions namespace)
to extract objects from the results.

