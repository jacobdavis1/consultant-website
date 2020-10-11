FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY bin/Release/netcoreapp3.1/publish App/
WORKDIR App/
ENTRYPOINT ["dotnet", "fileshare-server.dll"]
ENV ASPNETCORE_URLS=http://+:5000