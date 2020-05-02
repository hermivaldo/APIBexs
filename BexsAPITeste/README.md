# API BEXS

Projeto desenvolvido em C# e XUnit.

## Notas

* Nunca trabalhei com C#
* Queria criar algo que não tivesse nenhum vínculo com o que trabalhava até mesmo para testar meus conhecimentos.
* Agradeço pela oportunidade e como qualquer projeto sempre pode haver melhorias.
* Testes do projeto.
* Tentei aplicar TDD.
* É necessário o Visual Studio 2018 ou 2019.

## Começando

Os testes não estão diretamente dentro do projeto por uma questão de organização, com base nas leituras realizadas nessa semana o 
projeto de teste sempre estava separado.

Tentei aplicar o TDD para primeiramente criar todos os cenários de teste e só então depois desenvolver as funcionalidades, foi 
tudo por um bom caminho, exceto quando notei que o HttpRequestMessage sempre retornava requisições de forma assincronas, assim alguns
testes ficam chamando outros métodos para ter a informação necessária para o teste ser realizado com sucesso, porém mesmo assim algumas vezes
ocorre falha e não consegui identificar o motivo, seria necessário um pouco mais de leitura sobre C# e XUnit para entender seu 
funcionamento.

Existe 10 métodos para testar a Controller da Pergunta.
Existe 12 métodos para testar a Controller da Resposta.

Muita coisa pode ser refatorada ou melhorada, porém devido ao tempo preciso partir para a interface web.
