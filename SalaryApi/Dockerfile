#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SalaryApi/SalaryApi.csproj", "SalaryApi/"]
RUN dotnet restore "SalaryApi/SalaryApi.csproj"
COPY . .
WORKDIR "/src/SalaryApi"
RUN dotnet build "SalaryApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalaryApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalaryApi.dll"]