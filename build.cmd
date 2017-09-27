set artifacts=%~dp0artifacts

if exist %artifacts%  rd /q /s %artifacts%

set project=sdk\aliyun-oss-sdk-dotnet.csproj

call dotnet restore %project%

call dotnet build %project% -f netstandard2.0 -c Release -o %artifacts%\netstandard2.0
call dotnet build %project% -f net40 -c Release -o %artifacts%\net40

call dotnet pack %project% -c release -o %artifacts%
