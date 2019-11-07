using System;
using System.Text;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Alura.WebAPI.WebApp.Formatters
{
    public class LivroCsvFormatter : TextOutputFormatter
    {
        public LivroCsvFormatter()
        {
            var textCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            var csvMediaType = MediaTypeHeaderValue.Parse("application/csv");
            SupportedMediaTypes.Add(textCsvMediaType);
            SupportedMediaTypes.Add(csvMediaType);
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type) => type == typeof(LivroApi);

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
                                                    Encoding selectedEncoding)
        {
            var livroCsv = string.Empty;
            var objectContext = context.Object;

            if (objectContext is LivroApi)
            {
                var livro = objectContext as LivroApi;
                livroCsv = $"{livro.Titulo};{livro.Subtitulo};{livro.Lista}";
            }

            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
                return writer.WriteAsync(livroCsv);
        }
    }
}
