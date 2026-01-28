# Ambev Developer Evaluation

## 📋 Visão Geral

### 🎯 Objetivo do Projeto


### ✅ Requisitos entregues


## 🚀 Como Rodar a Aplicação

### Opção 1: Docker (Recomendado - Ambiente Completo)

#### Pré-requisitos:
- Docker Desktop
- Docker Compose

#### Passos:

```bash
# 1. Rodar todos os containers
docker compose -f docker-compose.yaml up -d

# 2. Verificar status
docker-compose ps

# 3. Acessar a aplicação
```

**URLs disponíveis:**
- **Health Check**: 
- **Swagger UI**: 

#### Configurações Docker:
- **Perfil**: Development (com variável Docker)
- **String de conexão**: `Host=ambev.developerevaluation.database;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n`
- **Migrações**: Rodar script.sql disponibilidado, a ideia é simular um cenário onde o dev não tem acesso ao banco de produção
e precisa gerar o script das migrations de forma idempotente e passar para um DBA com permissão analisar e aplicar o mesmo.

---

### Opção 2: Rider (Desenvolvimento Local) (ou Visual Studio)

#### Pré-requisitos:
- Rider (ou Visual Studio)
- .NET 8.0 SDK
- PostgreSQL local ou Docker

#### Passos:

```powershell
# 1. Configurar apenas o banco de dados via Docker
docker-compose up ambev.developerevaluation.database -d

# 2. Aplicar migrações (se necessário)
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

(ou se preferir rodar o script SQL disponível na pasta ORM/Sripts que é basicamente o script da migration 
gerado de forma idempotente)

# 3. Rodar no Rider (F5)
```

#### Configurações Rider (IDE utilizada para o desenvolvimento):
- **Perfil**: Development
- **String de conexão**: `Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n`
- **Migrações**: Podem ser aplicadas com o CLI do ef core, com `dotnet ef database update "MigrationName" `

---

## 📚 API de Vendas - Funcionalidades Completas

### 🎯 Regras de Negócio Implementadas

O domínio da aplicação tem por responsabilidade aplicar as seguintes regras de negócio: 

1. **4-9 itens idênticos**: 10% de desconto
2. **10-20 itens idênticos**: 20% de desconto
3. **Sem desconto**: Compras abaixo de 4 itens não têm desconto
4. **Limite máximo**: Não é possível vender mais de 20 itens idênticos

### ✅ CRUD Completo Implementado

- **CREATE** - Criar venda com validações de valores, quantidade e aplicar regras de negócio informadas
- **READ** - Retonar um registro de uma venda
- **UPDATE** - Incluir novo produto em uma venda
- **DELETE** - Cancelar vendas (apenas alteração da flag `IsCancelled`)

## 🏗️ Arquitetura e Padrões

### 📁 Estrutura do Projeto

```
abi-gth-omnia-developer-evaluation/
├── src/
│   ├── Ambev.DeveloperEvaluation.WebApi/     # 🌐 Requests, Validators,Controllers e endpoints da API, 
│   ├── Ambev.DeveloperEvaluation.Application/# 📋 Handlers, (CQRS) Commands e Queries, 
│   ├── Ambev.DeveloperEvaluation.Domain/     # 🏛️ Entidades, Regras de Negócio, Eventos
│   ├── Ambev.DeveloperEvaluation.ORM/        # 🗄️ Entity Framework, Migrações de banco de dados e Repositórios
│   ├── Ambev.DeveloperEvaluation.IoC/        # 🔧 Injeção de Dependência
│   └── Ambev.DeveloperEvaluation.Common/     # 🔨 Reutilizáveis, Segurança, Validação
├── tests/                                     # 🧪 Testes unitários, integração e funcionais
├── docs/                                      # 📚 Documentação detalhada da API
└── docker-compose.yml                         # 🐳 Configuração de containers
```

### 📐 Padrões Implementados

- **DDD (Domain-Driven Design)** - Separação clara de domínios
- **Entity Framework Core** - ORM com PostgreSQL
- **Repository Pattern** - Abstração da camada de dados
- **CQRS (Command Query Responsibility Segregation)** - Separação de comandos e consultas
- **Mediator Pattern** - Comunicação desacoplada via MediatR
- **FluentValidation** - Validações declarativas e robustas
- **AutoMapper** - Mapeamento automático de objetos 
- **Logging estruturado** - Logs estruturados com Serilog


### 🔧 Arquivos de Configuração

- **appsettings.json** - Configuração base
- **appsettings.Development.json** - Desenvolvimento local (localhost)

