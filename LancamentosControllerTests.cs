using FluxoCaixa.Controllers;
using FluxoCaixa.DTOs;
using FluxoCaixa.Repositories;
using FluxoCaixa.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace FluxoCaixa.Tests
{
    public class LancamentosControllerTests
    {
        private readonly LancamentosController _controller;

        public LancamentosControllerTests()
        {
            var repository = new LancamentoRepository();
            var service = new FluxoCaixaService(repository);
            _controller = new LancamentosController(repository, service);
        }

        [Fact]
        public void Post_Lancamento_Valido_Deve_Retornar_Ok()
        {
            var lancamentoDto = new LancamentoDto
            {
                Data = DateTime.Today,
                TipoDeLancamento = 2,
                Descricao = "Recebimento Teste",
                Conta = "12345-6",
                Banco = "Banco Teste",
                TipoDeConta = "Corrente",
                CpfCnpj = "98765432100",
                Valor = 100m
            };

            var resultado = _controller.Post(lancamentoDto);

            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact]
        public void Post_Lancamento_Com_Data_Passada_Deve_Retornar_BadRequest()
        {
            var lancamentoDto = new LancamentoDto
            {
                Data = DateTime.Today.AddDays(-1), // Data no passado
                TipoDeLancamento = 1, // Pagamento
                Descricao = "Pagamento inválido",
                Conta = "12345-6",
                Banco = "Banco Teste",
                TipoDeConta = "Corrente",
                CpfCnpj = "98765432100",
                Valor = 100m
            };

            var resultado = _controller.Post(lancamentoDto);

            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Post_TipoDeLancamento_Invalido_BadRequest()
        {
            var lancamentoDto = new LancamentoDto
            {
                Data = DateTime.Today,
                TipoDeLancamento = 3, // Tipo inválido (não 1 nem 2)
                Descricao = "Tipo inválido",
                Conta = "12345-6",
                Banco = "Banco Teste",
                TipoDeConta = "Corrente",
                CpfCnpj = "98765432100",
                Valor = 100m
            };

            var resultado = _controller.Post(lancamentoDto);

            Assert.IsType<BadRequestObjectResult>(resultado);
        }
    }
}
