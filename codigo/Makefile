.PHONY: help up down build restart logs logs-db shell migrate clean

help: ## Lista os comandos disponíveis
	@echo.
	@echo   up             Sobe todos os containers
	@echo   down           Para e remove os containers
	@echo   build          Reconstrói a imagem da API
	@echo   restart        Reinicia apenas a API
	@echo   logs           Exibe os logs da API em tempo real
	@echo   logs-db        Exibe os logs do banco em tempo real
	@echo   shell          Abre um shell dentro do container da API
	@echo   migrate        Aplica as migrations no banco
	@echo   clean          Para containers e remove volumes
	@echo.

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