- **Rider**: `appsettings.Development.json` e `launchSettings.json`

## 🛠️ Tecnologias e Frameworks

### Core Technologies
- **.NET 8.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados relacional
- **Docker** - Containerização

### Testing & Quality
- **xUnit** - Framework de testes unitários
- **FluentAssertions** - Assertions mais legíveis nos testes
- **NSubstitute** Libraty to mock classes and objets
- **Fixtures** Fixtures to improve mocks
- **Coverlet** - Cobertura de código

### Libraries & Patterns
- **MediatR** - Mediator pattern para CQRS
- **AutoMapper** - Mapeamento objeto-para-objeto
- **FluentValidation** - Validações fluidas e declarativas
- **Serilog** - Logging estruturado
- **Swagger/OpenAPI** - Documentação automática da API



## 🐛 Troubleshootings

### Alterção no docker-compose.yml
- Como eu desenvolvi em um MacOS, precisei fazer uma pequena alteração no compose para que conseguisse 
subir os containers, uma questão simples de diretórios de OS

Compose informado

```
#    volumes: 
#      - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro => Precisei alterar porque desenvolvi no MAC
#      - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro         => Precisei alterar porque desenvolvi no MacOS

```
- Alteração realizada para conseguir desenvolver no MacOS

```
    volumes:
      - ${HOME}/.microsoft/usersecrets:/home/app/.microsoft/usersecrets:ro
      - ${HOME}/.aspnet/https:/home/app/.aspnet/https:ro
```


## 📊 Score de Compliance

**✅ 100% Compliant** com todos os requisitos da avaliação:

### Requisitos Obrigatórios ✅ 
- **Repositório público** no GitHub
- **CRUD Completo** para Sales
- ** Campos obrigatórios informados** implementados
- **Regras de negócio** de desconto e limitação de quantidade validadas/aplicadas pelo domínio
- **Documentação completa** instruções de uso de scripts e afins.

### Requisitos opcionais ❔

#### Disparo de eventos

* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

## 📋 Endpoints Disponíveis

## Sales API
### - `GET /api/Sales/{id}` - Obter venda específica

#### Exemplo de requisição: 
```
curl --location 'https://localhost:7181/api/Sales/ae86743e-9451-45b2-8c0f-133a9f5c2c9c' \
--header 'accept: */*'
```

#### Exemplo de resposta:

```
{
    "data": {
        "id": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
        "number": 0,
        "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "totalAmount": 2022.8920,
        "cancelledAt": "2026-01-28T01:42:41.636159Z",
        "createdAt": "2026-01-28T01:37:57.213295Z",
        "updatedAt": "2026-01-28T05:21:32.221002Z",
        "isCancelled": false,
        "products": [
            {
                "id": "b14dbd1a-f852-4a6a-b2f2-d49ab8cc3599",
                "saleId": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
                "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "quantity": 1,
                "unitPrice": 10,
                "discountPercentage": 0,
                "discountAmount": 0,
                "totalAmount": 10,
                "createdAt": "0001-01-01T00:00:00",
                "updatedAt": null,
                "cancelledAt": null
            },
            {
                "id": "fce34c19-3aa1-424f-93a6-4afddb938baf",
                "saleId": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
                "productId": "08ad5c46-6ea9-43fb-9954-e4f597568a53",
                "quantity": 7,
                "unitPrice": 229.99,
                "discountPercentage": 0.10,
                "discountAmount": 160.9930,
                "totalAmount": 1448.9370,
                "createdAt": "2026-01-28T05:21:32.218666Z",
                "updatedAt": null,
                "cancelledAt": null
            }
        ]
    },
    "success": true,
    "message": "",
    "errors": []
}
```

### -`POST /api/Sales` - Criar nova venda

#### Exemplo de requisição:
```

curl --location 'https://localhost:7181/api/Sales' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "products": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 10,
      "unitPrice": 300
    }
  ]
}'

```
#### Exemplo de reposta:

```
{
    "data": {
        "id": "afa08b81-aa1b-444d-8ad7-daec7ebc97a2",
        "number": 0,
        "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "totalAmount": 0,
        "cancelledAt": null,
        "createdAt": "2026-01-28T05:26:46.154453Z",
        "updatedAt": "2026-01-28T05:26:46.157237Z",
        "products": [
            {
                "id": "798f0917-9df0-408a-8afe-36b168d3ce4d",
                "saleId": "afa08b81-aa1b-444d-8ad7-daec7ebc97a2",
                "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "unitPrice": 300,
                "discountPercentage": 0.20,
                "discountAmount": 600.00,
                "totalAmount": 0,
                "createdAt": "2026-01-28T05:26:46.15558Z",
                "updatedAt": null,
                "cancelledAt": null
            }
        ]
    }
}
```

