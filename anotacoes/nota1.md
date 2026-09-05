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


## migration:
- para criar uma nova migration (local):
```bash
dotnet ef migrations add <nome da migration>
```

- para aplicar as migrations pendentes no banco:
```bash
make migrate
```



## dúvidas que tive ao desenvover o projeto:
### por que fica em dominio os contratos de interface de entidade(a mais simplificada e a do repository), mas não a do service? sendo que a do service fica no dto? é porque é uma camada interna do servidor?
- resposta custa: sim, fica em domínio as interfaces que conversar com recursos externos ao servidor e fica em application interfaces que conversam internamente com o servidor.

- resposta longa:
	- Boa pergunta! Vamos pensar na responsabilidade de cada interface:
	**`IProductRepository`** fica no **domínio** porque é o domínio declarando o que precisa para persistir seus dados. É um contrato do núcleo da aplicação — não depende de nada externo.
	
	**`IProductService`** fica na **application** porque é um contrato de caso de uso — ela orquestra o domínio para atender uma necessidade da aplicação. Ela já conhece DTOs, já faz mapeamento, já lida com regras de aplicação. Não é pura o suficiente para ficar no domínio.
	
	A regra geral é:
	
	```
	Domain      → contratos que só falam em entidades puras
	Application → contratos que falam em DTOs e casos de uso
	```
	
	O domínio não pode conhecer DTOs — se `IProductService` ficasse no domínio, o domínio dependeria de `ProductResponse`, `CreateProductDto`, etc. Isso quebraria o isolamento do domínio.
	
	Resumindo: **domínio só conhece a si mesmo**, application pode conhecer o domínio mas o domínio não pode conhecer a application.

### dd