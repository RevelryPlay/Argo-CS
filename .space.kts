/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job(".NET Core desktop. Build, test"){
    container(image = "mcr.microsoft.com/dotnet/core/sdk:6.0"){
        shellScript {
            content = """
                echo Run build...
                dotnet build --no-restore
                echo Run tests...
                dotnet test --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx"
            """
        }
    }
}
