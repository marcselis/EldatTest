using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autohmation.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Autohmation
{
    public class HtmlOutputFormatter : TextOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/html"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (type == typeof(Models.Lamp))
                return true;
            if (type == typeof(DimmableLamp))
                return true;
            if (typeof(IEnumerable<DimmableLamp>).IsAssignableFrom(type))
                return true;
            if (typeof(IEnumerable<Models.Lamp>).IsAssignableFrom(type))
                return true;
            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            switch (context.Object)
            {
                case Models.Lamp lamp:
                    Format(buffer, lamp);
                    break;
                case IEnumerable<Models.Lamp> lamps:
                    foreach (var lamp in lamps)
                    {
                        Format(buffer, lamp);
                    }

                    break;
            }
            return response.WriteAsync($"<html><head><script src=\"https://code.jquery.com/jquery-3.3.1.min.js\"></script></head><body>{buffer}</body></html>");
        }

        private void Format(StringBuilder buffer, Models.Lamp lamp)
        {
            foreach (var action in lamp.Actions)

            {
            //    <button onclick="{ $.post('http://localhost:64420/api/lamps/KeukenTafel/turnoff'); }">Keuken gootsteen</button>

                var value = $"<button onclick=\"{{ $.{action.Verb.ToLower()}('{action.Link}');}}\">{lamp.Description}</button>";
                buffer.Append(value);
                buffer.Append(
                    $"<form action=\"{action.Link}\" method=\"post\"><input type=\"submit\" value=\"{lamp.Description}\" /></form>");
            }

        }

    }
}
