Tradervue API Sample Application
================================

This is a set of two simple C# applications that use the [Tradervue API](https://github.com/tradervue/api-docs)
to import some trade data. It also queries the import status before and after the import, to help
give you an idea how the status works.

There are two versions:

- `importer35` - targets .NET 3.5, and uses HttpWebRequest and friends.
- `importer45` - targets .NET 4.5, and uses HttpClient and friends.

Be sure to change the example username and password in Program.cs to be your own Tradervue username/password.
Then you can run the sample in Visual Studio 2012, which will import two trade executions into your account.

This sample isn't intended to show best practices - it's just a trivial example of an app that uses
the Tradervue API.