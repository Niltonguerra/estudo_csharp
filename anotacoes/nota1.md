criar logs estruturados:
- **Serilog.AspNetCore** — integração com o ASP.NET Core
- **Serilog.Sinks.Console** — saída no console
- **Serilog.Sinks.File** — saída em arquivo

## o que o AppDbContext.cs faz?
- em uma frase:
	- serve como arquivo central de configurações de entidades do lado do servidor servidor para conversar com o sgbd

- mais detalhadamente:
	- mapea as entidades para as tabelas do sgbd
	- carrega os configurations(schemas) das entidades do servidor
		- coisas como: constraints, índices, tamanhos de coluna e etc
	- rastreia todas as mudanças feitas nas entidades em memória

```
Entidade (Product, User)         ← o que existe
    ↓
Configuration (ProductConfiguration)  ← como é no banco
    ↓
AppDbContext                     ← centraliza tudo e conecta ao banco
    ↓
Migration                        ← foto do estado para aplicar no banco
    ↓
PostgreSQL                       ← banco de verdade
```