
$version = "1.0.2";

Write-Output "Publishing SteBakerDev.RabbitMQ.NetCore.$version.nupkg";

dotnet pack

dotnet nuget push bin\debug\SteBakerDev.RabbitMQ.NetCore.$version.nupkg -k ${env:NugetKey} -s https://api.nuget.org/v3/index.json