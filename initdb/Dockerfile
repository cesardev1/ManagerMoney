FROM debian:10

ENV ACCEPT_EULA=Y
ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https unixodbc-dev && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV PATH="${PATH}:/opt/mssql-tools/bin"

WORKDIR /scripts

ENTRYPOINT ["bash", "-c", "until sqlcmd -S sqlserver -U sa -P $SA_PASSWORD -Q 'SELECT 1' > /dev/null 2>&1; do echo 'Esperando SQL Server...'; sleep 1; done && sqlcmd -S sqlserver -U sa -P $SA_PASSWORD -i /scripts/init.sql"]