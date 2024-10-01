# DESAFIO ECommerce
### Tecnologias Utilizadas
- .NET 8.0
- Entity Framework Core - Para mapeamento objeto-relacional (ORM).
- FluentValidation - Para validações de entrada.
- MediatR - Para implementação de eventos de domínio e padrões de comunicação entre camadas.
- Quartz.NET - Para agendamento de tarefas de rotina (Jobs).
- SQLite - Banco de dados utilizado para persistência de dados.
- Docker - Para containerização da aplicação.

### Criar a imagem Docker e subir os containers:
Execute o comando abaixo na raiz do projeto, onde o arquivo docker-compose.yml está localizado:
```bash
docker-compose up --build
```
Este comando irá criar os containers do Docker para a aplicação e o banco de dados SQLite.

### **Checklist do Desafio de E-Commerce**
#### **1. Funcionalidades Gerais da API**
- [x]  Permitir que os usuários **criem pedidos** com um ou mais itens.
- [x]  Permitir que os usuários **acompanhem o status** dos pedidos.
- [x]  Integrar com diferentes serviços internos como **estoque**, **pagamento**, e **entrega**.
---

#### **2. Regras de Fluxo de Pedidos**

#### **Recepção de Pedidos**
- [x]  O usuário pode **criar um pedido** com um ou mais itens, onde cada item tem um preço e quantidade.
- [x]  O sistema deve **calcular o valor total** do pedido e aplicar os descontos conforme as regras de negócio:
    - [x]  Aplicar **desconto por quantidade** em reais para quantidades específicas.
    - [x]  Aplicar **desconto sazonal** (percentual) de acordo com a data do pedido.
- [x]  Um pedido criado com sucesso deve estar no estado "**Aguardando Processamento**".
- [x]  Permitir que pedidos em estado "Aguardando Processamento" sejam **cancelados**, mudando o estado para "Cancelado".

#### **Processando Pagamento**
- [x]  Pedidos que estão "Aguardando Processamento" devem entrar no estado "**Processando Pagamento**".
- [x]  Implementar o processamento do pagamento com a lógica do Padrão Strategy:
    - [x]  **Pagamento à vista com Pix** deve aplicar um desconto de 5%.
    - [x]  **Pagamento parcelado em até 12x no cartão** deve ser processado sem desconto adicional.
- [x]  Após o pagamento ser realizado com sucesso, o pedido deve mudar para o estado "**Pagamento Concluído**".
- [x]  Permitir que pedidos em estado posterior ao "Pagamento Concluído" possam ser cancelados, desde que exista uma **operação de estorno**.
- [x]  Em caso de falha no processamento do pagamento, deve haver até **3 tentativas** de reprocessamento. Se falhar após as tentativas, o pedido deve ser cancelado.

#### **Separando Pedido**
- [x]  Pedidos em "Pagamento Concluído" devem entrar no estado "**Separando Pedido**".
- [x]  Pedidos em "Separando Pedido" devem:
    - [x]  Realizar a **baixa dos produtos em estoque**.
    - [x]  **Enviar um e-mail** para vendas se algum produto não tiver estoque disponível.
- [x]  Pedidos com todos os itens separados com sucesso devem mudar para o estado "**Concluído**".
- [x]  Pedidos que tiverem problema na separação de itens devem ir para o estado "**Aguardando Estoque**".
- [x]  Pedidos que estão "Concluídos" **não podem ser cancelados**.
- [x]  Pedidos "Aguardando Estoque" podem ser cancelados, realizando o **estorno do pagamento** corretamente.
---

#### **3. Regras de Notificações**
- [x]  Enviar **notificações por e-mail** ao cliente para cada troca de estado do pedido.
- [x]  **Uma vez por dia**, gerar uma lista de pedidos do dia anterior e enviar por e-mail ao dono do produto.

### Oberservação no envio de notificações
Para atender ao requisito de enviar notificações aos clientes sobre as mudanças de status dos pedidos, adotei uma abordagem de simulação onde as notificações são salvas no banco de dados. Cada alteração relevante no status do pedido cria uma entrada de notificação no banco, simulando o envio real de um e-mail.
- Cada notificação possui informações como o tipo da notificação, a descrição e a data em que foi gerada.