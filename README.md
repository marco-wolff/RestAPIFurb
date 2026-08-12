# RestAPIFurb

API REST desenvolvida em **ASP.NET Core 8** para a Prova de Suficiência de Programação Web II (2026/2) — FURB.

Gerencia o cadastro de **equipamentos**, cada um vinculado a um **tipo** (ex.: Computador, Audiovisual, Impressora), com autenticação via **JWT**, persistência em banco relacional via **Entity Framework Core**, e documentação interativa via **Swagger**.

## ✅ Requisitos do enunciado atendidos

| # | Requisito | Implementação |
|---|-----------|----------------|
| 1 | Web Service REST com JSON e status codes corretos | `Controllers/` — retorna 200, 201, 400, 401, 404 conforme o caso |
| 2 | ORM com nomenclatura padrão (tabela plural, classe singular) | Entity Framework Core — `Models/` + `Data/AppDbContext.cs` |
| 3 | Rota protegida por autenticação via token | JWT — `[Authorize]` em `POST/PUT/DELETE /api/equipamentos` |
| 4 | Documentação via Swagger | Disponível em `/swagger` ao rodar o projeto |
| 5 | Arquitetura separando modelos e serviços (DAO) | `Dao/` (acesso a dados) + `Services/` (regras de negócio) + `Controllers/` |
| 6 | Validação de todos os atributos das classes modelo | `DataAnnotations` em `Models/*.cs` |

## 🛠️ Tecnologias

- ASP.NET Core 8 (Web API)
- Entity Framework Core 8 + SQLite
- JWT Bearer Authentication
- Swashbuckle (Swagger / OpenAPI)

## 📁 Estrutura do projeto

```
RestAPIFurb/
├── Controllers/     -> pontos de entrada HTTP (rotas)
├── Services/        -> regras de negócio
├── Dao/              -> acesso ao banco de dados (EF Core)
├── Models/           -> classes que representam as tabelas
├── Dtos/              -> objetos de entrada (login, atualização parcial)
├── Data/               -> DbContext e dados iniciais (seed)
└── Program.cs           -> configuração geral (EF, JWT, Swagger)
```

## ▶️ Como executar

Pré-requisito: **.NET 8 SDK**.

```bash
git clone https://github.com/marco-wolff/RestAPIFurb.git
cd RestAPIFurb/RestAPIFurb
dotnet restore
dotnet run
```

O banco (SQLite) é criado e populado automaticamente na primeira execução (migrations aplicadas via `db.Database.Migrate()` no `Program.cs`).

Acesse a documentação interativa em:
```
https://localhost:PORTA/swagger
```
(a porta exata aparece no terminal ao rodar, na linha `Now listening on: ...`)

## 🔐 Autenticação

Usuário de teste (já populado no banco):

```json
{
  "login": "admin",
  "senha": "123456"
}
```

1. `POST /api/auth/login` com o JSON acima → retorna `{ "token": "..." }`
2. No Swagger, clique em **Authorize** (ícone de cadeado) e informe: `Bearer {token}`
3. As rotas `POST`, `PUT` e `DELETE` de `/api/equipamentos` passam a funcionar

## 📋 Rotas disponíveis

| Verbo | Rota | Autenticação |
|-------|------|:---:|
| GET | `/api/equipamentos` | não |
| GET | `/api/equipamentos/{id}` | não |
| POST | `/api/equipamentos` | sim |
| PUT | `/api/equipamentos/{id}` | sim (aceita atualização parcial) |
| DELETE | `/api/equipamentos/{id}` | sim |
| GET | `/api/tipos` | não |
| POST | `/api/auth/login` | não |

### Exemplo — criar equipamento

```http
POST /api/equipamentos
Authorization: Bearer {token}
Content-Type: application/json

{
  "nome": "Impressora HP",
  "tipoId": 3
}
```

## 👤 Autor

Marco Wolff — Ciência da Computação, FURB
