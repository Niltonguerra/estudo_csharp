.PHONY: help up down build restart logs shell migrate

help: ## Lista os comandos disponíveis
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2}'

up: ## Sobe todos os containers
	docker compose up -d

down: ## Para e remove os containers
	docker compose down

build: ## Reconstrói a imagem da API
	docker compose build api

restart: ## Reinicia apenas a API
	docker compose restart api

logs: ## Exibe os logs da API em tempo real
	docker compose logs -f api

logs-db: ## Exibe os logs do banco em tempo real
	docker compose logs -f db

shell: ## Abre um shell dentro do container da API
	docker compose exec api sh

migrate: ## Aplica as migrations no banco
	docker compose exec api dotnet ef database update

clean: ## Para containers e remove volumes (apaga dados do banco)
	docker compose down -v
