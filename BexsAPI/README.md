# API BEXS

Projeto desenvolvido em C# com .Net Core 3.1, Entity Framework InMemory, Swagger e Docker.

## Notas

* Nunca trabalhei com C#
* Banco inMemory apenas para facilitar não criar mais um container docker
* Queria criar algo que não tivesse nenhum vínculo com o que trabalhava até mesmo para testar meus conhecimentos.
* Agradeço pela oportunidade e como qualquer projeto sempre pode haver melhorias.

## Começando

As instruções a seguir devem ser realizadas após uma cópia desse projeto para sua máquina.

### Pré-requisitos

Visual Studio 2019 ou 2018 para executar o projeto ou Docker com container Linux

### Instalação

Para compilar o projeto para o container Docker entre no diretório do '\APIBex\BexsAPI' e execute o seguinte comando:

```
docker build -t apibexs . 
```

verifique se foi criada uma imagem para o projeto com o comando. 

```
docker images
```

É esperado uma saída semelhante com essa:

```
bexsapi      latest     63ed9738f768  About a minute ago   232MB
```

Caso a imagem criada execute o comando:

```
docker run -p 9090:80 apibexs:latest 
```

Com isso o container será criado e basta ir no navegador e digitar 

```
http://localhost:9090/index.html
```

Será aberta uma janela com o Swagger mostrando todos os métodos disponibilizados pela API. Essa interface permite uma
interação com API sem a necessidade da interface web (segundo projeto).

