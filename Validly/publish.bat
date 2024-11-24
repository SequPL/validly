dotnet pack -c Release
set /p version=Package version:
dotnet nuget push bin\Release\Validly.%version%.nupkg --api-key %NUGET_VALIDLY_API_KEY% --source https://api.nuget.org/v3/index.json