### -`PATCH api/Sales/{saleId}/include-product`

#### Exemplo de requisição:

```
curl --location --request PATCH 'https://localhost:7181/api/Sales/ae86743e-9451-45b2-8c0f-133a9f5c2c9c/include-product' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
    "saleId": "f89a04ce-5b7d-4dd8-9591-0404f076ba28",
    "productId": "08ad5c46-6ea9-43fb-9954-e4f597568a53",
    "quantity": 3,
    "unitPrice":  1.99  
}'
```

#### Exemplo de resposta: 

```
{
    "data": {
        "id": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
        "number": 0,
        "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "totalAmount": 2028.8620,
        "cancelledAt": "2026-01-28T01:42:41.636159Z",
        "createdAt": "2026-01-28T01:37:57.213295Z",
        "updatedAt": "2026-01-28T14:07:56.161136Z",
        "isCancelled": false,
        "products": [
            {
                "id": "fce34c19-3aa1-424f-93a6-4afddb938baf",
                "saleId": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
                "productId": "08ad5c46-6ea9-43fb-9954-e4f597568a53",
                "quantity": 7,
                "unitPrice": 229.99,
                "discountPercentage": 0.10,
                "discountAmount": 160.9930,
                "totalAmount": 1448.9370,
                "createdAt": "2026-01-28T05:21:32.218666Z",
                "updatedAt": null,
                "cancelledAt": null
            },
            {
                "id": "0f2628ff-dc1e-42b5-8150-91b882d3b72a",
                "saleId": "ae86743e-9451-45b2-8c0f-133a9f5c2c9c",
                "productId": "08ad5c46-6ea9-43fb-9954-e4f597568a53",
                "quantity": 3,
                "unitPrice": 1.99,
                "discountPercentage": 0,
                "discountAmount": 0.00,
                "totalAmount": 5.97,
                "createdAt": "2026-01-28T14:07:56.159102Z",
                "updatedAt": null,
                "cancelledAt": null
            }
        ]
    }
}

```

### -`DELETE /api/Sales/{id}` - Cancelar venda

#### Exemplo de requisição

```
curl --location --request DELETE 'https://localhost:7181/api/Sales/f89a04ce-5b7d-4dd8-9591-0404f076ba28/cancel' \
--header 'accept: */*'
```
#### Exemplo de resposta

```
{
    "data": {
        "id": "f89a04ce-5b7d-4dd8-9591-0404f076ba28",
        "number": 0,
        "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "totalAmount": 450.00,
        "cancelledAt": "2026-01-28T14:09:22.350218Z",
        "createdAt": "2026-01-28T01:40:04.046303Z",
        "updatedAt": "2026-01-28T01:40:05.745405Z",
        "isCancelled": true,
        "products": [
            {
                "id": "9eb24ec4-e10b-45a5-b8d1-f508432057f8",
                "saleId": "f89a04ce-5b7d-4dd8-9591-0404f076ba28",
                "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "quantity": 5,
                "unitPrice": 100,
                "discountPercentage": 0.10,
                "discountAmount": 50.00,
                "totalAmount": 450.00,
                "createdAt": "2026-01-28T01:40:04.048392Z",
                "updatedAt": null,
                "createdAt": "2026-01-28T01:40:04.048392Z",
            }
        ]
    },
    "success": true,
    "message": "",
    "errors": []
}
```

### Users API (já pré existente no projeto)
- `GET /api/Users` - Listar usuários
- `GET /api/Users/{id}` - Obter usuário específico
- `POST /api/Users` - Criar usuário
- `PUT /api/Users/{id}` - Atualizar usuário
- `DELETE /api/Users/{id}` - Excluir usuário


## 🔐 Credenciais de Acesso

### Banco de Dados PostgreSQL
- **Host**: localhost (local) ou ambev.developerevaluation.database (Docker)
- **Port**: 5432
- **Database**: developer_evaluation
- **Username**: developer
- **Password**: ev@luAt10n

<!-- deveria ser -->
### Branches disponíveis
- `master` - Branch principal com código estável
- `develop` - Branch de desenvolvimento
- `feature/{card-key}` - Para possíveis novas implementações

#### * *Durante o desenvolvimento do projeto optei por fazer tudo na develop e ir abrindo pull requests para a main*

---
 