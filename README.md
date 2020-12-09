# Vrnz2.Challenge.Payments


=> API de Manutenção de Pagamentos (Swagger => /swagger/index.html)

-[POST]  => /api/payments - Cadastra Pagamentos

-[GET]   => /api/payments - Obtém Pagamentos cadastrados

-[GET]   => /api/payments - Obtém contabilização feita pelo serviços 'Vrnz2.Challenge.CustomerConsumption' com o valor devido pelos Clientes

=> Persiste na Collection 'Challenge\Payment'

=> Envia informações de Clientes para a fila 'challenge-paymnet-created'
