FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN mkdir -p app 
COPY app /app

WORKDIR /app

CMD ["dotnet", "Provider.dll"]
EXPOSE 5001

