.PHONY: help up down build restart logs logs-db shell migrate clean dev

help: ## Lista os comandos disponíveis
	@echo.
	@echo   up             Sobe todos os containers (producao)
	@echo   dev            Sobe o banco + API com hot reload
	@echo   down           Para e remove os containers
	@echo   build          Reconstrói a imagem da API
	@echo   restart        Reinicia apenas a API
	@echo   logs           Exibe os logs da API em tempo real
	@echo   logs-db        Exibe os logs do banco em tempo real
	@echo   shell          Abre um shell dentro do container da API
	@echo   migrate        Aplica as migrations no banco
	@echo   clean          Para containers e remove volumes
	@echo.

up: ## Sobe todos os containers (producao)
	docker compose up -d api db

dev: ## Sobe o banco + API com hot reload
	docker compose up -d db rabbitmq
	docker compose up -d notifications-worker-dev
	docker compose run --rm -it --service-ports api-dev

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
	docker compose build migrator
	docker compose run --rm migrator

clean: ## Para containers e remove volumes (apaga dados do banco)
	docker compose down -v

test: ## Roda todos os testes
	dotnet test

test-unit: ## Roda apenas os testes unitários
	dotnet test tests/UnitTests

test-integration: ## Roda apenas os testes de integração
	dotnet test tests/IntegrationTests