dotnet pack -c Release
set /p version=Package version:
dotnet nuget push bin\Release\Valigator.%version%.nupkg --api-key %NUGET_VALIGATOR_API_KEY% --source https://api.nuget.org/v3/index.json