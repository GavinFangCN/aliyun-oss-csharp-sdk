set /p ver=<VERSION
set sourceUrl=-s https://www.nuget.org/api/v2/package

dotnet nuget push artifacts/Aliyun.OSS.DotNetCore.%ver%.nupkg %sourceUrl%
