﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECommerceBem.Exception {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceErrorsMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceErrorsMessages() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ECommerceBem.Exception.ResourceErrorsMessages", typeof(ResourceErrorsMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Não é possível cancelar um pedido que já foi concluído..
        /// </summary>
        public static string ErroCancelamento {
            get {
                return ResourceManager.GetString("ErroCancelamento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Não foi possível processar o estorno para o cancelamento do pedido..
        /// </summary>
        public static string ErroEstorno {
            get {
                return ResourceManager.GetString("ErroEstorno", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Estoque insuficiente para a quantidade solicitada..
        /// </summary>
        public static string EstoqueInsuficiente {
            get {
                return ResourceManager.GetString("EstoqueInsuficiente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a A forma de pagamento selecionada é inválida..
        /// </summary>
        public static string FormaPagamentoInvalida {
            get {
                return ResourceManager.GetString("FormaPagamentoInvalida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a A forma de pagamento é obrigatória..
        /// </summary>
        public static string FormaPagamentoObrigatoria {
            get {
                return ResourceManager.GetString("FormaPagamentoObrigatoria", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a A quantidade do item deve ser maior que zero..
        /// </summary>
        public static string ItemQuantidadeInvalida {
            get {
                return ResourceManager.GetString("ItemQuantidadeInvalida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a O pedido deve conter pelo menos um item..
        /// </summary>
        public static string PedidoItensObrigatorio {
            get {
                return ResourceManager.GetString("PedidoItensObrigatorio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Pedido com ID {0} não foi encontrado..
        /// </summary>
        public static string PedidoNaoEncontrado {
            get {
                return ResourceManager.GetString("PedidoNaoEncontrado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a O pedido se encontra no status {0}, não permite essa operação..
        /// </summary>
        public static string PedidoStatusErro {
            get {
                return ResourceManager.GetString("PedidoStatusErro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a A quantidade a ser adicionada deve ser maior que zero..
        /// </summary>
        public static string ProdutoEstoqueErro {
            get {
                return ResourceManager.GetString("ProdutoEstoqueErro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Produto com ID {0} não foi encontrado..
        /// </summary>
        public static string ProdutoNaoEncontrado {
            get {
                return ResourceManager.GetString("ProdutoNaoEncontrado", resourceCulture);
            }
        }
    }
}