using FluxoCaixa.Models;
using FluxoCaixa.Repositories;
using FluxoCaixa.Services;
using System;
using Xunit;

namespace FluxoCaixa.Tests
{
    public class FluxoCaixaServiceTests
    {
        private readonly LancamentoRepository _repository;
        private readonly FluxoCaixaService _service;

        public FluxoCaixaServiceTests()
        {
            _repository = new LancamentoRepository();
            _service = new FluxoCaixaService(_repository);
        }

        [Fact]
        public void Acusa_Lancamento_Passado()
        {
            var lancamento = new Lancamento
            {
                Data = DateTime.Today.AddDays(-1),
                TipoDeLancamento = TipoLancamento.Recebimento,
                Descricao = "Teste",
                Conta = "12345-6",
                Banco = "Banco X",
                TipoDeConta = "Corrente",
                CpfCnpj = "123.456.789-00",
                Valor = 100
            };

            var podeLancar = _service.ValidarLancamento(lancamento, out string motivo);

            Assert.False(podeLancar);
            Assert.Equal("Não é permitido lançar com data no passado.", motivo);
        }

        [Fact]
        public void Acusa_Saldo_Negativo_Limite_Ultrapassado()
        {
            var lancamento = new Lancamento
            {
                Data = DateTime.Today,
                TipoDeLancamento = TipoLancamento.Pagamento,
                Descricao = "Pagamento Grande",
                Conta = "12345-6",
                Banco = "Banco Y",
                TipoDeConta = "Corrente",
                CpfCnpj = "123.456.789-00",
                Valor = 25000m
            };

            var podeLancar = _service.ValidarLancamento(lancamento, out string motivo);

            Assert.False(podeLancar);
            Assert.Equal("Saldo negativo máximo de R$ -20.000,00 ultrapassado.", motivo);
        }

        [Fact]
        public void Permite_Lancamento()
        {
            var lancamento = new Lancamento
            {
                Data = DateTime.Today,
                TipoDeLancamento = TipoLancamento.Recebimento,
                Descricao = "Recebimento válido",
                Conta = "12345-6",
                Banco = "Banco Z",
                TipoDeConta = "Corrente",
                CpfCnpj = "987.654.321-00",
                Valor = 500m
            };

            var podeLancar = _service.ValidarLancamento(lancamento, out string motivo);

            Assert.True(podeLancar);
            Assert.Equal(string.Empty, motivo);
        }
    }
}
