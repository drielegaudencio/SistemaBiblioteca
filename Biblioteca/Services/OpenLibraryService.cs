// Services/OpenLibraryService.cs
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Biblioteca.Services
{
    // DTO (Data Transfer Object) para os dados que queremos da Open Library API
    public class OpenLibraryBookData
    {
        public string? Title { get; set; }
        public int? NumberOfPages { get; set; }
        public List<string>? Authors { get; set; }
        public List<string>? Subjects { get; set; } // Gêneros/Assuntos
        public List<string>? Publishers { get; set; }
        public int? PublishYear { get; set; }
        public string? Isbn { get; set; } // Adicionado para a busca por título
    }

    // DTO para a resposta da API de busca por título (search.json)
    public class OpenLibrarySearchResponse
    {
        [JsonProperty("numFound")]
        public int NumFound { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("numFoundExact")]
        public bool NumFoundExact { get; set; }

        [JsonProperty("docs")]
        public List<OpenLibrarySearchDoc>? Docs { get; set; }
    }

    // DTO para cada documento (livro) dentro da resposta de busca por título
    public class OpenLibrarySearchDoc
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("author_name")]
        public List<string>? AuthorName { get; set; }

        [JsonProperty("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonProperty("number_of_pages_median")]
        public int? NumberOfPagesMedian { get; set; } // Mediana de páginas, pode ser útil

        [JsonProperty("isbn")]
        public List<string>? Isbn { get; set; } // Pode retornar múltiplos ISBNs

        [JsonProperty("subject")]
        public List<string>? Subject { get; set; } // Gêneros/Assuntos
    }


    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://openlibrary.org/");
        }

        /// <summary>
        /// Busca informações de um livro na Open Library API usando o ISBN.
        /// </summary>
        /// <param name="isbn">O ISBN do livro (ex: "9780321765723").</param>
        /// <returns>Um objeto OpenLibraryBookData com as informações do livro, ou null se não encontrado.</returns>
        public async Task<OpenLibraryBookData?> GetBookByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return null;
            }

            // URL da API: /api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data
            var requestUrl = $"api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); // Lança HttpRequestException para códigos de erro HTTP

                var jsonString = await response.Content.ReadAsStringAsync();

                dynamic? data = JsonConvert.DeserializeObject(jsonString);

                if (data == null) return null;

                dynamic? bookInfo = data[$"ISBN:{isbn}"];

                if (bookInfo != null)
                {
                    var bookData = new OpenLibraryBookData
                    {
                        Title = bookInfo.title?.ToString(),
                        NumberOfPages = bookInfo.number_of_pages?.ToObject<int>(),
                        PublishYear = bookInfo.publish_date != null ? ParsePublishYear(bookInfo.publish_date.ToString()) : null,
                        Isbn = isbn // O ISBN original da busca
                    };

                    if (bookInfo.authors != null)
                    {
                        bookData.Authors = new List<string>();
                        foreach (var author in bookInfo.authors)
                        {
                            bookData.Authors.Add(author.name?.ToString());
                        }
                    }

                    if (bookInfo.subjects != null)
                    {
                        bookData.Subjects = new List<string>();
                        foreach (var subject in bookInfo.subjects)
                        {
                            bookData.Subjects.Add(subject.name?.ToString());
                        }
                    }

                    if (bookInfo.publishers != null)
                    {
                        bookData.Publishers = new List<string>();
                        foreach (var publisher in bookInfo.publishers)
                        {
                            bookData.Publishers.Add(publisher.name?.ToString());
                        }
                    }

                    return bookData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro ao acessar a Open Library API por ISBN: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Erro ao desserializar a resposta da Open Library API por ISBN: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao buscar o livro por ISBN: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Busca livros na Open Library API por título.
        /// </summary>
        /// <param name="titleQuery">O título do livro para busca.</param>
        /// <returns>Uma lista de objetos OpenLibraryBookData com as informações dos livros encontrados.</returns>
        public async Task<List<OpenLibraryBookData>> SearchBooksByTitle(string titleQuery)
        {
            List<OpenLibraryBookData> results = new List<OpenLibraryBookData>();
            if (string.IsNullOrWhiteSpace(titleQuery))
            {
                return results;
            }

            // URL da API de busca por título: /search.json?q=termo
            var requestUrl = $"search.json?q={Uri.EscapeDataString(titleQuery)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var searchResponse = JsonConvert.DeserializeObject<OpenLibrarySearchResponse>(jsonString);

                if (searchResponse?.Docs != null)
                {
                    foreach (var doc in searchResponse.Docs)
                    {
                        results.Add(new OpenLibraryBookData
                        {
                            Title = doc.Title,
                            Authors = doc.AuthorName,
                            PublishYear = doc.FirstPublishYear,
                            NumberOfPages = doc.NumberOfPagesMedian, // Usar mediana de páginas
                            Isbn = doc.Isbn?.FirstOrDefault(), // Pega o primeiro ISBN se houver
                            Subjects = doc.Subject
                        });
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro ao acessar a Open Library API por título: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Erro ao desserializar a resposta da Open Library API por título: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao buscar livros por título: {ex.Message}");
            }

            return results;
        }

        // Helper para tentar parsear o ano de publicação de uma string
        private int? ParsePublishYear(string publishDate)
        {
            if (string.IsNullOrWhiteSpace(publishDate)) return null;

            if (int.TryParse(publishDate, out int year))
            {
                return year;
            }
            var match = System.Text.RegularExpressions.Regex.Match(publishDate, @"\d{4}$");
            if (match.Success && int.TryParse(match.Value, out year))
            {
                return year;
            }
            return null;
        }
    }
}
