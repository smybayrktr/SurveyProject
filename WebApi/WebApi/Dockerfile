FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi/SurveyProject.WebApi.csproj", "WebApi/WebApi/"]
COPY ["Infrastructure/SurveyProject.Infrastructure/SurveyProject.Infrastructure.csproj", "Infrastructure/SurveyProject.Infrastructure/"]
COPY ["Domain/SurveyProject.Entities/SurveyProject.Entities.csproj", "Domain/SurveyProject.Entities/"]
COPY ["Core/SurveyProject.Core/SurveyProject.Core.csproj", "Core/SurveyProject.Core/"]
COPY ["Application/SurveyProject.Services/SurveyProject.Services.csproj", "Application/SurveyProject.Services/"]
COPY ["Application/SurveyProject.DataTransferObjects/SurveyProject.DataTransferObjects.csproj", "Application/SurveyProject.DataTransferObjects/"]
RUN dotnet restore "WebApi/WebApi/SurveyProject.WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi/WebApi"
RUN dotnet build "SurveyProject.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SurveyProject.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyProject.WebApi.dll"]
