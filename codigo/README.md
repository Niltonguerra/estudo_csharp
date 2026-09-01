# Products API — ASP.NET Core 8 + JWT

CRUD de produtos com autenticação JWT.

## Como rodar

```bash
dotnet run
# API sobe em http://localhost:5000
```

## Endpoints

### Auth (sem token)
| Método | Rota               | Body                              |
|--------|--------------------|-----------------------------------|
| POST   | /api/auth/register | `{ "username": "", "password": "" }` |
| POST   | /api/auth/login    | `{ "username": "", "password": "" }` |

Login retorna: `{ "token": "eyJ..." }`

### Products
| Método | Rota                | Auth | Descrição        |
|--------|---------------------|------|------------------|
| GET    | /api/products       | Não  | Lista todos      |
| GET    | /api/products/{id}  | Não  | Busca por ID     |
| POST   | /api/products       | Sim  | Cria produto     |
| PUT    | /api/products/{id}  | Sim  | Atualiza produto |
| DELETE | /api/products/{id}  | Sim  | Remove produto   |

Para endpoints autenticados, incluir header:
```
Authorization: Bearer {token}
```

## Exemplo de uso

```bash
# 1. Registrar usuário
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username": "nilton", "password": "senha123"}'

# 2. Fazer login e pegar o token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "nilton", "password": "senha123"}'

# 3. Criar produto com o token
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{"name": "Notebook", "description": "Dell XPS", "price": 7500.00, "stock": 10}'

# 4. Listar produtos (sem token)
curl http://localhost:5000/api/products
```

## Estrutura do projeto

```
ProductsApi/
├── Controllers/
│   ├── AuthController.cs     # register + login
│   └── ProductsController.cs # CRUD produtos
├── Data/
│   └── AppDbContext.cs       # EF Core (InMemory)
├── DTOs/
│   └── ProductDto.cs         # objetos de entrada/saída
├── Models/
│   ├── Product.cs
│   └── User.cs
├── Services/
│   └── TokenService.cs       # geração do JWT
├── Program.cs                # configuração da aplicação
└── appsettings.json          # configuração do JWT
```

## Para usar banco de dados real

Trocar no `Program.cs`:
```csharp
// InMemory (atual)
opt.UseInMemoryDatabase("ProductsDb")

// SQL Server
opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))

// PostgreSQL
opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
```
